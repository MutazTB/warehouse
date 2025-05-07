using Domain.Entities;
using Infrastructure.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.Services.Audit
{
    public class AuditLogger : IAuditLogger
    {
        private readonly AppDbContext _context;

        public AuditLogger(AppDbContext context)
        {
            _context = context;
        }

        public Task<IEnumerable<AuditLog>> GetAllLogs(int PageNumber, int PageSize)
        {
            throw new NotImplementedException();
        }

        public async Task LogAsync(string userEmail, string action, string entityName, string entityId, object before = null, object after = null)
        {
            var log = new AuditLog
            {
                UserEmail = userEmail,
                Action = action,
                EntityName = entityName,
                EntityId = entityId,
                BeforeValue = before != null ? JsonSerializer.Serialize(before) : null,
                AfterValue = after != null ? JsonSerializer.Serialize(after) : null
            };

            _context.AuditLogs.Add(log);
            await _context.SaveChangesAsync();
        }
    }
}
