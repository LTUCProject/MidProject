using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MidProject.Models;
using MidProject.Models.Dto.Request;
using MidProject.Models.Dto.Request2;
using MidProject.Models.Dto.Response;
using MidProject.Repository.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MidProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "ServicerPolicy")]
    public class ServicerController : ControllerBase
    {
        private readonly IServicer _servicerService;

        public ServicerController(IServicer servicerService)
        {
            _servicerService = servicerService;
        }

        // ServiceInfo Endpoints

        [HttpPost("serviceinfo")]
        public async Task<IActionResult> CreateServiceInfo(ServiceInfoRequestDto serviceInfoDto)
        {
            var serviceInfoId = await _servicerService.CreateServiceInfoAsync(serviceInfoDto);
            return CreatedAtAction(nameof(GetServiceInfoById), new { id = serviceInfoId }, serviceInfoId);
        }

        [HttpGet("serviceinfo/{id}")]
        public async Task<IActionResult> GetServiceInfoById(int id)
        {
            var serviceInfo = await _servicerService.GetServiceInfoByIdAsync(id);
            if (serviceInfo == null)
            {
                return NotFound();
            }
            return Ok(serviceInfo);
        }

        [HttpGet("serviceinfos")]
        public async Task<IActionResult> GetAllServiceInfos()
        {
            var serviceInfos = await _servicerService.GetAllServiceInfosAsync();
            return Ok(serviceInfos);
        }

        [HttpPut("serviceinfo/{id}")]
        public async Task<IActionResult> UpdateServiceInfo(int id, ServiceInfoRequestDto serviceInfoDto)
        {
            var result = await _servicerService.UpdateServiceInfoAsync(id, serviceInfoDto);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("serviceinfo/{id}")]
        public async Task<IActionResult> DeleteServiceInfo(int id)
        {
            var result = await _servicerService.DeleteServiceInfoAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        // ServiceRequest Endpoints

        [HttpPost("servicerequest")]
        public async Task<IActionResult> CreateServiceRequest(ServiceRequestDto serviceRequestDto)
        {
            var serviceRequestId = await _servicerService.CreateServiceRequestAsync(serviceRequestDto);
            return CreatedAtAction(nameof(GetServiceRequestById), new { id = serviceRequestId }, serviceRequestId);
        }

        [HttpGet("servicerequest/{id}")]
        public async Task<IActionResult> GetServiceRequestById(int id)
        {
            var serviceRequest = await _servicerService.GetServiceRequestByIdAsync(id);
            if (serviceRequest == null)
            {
                return NotFound();
            }
            return Ok(serviceRequest);
        }

        [HttpGet("servicerequests/serviceinfo/{serviceInfoId}")]
        public async Task<IActionResult> GetServiceRequestsByServiceInfoId(int serviceInfoId)
        {
            var serviceRequests = await _servicerService.GetServiceRequestsByServiceInfoIdAsync(serviceInfoId);
            return Ok(serviceRequests);
        }

        [HttpPut("servicerequest/{id}/status")]
        public async Task<IActionResult> UpdateServiceRequestStatus(int id, [FromBody] string status)
        {
            var result = await _servicerService.UpdateServiceRequestStatusAsync(id, status);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("servicerequest/{id}")]
        public async Task<IActionResult> DeleteServiceRequest(int id)
        {
            var result = await _servicerService.DeleteServiceRequestAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        
        // Notification Management
        [HttpPost("ServicerNotifications")]
        public async Task<ActionResult<NotificationResponseDto>> CreateNotificationAsync([FromBody] NotificationDto notificationDto)
        {
            var notification = await _servicerService.CreateNotificationAsync(notificationDto);

            // Use CreatedAtRoute instead of CreatedAtAction
            return CreatedAtRoute(
                "ServicerGetNotificationById", // Name the route
                new { notificationId = notification.NotificationId },
                notification
            );
        }

        [HttpGet("ServicerNotifications/{notificationId}", Name = "ServicerGetNotificationById")]
        public async Task<ActionResult<NotificationResponseDto>> GetNotificationByIdAsync(int notificationId)
        {
            var notification = await _servicerService.GetNotificationByIdAsync(notificationId);
            if (notification == null)
            {
                return NotFound();
            }
            return Ok(notification);
        }



        [HttpGet("ServicerNotifications/client/{clientId}")]
        public async Task<ActionResult<IEnumerable<NotificationResponseDto>>> GetNotificationsByClientIdAsync(int clientId)
        {
            var notifications = await _servicerService.GetNotificationsByClientIdAsync(clientId);
            return Ok(notifications);
        }

        [HttpGet("posts")]
        public async Task<ActionResult<IEnumerable<PostResponseDto>>> GetAllPosts()
        {
            // Retrieve all posts from the database
            var posts = await _servicerService.GetAllPostsAsync();

            // Map posts to PostResponseDto
            var postDtos = posts.Select(p => new PostResponseDto
            {
                PostId = p.PostId,
                AccountId = p.AccountId, // Use AccountId
                UserName = p.Account != null ? p.Account.UserName : "Unknown", // Use UserName from Account
                Title = p.Title,
                Content = p.Content,
                Date = p.Date,
                Comments = p.Comments.Select(c => new CommentResponseDto
                {
                    CommentId = c.CommentId,
                    AccountId = c.AccountId, // Use AccountId
                    PostId = c.PostId,
                    Content = c.Content,
                    Date = c.Date,
                    UserName = c.Account != null ? c.Account.UserName : "Unknown" // Map UserName if needed
                }).ToList() // Ensure comments collection is materialized
            }).ToList(); // Materialize the posts collection with ToList

            return Ok(postDtos);
        }





        [HttpPost("posts")]
        public async Task<ActionResult<PostResponseDto>> AddPost([FromBody] PostDto postDto)
        {
            var postResponse = await _servicerService.AddPostAsync(postDto);
            return CreatedAtAction(nameof(GetAllPosts), new { postId = postResponse.PostId }, postResponse);
        }

        // UpdatePostById Controller Method
        [HttpPut("Posts/{id}")]
        public async Task<IActionResult> UpdatePostById(int id, [FromBody] PostDto postDto)
        {
            var updatedPost = await _servicerService.UpdatePostByIdAsync(id, postDto);

            if (updatedPost == null)
            {
                return NotFound(); // Handle case where post is not found
            }

            return Ok(updatedPost);
        }


        [HttpDelete("posts/{postId}")]
        public async Task<IActionResult> DeletePost(int postId)
        {
            await _servicerService.DeletePostAsync(postId);
            return NoContent();
        }

        // Comment management
        [HttpGet("comments")]
        public async Task<ActionResult<IEnumerable<CommentResponseDto>>> GetAllComments()
        {
            var commentDtos = await _servicerService.GetAllCommentsAsync();
            return Ok(commentDtos);
        }



        [HttpPost("comments")]
        public async Task<ActionResult<CommentResponseDto>> AddComment([FromBody] CommentDto commentDto)
        {
            // Call the service method to add the comment and get the response DTO
            CommentResponseDto commentResponseDto = await _servicerService.AddCommentAsync(commentDto);

            // Return the response DTO with UserName populated
            return Ok(commentResponseDto);
        }


        [HttpPut("Comments/{id}")]
        public async Task<IActionResult> UpdateCommentById(int id, [FromBody] CommentDto commentDto)
        {
            var updatedComment = await _servicerService.UpdateCommentByIdAsync(id, commentDto);

            if (updatedComment == null)
            {
                return NotFound(); // Handle case where comment is not found
            }

            return Ok(updatedComment);
        }


        [HttpDelete("comments/{commentId}")]
        public async Task<IActionResult> DeleteComment(int commentId)
        {
            await _servicerService.DeleteCommentAsync(commentId);
            return NoContent();
        }

    }
}

//// Get Vehicles by ServiceInfoId
//[HttpGet("services/{serviceId}/vehicles")]
//public async Task<ActionResult<IEnumerable<VehicleDtoResponse>>> GetVehiclesAsync(int serviceId)
//{
//    try
//    {
//        var vehicles = await _servicerService.GetVehiclesAsync(serviceId);
//        return Ok(vehicles);
//    }
//    catch (Exception ex)
//    {
//        // Log the exception
//        Console.WriteLine($"Error fetching vehicles: {ex.Message}");
//        return StatusCode(500, "Internal server error");
//    }
//}

//// Get Vehicle by ID
//[HttpGet("vehicles/{vehicleId}")]
//public async Task<ActionResult<VehicleDtoResponse>> GetVehicleByIdAsync(int vehicleId)
//{
//    try
//    {
//        var vehicle = await _servicerService.GetVehicleByIdAsync(vehicleId);
//        if (vehicle == null)
//        {
//            return NotFound();
//        }
//        return Ok(vehicle);
//    }
//    catch (Exception ex)
//    {
//        // Log the exception
//        Console.WriteLine($"Error fetching vehicle by ID: {ex.Message}");
//        return StatusCode(500, "Internal server error");
//    }
//}

//// Add Vehicle
//[HttpPost("vehicles")]
//public async Task<IActionResult> AddVehicle([FromBody] VehicleDto vehicleDto)
//{
//    try
//    {
//        await _servicerService.AddVehicleAsync(vehicleDto);
//        //  return CreatedAtAction(nameof(GetVehicleByIdAsync), new { vehicleId = vehicleDto.VehicleId }, vehicleDto);

//        var x = new Vehicle
//        {
//            LicensePlate= vehicleDto.LicensePlate,

//        };
//        return Ok(x);
//    }
//    catch (Exception ex)
//    {
//        // Log the exception
//        Console.WriteLine($"Error adding vehicle: {ex.Message}");
//        return StatusCode(500, "Internal server error");
//    }
//}

//// Remove Vehicle
//[HttpDelete("vehicles/{vehicleId}")]
//public async Task<IActionResult> RemoveVehicle(int vehicleId)
//{
//    try
//    {
//        await _servicerService.RemoveVehicleAsync(vehicleId);
//        return NoContent();
//    }
//    catch (Exception ex)
//    {
//        // Log the exception
//        Console.WriteLine($"Error removing vehicle: {ex.Message}");
//        return StatusCode(500, "Internal server error");
//    }
//}

//// Get Feedbacks by ServiceInfoId
//[HttpGet("services/{serviceId}/feedbacks")]
//public async Task<ActionResult<IEnumerable<FeedbackDtoResponse>>> GetFeedbacksAsync(int serviceId)
//{
//    try
//    {
//        var feedbacks = await _servicerService.GetFeedbacksAsync(serviceId);
//        return Ok(feedbacks);
//    }
//    catch (Exception ex)
//    {
//        // Log the exception
//        Console.WriteLine($"Error fetching feedbacks: {ex.Message}");
//        return StatusCode(500, "Internal server error");
//    }
//}