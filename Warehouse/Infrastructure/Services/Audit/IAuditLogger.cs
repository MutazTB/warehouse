using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.Audit
{
    public interface IAuditLogger
    {
        Task LogAsync(string userEmail, string action, string entityName, string entityId, object before = null, object after = null);
        Task<IEnumerable<AuditLog>> GetAllLogs(int PageNumber , int PageSize);
    }

}
