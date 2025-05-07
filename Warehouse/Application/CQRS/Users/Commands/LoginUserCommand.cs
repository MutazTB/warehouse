using Application.DTOs;
using Application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Users.Commands
{
    public class LoginUserCommand : IRequest<Response<string>>
    {
        public UserLoginDto LoginDto { get; set; }

        public LoginUserCommand(UserLoginDto dto)
        {
            LoginDto = dto;
        }
    }
}
