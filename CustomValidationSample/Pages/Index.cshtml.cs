using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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
    public string Name1 { get; set; } = string.Empty;

    public string Name2 { get; set; } = string.Empty;
}

