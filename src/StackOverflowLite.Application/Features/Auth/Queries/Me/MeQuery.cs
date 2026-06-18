using MediatR;
using StackOverflowLite.Application.Features.Auth.DTOs;

namespace StackOverflowLite.Application.Features.Auth.Queries.Me;

public record MeQuery : IRequest<CurrentUserDto?>;
