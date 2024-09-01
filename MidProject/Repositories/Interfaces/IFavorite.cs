using MidProject.Models;
using MidProject.Models.Dto.Request;

namespace MidProject.Repositories.Interfaces
{
    public interface IFavorite
    {
        Task<IEnumerable<Favorite>> GetAllFavorites();
        Task<Favorite> GetFavoriteById(int id);
        Task AddFavorite(FavoriteDto favoriteDto);
        Task UpdateFavorite(FavoriteDto favoriteDto);
        Task DeleteFavorite(int id);
    }
}