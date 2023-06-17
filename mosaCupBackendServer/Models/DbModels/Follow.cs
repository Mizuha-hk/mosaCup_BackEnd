using System.ComponentModel.DataAnnotations;

namespace mosaCupBackendServer.Models.DbModels
{
    public class Follow
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Uid { get; set; }
        [Required]
        public string FollowedUid { get; set; }
    }
}
