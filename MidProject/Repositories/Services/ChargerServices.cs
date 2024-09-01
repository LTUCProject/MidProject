using MidProject.Data;
using MidProject.Models;
using MidProject.Repositories.Interfaces;

namespace MidProject.Repositories.Services
{
    public class ChargerServices : Repository<Charger>, ICharger
    {
        private readonly MidprojectDbContext _context;

        public ChargerServices(MidprojectDbContext context) : base(context)
        {
            _context = context;
        }

    }
}
