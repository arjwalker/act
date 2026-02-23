using ActViewer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ActViewer.Pages.Companies;

public class DetailModel : PageModel
{
    private readonly ActRepository _repo;
    public DetailModel(ActRepository repo) => _repo = repo;

    public CompanyDetailRow? Company { get; set; }
    public IReadOnlyList<ContactMiniRow> Contacts { get; set; } = Array.Empty<ContactMiniRow>();
    public IReadOnlyList<TimelineItemVm> Timeline { get; set; } = Array.Empty<TimelineItemVm>();

    public async Task<IActionResult> OnGetAsync(Guid id)
    {
        Company = await _repo.GetCompanyAsync(id);
        if (Company is null) return NotFound();

        Contacts = await _repo.GetCompanyContactsAsync(id);

        var rows = await _repo.GetCompanyTimelineAsync(id, take: 200);

        var items = new List<TimelineItemVm>(rows.Count);
        foreach (var r in rows)
        {
            var atts = await _repo.GetAttachmentsAsync(r.ItemType, r.ItemID);
            items.Add(new TimelineItemVm(r, atts.ToList()));
        }

        Timeline = items;
        return Page();
    }
}
