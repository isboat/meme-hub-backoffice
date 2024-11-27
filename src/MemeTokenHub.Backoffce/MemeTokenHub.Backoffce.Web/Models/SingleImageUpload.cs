namespace Partners.Management.Web.Models
{
    public class SingleImageUpload
    {
        public string? MemepageId { get; set; }

        public string? ImgType { get; set; }
        public string? Section { get; set; }

        public IFormFile? File { get; set; }
    }
}