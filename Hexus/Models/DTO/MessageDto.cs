using Hexus.Models.Domain;

namespace Hexus.Models.DTO
{
    public class MessageDto
    {
        public Guid Id { get; set; }
        public string SenderId { get; set; }
        public string SenderUserName { get; set; }
       // public string SenderImageUrl { get; set; }
        public string ReceiverId { get; set; }
        public string ReceiverUserName { get; set; }
      //  public string ReceiverImageUrl { get; set; }
        public string Content { get; set; }
        public DateTime? DateRead { get; set; }
        public DateTime MessageSent { get; set; }
        
    }
}
