using System;
using api.model;
using Microsoft.AspNetCore.Mvc;

namespace api
{
    [Route("/")]
    public class Controller1 : Controller
    {
        // GET
        public IActionResult Get()
        {
            return Ok("Hello from 1");
        }

        [Route("date-time-query")]
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
        public IActionResult GetDateTimeBody([FromBody] Model1 model1)
        {
            /*
            For NewtonsoftJson with DateTimeZoneHandling.UTC:
             */
            return Ok(model1.DateTimeField.ToString("O"));
        }
    }
}