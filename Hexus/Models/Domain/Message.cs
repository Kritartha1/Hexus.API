namespace Hexus.Models.Domain
{
    public class Message
    {
        public Guid Id { get; set; }
        public string SenderId { get; set; }    
        public string SenderUserName { get; set; }
        public User Sender {  get; set; }
        public string ReceiverId { get; set; }
        public string ReceiverUserName { get; set; }
        public User Receiver { get; set; }
        public string Content { get; set; }
        public DateTime? DateRead { get; set; }
        public DateTime MessageSent { get; set; }
        public bool senderDeleted { get; set; } 
        public bool receiverDeleted { get; set; }


    }
}
