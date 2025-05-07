using Application.DTOs;
using Application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Warehouses.Commands
{
    public class CreateWarehouseCommand : IRequest<Response<string>>
    {
        public WarehouseCreateDto Dto { get; set; }

        public CreateWarehouseCommand(WarehouseCreateDto dto)
        {
            Dto = dto;
        }
    }

}
