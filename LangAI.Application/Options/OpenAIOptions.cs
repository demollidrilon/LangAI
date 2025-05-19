namespace LangAI.Application.Options;
public class OpenAIOptions
{
    public string URL { get; set; } = string.Empty;
    public string GTPModel { get; set; } = string.Empty;
    public int Temperature { get; set; }
    public int MaxTokens { get; set; }
    public string SecretKey { get; set; } = string.Empty;
}