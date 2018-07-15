using System.Threading.Tasks;

namespace VehicleDealer.API.Core
{
    public interface IUnitOfWork
    {
        Task CompleteAsync();
    }
}