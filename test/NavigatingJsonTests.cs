using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace test
{
    public class NavigatingJsonTests
    {
        [Fact]
        public void ExtractDeepValue()
        {
            var jsonStr =
                "{'outer': { 'name': 'outer-name', 'inner': { 'name': 'inner-name', 'target': [ 'one', 'two', 'three' ] }}}"
                    .Replace('\'', '"');
            var root = JsonConvert.DeserializeObject<JObject>(jsonStr);
            Debug.Assert(root.ContainsKey("outer"), "Did not deserialize as expected");
            
            var outer = root["outer"];
            var inner = outer["inner"];
            var target = inner["target"];
            Debug.Assert(target is JArray, "target is not a JArray");
            
            var three = ((JArray) target)[2];
            Debug.Assert(three.Equals(new JValue("three")), "Cannot confirm presence of 'three'");
            
            var targetJsonStr = JsonConvert.SerializeObject(target);
            Debug.Assert(targetJsonStr == "['one','two','three']".Replace('\'', '"'));
        }
    }
}