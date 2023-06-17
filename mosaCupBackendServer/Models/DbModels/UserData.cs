using System.ComponentModel.DataAnnotations;

namespace mosaCupBackendServer.Models.DbModels
{
    public class UserData
    {
        [Key]
        public string Uid { get; set; }
        [Required]
        public string DisplayName { get; set; }
        [Required]
        public string Name { get; set; }

        public string? Description { get; set; }

        public DateTime? DeletedAt { get; set; } = null;
    }
}
