using Microsoft.AspNetCore.Http;
using MidProject.Models;
using MidProject.Models.Dto.Request;

namespace MidProject.Repositories.Interfaces
{
    public interface ISessionx
    {
        Task<IEnumerable<Session>> GetAllSessions();
        Task<Session> GetSessionById(int id);
        Task AddSession(SessionDto sessionDto);
        Task UpdateSession(SessionDto sessionDto);
        Task DeleteSession(int id);
    }
}