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
    public class UpdateWarehouseCommand : IRequest<Response<string>>
    {
        public int Id { get; set; }
        public WarehouseCreateDto Dto { get; set; }

        public UpdateWarehouseCommand(int id, WarehouseCreateDto dto)
        {
            Id = id;
            Dto = dto;
        }
    }
}
