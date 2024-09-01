using MidProject.Data;
using MidProject.Models;
using MidProject.Models.Dto.Request;
using MidProject.Repositories.Interfaces;

namespace MidProject.Repositories.Services
{
    public class FavoriteServices : IFavorite
    {
        private readonly MidprojectDbContext _context;

        public FavoriteServices(MidprojectDbContext context) 
        {
            _context = context;
        }

        public Task AddFavorite(FavoriteDto favoriteDto)
        {
            throw new NotImplementedException();
        }

        public Task DeleteFavorite(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Favorite>> GetAllFavorites()
        {
            throw new NotImplementedException();
        }

        public Task<Favorite> GetFavoriteById(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateFavorite(FavoriteDto favoriteDto)
        {
            throw new NotImplementedException();
        }
    }
}
