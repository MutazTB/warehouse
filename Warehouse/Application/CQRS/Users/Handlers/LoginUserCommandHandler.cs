using Application.CQRS.Users.Commands;
using Application.Responses;
using Domain.Entities;
using Infrastructure.Repositories;
using Infrastructure.Services.Audit;
using Infrastructure.Services.Auth;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.CQRS.Users.Handlers
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Response<string>>
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly IAuthService _authService;
        private readonly IAuditLogger _auditLogger;

        public LoginUserCommandHandler(IGenericRepository<User> userRepository, IAuthService authService, IAuditLogger auditLogger)
        {
            _userRepository = userRepository;
            _authService = authService;
            _auditLogger = auditLogger;
        }

        public async Task<Response<string>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var dto = request.LoginDto;

            var user = (await _userRepository.FindAsync(u => u.Email == dto.Email)).FirstOrDefault();
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return Response<string>.Fail("Invalid credentials.", 401);

            if (!user.IsActive)
                return Response<string>.Fail("User account is inactive.", 403);

            var userEmail = user.FullName;
            await _auditLogger.LogAsync(
            userEmail,
            "Login",
            "User Login",
            user.Id.ToString(),
            after: user
        );
            var token = _authService.GenerateToken(user.Email, user.Role);
            return Response<string>.SuccessResponse(token);
        }
    }
}
