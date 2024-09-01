using MidProject.Data;
using MidProject.Models;
using MidProject.Models.Dto.Request;
using MidProject.Repositories.Interfaces;

namespace MidProject.Repositories.Services
{
    public class ChargerServices : ICharger
    {
        private readonly MidprojectDbContext _context;

        public ChargerServices(MidprojectDbContext context) 
        {
            _context = context;
        }

        public Task AddCharger(ChargerDto chargerDto)
        {
            throw new NotImplementedException();
        }

        public Task DeleteCharger(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Charger>> GetAllChargers()
        {
            throw new NotImplementedException();
        }

        public Task<Charger> GetChargerById(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateCharger(ChargerDto chargerDto)
        {
            throw new NotImplementedException();
        }
    }
}
