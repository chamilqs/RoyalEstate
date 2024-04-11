using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoyalState.Core.Application.DTOs.Property
{
    public class PropertyDTO
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string PropertyType { get; set; }
        public string SaleType { get; set; }
        public double Price { get; set; }
        public double Meters { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public string Description { get; set; }
        public int AgentId { get; set; }
        public string AgentFirstName { get; set; }
        public List<string>? Improvements { get; set; }
    }
}
