
namespace api.Models;

public class Movie : BaseModel
{
    public string Title { get; set; } = string.Empty; 
    public string Director { get; set; } = string.Empty;
    public int Stock { get; set; }
    public string CoverSource { get; set; } = string.Empty;
    public string Source { get; set; } = string.Empty;
}
