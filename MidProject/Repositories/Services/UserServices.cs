using MidProject.Data;
using MidProject.Models;
using MidProject.Models.Dto.Request;
using MidProject.Repositories.Interfaces;

namespace MidProject.Repositories.Services
{
    public class UserServices : IUser
    {
        private readonly MidprojectDbContext _context;

        public UserServices(MidprojectDbContext context)
        {
            _context = context;
        }

        public Task AddUser(UserDto userDto)
        {
            throw new NotImplementedException();
        }

        public Task DeleteUser(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<User>> GetAllUsers()
        {
            throw new NotImplementedException();
        }

        public Task<User> GetUserById(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateUser(UserDto userDto)
        {
            throw new NotImplementedException();
        }
    }
   
}
