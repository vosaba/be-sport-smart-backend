namespace Bss.Api.Data.Models
{
    public enum InputType
    {
        Boolean,
        Number,
        String,
    }

    public class Input
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public InputType Type { get; set; }
        public string[] Options { get; set; } = Array.Empty<string>();

        public ICollection<ScoreProviderInput> ScoreProviderInputs { get; set; } = new List<ScoreProviderInput>();
    }
}