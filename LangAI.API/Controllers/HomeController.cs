using Microsoft.AspNetCore.Mvc;
using LangAI.Application.DTOs;
using LangAI.Application.Interfaces;

namespace LangAI.API.Controllers;
public class HomeController : Controller
{
    private readonly ITranslationService _translationService;
    
    public HomeController(ITranslationService translationService)
    {
        _translationService = translationService;
    }

    public IActionResult Index()
    {
        return View(new TranslationRequestDTO());
    }

    [HttpPost]
    public async Task<IActionResult> FetchAIReponse(TranslationRequestDTO request)
    {
        if (!ModelState.IsValid)
            return View("Index", request);

        var result = await _translationService.TranslateAsync(request.TextToTranslate, request.TargetLanguage);

        ViewBag.Result = result;
        return View("Index", request);
    }
}