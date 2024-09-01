using MidProject.Models;
using MidProject.Models.Dto.Request;

namespace MidProject.Repositories.Interfaces
{
    public interface IUser
    {
        Task<IEnumerable<User>> GetAllUsers();
        Task<User> GetUserById(int id);
        Task AddUser(UserDto userDto);
        Task UpdateUser(UserDto userDto);
        Task DeleteUser(int id);
    }
}