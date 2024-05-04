namespace Bss.Api.Dtos.Evaluation
{
    public enum EvaluationResultStatus
    {
        Success,
        Error
    }

    public class EvaluationResultDto
    {
        public string Name { get; set; } = string.Empty;
        public double Score { get; set; }
    }
}
