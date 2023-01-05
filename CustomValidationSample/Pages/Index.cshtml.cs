using CustomValidationSample.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace CustomValidationSample.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    [BindProperty]
    public ViewModel Data { get; set; } = new ViewModel();

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {

    }
}

public class ViewModel
{
	const string ErrMessage = "{0},{1}どちらかを入力してください";

    [Display(Name="名前1")]
	[RequiredEither(nameof(Name2), ErrorMessage = ErrMessage)]
	public string? Name1 { get; set; } = string.Empty;

	[Display(Name = "名前2")]
	[RequiredEither(nameof(Name1), ErrorMessage = ErrMessage)]
    public string? Name2 { get; set; } = string.Empty;
}

