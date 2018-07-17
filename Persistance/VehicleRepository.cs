using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VehicleDealer.API.Core;
using VehicleDealer.API.Core.Models;

namespace VehicleDealer.API.Persistance
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly VegaDbContext _context;
        public VehicleRepository(VegaDbContext context)
        {
            this._context = context;

        }
        public async Task<Vehicle> GetVehicle(int id, bool includeRelated = true)
        {
            if (!includeRelated)
                return await _context.Vehicles.FindAsync(id);

            return await _context.Vehicles
            .Include(v => v.Features)
            .ThenInclude(vf => vf.Feature)
            .Include(v => v.Model)
            .ThenInclude(m => m.Make)
            .SingleOrDefaultAsync(v => v.Id == id);
        }

        public void Add(Vehicle vehicle)
        {
            _context.Vehicles.Add(vehicle);
        }

        public void Remove(Vehicle vehicle)
        {
            _context.Vehicles.Remove(vehicle);
        }

        public async Task<IEnumerable<Vehicle>> GetVehicles(Filter filter, bool includeRelated = true)
        {
            if (!includeRelated)
            {
                var queryOnlyVehicles = _context.Vehicles.AsQueryable();

                if (filter.MakeId.HasValue)
                {
                    queryOnlyVehicles = queryOnlyVehicles.Where(v => v.Model.MakeId == filter.MakeId.Value);
                }
                if (filter.ModelId.HasValue)
                {
                    queryOnlyVehicles = queryOnlyVehicles.Where(v => v.ModelId == filter.ModelId.Value);
                }

                return await queryOnlyVehicles.ToListAsync();
            }

            var query = _context.Vehicles
            .Include(v => v.Features)
            .ThenInclude(vf => vf.Feature)
            .Include(v => v.Model)
            .ThenInclude(m => m.Make)
            .AsQueryable();

            if (filter.MakeId.HasValue)
            {
                query = query.Where(v => v.Model.MakeId == filter.MakeId.Value);
            }

            if (filter.ModelId.HasValue)
            {
                query = query.Where(v => v.ModelId == filter.ModelId.Value);
            }


            return await query.ToListAsync();
        }
    }
}