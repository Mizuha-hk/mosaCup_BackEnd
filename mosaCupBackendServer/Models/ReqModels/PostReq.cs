namespace mosaCupBackendServer.Models.ReqModels
{
    public class PostReq
    {
        public string Uid { get; set; }
        public string Content { get; set; }
        public int? ReplyId { get; set; } = null;
    }
}
