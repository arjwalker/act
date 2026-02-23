using ActViewer.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ActViewer.Pages.Companies;

public class IndexModel : PageModel
{
    private readonly ActRepository _repo;
    public IndexModel(ActRepository repo) => _repo = repo;

    public string? Q { get; set; }
    public IReadOnlyList<CompanyListRow> Results { get; set; } = Array.Empty<CompanyListRow>();

    public async Task OnGetAsync(string? q)
    {
        Q = q?.Trim();

        // IMPORTANT: show nothing until a search is entered
        if (string.IsNullOrWhiteSpace(Q))
        {
            Results = Array.Empty<CompanyListRow>();
            return;
        }

        Results = await _repo.SearchCompaniesAsync(Q, take: 200);
    }
}
