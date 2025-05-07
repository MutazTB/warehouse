using Application.CQRS.Users.Commands;
using Domain.Entities;
using MediatR;
using Infrastructure.Repositories;
using AutoMapper;
using Application.Responses;
using Infrastructure.Services.Audit;
using Microsoft.AspNetCore.Http;

namespace Application.CQRS.Users.Handlers
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Response<string>>
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly IMapper _mapper;

        private readonly IAuditLogger _auditLogger;
        private readonly IHttpContextAccessor _httpContext;

        public CreateUserCommandHandler(IGenericRepository<User> userRepository, IMapper mapper, IAuditLogger auditLogger, IHttpContextAccessor httpContext)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _auditLogger = auditLogger;
            _httpContext = httpContext;
        }

        public async Task<Response<string>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var dto = request.UserCreateDto;

            var exists = (await _userRepository.FindAsync(u => u.Email == dto.Email)).FirstOrDefault();
            if (exists != null)
                return Response<string>.Fail("Email already exists.");

            var user = _mapper.Map<User>(dto);
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            var userEmail = _httpContext.HttpContext?.User?.Identity?.Name ?? "System";
            await _auditLogger.LogAsync(
            userEmail,
            "Create",
            "Warehouse Item",
            user.Id.ToString(),
            after: user
        );

            return Response<string>.SuccessResponse("User created successfully.");
        }

    }
}
