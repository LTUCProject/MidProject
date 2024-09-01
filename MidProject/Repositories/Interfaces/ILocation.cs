using MidProject.Models;
using MidProject.Models.Dto.Request;

namespace MidProject.Repositories.Interfaces
{
    public interface ILocation
    {
        Task<IEnumerable<Location>> GetAllLocations();
        Task<Location> GetLocationById(int id);
        Task AddLocation(LocationDto locationDto);
        Task UpdateLocation(LocationDto locationDto);
        Task DeleteLocation(int id);
    }
}