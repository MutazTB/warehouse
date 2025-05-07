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
    public class CreateUserCommand : IRequest<Response<string>>
    {
        public UserCreateDto UserCreateDto { get; set; }

        public CreateUserCommand(UserCreateDto userCreateDto)
        {
            UserCreateDto = userCreateDto;
        }
    }

}
