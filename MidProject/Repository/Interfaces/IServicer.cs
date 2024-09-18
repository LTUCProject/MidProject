using MidProject.Models;
using MidProject.Models.Dto.Request;
using MidProject.Models.Dto.Request2;
using MidProject.Models.Dto.Response;

namespace MidProject.Repository.Interfaces
{
    public interface IServicer
    {
        // Service management
        Task<ServiceInfoResponseDto> CreateServiceInfoAsync(ServiceInfoRequestDto serviceInfoDto);
        Task<ServiceInfoResponseDto> GetServiceInfoByIdAsync(int serviceInfoId);
        Task<IEnumerable<ServiceInfoResponseDto>> GetAllServiceInfosAsync();
        Task<bool> UpdateServiceInfoAsync(int serviceInfoId, ServiceInfoRequestDto serviceInfoDto);
        Task<bool> DeleteServiceInfoAsync(int serviceInfoId);

        // Service requests management
        //Task<ServiceRequestDto> CreateServiceRequestAsync(ServiceRequestDto serviceRequestDto);
        Task<ServiceRequestDto> GetServiceRequestByIdAsync(int serviceRequestId);
        Task<IEnumerable<ServiceRequestDto>> GetServiceRequestsByServiceInfoIdAsync(int serviceInfoId);
        Task<bool> UpdateServiceRequestStatusAsync(int serviceRequestId, string status);
        Task<bool> DeleteServiceRequestAsync(int serviceRequestId);


        // Notification management
        Task<NotificationResponseDto> CreateNotificationAsync(NotificationDto notificationDto);
        Task<NotificationResponseDto> GetNotificationByIdAsync(int notificationId);
        Task<IEnumerable<NotificationResponseDto>> GetNotificationsByClientIdAsync(int clientId);

        // Post management
        Task<IEnumerable<Post>> GetAllPostsAsync();
        Task<PostResponseDto> AddPostAsync(PostDto postDto);
        Task<PostResponseDto> UpdatePostByIdAsync(int postId, PostDto postDto);

        Task DeletePostAsync(int postId);

        // Comment management
        Task<IEnumerable<CommentResponseDto>> GetAllCommentsAsync();
        Task<CommentResponseDto> AddCommentAsync(CommentDto commentDto);
        Task<CommentResponseDto> UpdateCommentByIdAsync(int commentId, CommentDto commentDto);
        Task DeleteCommentAsync(int commentId);
    }
}


