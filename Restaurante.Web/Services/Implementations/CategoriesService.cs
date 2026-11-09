using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Restaurante.Web.Core;
using Restaurante.Web.Core.Pagination;
using Restaurante.Web.Data;
using Restaurante.Web.Data.Entities;
using Restaurante.Web.DTOs.Categories;
using Restaurante.Web.Services.Abstractions;

namespace Restaurante.Web.Services.Implementations
{
    public class CategoriesService : CustomQueryableOperationsService, ICategoriesService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public CategoriesService(DataContext context, IMapper mapper) : base(context, mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response<PaginationResponse<CategoryDTO>>> GetPaginationAsync(PaginationRequest request)
        {
            IQueryable<Category> query = _context.Categories.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Filter))
            {
                string filter = request.Filter.ToLower();

                query = query.Where(c => c.Name.ToLower().Contains(filter)
                                      || (c.Description != null && c.Description.ToLower().Contains(filter)));
            }

            
            query = query.OrderBy(c => c.Name);

            return await GetPagedListAsync<CategoryDTO, Category>(request, query);
        }

        
        public async Task<Response<List<CategoryDTO>>> GetAllAsync()
        {
            try
            {
                List<Category> categories = await _context.Categories
                    .OrderBy(c => c.Name)
                    .ToListAsync();

                return Response<List<CategoryDTO>>.Success(_mapper.Map<List<CategoryDTO>>(categories));
            }
            catch (Exception ex)
            {
                return Response<List<CategoryDTO>>.Failure(ex);
            }
        }

        public async Task<Response<CategoryDTO>> GetOneAsync(Guid id)
        {
            return await GetOneAsync<CategoryDTO, Category>(id);
        }

        public async Task<Response<CreateCategoryDTO>> CreateAsync(CreateCategoryDTO dto)
        {
            return await CreateAsync<CreateCategoryDTO, Category>(dto);
        }

        public async Task<Response<UpdateCategoryDTO>> UpdateAsync(UpdateCategoryDTO dto)
        {
            return await UpdateAsync<UpdateCategoryDTO, Category>(dto, dto.Id);
        }

        public async Task<Response> DeleteAsync(Guid id)
        {
            bool hasDishes = await _context.Dishes.AnyAsync(d => d.CategoryId == id);

            if (hasDishes)
            {
                return Response.Failure("No se puede eliminar la categoría porque tiene platos asociados.");
            }

            return await DeleteAsync<Category>(id);
        }
    }
}