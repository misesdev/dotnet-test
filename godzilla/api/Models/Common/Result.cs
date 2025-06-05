
namespace api.Models.Common;

public class Result<Entity> {
    public int Page { get; set; }
    public int ItemsPerPage { get; set; }
    public List<Entity> Items { get; set; } = new List<Entity>();
}
