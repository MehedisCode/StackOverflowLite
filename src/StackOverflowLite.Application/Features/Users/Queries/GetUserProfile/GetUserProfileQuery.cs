using MediatR;
using StackOverflowLite.Application.Features.Users.DTOs;

namespace StackOverflowLite.Application.Features.Users.Queries.GetUserProfile;

public record GetUserProfileQuery : IRequest<UserProfileDto?>;
