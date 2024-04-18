using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel;

namespace RoyalState.Core.Application.Features.Properties.Queries.GetPropertyByCode
{
    /// <summary>
    /// Parameters to filter properties by code
    /// </summary>  
    public class GetPropertyByCodeParameter
    {
        [SwaggerParameter(Description = "Insert the code of the property you wish to get.")]
        [DefaultValue("1")]
        public string? Code { get; set; }
    }
}
