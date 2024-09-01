using MidProject.Data;
using MidProject.Models;
using MidProject.Models.Dto.Request;
using MidProject.Repositories.Interfaces;

namespace MidProject.Repositories.Services
{
    public class PostServices : IPost
    {
        private readonly MidprojectDbContext _context;

        public PostServices(MidprojectDbContext context) 
        {
            _context = context;
        }

        public Task AddPost(PostDto postDto)
        {
            throw new NotImplementedException();
        }

        public Task DeletePost(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Post>> GetAllPosts()
        {
            throw new NotImplementedException();
        }

        public Task<Post> GetPostById(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdatePost(PostDto postDto)
        {
            throw new NotImplementedException();
        }
    }
}
