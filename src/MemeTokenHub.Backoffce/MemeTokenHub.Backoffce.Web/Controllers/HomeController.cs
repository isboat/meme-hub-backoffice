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
using MemeTokenHub.Backoffce.Services;

namespace Partners.Management.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AuthSettings _authSettings;
        private readonly IService<PartnerModel> _partnerService;
        private readonly IEncryptionService _encryptionService;

        public HomeController(
            ILogger<HomeController> logger, 
            IOptions<AuthSettings> settings, 
            IService<PartnerModel> partnerService, 
            IEncryptionService encryptionService)
        {
            _logger = logger;
            _authSettings = settings.Value;
            _partnerService = partnerService;
            _encryptionService = encryptionService;
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

                var partner = (await _partnerService.GetByFilter((x) => FilterByEmailAsync(x, username))).FirstOrDefault();
                if (partner == null)
                {
                    return View();
                }

                var passwdVerified = _encryptionService.Verify(password, partner.Password);
                var claims = new List<Claim>
                {
                    new Claim("partnerid", partner.Id),
                    new Claim(ClaimTypes.Name, partner.Name),
                    new Claim(ClaimTypes.Email, partner.Email),
                    new Claim(ClaimTypes.PostalCode, partner.Postcode)
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

        private static bool FilterByEmailAsync(PartnerModel model, string username)
        {
            return model.Email?.ToLowerInvariant() == username.ToLowerInvariant();
        }

    }
}