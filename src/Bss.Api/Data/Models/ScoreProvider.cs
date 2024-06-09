namespace Bss.Api.Data.Models
{
    public enum ScoreProviderType
    {
        Measure,
        Sport,
        Score
    }

    public class ScoreProvider
    {
        public int Id { get; set; }
        public ScoreProviderType Type { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool Disabled { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public string Formula { get; set; } = string.Empty;
        public string[] DependentProviders { get; set; } = Array.Empty<string>();
        public string[] DependentInputs { get; set; } = Array.Empty<string>();

        //public ICollection<ScoreProviderInput> ScoreProviderInputs { get; set; } = new List<ScoreProviderInput>();
    }
}