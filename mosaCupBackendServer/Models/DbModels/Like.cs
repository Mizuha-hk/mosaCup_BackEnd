using System.ComponentModel.DataAnnotations;

namespace mosaCupBackendServer.Models.DbModels
{
    public class Like
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public Guid PostId { get; set; }
        [Required]
        public string Uid { get; set; }
    }
}
