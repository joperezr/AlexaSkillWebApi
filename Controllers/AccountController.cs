using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using AlexaSkill.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AlexaSkill.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { ReturnUrl = returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
            {
                return ReturnWithError($"Error from external provider: {remoteError}", nameof(Login), new { ReturnUrl = returnUrl });
            }

            if (User == null)
            {
                return ReturnWithError($"Error when loging in.", nameof(Login), new { ReturnUrl = returnUrl });
            }

            var email = User.FindFirstValue(ClaimTypes.Email);
            var properties = new AuthenticationProperties { AllowRefresh = true, IsPersistent = false, ExpiresUtc = DateTime.UtcNow.AddMinutes(30) };

            if (await _userManager.FindByNameAsync(email) != null)
            {
                if (User.Identities.Any(identity => identity.IsAuthenticated))
                {
                    return RedirectToAction(nameof(HomeController.Index), "Home");
                }

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, User, properties);
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            var user = new ApplicationUser { UserName = email, Email = email, ExternalId = User.FindFirstValue(ClaimTypes.NameIdentifier)};

            var userIdentity = await _userManager.CreateAsync(user);

            if (userIdentity.Succeeded)
            {
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, User, properties);
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            return ReturnWithError("External login failed, please try again.", nameof(Login), new { ReturnUrl = returnUrl });
        }

        [HttpGet]
        [AllowAnonymous]
        public string ExternalError(string error)
        {
            if (error.Contains("access_denied"))
                return "Acces was denied by the user";

            return $"There was an error processing your request: {error}";
        }

        private IActionResult ReturnWithError(string error, string action, object routeValues)
        {
            ModelState.AddModelError(string.Empty, error);
            return RedirectToAction(action, routeValues);
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {

            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }
    }
}
