using Microsoft.EntityFrameworkCore;
using MidProject.Models;
using MidProject.Repository.Interfaces;
using MidProject.Data;
using Microsoft.AspNetCore.Identity;
using MidProject.Models.Dto.Response;
using MidProject.Models.Dto.Request2;
using MidProject.Models.Dto.Request;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Security.Claims;

namespace MidProject.Repository.Services
{
    public class AdminServices : IAdmin
    {
        private readonly MidprojectDbContext _context;
        private readonly UserManager<Account> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IHttpContextAccessor _httpContextAccessor;



        public AdminServices(MidprojectDbContext context, UserManager<Account> userManager, RoleManager<IdentityRole> roleManager, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _httpContextAccessor = httpContextAccessor;
        }

        private string GetAccountId() => _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        // Start Client management ================================================================================================================
        public async Task<IEnumerable<ClientResponseDto>> GetAllClientsAsync()
        {
            var clients = await _context.Clients
                                        .Include(c => c.Account)
                                        .ToListAsync();

            var clientDtos = new List<ClientResponseDto>();

            foreach (var client in clients)
            {
                clientDtos.Add(new ClientResponseDto
                {
                    ClientId = client.ClientId,
                    Name = client.Name,
                    Email = client.Email,
                    AccountId = client.AccountId
                });
            }

            return clientDtos;
        }

        public async Task<ClientResponseDto> GetClientByIdAsync(int clientId)
        {
            var client = await _context.Clients
                                       .Include(c => c.Account)
                                       .FirstOrDefaultAsync(c => c.ClientId == clientId);

            if (client == null)
            {
                return null; // or throw an exception
            }

            return new ClientResponseDto
            {
                ClientId = client.ClientId,
                Name = client.Name,
                Email = client.Email,
                AccountId = client.AccountId
            };
        }

        public async Task DeleteClientAsync(int clientId)
        {
            var client = await _context.Clients.FindAsync(clientId);

            if (client == null)
            {
                return; // or throw an exception
            }

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
        }

        // End Client management ================================================================================================================

        // Start Provider management ================================================================================================================
        // Get all providers
        public async Task<IEnumerable<Provider>> GetAllProvidersAsync()
        {
            return await _context.Providers
                .AsNoTracking()
                .ToListAsync();
        }

        // Get provider by ID
        public async Task<Provider> GetProviderByIdAsync(int providerId)
        {
            return await _context.Providers
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.ProviderId == providerId);
        }

        // Delete provider by ID
        public async Task DeleteProviderAsync(int providerId)
        {
            var provider = await _context.Providers
                .FindAsync(providerId);

            if (provider != null)
            {
                _context.Providers.Remove(provider);
                await _context.SaveChangesAsync();
            }
        }
        // End Provider management ================================================================================================================


        // Start Client subscriptions management ================================================================================================

        // Get Client Subscription by Client Id
        public async Task<IEnumerable<ClientSubscriptionResponseDto>> GetClientSubscriptionByIdAsync(int clientId)
        {
            var clientSubscriptions = await _context.ClientSubscriptions
                .Where(cs => cs.ClientId == clientId)
                .Include(cs => cs.SubscriptionPlan)
                .ToListAsync();

            var subscriptionDtos = clientSubscriptions.Select(cs => new ClientSubscriptionResponseDto
            {
                ClientSubscriptionId = cs.ClientSubscriptionId,
                ClientId = cs.ClientId,
                SubscriptionPlanName = cs.SubscriptionPlan.Name,
                SubscriptionPlanDescription = cs.SubscriptionPlan.Description,
                Price = cs.SubscriptionPlan.Price,
                StartDate = cs.StartDate,
                EndDate = cs.EndDate,
                Status = cs.Status
            });

            return subscriptionDtos;
        }

        public async Task RemoveClientSubscriptionAsync(int clientSubscriptionId)
        {
            var clientSubscription = await _context.ClientSubscriptions.FindAsync(clientSubscriptionId);
            if (clientSubscription != null)
            {
                _context.ClientSubscriptions.Remove(clientSubscription);
                await _context.SaveChangesAsync();
            }
        }
        // End Client subscriptions management ================================================================================================

        // Start Subscription plan management ================================================================================================
        public async Task<IEnumerable<SubscriptionPlanResponseDto>> GetAllSubscriptionPlansAsync()
        {
            var subscriptionPlans = await _context.SubscriptionPlans.ToListAsync();

            return subscriptionPlans.Select(sp => new SubscriptionPlanResponseDto
            {
                SubscriptionPlanId = sp.SubscriptionPlanId,
                Name = sp.Name,
                Description = sp.Description,
                Price = sp.Price,
                DurationInDays = sp.DurationInDays
            }).ToList();
        }

        // Create a new subscription plan
        public async Task<SubscriptionPlan> CreateSubscriptionPlanAsync(SubscriptionPlanDto newSubscriptionPlanDto)
        {
            // Create a new SubscriptionPlan entity from the DTO
            var subscriptionPlan = new SubscriptionPlan
            {
                Name = newSubscriptionPlanDto.Name,
                Description = newSubscriptionPlanDto.Description,
                Price = newSubscriptionPlanDto.Price,
                DurationInDays = newSubscriptionPlanDto.DurationInDays
            };

            // Add to the context and save
            await _context.SubscriptionPlans.AddAsync(subscriptionPlan);
            await _context.SaveChangesAsync();

            return subscriptionPlan;


        }

        public async Task<SubscriptionPlanResponseDto> GetSubscriptionPlanByIdAsync(int subscriptionPlanId)
        {
            var subscriptionPlan = await _context.SubscriptionPlans.FindAsync(subscriptionPlanId);

            if (subscriptionPlan == null)
            {
                return null; // or throw an exception depending on your approach
            }

            return new SubscriptionPlanResponseDto
            {
                SubscriptionPlanId = subscriptionPlan.SubscriptionPlanId,
                Name = subscriptionPlan.Name,
                Description = subscriptionPlan.Description,
                Price = subscriptionPlan.Price,
                DurationInDays = subscriptionPlan.DurationInDays
            };
        }

        public async Task DeleteSubscriptionPlanAsync(int subscriptionPlanId)
        {
            var subscriptionPlan = await _context.SubscriptionPlans.FindAsync(subscriptionPlanId);

            if (subscriptionPlan != null)
            {
                _context.SubscriptionPlans.Remove(subscriptionPlan);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<ClientSubscriptionResponseDto>> GetAllClientSubscriptionsAsync()
        {
            try
            {
                // Fetch all client subscriptions with related Client and SubscriptionPlan
                var clientSubscriptions = await _context.ClientSubscriptions
                    .Include(cs => cs.Client)  // Include related client data
                    .Include(cs => cs.SubscriptionPlan)  // Include related subscription plan data
                    .ToListAsync();

                // Map the client subscriptions to a DTO
                var subscriptionDtos = clientSubscriptions.Select(cs => new ClientSubscriptionResponseDto
                {
                    ClientSubscriptionId = cs.ClientSubscriptionId,
                    ClientId = cs.ClientId,
                    ClientName = cs.Client.Name,  // Assuming Client has a 'Name' property
                    SubscriptionPlanName = cs.SubscriptionPlan.Name,
                    SubscriptionPlanDescription = cs.SubscriptionPlan.Description,
                    Price = cs.SubscriptionPlan.Price,
                    StartDate = cs.StartDate,
                    EndDate = cs.EndDate,
                    Status = cs.Status
                }).ToList();

                return subscriptionDtos;
            }
            catch (Exception ex)
            {
                // Log the error
                throw new Exception("An error occurred while retrieving client subscriptions.", ex);
            }
        }


        // End Subscription plan management ================================================================================================

        // Start Notifications management ================================================================================================
        public async Task<IEnumerable<NotificationResponseDto>> GetAdminNotificationsAsync()
        {
            // Get the admin's account ID
            var accountId = GetAccountId();
            var adminExists = await _context.Admins.AnyAsync(a => a.AccountId == accountId);

            if (!adminExists)
            {
                throw new UnauthorizedAccessException("Admin not found");
            }

            // Retrieve all notifications
            var notifications = await _context.Notifications
                .Select(n => new NotificationResponseDto
                {
                    NotificationId = n.NotificationId,
                    Title = n.Title,
                    Message = n.Message,
                    Date = n.Date,
                })
                .ToListAsync();

            return notifications;
        }



        // Add a notification for all clients
        public async Task AddNotificationForAllClientsAsync(NotificationDto notificationDto)
        {
            var clientIds = await _context.Clients.Select(c => c.ClientId).ToListAsync();

            var notifications = clientIds.Select(clientId => new Notification
            {
                ClientId = clientId,
                Title = notificationDto.Title,
                Message = notificationDto.Message,
                Date = notificationDto.Date,
            }).ToList();

            await _context.Notifications.AddRangeAsync(notifications);
            await _context.SaveChangesAsync();
        }

        // Delete a specific notification by its ID
        public async Task DeleteNotificationAsync(int notificationId)
        {
            var notification = await _context.Notifications.FindAsync(notificationId);
            if (notification != null)
            {
                _context.Notifications.Remove(notification);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException($"Notification with ID {notificationId} not found.");
            }
        }
        // End Notifications management ================================================================================================

        // Start Charging station management ================================================================================================
        public async Task<IEnumerable<ChargingStationResponseAdminDto>> GetAllChargingStationsAsync()
        {
            return await _context.ChargingStations
                .Include(cs => cs.Chargers)  // Include chargers
                .Include(cs => cs.Provider)  // Include provider
                .Select(cs => new ChargingStationResponseAdminDto
                {
                    ChargingStationId = cs.ChargingStationId,
                    StationLocation = cs.StationLocation,
                    Name = cs.Name,
                    HasParking = cs.HasParking,
                    Status = cs.Status,
                    PaymentMethod = cs.PaymentMethod,

                    // Location properties
                    Address = cs.Address,
                    Latitude = cs.Latitude,
                    Longitude = cs.Longitude,

                    Chargers = cs.Chargers.Select(c => new ChargerResponseDto
                    {
                        ChargerId = c.ChargerId,
                        Type = c.Type,
                        Power = c.Power,
                        Speed = c.Speed,
                        ChargingStationId = c.ChargingStationId
                    }).ToList(),
                    Provider = new ProviderResponseDto
                    {
                        ProviderId = cs.Provider.ProviderId,
                        Name = cs.Provider.Name,
                        Email = cs.Provider.Email,
                        Type = cs.Provider.Type
                    }
                })
                .ToListAsync();
        }

        public async Task<ChargingStationResponseAdminDto> GetChargingStationByIdAsync(int chargingStationId)
        {
            var chargingStation = await _context.ChargingStations
                .Include(cs => cs.Chargers)  // Include chargers
                .Include(cs => cs.Provider)  // Include provider
                .Where(cs => cs.ChargingStationId == chargingStationId)
                .Select(cs => new ChargingStationResponseAdminDto
                {
                    ChargingStationId = cs.ChargingStationId,
                    StationLocation = cs.StationLocation,
                    Name = cs.Name,
                    HasParking = cs.HasParking,
                    Status = cs.Status,
                    PaymentMethod = cs.PaymentMethod,

                    // Location properties
                    Address = cs.Address,
                    Latitude = cs.Latitude,
                    Longitude = cs.Longitude,

                    Chargers = cs.Chargers.Select(c => new ChargerResponseDto
                    {
                        ChargerId = c.ChargerId,
                        Type = c.Type,
                        Power = c.Power,
                        Speed = c.Speed,
                        ChargingStationId = c.ChargingStationId
                    }).ToList(),
                    Provider = new ProviderResponseDto
                    {
                        ProviderId = cs.Provider.ProviderId,
                        Name = cs.Provider.Name,
                        Email = cs.Provider.Email,
                        Type = cs.Provider.Type
                    }
                })
                .FirstOrDefaultAsync();

            return chargingStation;
        }




        public async Task DeleteChargingStationAsync(int chargingStationId)
        {
            var chargingStation = await _context.ChargingStations
                .FindAsync(chargingStationId);

            if (chargingStation != null)
            {
                _context.ChargingStations.Remove(chargingStation);
                await _context.SaveChangesAsync();
            }
        }
        // End Charging station management ================================================================================================

        // Start Vehicle management ================================================================================================
        public async Task<IEnumerable<Vehicle>> GetAllVehiclesAsync()
        {
            return await _context.Vehicles.ToListAsync();
        }

        public async Task<Vehicle> GetVehicleByIdAsync(int vehicleId)
        {
            return await _context.Vehicles
                .Include(v => v.Client) // Include related entities if needed
                .SingleOrDefaultAsync(v => v.VehicleId == vehicleId);
        }

        public async Task DeleteVehicleAsync(int vehicleId)
        {
            var vehicle = await _context.Vehicles.FindAsync(vehicleId);
            if (vehicle != null)
            {
                _context.Vehicles.Remove(vehicle);
                await _context.SaveChangesAsync();
            }
        }
        // End Vehicle management ================================================================================================


        // Start Location management ================================================================================================
        public async Task<IEnumerable<Location>> GetAllLocationsAsync()
        {
            return await _context.Locations
                .Include(l => l.ChargingStations) // Include related ChargingStations
                .ToListAsync();
        }

        public async Task<Location> GetLocationByIdAsync(int locationId)
        {
            return await _context.Locations
                .Include(l => l.ChargingStations) // Include related ChargingStations
                .SingleOrDefaultAsync(l => l.LocationId == locationId);
        }

        public async Task CreateLocationAsync(Location location)
        {
            _context.Locations.Add(location);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateLocationAsync(Location location)
        {
            _context.Entry(location).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteLocationAsync(int locationId)
        {
            var location = await _context.Locations.FindAsync(locationId);
            if (location != null)
            {
                _context.Locations.Remove(location);
                await _context.SaveChangesAsync();
            }
        }
        // End Location management ================================================================================================


        // Start Post management ================================================================================================
        public async Task<IEnumerable<Post>> GetAllPostsAsync()
        {
            return await _context.Posts
                .Include(p => p.Account)          // Include Account to fetch UserName for the Post
                .Include(p => p.Comments)         // Include Comments collection
                    .ThenInclude(c => c.Account)
                .ToListAsync();
        }

        public async Task<Post> GetPostByIdAsync(int postId)
        {
            return await _context.Posts
                .Include(p => p.Account) // Include the Account to access UserName
                .Include(p => p.Comments) // Include Comments
                    .ThenInclude(c => c.Account) // Include Account for each Comment to access UserName
                .FirstOrDefaultAsync(p => p.PostId == postId);
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
            var post = await _context.Posts.FindAsync(postId);

            if (post != null)
            {
                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();
            }
            // Optionally, handle cases where the post is not found
        }
        // End Post management ================================================================================================
        public async Task<IEnumerable<Comment>> GetAllCommentsAsync()
        {
            return await _context.Comments
                .Include(c => c.Post)    // Optionally include related Post
                .Include(c => c.Account) // Include the Account related to the Comment
                .ToListAsync();
        }

        public async Task<Comment> GetCommentByIdAsync(int commentId)
        {
            return await _context.Comments
                .Include(c => c.Post)    // Optionally include related Post
                .Include(c => c.Account) // Include the Account related to the Comment
                .FirstOrDefaultAsync(c => c.CommentId == commentId);
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
