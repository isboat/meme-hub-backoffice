namespace MemeTokenHub.Backoffce.Models
{
    public class MemePageModel: IModelItem
    {
        public string? Id { get; set; }

        public List<string>? OwnerIds { get; set; }

        public string? Name { get; set; }

        public string? PathUrl { get; set; }

        public string? AboutMe { get; set; }

        public DateTime Created { get; set; }

        public PageStatus Status { get; set; }

        public Dictionary<string, string>? Metadata { get; set; }
    }

    public enum PageStatus
    {
        Created,
        Inactive,
        Active,
        Deleted
    }
}
