using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TenmoServer.Controllers
{
    [Route("/")]
    [ApiController]
    public class ExampleController : ControllerBase
    {
        [HttpGet]
        public object Home()
        {
            object sampleData = new { title = "Hello World!", message = "This is just an example endpoint. You can delete the entire ExampleController once you add your own." };
            return Ok(sampleData);
        }
    }
}
