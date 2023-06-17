namespace mosaCupBackendServer.Models.ReqModels
{
    public class EditProfile
    {
        public string Uid { get; set; }
        public string DisplayName { get; set; }
        public string? Description { get; set; }
    }
}
