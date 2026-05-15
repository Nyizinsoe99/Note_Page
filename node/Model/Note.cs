using System.ComponentModel.DataAnnotations;

namespace Node_Page.Model;

public class Note
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Content { get; set; } = string.Empty;

    // Stores image as Base64 string
    public string? Photo { get; set; } 

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}