using MidProject.Data;
using MidProject.Models;
using MidProject.Models.Dto.Request;
using MidProject.Repositories.Interfaces;

namespace MidProject.Repositories.Services
{
    public class FeedbackServices : IFeedback
    {
        private readonly MidprojectDbContext _context;

        public FeedbackServices(MidprojectDbContext context) 
        {
            _context = context;
        }

        public Task AddFeedback(FeedbackDto feedbackDto)
        {
            throw new NotImplementedException();
        }

        public Task DeleteFeedback(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Feedback>> GetAllFeedbacks()
        {
            throw new NotImplementedException();
        }

        public Task<Feedback> GetFeedbackById(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateFeedback(FeedbackDto feedbackDto)
        {
            throw new NotImplementedException();
        }
    }
    
}
