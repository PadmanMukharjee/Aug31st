using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Services;
using Meridian.AuthServer.Api.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Meridian.AuthServer.BusinessModel;
using System;

namespace Meridian.AuthServer.Api.Controllers
{
    public class AccountController : Controller
    {
        private readonly IEventService _events;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IList<Application> _applications;


        public AccountController(
            IEventService events,
            IIdentityServerInteractionService interaction,
            IList<Application> applications)
        {
            _events = events;
            _interaction = interaction;
            _applications = applications;
        }

        /// <summary>
        /// Show login page
        /// </summary>
        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            var model = new LoginViewModel
            {
                ReturnUrl = returnUrl,
            };
            return View(model);
        }

        /// <summary>
        /// Handle postback from username/password login
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var applicationKey = model.ReturnUrl.Split('?')[1].Split('&')[0].Split('=')[1];
                var application = _applications.FirstOrDefault(app => app.Key == applicationKey);
                UserLogin user = ValidateUserCredentials(model.UserName, model.Password, application.Key);
                if (user != null)
                {
                    //User user = _userBusiness.FindByUserName(model.UserName);
                    if (user != null)
                    {
                        var props = new AuthenticationProperties
                        {
                            IsPersistent = true,
                        };

                        await _events.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.UserId.ToString(), user.FirstName));

                        // issue authentication cookie with subject ID and username
                        await HttpContext.SignInAsync(user.UserId.ToString(), user.UserName, props);

                        // make sure the returnUrl is still valid, and if so redirect back to authorize endpoint or a local page
                        if (_interaction.IsValidReturnUrl(model.ReturnUrl) || Url.IsLocalUrl(model.ReturnUrl))
                        {
                            return Redirect(model.ReturnUrl);
                        }

                        return Redirect("~/");
                    }
                }

                await _events.RaiseAsync(new UserLoginFailureEvent(model.UserName, "invalid credentials"));
                ModelState.AddModelError("", "Invalid username or password");
            }
            return View();
        }

        private UserLogin ValidateUserCredentials(string userName, string password, object applicationCode)
        {
            return new UserLogin() {
                UserId = "821",
                UserName = "Prashanth"
            };
        }

        /// <summary>
        /// Show logout page
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            var user = HttpContext.User;
            if (user?.Identity.IsAuthenticated == true)
            {
                // delete local authentication cookie
                await HttpContext.SignOutAsync();

                // raise the logout event
                await _events.RaiseAsync(new UserLogoutSuccessEvent(user.GetSubjectId(), user.GetDisplayName()));
            }
            return View();
        }
    }
}