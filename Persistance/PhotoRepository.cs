using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VehicleDealer.API.Core;
using VehicleDealer.API.Core.Models;

namespace VehicleDealer.API.Persistance
{
    public class PhotoRepository : IPhotoRepository
    {
        private readonly VegaDbContext _context;
        public PhotoRepository(VegaDbContext context)
        {
            this._context = context;

        }

        public async Task<IEnumerable<Photo>> GetPhotos(int vehicleId)
        {
            return await _context.Photos
            .Where(p => p.VehicleId == vehicleId)
            .ToListAsync();
        }
    }
}