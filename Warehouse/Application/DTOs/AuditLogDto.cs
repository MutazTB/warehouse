using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class AuditLogDto
    {
        public int Id { get; set; }
        public string UserEmail { get; set; }
        public string Action { get; set; }
        public string EntityName { get; set; }
        public string EntityId { get; set; }
        public DateTime Timestamp { get; set; }
        public string BeforeValue { get; set; }
        public string AfterValue { get; set; }
    }
}
