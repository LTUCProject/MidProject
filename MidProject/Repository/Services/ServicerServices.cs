using MidProject.Models.Dto.Request2;
using MidProject.Models.Dto.Response;
using MidProject.Models;
using MidProject.Repository.Interfaces;
using MidProject.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using MidProject.Models.Dto.Request;

public class ServicerService : IServicer
{
    private readonly MidprojectDbContext _context;

    public ServicerService(MidprojectDbContext context)
    {
        _context = context;
    }

    // Service management

    public async Task<ServiceInfoResponseDto> CreateServiceInfoAsync(ServiceInfoRequestDto serviceInfoDto)
    {
        var serviceInfo = new ServiceInfo
        {
            Name = serviceInfoDto.Name,
            Description = serviceInfoDto.Description,
            Contact = serviceInfoDto.Contact,
            Type = serviceInfoDto.Type,
            ProviderId = serviceInfoDto.ProviderId,
        };

        _context.ServiceInfos.Add(serviceInfo);
        await _context.SaveChangesAsync();

        return new ServiceInfoResponseDto
        {
            ServiceInfoId = serviceInfo.ServiceInfoId,
            Name = serviceInfo.Name,
            Description = serviceInfo.Description,
            Contact = serviceInfo.Contact,
            Type = serviceInfo.Type,

        };
    }

    public async Task<ServiceInfoResponseDto> GetServiceInfoByIdAsync(int serviceInfoId)
    {
        var serviceInfo = await _context.ServiceInfos
            .Include(s => s.Provider)
            .FirstOrDefaultAsync(s => s.ServiceInfoId == serviceInfoId);

        if (serviceInfo == null)
        {
            return null;
        }

        return new ServiceInfoResponseDto
        {
            ServiceInfoId = serviceInfo.ServiceInfoId,
            Name = serviceInfo.Name,
            Description = serviceInfo.Description,
            Contact = serviceInfo.Contact,
            Type = serviceInfo.Type,

        };
    }

    public async Task<IEnumerable<ServiceInfoResponseDto>> GetAllServiceInfosAsync()
    {
        return await _context.ServiceInfos
            .Include(s => s.Provider)
            .Select(serviceInfo => new ServiceInfoResponseDto
            {
                ServiceInfoId = serviceInfo.ServiceInfoId,
                Name = serviceInfo.Name,
                Description = serviceInfo.Description,
                Contact = serviceInfo.Contact,
                Type = serviceInfo.Type,



            }).ToListAsync();
    }

    public async Task<bool> UpdateServiceInfoAsync(int serviceInfoId, ServiceInfoRequestDto serviceInfoDto)
    {
        var serviceInfo = await _context.ServiceInfos.FindAsync(serviceInfoId);

        if (serviceInfo == null)
        {
            return false;
        }

        serviceInfo.Name = serviceInfoDto.Name;
        serviceInfo.Description = serviceInfoDto.Description;
        serviceInfo.Contact = serviceInfoDto.Contact;
        serviceInfo.Type = serviceInfoDto.Type;
        serviceInfo.ProviderId = serviceInfoDto.ProviderId;

        _context.ServiceInfos.Update(serviceInfo);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteServiceInfoAsync(int serviceInfoId)
    {
        var serviceInfo = await _context.ServiceInfos.FindAsync(serviceInfoId);

        if (serviceInfo == null)
        {
            return false;
        }

        _context.ServiceInfos.Remove(serviceInfo);
        await _context.SaveChangesAsync();

        return true;
    }

    // Service requests management

    //public async Task<ServiceRequestDto> CreateServiceRequestAsync(ServiceRequestDto serviceRequestDto)
    //{
    //    var serviceRequest = new ServiceRequest
    //    {
    //        ServiceInfoId = serviceRequestDto.ServiceInfoId,
    //        ClientId = serviceRequestDto.ClientId,
    //        ProviderId = serviceRequestDto.ProviderId,
    //        Status = serviceRequestDto.Status
    //    };

    //    _context.ServiceRequests.Add(serviceRequest);
    //    await _context.SaveChangesAsync();

    //    return serviceRequestDto;
    //}

    public async Task<ServiceRequestDto> GetServiceRequestByIdAsync(int serviceRequestId)
    {
        var serviceRequest = await _context.ServiceRequests
            .Include(sr => sr.ServiceInfo)
            .Include(sr => sr.Client)
            .Include(sr => sr.Provider)
            .FirstOrDefaultAsync(sr => sr.ServiceRequestId == serviceRequestId);

        if (serviceRequest == null)
        {
            return null;
        }

        return new ServiceRequestDto
        {
            ServiceRequestId = serviceRequest.ServiceRequestId,
            ServiceInfoId = serviceRequest.ServiceInfoId,
            ClientId = serviceRequest.ClientId,
            ProviderId = serviceRequest.ProviderId,
            Status = serviceRequest.Status,
            ServiceInfo = new ServiceInfoResponseDto
            {
                ServiceInfoId = serviceRequest.ServiceInfo.ServiceInfoId,
                Name = serviceRequest.ServiceInfo.Name,
                Description = serviceRequest.ServiceInfo.Description,
                Contact = serviceRequest.ServiceInfo.Contact,
                Type = serviceRequest.ServiceInfo.Type
            },
            Client = new ClientDto
            {
                ClientId = serviceRequest.Client.ClientId,
                Name = serviceRequest.Client.Name,
                Email = serviceRequest.Client.Email,

            }
        };
    }

    public async Task<IEnumerable<ServiceRequestDto>> GetServiceRequestsByServiceInfoIdAsync(int serviceInfoId)
    {
        return await _context.ServiceRequests
            .Where(sr => sr.ServiceInfoId == serviceInfoId)
            .Include(sr => sr.ServiceInfo)
            .Include(sr => sr.Client)
            .Include(sr => sr.Provider)
            .Include(sr => sr.Vehicle) // Ensure Vehicle is included
            .Select(serviceRequest => new ServiceRequestDto
            {
                ServiceRequestId = serviceRequest.ServiceRequestId,
                ServiceInfoId = serviceRequest.ServiceInfoId,
                ClientId = serviceRequest.ClientId,
                ProviderId = serviceRequest.ProviderId,
                Status = serviceRequest.Status,

                // Mapping ServiceInfo
                ServiceInfo = new ServiceInfoResponseDto
                {
                    ServiceInfoId = serviceRequest.ServiceInfo.ServiceInfoId,
                    Name = serviceRequest.ServiceInfo.Name,
                    Description = serviceRequest.ServiceInfo.Description,
                    Contact = serviceRequest.ServiceInfo.Contact,
                    Type = serviceRequest.ServiceInfo.Type
                },

                // Mapping Client
                Client = new ClientDto
                {
                    ClientId = serviceRequest.Client.ClientId,
                    Name = serviceRequest.Client.Name,
                    Email = serviceRequest.Client.Email,
                },

                // Mapping Provider
                Provider = new ProviderDto
                {
                    ProviderId = serviceRequest.Provider.ProviderId,
                    Name = serviceRequest.Provider.Name,
                },

                // Mapping Vehicle
                Vehicle = new VehicleDto
                {
                    VehicleId = serviceRequest.Vehicle.VehicleId,
                    LicensePlate = serviceRequest.Vehicle.LicensePlate,
                    Model = serviceRequest.Vehicle.Model,
                    Year = serviceRequest.Vehicle.Year,
                    BatteryCapacity = serviceRequest.Vehicle.BatteryCapacity,
                    ElectricType = serviceRequest.Vehicle.ElectricType
                }
            })
            .ToListAsync();
    }



    public async Task<bool> UpdateServiceRequestStatusAsync(int serviceRequestId, string status)
    {
        var serviceRequest = await _context.ServiceRequests.FindAsync(serviceRequestId);

        if (serviceRequest == null)
        {
            return false;
        }

        serviceRequest.Status = status;
        _context.ServiceRequests.Update(serviceRequest);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteServiceRequestAsync(int serviceRequestId)
    {
        var serviceRequest = await _context.ServiceRequests.FindAsync(serviceRequestId);

        if (serviceRequest == null)
        {
            return false;
        }

        _context.ServiceRequests.Remove(serviceRequest);
        await _context.SaveChangesAsync();

        return true;
    }

    //Notification
    public async Task<NotificationResponseDto> CreateNotificationAsync(NotificationDto notificationDto)
    {
        var notification = new Notification
        {
            ClientId = notificationDto.ClientId, // Ensure this is correct and matches the Client entity type
            Title = notificationDto.Title,
            Message = notificationDto.Message,
            Date = notificationDto.Date
        };

        await _context.Notifications.AddAsync(notification); // Add the notification to the context
        await _context.SaveChangesAsync(); // Save changes to the database

        // Return the created notification's details
        return new NotificationResponseDto
        {
            NotificationId = notification.NotificationId, // ID generated by the database
            ClientId = notification.ClientId,
            Title = notification.Title,
            Message = notification.Message,
            Date = notification.Date
        };
    }


    public async Task<NotificationResponseDto> GetNotificationByIdAsync(int notificationId)
    {
        var notification = await _context.Notifications
            .FirstOrDefaultAsync(n => n.NotificationId == notificationId);

        if (notification == null)
        {
            return null;
        }

        return new NotificationResponseDto
        {
            NotificationId = notification.NotificationId,
            ClientId = notification.ClientId,
            Title = notification.Title,
            Message = notification.Message,
            Date = notification.Date
        };
    }

    public async Task<IEnumerable<NotificationResponseDto>> GetNotificationsByClientIdAsync(int clientId)
    {
        var notifications = await _context.Notifications
            .Where(n => n.ClientId == clientId)
            .ToListAsync();

        return notifications.Select(n => new NotificationResponseDto
        {
            NotificationId = n.NotificationId,
            ClientId = n.ClientId,
            Title = n.Title,
            Message = n.Message,
            Date = n.Date
        });
    }


    public async Task<IEnumerable<Post>> GetAllPostsAsync()
    {
        return await _context.Posts
            .Include(p => p.Account)  // Include Account to fetch UserName
            .Include(p => p.Comments) // Include Comments if needed
            .ToListAsync();
    }


    public async Task<PostResponseDto> AddPostAsync(PostDto postDto)
    {
        // Create a new Post instance from the provided PostDto
        var post = new Post
        {
            AccountId = postDto.AccountId, // Set AccountId instead of ClientId
            Title = postDto.Title,
            Content = postDto.Content,
            Date = postDto.Date
        };

        // Add the post to the context and save changes
        await _context.Posts.AddAsync(post);
        await _context.SaveChangesAsync();

        // Fetch the user information to populate AccountId and UserName in PostResponseDto
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == post.AccountId); // Fetch the user by Id

        // Return the PostResponseDto
        return new PostResponseDto
        {
            PostId = post.PostId,
            AccountId = post.AccountId, // Use AccountId
            UserName = user != null ? user.UserName : "Unknown", // Use UserName from IdentityUser
            Title = post.Title,
            Content = post.Content,
            Date = post.Date,
            Comments = new List<CommentResponseDto>() // Initialize empty list
        };
    }


    public async Task<PostResponseDto> UpdatePostByIdAsync(int postId, PostDto postDto)
    {
        var post = await _context.Posts
            .Include(p => p.Comments) // Include Comments to avoid null references
            .FirstOrDefaultAsync(p => p.PostId == postId);

        if (post == null)
        {
            return null; // Handle post not found
        }

        // Update post fields
        post.Title = postDto.Title;
        post.Content = postDto.Content;
        post.Date = postDto.Date;

        await _context.SaveChangesAsync();

        // Fetch updated UserName from Account
        var userName = await _context.Users
            .Where(u => u.Id == post.AccountId)
            .Select(u => u.UserName)
            .FirstOrDefaultAsync();

        return new PostResponseDto
        {
            PostId = post.PostId,
            AccountId = post.AccountId,
            UserName = userName ?? "Unknown",
            Title = post.Title,
            Content = post.Content,
            Date = post.Date,
            Comments = post.Comments?.Select(comment => new CommentResponseDto
            {
                CommentId = comment.CommentId,
                AccountId = comment.AccountId,
                PostId = comment.PostId,
                Content = comment.Content,
                Date = comment.Date,
                UserName = comment.Account != null ? comment.Account.UserName : "Unknown"
            }).ToList() ?? new List<CommentResponseDto>() // Handle null Comments collection
        };
    }


    public async Task DeletePostAsync(int postId)
    {
        var post = await _context.Posts
            .Include(p => p.Comments) // Include comments to handle related data
            .FirstOrDefaultAsync(p => p.PostId == postId);

        if (post != null)
        {
            _context.Comments.RemoveRange(post.Comments); // Remove related comments
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<CommentResponseDto>> GetAllCommentsAsync()
    {
        return await _context.Comments
            .Select(c => new CommentResponseDto
            {
                CommentId = c.CommentId,
                AccountId = c.AccountId,
                PostId = c.PostId,
                Content = c.Content,
                Date = c.Date,
                UserName = _context.Users
                    .Where(u => u.Id == c.AccountId)
                    .Select(u => u.UserName)
                    .FirstOrDefault()
            })
            .ToListAsync();
    }


    public async Task<CommentResponseDto> AddCommentAsync(CommentDto commentDto)
    {
        // Create and save the comment
        var comment = new Comment
        {
            AccountId = commentDto.AccountId,
            PostId = commentDto.PostId,
            Content = commentDto.Content,
            Date = commentDto.Date
        };

        await _context.Comments.AddAsync(comment);
        await _context.SaveChangesAsync();

        // Fetch the UserName from the AccountId
        var userName = await _context.Users
            .Where(u => u.Id == comment.AccountId)
            .Select(u => u.UserName) // Fetch UserName directly
            .FirstOrDefaultAsync();

        // Return the CommentResponseDto with UserName
        return new CommentResponseDto
        {
            CommentId = comment.CommentId,
            AccountId = comment.AccountId,
            PostId = comment.PostId,
            Content = comment.Content,
            Date = comment.Date,
            UserName = userName ?? "Unknown" // Use UserName or "Unknown" if null
        };
    }


    public async Task<CommentResponseDto> UpdateCommentByIdAsync(int commentId, CommentDto commentDto)
    {
        var comment = await _context.Comments.FindAsync(commentId);

        if (comment == null)
        {
            return null; // Handle comment not found
        }

        // Update comment fields
        comment.Content = commentDto.Content;
        comment.Date = commentDto.Date;

        await _context.SaveChangesAsync();

        // Fetch updated UserName from Account
        var userName = await _context.Users
            .Where(u => u.Id == comment.AccountId)
            .Select(u => u.UserName)
            .FirstOrDefaultAsync();

        return new CommentResponseDto
        {
            CommentId = comment.CommentId,
            AccountId = comment.AccountId,
            PostId = comment.PostId,
            Content = comment.Content,
            Date = comment.Date,
            UserName = userName ?? "Unknown"
        };
    }

    public async Task DeleteCommentAsync(int commentId)
    {
        var comment = await _context.Comments.FindAsync(commentId);
        if (comment != null)
        {
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
        }
    }

}



//// Feedback management
//public async Task<IEnumerable<FeedbackDtoResponse>> GetFeedbacksAsync(int serviceId)
//{
//    try
//    {
//        var feedbacks = await _context.Feedbacks
//            .Where(f => f.ServiceInfoId == serviceId)
//            .ToListAsync();

//        return feedbacks.Select(f => new FeedbackDtoResponse
//        {
//            FeedbackId = f.FeedbackId,
//            ClientId = f.ClientId,
//            ServiceInfoId = f.ServiceInfoId,
//            Rating = f.Rating,
//            Comments = f.Comments,
//            Date = f.Date
//        });
//    }
//    catch (Exception ex)
//    {
//        // Log the exception
//        Console.WriteLine($"Error fetching feedbacks: {ex.Message}");
//        return Enumerable.Empty<FeedbackDtoResponse>();
//    }
//}