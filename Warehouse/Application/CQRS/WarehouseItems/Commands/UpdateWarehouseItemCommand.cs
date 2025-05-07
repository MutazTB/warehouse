using Application.DTOs;
using Application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.WarehouseItems.Commands
{
    public class UpdateWarehouseItemCommand : IRequest<Response<string>>
    {
        public int Id { get; set; }
        public WarehouseItemCreateDto Dto { get; set; }

        public UpdateWarehouseItemCommand(int id, WarehouseItemCreateDto dto)
        {
            Id = id;
            Dto = dto;
        }
    }
}
