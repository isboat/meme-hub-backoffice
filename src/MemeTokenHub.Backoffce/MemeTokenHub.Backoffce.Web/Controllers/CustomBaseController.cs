using Microsoft.AspNetCore.Mvc;

namespace Partners.Management.Web.Controllers
{
    public class CustomBaseController : Controller
    {

        [NonAction]
        public string GetRequestPartnerId()
        {
            var tenantClaim = HttpContext.User.Claims.FirstOrDefault(x => x.Type.Equals("partnerid", StringComparison.OrdinalIgnoreCase));
            if (tenantClaim == null)
            {
                //throw new InvalidTenantException();
                throw new Exception("partnerid_not_found_in_claims");
            }

            return tenantClaim.Value;
        }

        [NonAction]
        public string GetAuthorizedUserInitials()
        {
            var tenantClaim = HttpContext.User.Claims.FirstOrDefault(x => x.Type.Equals("initials", StringComparison.OrdinalIgnoreCase));
            if (tenantClaim == null)
            {
                //throw new InvalidTenantException();
                throw new Exception("initials_not_found_in_claims");
            }

            return tenantClaim.Value;
        }

        [NonAction]
        public string GetAuthorizedUserEmail()
        {
            var tenantClaim = HttpContext.User.Claims.FirstOrDefault(x => x.Type.Equals("email", StringComparison.OrdinalIgnoreCase));
            if (tenantClaim == null)
            {
                //throw new InvalidTenantException();
                throw new Exception("email_not_found_in_claims");
            }

            return tenantClaim.Value;
        }
    }
}
