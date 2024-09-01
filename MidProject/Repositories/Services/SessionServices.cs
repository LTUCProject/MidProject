using MidProject.Data;
using MidProject.Models;
using MidProject.Models.Dto.Request;
using MidProject.Repositories.Interfaces;

namespace MidProject.Repositories.Services
{
    public class SessionServices : ISessionx
    {
        private readonly MidprojectDbContext _context;

        public SessionServices(MidprojectDbContext context) 
        {
            _context = context;
        }

        public Task AddSession(SessionDto sessionDto)
        {
            throw new NotImplementedException();
        }

        public Task DeleteSession(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Session>> GetAllSessions()
        {
            throw new NotImplementedException();
        }

        public Task<Session> GetSessionById(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateSession(SessionDto sessionDto)
        {
            throw new NotImplementedException();
        }
    }
}
