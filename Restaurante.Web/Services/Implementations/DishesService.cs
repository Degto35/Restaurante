using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Restaurante.Web.Core;
using Restaurante.Web.Core.Pagination;
using Restaurante.Web.Data;
using Restaurante.Web.Data.Entities;
using Restaurante.Web.DTOs.Dishes;
using Restaurante.Web.Services.Abstractions;

namespace Restaurante.Web.Services.Implementations
{
    public class DishesService : CustomQueryableOperationsService, IDishesService
    {
        private readonly DataContext _context;

        public DishesService(DataContext context, IMapper mapper) : base(context, mapper)
        {
            _context = context;
        }

        public async Task<Response<PaginationResponse<DishDTO>>> GetPaginationAsync(PaginationRequest request)
        {
            
            IQueryable<Dish> query = _context.Dishes.Include(d => d.Category);

            if (!string.IsNullOrWhiteSpace(request.Filter))
            {
                string filter = request.Filter.ToLower();

                query = query.Where(d => d.Name.ToLower().Contains(filter)
                                      || (d.Description != null && d.Description.ToLower().Contains(filter))
                                      || d.Category.Name.ToLower().Contains(filter));
            }

            query = query.OrderBy(d => d.Name);

            return await GetPagedListAsync<DishDTO, Dish>(request, query);
        }

        public async Task<Response<DishDTO>> GetOneAsync(Guid id)
        {
            return await GetOneAsync<DishDTO, Dish>(id, _context.Dishes.Include(d => d.Category));
        }

        public async Task<Response<CreateDishDTO>> CreateAsync(CreateDishDTO dto)
        {
            Response check = await ValidateCategoryAsync(dto.CategoryId, currentCategoryId: null);

            if (!check.IsSuccess)
            {
                return Response<CreateDishDTO>.Failure(check.Message!);
            }

            return await CreateAsync<CreateDishDTO, Dish>(dto);
        }

        public async Task<Response<UpdateDishDTO>> UpdateAsync(UpdateDishDTO dto)
        {
            
            Guid? currentCategoryId = await _context.Dishes
                .Where(d => d.Id == dto.Id)
                .Select(d => (Guid?)d.CategoryId)
                .FirstOrDefaultAsync();

            Response check = await ValidateCategoryAsync(dto.CategoryId, currentCategoryId);

            if (!check.IsSuccess)
            {
                return Response<UpdateDishDTO>.Failure(check.Message!);
            }

            return await UpdateAsync<UpdateDishDTO, Dish>(dto, dto.Id);
        }

        public async Task<Response> DeleteAsync(Guid id)
        {
            bool isInOrders = await _context.OrderDetails.AnyAsync(d => d.DishId == id);

            if (isInOrders)
            {
                return Response.Failure("No se puede eliminar el plato porque está asociado a pedidos. Puede desactivarlo en su lugar.");
            }

            return await DeleteAsync<Dish>(id);
        }

        private async Task<Response> ValidateCategoryAsync(Guid categoryId, Guid? currentCategoryId)
        {
            Category? category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);

            if (category is null)
            {
                return Response.Failure("La categoría seleccionada no existe.");
            }

            if (!category.IsActive && categoryId != currentCategoryId)
            {
                return Response.Failure("No se puede asignar una categoría inactiva.");
            }

            return Response.Success();
        }
    }
}