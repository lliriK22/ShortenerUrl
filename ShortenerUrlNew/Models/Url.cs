using System.ComponentModel.DataAnnotations;

namespace ShortenerUrlNew.Models
{
    public class Url
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string longUrl { get; set; }
        public string? shortUrl { get; set; }
        public DateTime? dateTime { get; set; }
        public int? countTransitionUrl { get; set; }
    }
}
