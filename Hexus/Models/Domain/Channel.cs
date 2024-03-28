namespace Hexus.Models.Domain
{
    public class Channel
    {
        public Guid Id { get; set; }
        public bool IsText { get; set; } //if text then true, if audio then false
        public Voice? Voice { get; set; }
        public Text? Text { get; set; }
    }
}
