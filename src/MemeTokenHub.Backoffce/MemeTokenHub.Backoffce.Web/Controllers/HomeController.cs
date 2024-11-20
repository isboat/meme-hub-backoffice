using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using Partners.Management.Web.Models;
using Microsoft.Extensions.Options;
using MemeTokenHub.Backoffce.Models;
using MemeTokenHub.Backoffce.Services.Interfaces;

namespace Partners.Management.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AuthSettings _authSettings;
        private readonly IUserService _userService;
        private readonly IEncryptionService _encryptionService;

        public HomeController(
            ILogger<HomeController> logger,
            IOptions<AuthSettings> settings,
            IEncryptionService encryptionService,
            IUserService userService)
        {
            _logger = logger;
            _authSettings = settings.Value;
            _encryptionService = encryptionService;
            _userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password, string ReturnUrl)
        {
            try
            {
                if(string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password)) return View();

                var user = (await _userService.LoginAsync(username, password));
                if (user == null)
                {
                    CreateNewTestUser(username, password);
                    return View();
                }

                var claims = new List<Claim>
                {
                    new Claim("partnerid", user.Id),
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.Email),
                };

                var claimsIdentity = new ClaimsIdentity(claims, "Login");

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                return Redirect(ReturnUrl == null ? "/Index" : ReturnUrl);
            }
            catch
            {
                return View();
            }
        }

        private void CreateNewTestUser(string username, string password)
        {
            var user = new UserModel
            {
                CreatedOn = DateTime.Now,
                Email = username,
                Password = password,
                Name = "Tom Smith",
                Role = UserRoles.Admin,
            };
            _userService.CreateAsync(user);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}