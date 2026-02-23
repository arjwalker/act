using ActViewer.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ActViewer.Pages.Contacts;

public class IndexModel : PageModel
{
    private readonly ActRepository _repo;
    public IndexModel(ActRepository repo) => _repo = repo;

    public string? Q { get; set; }
    public IReadOnlyList<ContactListRow> Results { get; set; } = Array.Empty<ContactListRow>();

    public async Task OnGetAsync(string? q)
    {
        Q = q?.Trim();

        // IMPORTANT: show nothing until a search is entered
        if (string.IsNullOrWhiteSpace(Q))
        {
            Results = Array.Empty<ContactListRow>();
            return;
        }

        Results = await _repo.SearchContactsAsync(Q, take: 200);
    }
}
