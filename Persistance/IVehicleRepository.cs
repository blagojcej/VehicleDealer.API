using System.Threading.Tasks;
using VehicleDealer.API.Models;

namespace VehicleDealer.API.Persistance
{
    public interface IVehicleRepository
    {
        Task<Vehicle> GetVehicle(int id, bool includeRelated = true);
        void Add(Vehicle vehicle);
        void Remove(Vehicle vehicle);
    }
}