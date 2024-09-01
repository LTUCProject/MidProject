using MidProject.Models;
using MidProject.Models.Dto.Request;

namespace MidProject.Repositories.Interfaces
{
    public interface IComment
    {
        Task<IEnumerable<Comment>> GetAllComments();
        Task<Comment> GetCommentById(int id);
        Task AddComment(CommentDto commentDto);
        Task UpdateComment(CommentDto commentDto);
        Task DeleteComment(int id);
    }
}