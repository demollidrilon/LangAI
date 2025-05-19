using System.ComponentModel.DataAnnotations;

namespace LangAI.Application.DTOs;
public class TranslationRequestDTO
{
    [Required(ErrorMessage = "Please enter text to translate.")]
    public string TextToTranslate { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please choose a target language.")]
    public string TargetLanguage { get; set; } = string.Empty;
}