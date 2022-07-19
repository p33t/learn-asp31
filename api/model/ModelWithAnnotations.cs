using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace api.model
{
    public class ModelWithAnnotations
    {
        // this only works for 'null'
        [JsonProperty(Required = Required.Always)]

        // These don't seem to do anything
        // [Required(AllowEmptyStrings = false)]
        // [StringLength(100, MinimumLength = 3)]
        // [MinLength(3)]
        public string Name;
    }
}