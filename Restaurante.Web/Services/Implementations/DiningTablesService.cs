using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Restaurante.Web.Core;
using Restaurante.Web.Core.Pagination;
using Restaurante.Web.Data;
using Restaurante.Web.Data.Entities;
using Restaurante.Web.Data.Enums;
using Restaurante.Web.DTOs.DiningTables;
using Restaurante.Web.Services.Abstractions;

namespace Restaurante.Web.Services.Implementations
{
    public class DiningTablesService : CustomQueryableOperationsService, IDiningTablesService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public DiningTablesService(DataContext context, IMapper mapper) : base(context, mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response<PaginationResponse<DiningTableDTO>>> GetPaginationAsync(PaginationRequest request)
        {
            IQueryable<DiningTable> query = _context.DiningTables.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Filter))
            {
               
                string filter = request.Filter.Trim();
                query = query.Where(t => t.Number.ToString().Contains(filter));
            }

            query = query.OrderBy(t => t.Number);

            return await GetPagedListAsync<DiningTableDTO, DiningTable>(request, query);
        }

        
        public async Task<Response<List<DiningTableDTO>>> GetAllAsync()
        {
            try
            {
                List<DiningTable> tables = await _context.DiningTables
                    .OrderBy(t => t.Number)
                    .ToListAsync();

                return Response<List<DiningTableDTO>>.Success(_mapper.Map<List<DiningTableDTO>>(tables));
            }
            catch (Exception ex)
            {
                return Response<List<DiningTableDTO>>.Failure(ex);
            }
        }

        public async Task<Response<DiningTableDTO>> GetOneAsync(Guid id)
        {
            return await GetOneAsync<DiningTableDTO, DiningTable>(id);
        }

        public async Task<Response<CreateDiningTableDTO>> CreateAsync(CreateDiningTableDTO dto)
        {
            bool numberExists = await _context.DiningTables.AnyAsync(t => t.Number == dto.Number);

            if (numberExists)
            {
                return Response<CreateDiningTableDTO>.Failure($"Ya existe una mesa con el número {dto.Number}.");
            }

            return await CreateAsync<CreateDiningTableDTO, DiningTable>(dto);
        }

        public async Task<Response<UpdateDiningTableDTO>> UpdateAsync(UpdateDiningTableDTO dto)
        {
            bool numberExists = await _context.DiningTables.AnyAsync(t => t.Number == dto.Number && t.Id != dto.Id);

            if (numberExists)
            {
                return Response<UpdateDiningTableDTO>.Failure($"Ya existe una mesa con el número {dto.Number}.");
            }

            return await UpdateAsync<UpdateDiningTableDTO, DiningTable>(dto, dto.Id);
        }

        public async Task<Response> DeleteAsync(Guid id)
        {
            bool hasOrders = await _context.Orders.AnyAsync(o => o.DiningTableId == id);

            if (hasOrders)
            {
                return Response.Failure("No se puede eliminar la mesa porque tiene pedidos asociados.");
            }

            return await DeleteAsync<DiningTable>(id);
        }

        public async Task<Response> ToggleAvailabilityAsync(Guid id)
        {
            try
            {
                DiningTable? table = await _context.DiningTables.FirstOrDefaultAsync(t => t.Id == id);

                if (table is null)
                {
                    return Response.Failure("La mesa no existe.");
                }

               
                if (!table.IsAvailable)
                {
                    bool hasOpenOrder = await _context.Orders.AnyAsync(o => o.DiningTableId == id
                                                                         && o.Status != OrderStatus.Paid
                                                                         && o.Status != OrderStatus.Cancelled);

                    if (hasOpenOrder)
                    {
                        return Response.Failure("La mesa tiene un pedido abierto. Pague o cancele el pedido para liberarla.");
                    }
                }

                table.IsAvailable = !table.IsAvailable;
                await _context.SaveChangesAsync();

                return Response.Success(table.IsAvailable ? "Mesa marcada como disponible." : "Mesa marcada como ocupada.");
            }
            catch (Exception ex)
            {
                return Response.Failure(ex);
            }
        }
    }
}