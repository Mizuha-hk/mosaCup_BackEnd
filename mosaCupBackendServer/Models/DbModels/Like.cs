using System.ComponentModel.DataAnnotations;

namespace mosaCupBackendServer.Models.DbModels
{
    public class Like
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int PostId { get; set; }
        [Required]
        public string Uid { get; set; }
    }
}
