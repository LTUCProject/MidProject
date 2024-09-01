using MidProject.Data;
using MidProject.Models;
using MidProject.Models.Dto.Request;
using MidProject.Repositories.Interfaces;

namespace MidProject.Repositories.Services
{
    public class LocationServices : ILocation
    {
        private readonly MidprojectDbContext _context;

        public LocationServices(MidprojectDbContext context) 
        {
            _context = context;
        }

        public Task AddLocation(LocationDto locationDto)
        {
            throw new NotImplementedException();
        }

        public Task DeleteLocation(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Location>> GetAllLocations()
        {
            throw new NotImplementedException();
        }

        public Task<Location> GetLocationById(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateLocation(LocationDto locationDto)
        {
            throw new NotImplementedException();
        }
    }
}
