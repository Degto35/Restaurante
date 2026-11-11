using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Restaurante.Web.Core;
using Restaurante.Web.Core.Pagination;
using Restaurante.Web.Data;
using Restaurante.Web.Data.Entities;
using Restaurante.Web.Data.Enums;
using Restaurante.Web.DTOs.Orders;
using Restaurante.Web.Services.Abstractions;

namespace Restaurante.Web.Services.Implementations
{
    public class OrdersService : CustomQueryableOperationsService, IOrdersService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public OrdersService(DataContext context, IMapper mapper) : base(context, mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        private IQueryable<Order> OrdersWithRelations()
            => _context.Orders
                .Include(o => o.DiningTable)
                .Include(o => o.User)
                .Include(o => o.Bill)
                .Include(o => o.OrderDetails).ThenInclude(d => d.Dish);


        private static bool IsClosed(OrderStatus status)
            => status == OrderStatus.Paid || status == OrderStatus.Cancelled;

        public async Task<Response<PaginationResponse<OrderDTO>>> GetPaginationAsync(PaginationRequest request)
        {
            IQueryable<Order> query = OrdersWithRelations();

            if (!string.IsNullOrWhiteSpace(request.Filter))
            {
                string filter = request.Filter.ToLower();

                query = query.Where(o => (o.User.FirstName + " " + o.User.LastName).ToLower().Contains(filter)
                       || o.DiningTable.Number.ToString().Contains(filter));
            }

            query = query.OrderByDescending(o => o.Date);

            return await GetPagedListAsync<OrderDTO, Order>(request, query);
        }

        public async Task<Response<OrderDTO>> GetOneAsync(Guid id)
        {
            return await GetOneAsync<OrderDTO, Order>(id, OrdersWithRelations());
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

                bool userExists = await _context.Users.AnyAsync(u => u.Id == dto.UserId);

                if (!userExists)
                {
                    return Response<OrderDTO>.Failure("El mesero seleccionado no existe.");
                }

                Order order = new()
                {
                    DiningTableId = dto.DiningTableId,
                    UserId = dto.UserId,
                    Date = DateTime.Now,
                    Status = OrderStatus.Pending
                };

                table.IsAvailable = false;
                _context.Orders.Add(order);

                await _context.SaveChangesAsync();

                Order created = await OrdersWithRelations().FirstAsync(o => o.Id == order.Id);

                return Response<OrderDTO>.Success(_mapper.Map<OrderDTO>(created), "Pedido creado correctamente.");
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

                if (dto.Status == OrderStatus.Paid)
                {
                    return Response.Failure("Para pagar un pedido use la opción Pagar.");
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

                var dish = await _context.Dishes.Include(d => d.Category).FirstOrDefaultAsync(d => d.Id == dto.DishId);

                if (dish is null || !dish.IsActive || !dish.Category.IsActive)
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

        public async Task<Response> PayAsync(PayOrderDTO dto)
        {
            try
            {
                var order = await _context.Orders
                    .Include(o => o.DiningTable)
                    .Include(o => o.OrderDetails)
                    .FirstOrDefaultAsync(o => o.Id == dto.OrderId);

                if (order is null)
                {
                    return Response.Failure("El pedido no existe.");
                }

                if (IsClosed(order.Status))
                {
                    return Response.Failure("El pedido ya está cerrado.");
                }

                if (!order.OrderDetails.Any())
                {
                    return Response.Failure("No se puede pagar un pedido sin platos.");
                }

                if (!Enum.IsDefined(dto.PaymentMethod))
                {
                    return Response.Failure("El método de pago no es válido.");
                }

                Bill bill = new()
                {
                    OrderId = order.Id,
                    PaymentMethod = dto.PaymentMethod,
                    PaidAt = DateTime.Now,
                    Total = order.Total
                };

                order.Status = OrderStatus.Paid;
                order.DiningTable.IsAvailable = true;
                _context.Bills.Add(bill);

                await _context.SaveChangesAsync();

                return Response.Success("Pago registrado correctamente.");
            }
            catch (Exception ex)
            {
                return Response.Failure(ex);
            }
        }
    }
}