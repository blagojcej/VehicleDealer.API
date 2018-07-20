using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VehicleDealer.API.Core;
using VehicleDealer.API.Core.Models;
using VehicleDealer.API.Extensions;

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

        public async Task<QueryResult<Vehicle>> GetVehicles(VehicleQuery queryObject)
        {
            var result = new QueryResult<Vehicle>();

            var query = _context.Vehicles
            .Include(v => v.Features)
            .ThenInclude(vf => vf.Feature)
            .Include(v => v.Model)
            .ThenInclude(m => m.Make)
            .AsQueryable();

            if (queryObject.MakeId.HasValue)
            {
                query = query.Where(v => v.Model.MakeId == queryObject.MakeId.Value);
            }

            if (queryObject.ModelId.HasValue)
            {
                query = query.Where(v => v.ModelId == queryObject.ModelId.Value);
            }

            var columnsMap = new Dictionary<string, Expression<Func<Vehicle, object>>>()
            {
                ["make"] = v => v.Model.Make.Name,
                ["model"] = v => v.Model.Name,
                ["contactName"] = v => v.ContactName,
                // ["id"] = v => v.Id,
            };            

            result.TotalItems = await query.CountAsync();
            query = query.ApplyPaging<Vehicle>(queryObject);

            // query = ApplyOrdering(queryObject, query, columnsMap);
            query = query.ApplyOrdering<Vehicle>(queryObject, columnsMap);

            /*
            if (queryObject.SortBy == "make")
            {
                query = (queryObject.IsSortAscending) ? query.OrderBy(v => v.Model.Make.Name) : query.OrderByDescending(v => v.Model.Make.Name);
            }
            if (queryObject.SortBy == "model")
            {
                query = (queryObject.IsSortAscending) ? query.OrderBy(v => v.Model.Name) : query.OrderByDescending(v => v.Model.Name);
            }
            if (queryObject.SortBy == "contactName")
            {
                query = (queryObject.IsSortAscending) ? query.OrderBy(v => v.ContactName) : query.OrderByDescending(v => v.ContactName);
            }
            if (queryObject.SortBy == "id")
            {
                query = (queryObject.IsSortAscending) ? query.OrderBy(v => v.Id) : query.OrderByDescending(v => v.Id);
            }
            */

            result.Items = await query.ToListAsync();

            return result;
        }


    }
}