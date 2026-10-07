using Microsoft.EntityFrameworkCore;
using Restaurante.Web.Data;
using Restaurante.Web.DTOs.Categorias;
using Restaurante.Web.Entities;
using Restaurante.Web.Data.Entities;

namespace Restaurante.Web.Services
{
    public class CategoriasService : ICategoriasService
    {
        private readonly DataContext _context;

        public CategoriasService(DataContext context)
        {
            _context = context;
        }

        public async Task<List<CategoriaDTO>> ObtenerTodasAsync()
        {
            return await _context.Categorias
                .Select(c => new CategoriaDTO
                {
                    IdCategorias = c.IdCategorias,
                    Nombre = c.Nombre,
                    Descripcion = c.Descripcion,
                    Estado = c.Estado
                }).ToListAsync();
        }

        public async Task<CategoriaDTO?> ObtenerPorIdAsync(Guid id)
        {
            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null) return null;

            return new CategoriaDTO
            {
                IdCategorias = categoria.IdCategorias,
                Nombre = categoria.Nombre,
                Descripcion = categoria.Descripcion,
                Estado = categoria.Estado
            };
        }

        public async Task<bool> CrearAsync(CrearCategoriaDTO dto)
        {

            var categorias = new Categorias
            {
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                Estado = true 
            };

            _context.Categorias.Add(categorias);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ActualizarAsync(ActualizarCategoriaDTO dto)
        {
            var categoria = await _context.Categorias.FindAsync(dto.IdCategorias);
            if (categoria == null) return false;

            categoria.Nombre = dto.Nombre;
            categoria.Descripcion = dto.Descripcion;
            categoria.Estado = dto.Estado;

            _context.Categorias.Update(categoria);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> EliminarAsync(Guid id)
        {
            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null) return false;
            var tienePlatos = await _context.Plato.AnyAsync(p => p.IdCategorias == id);
            if (tienePlatos) return false;

            _context.Categorias.Remove(categoria);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}