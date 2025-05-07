using Application.CQRS.Users.Commands;
using Application.Responses;
using AutoMapper;
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
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Response<string>>
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly IMapper _mapper;
        private readonly IAuditLogger _auditLogger;
        private readonly IHttpContextAccessor _httpContext;

        public UpdateUserCommandHandler(IGenericRepository<User> userRepository, IMapper mapper, IAuditLogger auditLogger, IHttpContextAccessor httpContext)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _auditLogger = auditLogger;
            _httpContext = httpContext;
        }

        public async Task<Response<string>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.Id);
            if (user == null)
                return Response<string>.Fail("User not found.", 404);

            _mapper.Map(request.UserUpdateDto, user);

            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();

            var userEmail = _httpContext.HttpContext?.User?.Identity?.Name ?? "System";
            await _auditLogger.LogAsync(
            userEmail,
            "Update",
            "User",
            request.Id.ToString(),
            before: request,
            after: user
        );

            return Response<string>.SuccessResponse("User updated successfully.");
        }
    }
}
