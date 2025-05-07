using Application.CQRS.Users.Commands;
using Application.Responses;
using Domain.Entities;
using Infrastructure.Repositories;
using Infrastructure.Services.Audit;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Users.Handlers
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Response<string>>
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly IAuditLogger _auditLogger;
        private readonly IHttpContextAccessor _httpContext;

        public DeleteUserCommandHandler(IGenericRepository<User> userRepository, IAuditLogger auditLogger, IHttpContextAccessor httpContext)
        {
            _userRepository = userRepository;
            _auditLogger = auditLogger;
            _httpContext = httpContext;
        }

        public async Task<Response<string>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.Id);
            if (user == null)
                return Response<string>.Fail("User not found.", 404);

            _userRepository.Delete(user);
            await _userRepository.SaveChangesAsync();

            var userEmail = _httpContext.HttpContext?.User?.Identity?.Name ?? "System";
            await _auditLogger.LogAsync(
            userEmail,
            "Delete",
            "User",
            request.Id.ToString(),
            before: user
        );

            return Response<string>.SuccessResponse("User deleted successfully.");
        }
    }
}
