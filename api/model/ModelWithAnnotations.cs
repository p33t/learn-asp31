using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace api.model
{
    public class ModelWithAnnotations
    {
        [Required(AllowEmptyStrings = false)]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; }
        
        // prevents annotation checking. Need to use { get; set; }
        // public string Name;
    }
}