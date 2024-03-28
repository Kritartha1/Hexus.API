namespace Hexus.Models.Domain
{
    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IList<Channel>? Channels { get; set; }
    }
}
