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
    public class CreateWarehouseItemCommand : IRequest<Response<string>>
    {
        public WarehouseItemCreateDto Dto { get; set; }
        public CreateWarehouseItemCommand(WarehouseItemCreateDto dto) => Dto = dto;
    }
}
