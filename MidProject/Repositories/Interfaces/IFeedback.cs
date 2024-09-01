using MidProject.Models;
using MidProject.Models.Dto.Request;

namespace MidProject.Repositories.Interfaces
{
    public interface IFeedback
    {
        Task<IEnumerable<Feedback>> GetAllFeedbacks();
        Task<Feedback> GetFeedbackById(int id);
        Task AddFeedback(FeedbackDto feedbackDto);
        Task UpdateFeedback(FeedbackDto feedbackDto);
        Task DeleteFeedback(int id);
    }
}