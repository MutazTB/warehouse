using Application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.WarehouseItems.Commands
{
    public class DeleteWarehouseItemCommand : IRequest<Response<string>>
    {
        public int Id { get; set; }

        public DeleteWarehouseItemCommand(int id)
        {
            Id = id;
        }
    }
}
