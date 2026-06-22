using MediatR;
using StackOverflowLite.Application.Features.Users.DTOs;

namespace StackOverflowLite.Application.Features.Users.Queries.GetUserStats;

public record GetUserStatsQuery(Guid UserId) : IRequest<UserStatsDto?>;
