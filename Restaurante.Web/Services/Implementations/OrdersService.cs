using Microsoft.EntityFrameworkCore;
using Restaurante.Web.Core;
using Restaurante.Web.Data;
using Restaurante.Web.Data.Entities;
using Restaurante.Web.Data.Enums;
using Restaurante.Web.DTOs.Orders;
using Restaurante.Web.Services.Abstractions;
using System.Linq.Expressions;

namespace Restaurante.Web.Services.Implementations
{
    public class OrdersService : IOrdersService
    {
        private readonly DataContext _context;

        public OrdersService(DataContext context)
        {
            _context = context;
        }

        
        private static readonly Expression<Func<Order, OrderDTO>> ToDto = o => new OrderDTO
        {
            Id = o.Id,
            Date = o.Date,
            Status = o.Status,
            DiningTableId = o.DiningTableId,
            TableNumber = o.DiningTable.Number,
            EmployeeId = o.EmployeeId,
            EmployeeName = o.Employee.Name,
            OrderDetails = o.OrderDetails.Select(d => new OrderDetailDTO
            {
                Id = d.Id,
                DishId = d.DishId,
                DishName = d.Dish.Name,
                Quantity = d.Quantity,
                UnitPrice = d.UnitPrice
            }).ToList()
        };

      
        private static bool IsClosed(OrderStatus status)
            => status == OrderStatus.Paid || status == OrderStatus.Cancelled;

        public async Task<Response<List<OrderDTO>>> GetAllAsync()
        {
            try
            {
                List<OrderDTO> orders = await _context.Orders
                    .OrderByDescending(o => o.Date)
                    .Select(ToDto)
                    .ToListAsync();

                return Response<List<OrderDTO>>.Success(orders);
            }
            catch (Exception ex)
            {
                return Response<List<OrderDTO>>.Failure(ex);
            }
        }

        public async Task<Response<OrderDTO>> GetByIdAsync(Guid id)
        {
            try
            {
                OrderDTO? order = await _context.Orders
                    .Where(o => o.Id == id)
                    .Select(ToDto)
                    .FirstOrDefaultAsync();

                if (order is null)
                {
                    return Response<OrderDTO>.Failure("El pedido no existe.");
                }

                return Response<OrderDTO>.Success(order);
            }
            catch (Exception ex)
            {
                return Response<OrderDTO>.Failure(ex);
            }
        }

        public async Task<Response<OrderDTO>> CreateAsync(CreateOrderDTO dto)
        {
            try
            {
                var table = await _context.DiningTables.FindAsync(dto.DiningTableId);

                if (table is null)
                {
                    return Response<OrderDTO>.Failure("La mesa seleccionada no existe.");
                }

                if (!table.IsAvailable)
                {
                    return Response<OrderDTO>.Failure("La mesa seleccionada no está disponible.");
                }

                bool employeeExists = await _context.Employees.AnyAsync(e => e.IdEmployee == dto.EmployeeId);

                if (!employeeExists)
                {
                    return Response<OrderDTO>.Failure("El mesero seleccionado no existe.");
                }

                Order order = new()
                {
                    DiningTableId = dto.DiningTableId,
                    EmployeeId = dto.EmployeeId,
                    Date = DateTime.Now,
                    Status = OrderStatus.Pending
                };

                table.IsAvailable = false;
                _context.Orders.Add(order);

                await _context.SaveChangesAsync();

                OrderDTO created = await _context.Orders
                    .Where(o => o.Id == order.Id)
                    .Select(ToDto)
                    .FirstAsync();

                return Response<OrderDTO>.Success(created, "Pedido creado correctamente.");
            }
            catch (Exception ex)
            {
                return Response<OrderDTO>.Failure(ex);
            }
        }

        public async Task<Response> UpdateStatusAsync(UpdateOrderDTO dto)
        {
            try
            {
                var order = await _context.Orders
                    .Include(o => o.DiningTable)
                    .Include(o => o.OrderDetails)
                    .FirstOrDefaultAsync(o => o.Id == dto.Id);

                if (order is null)
                {
                    return Response.Failure("El pedido no existe.");
                }

                if (IsClosed(order.Status))
                {
                    return Response.Failure("Un pedido pagado o cancelado ya no se puede modificar.");
                }

                if (dto.Status == OrderStatus.Paid && !order.OrderDetails.Any())
                {
                    return Response.Failure("No se puede pagar un pedido sin platos.");
                }

                order.Status = dto.Status;

      
                if (IsClosed(order.Status))
                {
                    order.DiningTable.IsAvailable = true;
                }

                await _context.SaveChangesAsync();

                return Response.Success("Estado del pedido actualizado.");
            }
            catch (Exception ex)
            {
                return Response.Failure(ex);
            }
        }

        public async Task<Response> AddDetailAsync(CreateOrderDetailDTO dto)
        {
            try
            {
                var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == dto.OrderId);

                if (order is null)
                {
                    return Response.Failure("El pedido no existe.");
                }

                if (IsClosed(order.Status))
                {
                    return Response.Failure("No se pueden agregar platos a un pedido cerrado.");
                }

                var dish = await _context.Dishes.FindAsync(dto.DishId);

                if (dish is null || !dish.State)
                {
                    return Response.Failure("El plato seleccionado no está disponible.");
                }

                var detail = await _context.OrderDetails
                    .FirstOrDefaultAsync(d => d.OrderId == dto.OrderId && d.DishId == dto.DishId);

                if (detail is null)
                {
                   
                    _context.OrderDetails.Add(new OrderDetail
                    {
                        OrderId = dto.OrderId,
                        DishId = dto.DishId,
                        Quantity = dto.Quantity,
                        UnitPrice = dish.Price
                    });
                }
                else
                {
                    
                    detail.Quantity += dto.Quantity;
                }

                await _context.SaveChangesAsync();

                return Response.Success("Plato agregado al pedido.");
            }
            catch (Exception ex)
            {
                return Response.Failure(ex);
            }
        }

        public async Task<Response> RemoveDetailAsync(Guid detailId)
        {
            try
            {
                var detail = await _context.OrderDetails
                    .Include(d => d.Order)
                    .FirstOrDefaultAsync(d => d.Id == detailId);

                if (detail is null)
                {
                    return Response.Failure("La línea del pedido no existe.");
                }

                if (IsClosed(detail.Order.Status))
                {
                    return Response.Failure("No se pueden quitar platos de un pedido cerrado.");
                }

                _context.OrderDetails.Remove(detail);
                await _context.SaveChangesAsync();

                return Response.Success("Plato eliminado del pedido.");
            }
            catch (Exception ex)
            {
                return Response.Failure(ex);
            }
        }
    }
}