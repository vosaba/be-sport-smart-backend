namespace Bss.Api.Data.Models
{
    public enum InputType
    {
        Boolean,
        Number,
        String,
    }

    public enum InputSource
    {
        User,
        Professional,
    }

    public class Input
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public InputType Type { get; set; }
        public InputSource InputSource { get; set; }
        public string[] Options { get; set; } = Array.Empty<string>();
    }
}