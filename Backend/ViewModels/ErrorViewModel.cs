namespace Backend.ViewModels
{
    public class ErrorViewModel
    {
        // Properties
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public string RequestId { get; set; }
    }
}