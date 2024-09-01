using MidProject.Models;
using MidProject.Models.Dto.Request;

namespace MidProject.Repositories.Interfaces
{
    public interface IPost
    {
        Task<IEnumerable<Post>> GetAllPosts();
        Task<Post> GetPostById(int id);
        Task AddPost(PostDto postDto);
        Task UpdatePost(PostDto postDto);
        Task DeletePost(int id);
    }
}