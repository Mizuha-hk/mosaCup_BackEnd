using System.ComponentModel.DataAnnotations;

namespace mosaCupBackendServer.Models.ReqModels
{
    public class EditProfile
    {
        [Key]
        public string Uid { get; set; }
        [Required]
        public string DisplayName { get; set; }

        public string? Description { get; set; }
    }
}
