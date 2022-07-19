using System;
using System.Threading.Tasks;
using api.model;
using Microsoft.AspNetCore.Mvc;

namespace api
{
    [Route("/")]
    public class Controller1 : Controller
    {
        // GET
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Hello from 1");
        }

        [Route("date-time-query")]
        [HttpGet]
        public IActionResult GetDateTimeQuery([FromQuery] DateTime arg)
        {
            /*
            For built-in JSON serialization AND NewtonsoftJson AND NewtonsoftJson with DateTimeZoneHandling.UTC:
              10:00-5 >> 07:00-8 
              10:00Z  >> 02:00-8
             */
            return Ok(arg.ToString("O"));
        }

        [Route("date-time-offset-query")]
        [HttpGet]
        public IActionResult GetDateTimeOffsetQuery([FromQuery] DateTimeOffset arg)
        {
            /*
            For NewtonsoftJson with DateTimeZoneHandling.UTC:
              10:00-5 >> 10:00-5 
              10:00Z  >> 10:00+0
             */
            return Ok(arg.ToString("O"));
        }
        
        [Route("date-time-body")]
        [HttpGet]
        public IActionResult GetDateTimeBody([FromBody] DateTime arg)
        {
            /*
            For built-in JSON serialization AND NewtonsoftJson:
              10:00-5 >> 07:00-8 
              10:00Z  >> 10:00Z
            For NewtonsoftJson with DateTimeZoneHandling.UTC:
              10:00-5 >> 15:00Z
              10:00Z  >> 10:00Z
             */
            return Ok(arg.ToString("O"));
        }

        [Route("model-body")]
        [HttpGet]
        public IActionResult GetDateTimeBody([FromBody] Model1 model1)
        {
            /*
            For NewtonsoftJson with DateTimeZoneHandling.UTC:
             */
            return Ok(model1.DateTimeField.ToString("O"));
        }

        [HttpPost("validating-endpoint")]
        public IActionResult ValidatingEndpoint([FromBody] ModelWithAnnotations model)
        {
            if (!ModelState.IsValid)
            {
                // hmm... can't seem to reach here with proper validation. See ModelWithAnnotations
                return BadRequest(ModelState);
            }

            return Ok(model);
        }
    }
}