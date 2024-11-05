using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using MidProject.Data;
using MidProject.Models;
using MidProject.Models.Dto.Request;
using MidProject.Models.Dto.Request2;
using MidProject.Models.Dto.Response;
using MidProject.Repository.Interfaces;

namespace MidProject.Repository.Services
{
    public class ClientServices : IClient
    {
        private readonly MidprojectDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClientServices(MidprojectDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        private string GetAccountId()=> _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        // Session management
        public async Task<IEnumerable<Session>> GetClientSessionsAsync(int clientId)
        {
            return await _context.Sessions
                .Where(s => s.ClientId == clientId)
                .ToListAsync();
        }

        public async Task<Session> GetSessionByIdAsync(int sessionId)
        {
            return await _context.Sessions
                .FirstOrDefaultAsync(s => s.SessionId == sessionId);
        }

        public async Task<Session> StartSessionAsync(SessionDto sessionDto)
        {
            var session = new Session
            {
                ClientId = sessionDto.ClientId,
                ChargingStationId = sessionDto.ChargingStationId,
                StartTime = sessionDto.StartTime,
                EndTime = sessionDto.EndTime,
                EnergyConsumed = sessionDto.EnergyConsumed,
                Cost=sessionDto.Cost
            };

            await _context.Sessions.AddAsync(session);
            await _context.SaveChangesAsync();
            return session;
        }

        public async Task EndSessionAsync(int sessionId)
        {
            var session = await _context.Sessions.FindAsync(sessionId);
            if (session != null)
            {
                session.EndTime = DateTime.UtcNow;
                _context.Sessions.Update(session);
                await _context.SaveChangesAsync();
            }
            else
            {
                // Handle the case where the session is not found
                throw new KeyNotFoundException("Session not found.");
            }
        }

        // Payment transactions management
        public async Task<IEnumerable<PaymentTransaction>> GetClientPaymentsAsync(int sessionId)
        {
            return await _context.PaymentTransactions
                .Where(pt => pt.SessionId == sessionId)
                .ToListAsync();
        }

        public async Task<PaymentTransaction> AddPaymentAsync(PaymentTransactionDto paymentDto)
        {
            var payment = new PaymentTransaction
            {
                SessionId = paymentDto.SessionId,
                ClientId = paymentDto.ClientId,
                Amount = paymentDto.Amount,
                PaymentDate = paymentDto.PaymentDate,
                PaymentMethod = paymentDto.PaymentMethod,
                Status = paymentDto.Status
            };

            await _context.PaymentTransactions.AddAsync(payment);
            await _context.SaveChangesAsync();

            return payment;
        }

        public async Task RemovePaymentAsync(int paymentId)
        {
            var payment = await _context.PaymentTransactions.FindAsync(paymentId);
            if (payment != null)
            {
                _context.PaymentTransactions.Remove(payment);
                await _context.SaveChangesAsync();
            }
            else
            {
                // Handle the case where the payment is not found
                throw new KeyNotFoundException("Payment transaction not found.");
            }
        }

        // Favorites management
        // Retrieve all charging station favorites for a client with detailed information
        public async Task<IEnumerable<FavoriteChargingStationResponseDto>> GetClientChargingStationFavoritesAsync(int clientId)
        {
            return await _context.ChargingStationFavorites
                .Where(f => f.ClientId == clientId)
                .Include(f => f.ChargingStation) // Include the related ChargingStation entity
                .Select(f => new FavoriteChargingStationResponseDto
                {
                    FavoriteId = f.ChargingStationFavoriteId,
                    ChargingStationId = f.ChargingStationId,
                    ClientId = f.ClientId,
                    ChargingStationName = f.ChargingStation.Name, // Select only needed properties
                    StationLocation = f.ChargingStation.StationLocation,
                    HasParking = f.ChargingStation.HasParking,
                    Status = f.ChargingStation.Status,
                    PaymentMethod = f.ChargingStation.PaymentMethod
                })
                .ToListAsync();
        }

        // Retrieve all service info favorites for a client with detailed information
        public async Task<IEnumerable<FavoriteServiceInfoResponseDto>> GetClientServiceInfoFavoritesAsync(int clientId)
        {
            return await _context.ServiceInfoFavorites
                .Where(f => f.ClientId == clientId)
                .Include(f => f.ServiceInfo) // Include the related ServiceInfo entity
                .Select(f => new FavoriteServiceInfoResponseDto
                {
                    FavoriteId = f.ServiceInfoFavoriteId,
                    ServiceInfoId = f.ServiceInfoId,
                    ClientId = f.ClientId,
                    ServiceInfoName = f.ServiceInfo.Name, // Select only needed properties
                    Description = f.ServiceInfo.Description,
                    Contact = f.ServiceInfo.Contact,
                    Type = f.ServiceInfo.Type
                })
                .ToListAsync();
        }

        // Add a new charging station favorite
        public async Task<FavoriteChargingStationResponseDto> AddChargingStationFavoriteAsync(FavoriteChargingStationDto favoriteDto)
        {
            var favorite = new ChargingStationFavorite
            {
                ClientId = favoriteDto.ClientId,
                ChargingStationId = favoriteDto.ChargingStationId
            };

            _context.ChargingStationFavorites.Add(favorite);
            await _context.SaveChangesAsync();

            var chargingStation = await _context.ChargingStations
                .Where(cs => cs.ChargingStationId == favoriteDto.ChargingStationId)
                .Select(cs => new FavoriteChargingStationResponseDto
                {
                    FavoriteId = favorite.ChargingStationFavoriteId,
                    ChargingStationId = cs.ChargingStationId,
                    ClientId = favorite.ClientId,
                    ChargingStationName = cs.Name, // Select only needed properties
                    StationLocation = cs.StationLocation,
                    HasParking = cs.HasParking,
                    Status = cs.Status,
                    PaymentMethod = cs.PaymentMethod
                })
                .FirstOrDefaultAsync();

            return chargingStation;
        }

        // Add a new service info favorite
        public async Task<FavoriteServiceInfoResponseDto> AddServiceInfoFavoriteAsync(FavoriteServiceInfoDto favoriteDto)
        {
            var favorite = new ServiceInfoFavorite
            {
                ClientId = favoriteDto.ClientId,
                ServiceInfoId = favoriteDto.ServiceInfoId
            };

            _context.ServiceInfoFavorites.Add(favorite);
            await _context.SaveChangesAsync();

            var serviceInfo = await _context.ServiceInfos
                .Where(si => si.ServiceInfoId == favoriteDto.ServiceInfoId)
                .Select(si => new FavoriteServiceInfoResponseDto
                {
                    FavoriteId = favorite.ServiceInfoFavoriteId,
                    ServiceInfoId = si.ServiceInfoId,
                    ClientId = favorite.ClientId,
                    ServiceInfoName = si.Name, // Select only needed properties
                    Description = si.Description,
                    Contact = si.Contact,
                    Type = si.Type
                })
                .FirstOrDefaultAsync();

            return serviceInfo;
        }

        // Remove a charging station favorite
        public async Task RemoveChargingStationFavoriteAsync(int favoriteId)
        {
            var favorite = await _context.ChargingStationFavorites.FindAsync(favoriteId);
            if (favorite != null)
            {
                _context.ChargingStationFavorites.Remove(favorite);
                await _context.SaveChangesAsync();
            }
        }

        // Remove a service info favorite
        public async Task RemoveServiceInfoFavoriteAsync(int favoriteId)
        {
            var favorite = await _context.ServiceInfoFavorites.FindAsync(favoriteId);
            if (favorite != null)
            {
                _context.ServiceInfoFavorites.Remove(favorite);
                await _context.SaveChangesAsync();
            }
        }

        // Vehicle management
        public async Task<IEnumerable<VehicleResponseDto>> GetClientVehiclesAsync(string accountId)
        {
            try
            {
                // Fetch the client based on the accountId and include related Vehicles
                var client = await _context.Clients
                    .Include(c => c.Vehicles)
                    .FirstOrDefaultAsync(c => c.AccountId == accountId);

                if (client == null)
                {
                    throw new UnauthorizedAccessException("Client not found");
                }

                // Map the vehicles to a DTO
                var vehicleDtos = client.Vehicles.Select(v => new VehicleResponseDto
                {
                    VehicleId = v.VehicleId,
                    LicensePlate = v.LicensePlate,
                    Model = v.Model,
                    Year = v.Year,
                    BatteryCapacity = v.BatteryCapacity,
                    ElectricType = v.ElectricType,
                    ClientId = v.ClientId  // Optional: Remove if sensitive
                }).ToList();

                return vehicleDtos;
            }
            catch (Exception ex)
            {
                // Log the error
                throw new Exception("An error occurred while retrieving client vehicles.", ex);
            }
        }



        public async Task<Vehicle> GetVehicleByIdAsync(int vehicleId)
        {
            return await _context.Vehicles
                .FirstOrDefaultAsync(v => v.VehicleId == vehicleId);
        }

        public async Task<Vehicle> AddVehicleAsync(VehicleDto vehicleDto, string accountId)
        {
            // Retrieve the client using the accountId
            var client = await _context.Clients.FirstOrDefaultAsync(c => c.AccountId == accountId);

            if (client == null)
            {
                throw new UnauthorizedAccessException("Client not found.");
            }

            // Create the vehicle and assign the ClientId from the found client
            var vehicle = new Vehicle
            {
                LicensePlate = vehicleDto.LicensePlate,
                Model = vehicleDto.Model,
                Year = vehicleDto.Year,
                BatteryCapacity = vehicleDto.BatteryCapacity,
                ElectricType = vehicleDto.ElectricType,
                ClientId = client.ClientId  // Automatically assign ClientId
            };

            // Add vehicle to the database and save changes
            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();

            return vehicle;
        }

        public async Task RemoveVehicleAsync(int vehicleId)
        {
            var vehicle = await _context.Vehicles.FindAsync(vehicleId);
            if (vehicle != null)
            {
                _context.Vehicles.Remove(vehicle);
                await _context.SaveChangesAsync();
            }
        }

        //Booking
        // Retrieves all bookings for a specific client
        // Retrieves client bookings without cyclic references
        public async Task<IEnumerable<BookingDto>> GetClientBookingsAsync(int clientId)
        {
            return await _context.Bookings
                                 .Where(b => b.ClientId == clientId)
                                 .Select(b => new BookingDto
                                 {
                                     BookingId = b.BookingId,
                                     ClientId = b.ClientId,
                                     ChargingStationId = b.ChargingStationId,
                                     VehicleId = b.VehicleId,
                                     StartTime = b.StartTime,
                                     EndTime = b.EndTime,
                                     Status = b.Status,
                                     Cost = b.Cost
                                 })
                                 .ToListAsync();
        }

        // Retrieves a booking by its ID without cyclic references
        public async Task<BookingDto> GetBookingByIdAsync(int bookingId)
        {
            var booking = await _context.Bookings
                                        .Where(b => b.BookingId == bookingId)
                                        .Select(b => new BookingDto
                                        {
                                            BookingId = b.BookingId,
                                            ClientId = b.ClientId,
                                            ChargingStationId = b.ChargingStationId,
                                            VehicleId = b.VehicleId,
                                            StartTime = b.StartTime,
                                            EndTime = b.EndTime,
                                            Status = b.Status,
                                            Cost = b.Cost
                                        })
                                        .FirstOrDefaultAsync();

            if (booking == null)
            {
                throw new KeyNotFoundException("Booking not found.");
            }

            return booking;
        }


        // Adds a new booking using the provided DTO
        public async Task<Booking> AddBookingAsync(BookingDto bookingDto)
        {
            // Map the DTO to the Booking entity
            var booking = new Booking
            {
                ClientId = bookingDto.ClientId,
                ChargingStationId = bookingDto.ChargingStationId,
                VehicleId = bookingDto.VehicleId,
                StartTime = bookingDto.StartTime,
                EndTime = bookingDto.EndTime,
                Status = bookingDto.Status,  // Status is set from DTO; defaults to "Pending"
                Cost = bookingDto.Cost       // Cost is set from DTO; defaults to 0
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return booking;
        }

        // Removes a booking by its ID
        public async Task RemoveBookingAsync(int bookingId)
        {
            var booking = await _context.Bookings.FindAsync(bookingId);

            if (booking == null)
            {
                throw new KeyNotFoundException("Booking not found.");
            }

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
        }

        // Service request management
        public async Task<IEnumerable<ServiceRequest>> GetClientServiceRequestsAsync(int clientId)
        {
            return await _context.ServiceRequests
                .Where(sr => sr.ClientId == clientId)
                .ToListAsync();
        }

        public async Task<ServiceRequest> GetServiceRequestByIdAsync(int requestId)
        {
            return await _context.ServiceRequests
                .FirstOrDefaultAsync(sr => sr.ServiceRequestId == requestId);
        }

        public async Task<ServiceRequest> CreateServiceRequestAsync(ClientServiceRequestDto requestDto)
        {
            var serviceRequest = new ServiceRequest
            {
                ServiceInfoId = requestDto.ServiceInfoId,
                ClientId = requestDto.ClientId,
                ProviderId = requestDto.ProviderId,
                VehicleId = requestDto.VehicleId, // Add vehicle information to the request
                Status = requestDto.Status
            };

            await _context.ServiceRequests.AddAsync(serviceRequest);
            await _context.SaveChangesAsync();
            return serviceRequest;
        }

        public async Task DeleteServiceRequestAsync(int requestId)
        {
            var serviceRequest = await _context.ServiceRequests.FindAsync(requestId);
            if (serviceRequest != null)
            {
                _context.ServiceRequests.Remove(serviceRequest);
                await _context.SaveChangesAsync();
            }
        }


        // Feedback management
        public async Task<IEnumerable<Feedback>> GetClientFeedbacksAsync(int clientId)
        {
            return await _context.Feedbacks
                .Where(f => f.ClientId == clientId)
                .ToListAsync();
        }

        public async Task<Feedback> AddFeedbackAsync(FeedbackDto feedbackDto)
        {
            var feedback = new Feedback
            {
                ClientId = feedbackDto.ClientId,
                ServiceInfoId = feedbackDto.ServiceInfoId,
                Rating = feedbackDto.Rating,
                Comments = feedbackDto.Comments,
                Date = feedbackDto.Date
            };

            await _context.Feedbacks.AddAsync(feedback);
            await _context.SaveChangesAsync();
            return feedback;
        }

        public async Task RemoveFeedbackAsync(int feedbackId)
        {
            var feedback = await _context.Feedbacks.FindAsync(feedbackId);
            if (feedback != null)
            {
                _context.Feedbacks.Remove(feedback);
                await _context.SaveChangesAsync();
            }
        }

      

        // Notification management
        public async Task<IEnumerable<Notification>> GetClientNotificationsAsync()
        {
            var accountId = GetAccountId();
            var client = await _context.Clients.FirstOrDefaultAsync(c => c.AccountId == accountId);

            if (client == null)
            {
                throw new UnauthorizedAccessException("Client not found");
            }

            return await _context.Notifications
                .Where(n => n.ClientId == client.ClientId)
                .ToListAsync();
        }

        public async Task<Notification> GetNotificationByIdAsync(int notificationId)
        {
            var accountId = GetAccountId();
            var client = await _context.Clients.FirstOrDefaultAsync(c => c.AccountId == accountId);

            if (client == null)
            {
                throw new UnauthorizedAccessException("Client not found");
            }

            var notification = await _context.Notifications
                .FirstOrDefaultAsync(n => n.NotificationId == notificationId && n.ClientId == client.ClientId);

            if (notification == null)
            {
                throw new KeyNotFoundException("Notification not found or does not belong to the client");
            }

            return notification;
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
}