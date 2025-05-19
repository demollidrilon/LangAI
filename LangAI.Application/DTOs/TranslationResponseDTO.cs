using LangAI.Domain.Entities;

namespace LangAI.Application.DTOs;
public class TranslationResponseDTO
{
    public string Id { get; set; } = string.Empty;
    public string Object { get; set; } = string.Empty;
    public int Created { get; set; }
    public string Model { get; set; } = string.Empty;
    public List<Choice> Choices { get; set; }
}