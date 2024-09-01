using MidProject.Models;
using MidProject.Models.Dto.Request;

namespace MidProject.Repositories.Interfaces
{
    public interface ICharger
    {
        Task<IEnumerable<Charger>> GetAllChargers();
        Task<Charger> GetChargerById(int id);
        Task AddCharger(ChargerDto chargerDto);
        Task UpdateCharger(ChargerDto chargerDto);
        Task DeleteCharger(int id);
    }
}