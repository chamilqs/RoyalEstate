using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoyalState.Core.Application.DTOs.Agent
{
    public class AgentDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int NumberOfProperties { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
