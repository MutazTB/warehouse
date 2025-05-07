using Application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Warehouses.Commands
{
    public class DeleteWarehouseCommand : IRequest<Response<string>>
    {
        public int Id { get; set; }

        public DeleteWarehouseCommand(int id)
        {
            Id = id;
        }
    }
}
