using M3Pact.Infrastructure.Enums;

namespace M3Pact.BusinessModel
{
    public class MessageDTO
    {
        public string Message { get; set; }
        public string Description { get; set; }
        public MessageType MessageType { get; set; }
    }
}
