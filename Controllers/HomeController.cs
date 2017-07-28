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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

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

        [Authorize]
        public IActionResult Index()
        {
            ViewData["Name"] = User.FindFirst(ClaimTypes.Name).Value;
            ViewData["ExternalId"] = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return View();
        }
    }
}