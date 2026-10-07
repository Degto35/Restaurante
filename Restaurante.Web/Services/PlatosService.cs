using Microsoft.EntityFrameworkCore;
using Restaurante.Web.Data;
using Restaurante.Web.DTOs.Platos;
using Restaurante.Web.Models;

namespace Restaurante.Web.Services
{
    public class PlatosService : IPlatosService
    {
        private readonly DataContext _context;

        public PlatosService(DataContext context)
        {
            _context = context;
        }

        public async Task<List<PlatoDTO>> ObtenerTodosAsync()
        {
            return await _context.Plato
                .Include(p => p.Categorias) // ¡IMPORTANTE! Trae los datos de la categoría
                .Select(p => new PlatoDTO
                {
                    IdPlato = p.IdPlato,
                    Nombre = p.Nombre,
                    Descripcion = p.Descripcion,
                    Precio = p.Precio,
                    Estado = p.Estado,
                    IdCategorias = p.IdCategorias,
                    NombreCategorias = p.Categorias.Nombre // Se llena gracias al Include
                }).ToListAsync();
        }

        public async Task<PlatoDTO?> ObtenerPorIdAsync(int id)
        {
            var plato = await _context.Plato
                .Include(p => p.Categorias)
                .FirstOrDefaultAsync(p => p.IdPlato == id);

            if (plato == null) return null;

            return new PlatoDTO
            {
                IdPlato = plato.IdPlato,
                Nombre = plato.Nombre,
                Descripcion = plato.Descripcion,
                Precio = plato.Precio,
                Estado = plato.Estado,
                IdCategorias = plato.IdCategorias,
                NombreCategorias = plato.Categorias.Nombre
            };
        }

        public async Task<bool> CrearAsync(CrearPlatoDTO dto)
        {
            var plato = new Plato
            {
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                Precio = dto.Precio,
                IdCategorias = dto.IdCategorias,
                Estado = true
            };

            _context.Plato.Add(plato);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ActualizarAsync(ActualizarPlatoDTO dto)
        {
            var plato = await _context.Plato.FindAsync(dto.IdPlato);
            if (plato == null) return false;

            plato.Nombre = dto.Nombre;
            plato.Descripcion = dto.Descripcion;
            plato.Precio = dto.Precio;
            plato.IdCategorias = dto.IdCategorias;
            plato.Estado = dto.Estado;

            _context.Plato.Update(plato);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var plato = await _context.Plato.FindAsync(id);
            if (plato == null) return false;

            var estaEnPedido = await _context.DetallesPedido.AnyAsync(d => d.IdPlato == id);
            if (estaEnPedido) return false;

            _context.Plato.Remove(plato);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}