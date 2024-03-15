namespace WebApp_OpenIDConnect_DotNet.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }
        public string userMessage { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}