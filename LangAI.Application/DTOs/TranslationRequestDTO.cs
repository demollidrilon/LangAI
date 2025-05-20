using System.ComponentModel.DataAnnotations;

namespace LangAI.Application.DTOs;
public class TranslationRequestDTO
{
    [Required(ErrorMessage = "This field can’t be empty. What do you want to translate?")]
    public string TextToTranslate { get; set; } = string.Empty;

    [Required(ErrorMessage = "Almost there! Choose your target language.")]
    public string TargetLanguage { get; set; } = string.Empty;
}