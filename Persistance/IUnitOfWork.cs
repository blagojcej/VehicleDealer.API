using System.Threading.Tasks;

namespace VehicleDealer.API.Persistance
{
    public interface IUnitOfWork
    {
        Task CompleteAsync();
    }
}