namespace Bss.Api.Data.Models
{
    public enum ScoreProviderType
    {
        Measure,
        Sport
    }

    public class ScoreProvider
    {
        public int Id { get; set; }
        public ScoreProviderType Type { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Formula { get; set; } = string.Empty;
        public string[] DependentProviders { get; set; } = Array.Empty<string>();

        public ICollection<ScoreProviderInput> ScoreProviderInputs { get; set; } = new List<ScoreProviderInput>();
    }
}