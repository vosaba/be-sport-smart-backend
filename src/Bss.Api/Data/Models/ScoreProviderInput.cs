namespace Bss.Api.Data.Models
{
    public class ScoreProviderInput
    {
        public int ScoreProviderId { get; set; }
        public ScoreProvider ScoreProvider { get; set; } = new ScoreProvider();

        public int InputId { get; set; }
        public Input Input { get; set; } = new Input();
    }
}