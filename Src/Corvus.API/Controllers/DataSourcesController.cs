using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Corvus.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataSourcesController : ODataController
    {
        // GET: api/<DataSourcesController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<DataSourcesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<DataSourcesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<DataSourcesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<DataSourcesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
