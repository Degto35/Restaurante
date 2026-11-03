using Microsoft.EntityFrameworkCore;
using Restaurante.Web.Data;
using Restaurante.Web.Data.Entities; 
using Restaurante.Web.DTOs.Tables;
using Restaurante.Web.Services.Abstractions;

namespace Restaurante.Web.Services.Implementations
{
    public class TablesService : ITablesService
    {
        private readonly DataContext _context;

        public TablesService(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TableDTO>> ObtenerTodasAsync()
        {
            return await _context.DiningTables
                .OrderBy(t => t.Number)
                .Select(t => new TableDTO
                {
                    IdTable = t.IdTable,
                    Number = t.Number,
                    Capacity = t.Capacity,
                    IsAvailable = t.IsAvailable
                })
                .ToListAsync();
        }

        public async Task<TableDTO?> ObtenerPorIdAsync(Guid id)
        {
            var table = await _context.DiningTables.FindAsync(id);
            if (table == null) return null;

            return new TableDTO
            {
                IdTable = table.IdTable,
                Number = table.Number,
                Capacity = table.Capacity,
                IsAvailable = table.IsAvailable
            };
        }

        public async Task<bool> CrearAsync(CreateTableDTO dto)
        {
            var table = new DiningTable
            {
                IdTable = Guid.NewGuid(),
                Number = dto.Number,
                Capacity = dto.Capacity,
                IsAvailable = true
            };

            _context.DiningTables.Add(table);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ActualizarAsync(UpdateTableDTO dto)
        {
            var table = await _context.DiningTables.FindAsync(dto.IdTable);
            if (table == null) return false;

            table.Number = dto.Number;
            table.Capacity = dto.Capacity;
            table.IsAvailable = dto.IsAvailable;

            _context.DiningTables.Update(table);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> EliminarAsync(Guid id)
        {
            var table = await _context.DiningTables.FindAsync(id);
            if (table == null) return false;

            _context.DiningTables.Remove(table);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> CambiarDisponibilidadAsync(Guid id)
        {
            var table = await _context.DiningTables.FindAsync(id);
            if (table == null) return false;

            table.IsAvailable = !table.IsAvailable;
            _context.DiningTables.Update(table);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}