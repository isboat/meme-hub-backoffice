using MemeTokenHub.Backoffce.Models;

namespace Partners.Management.Web.Models
{
    public class UserListViewModel
    {
        public IEnumerable<UserModel> Users { get; set; }
        public string TenantId { get; set; }

        public bool AllowedPartnerPermission { get; set; }
    }
}
