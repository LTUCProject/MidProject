using MidProject.Data;
using MidProject.Models;
using MidProject.Repositories.Interfaces;

namespace MidProject.Repositories.Services
{
    public class FeedbackServices : Repository<Feedback>, IFeedback
    {
        private readonly MidprojectDbContext _context;

        public FeedbackServices(MidprojectDbContext context) : base(context)
        {
            _context = context;
        }

    }
    
}
