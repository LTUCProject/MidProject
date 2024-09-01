using MidProject.Data;
using MidProject.Models;
using MidProject.Models.Dto.Request;
using MidProject.Repositories.Interfaces;

namespace MidProject.Repositories.Services
{
    public class CommentServices : IComment
    {
        private readonly MidprojectDbContext _context;

        public CommentServices(MidprojectDbContext context) 
        {
            _context = context;
        }

        public Task AddComment(CommentDto commentDto)
        {
            throw new NotImplementedException();
        }

        public Task DeleteComment(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Comment>> GetAllComments()
        {
            throw new NotImplementedException();
        }

        public Task<Comment> GetCommentById(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateComment(CommentDto commentDto)
        {
            throw new NotImplementedException();
        }
    }
}
