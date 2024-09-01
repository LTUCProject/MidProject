using MidProject.Data;
using MidProject.Models;
using MidProject.Repositories.Interfaces;

namespace MidProject.Repositories.Services
{
    public class UserServices : Repository<User>, IUser
    {
        private readonly MidprojectDbContext _context;

        public UserServices(MidprojectDbContext context) : base(context)
        {
            _context = context;
        }

    }
   
}
