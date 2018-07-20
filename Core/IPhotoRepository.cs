using System.Collections.Generic;
using System.Threading.Tasks;
using VehicleDealer.API.Core.Models;

namespace VehicleDealer.API.Core
{
    public interface IPhotoRepository
    {
         Task<IEnumerable<Photo>> GetPhotos(int vehicleId);
    }
}