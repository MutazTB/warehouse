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
    public class UpdateUserCommand : IRequest<Response<string>>
    {
        public int Id { get; set; }
        public UserUpdateDto UserUpdateDto { get; set; }

        public UpdateUserCommand(int id, UserUpdateDto userUpdateDto)
        {
            Id = id;
            UserUpdateDto = userUpdateDto;
        }
    }

}
