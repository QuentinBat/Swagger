using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Swagger.Controllers
{
    [ApiController]
    [Route("api/[action]")]
    public class ValuesController : Controller
    {

        [HttpGet]
        [SwaggerOperation(Summary = "Gets two values", Description = "Gets two hardcoded values")]
        [SwaggerResponse(200, "I guess everything worked")]
        [SwaggerResponse(400, "BAD REQUEST")]
        public IEnumerable<string> Values()
        {
            return new string[] { "value1", "value2" };
        }

        /// <summary>
        /// Gets a value.
        /// </summary>
        /// <param name="id">The id !!</param>x
        [HttpGet]
        public string Get(int id)
        {
            return "value";
        }

        /// <summary>
        /// Posts a value.
        /// </summary>
        /// <param name="value">The value !!</param>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        /// <summary>
        /// Puts a value.
        /// </summary>
        /// <param name="id">The id !!</param>
        /// <param name="value">The value !!</param>
        [HttpPut]
        public void Put(int id, [FromBody]string value)
        {
        }

        /// <summary>
        /// Deletes an entry.
        /// </summary>
        /// <param name="id">The id !!</param>
        [HttpDelete]
        public void Delete(int id)
        {
        }

    }
}
