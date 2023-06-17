using System.ComponentModel.DataAnnotations;

namespace mosaCupBackendServer.Models.DbModels
{
    public class Follow
    {
        [Key]
        public int Id { get; set; }
        public string Uid { get; set; }
        public string FollowedUid { get; set; }
    }
}
