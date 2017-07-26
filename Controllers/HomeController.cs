using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.Abstractions;
using System.Net.Http;
using AlexaSkill;
using System.Net.Http.Headers;
using Microsoft.Extensions.Primitives;
using System.IO;
using Microsoft.AspNetCore.Mvc.WebApiCompatShim;
using System.Diagnostics;

namespace AlexaSkill.Controllers
{
    public class HomeController : Controller
    {
        [Route("api/Sensor")]
        [HttpPost]
        public async Task<IActionResult> Sensor()
        {
            var speechlet = new DotNetApplet();
            var message = Request.HttpContext.GetHttpRequestMessage();
            var response = await speechlet.GetResponseAsync(message);
            return Ok(await response.Content.ReadAsStreamAsync());
        }
    }
}