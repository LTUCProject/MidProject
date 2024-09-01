using MidProject.Data;
using MidProject.Models;
using MidProject.Repositories.Interfaces;

namespace MidProject.Repositories.Services
{
    public class CommentServices : Repository<Comment>, IComment
    {
        private readonly MidprojectDbContext _context;

        public CommentServices(MidprojectDbContext context) : base(context)
        {
            _context = context;
        }

    }
}
