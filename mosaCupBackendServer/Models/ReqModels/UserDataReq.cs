namespace mosaCupBackendServer.Models.ReqModels
{
    public class UserDataReq
    {
        public string Uid { get; set; }
        public string DisplayName { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}
