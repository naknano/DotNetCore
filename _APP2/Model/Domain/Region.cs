using System.ComponentModel.DataAnnotations;

namespace API.Model.Domain
{
    public class Region
    {
        [Required]
        [MinLength(3, ErrorMessage ="Code has to be minimum of 3 charactor!")]
        public Guid Id { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage ="Name has to be maximum of 100 charactor!")]
        public string Name { get; set; }

        public string? RegionImageUrl { get; set; }
    }
}
