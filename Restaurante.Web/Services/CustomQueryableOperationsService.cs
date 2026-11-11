using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Restaurante.Web.Core;
using Restaurante.Web.Core.Pagination;
using Restaurante.Web.Data;
using Restaurante.Web.Data.Abstractions;

namespace Restaurante.Web.Services
{

    public class CustomQueryableOperationsService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public CustomQueryableOperationsService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response<TDto>> CreateAsync<TDto, TEntity>(TDto dto) where TEntity : class, IId
        {
            try
            {
                TEntity entity = _mapper.Map<TEntity>(dto);
                entity.Id = Guid.NewGuid();

                await _context.Set<TEntity>().AddAsync(entity);
                await _context.SaveChangesAsync();

                return Response<TDto>.Success(dto, "Registro creado con éxito");
            }
            catch (Exception ex)
            {
                return Response<TDto>.Failure(ex);
            }
        }

        public async Task<Response> DeleteAsync<TEntity>(Guid id) where TEntity : class, IId
        {
            try
            {
                TEntity? entity = await _context.Set<TEntity>().FirstOrDefaultAsync(e => e.Id == id);

                if (entity is null)
                {
                    return Response.Failure($"No existe registro con id {id}");
                }

                _context.Set<TEntity>().Remove(entity);
                await _context.SaveChangesAsync();

                return Response.Success("Registro eliminado con éxito");
            }
            catch (Exception ex)
            {
                return Response.Failure(ex);
            }
        }

        public async Task<Response<TDto>> UpdateAsync<TDto, TEntity>(TDto dto, Guid id) where TEntity : class, IId
        {
            try
            {
                if (id == Guid.Empty)
                {
                    return Response<TDto>.Failure("El id no puede ser vacío");
                }

                TEntity? entity = await _context.Set<TEntity>().FirstOrDefaultAsync(e => e.Id == id);

                if (entity is null)
                {
                    return Response<TDto>.Failure($"No existe registro con id {id}");
                }

                
                _mapper.Map(dto, entity);
                entity.Id = id;

                await _context.SaveChangesAsync();

                return Response<TDto>.Success(dto, "Registro actualizado con éxito");
            }
            catch (Exception ex)
            {
                return Response<TDto>.Failure(ex);
            }
        }

        public async Task<Response<TDto>> GetOneAsync<TDto, TEntity>(Guid id, IQueryable<TEntity>? query = null) where TEntity : class, IId
        {
            try
            {
               
                IQueryable<TEntity> source = query ?? _context.Set<TEntity>();

                TEntity? entity = await source.FirstOrDefaultAsync(e => e.Id == id);

                if (entity is null)
                {
                    return Response<TDto>.Failure($"No existe registro con id {id}");
                }

                TDto dto = _mapper.Map<TDto>(entity);

                return Response<TDto>.Success(dto, "Registro obtenido con éxito");
            }
            catch (Exception ex)
            {
                return Response<TDto>.Failure(ex);
            }
        }

        public async Task<Response<PaginationResponse<TDto>>> GetPagedListAsync<TDto, TEntity>(PaginationRequest request, IQueryable<TEntity>? query = null)
            where TEntity : class
            where TDto : class
        {
            try
            {
                
                IQueryable<TEntity> source = query ?? _context.Set<TEntity>();

                PagedList<TEntity> list = await PagedList<TEntity>.ToPagedListAsync(source, request);

                PaginationResponse<TDto> response = new PaginationResponse<TDto>
                {
                    List = _mapper.Map<PagedList<TDto>>(list),
                    CurrentPage = list.CurrentPage,
                    TotalPages = list.TotalPages,
                    RecordsPerPage = list.RecordsPerPage,
                    TotalCount = list.TotalCount,
                    Filter = request.Filter
                };

                return Response<PaginationResponse<TDto>>.Success(response, "Registro obtenido con éxito");
            }
            catch (Exception ex)
            {
                return Response<PaginationResponse<TDto>>.Failure(ex);
            }
        }
    }
}