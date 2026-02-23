using ActViewer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ActViewer.Pages.Contacts;

public class DetailModel : PageModel
{
    private readonly ActRepository _repo;
    public DetailModel(ActRepository repo) => _repo = repo;

    public ContactDetailRow? Contact { get; set; }
    public IReadOnlyList<CompanyLinkRow> Companies { get; set; } = Array.Empty<CompanyLinkRow>();
    public IReadOnlyList<TimelineItemVm> Timeline { get; set; } = Array.Empty<TimelineItemVm>();

    public async Task<IActionResult> OnGetAsync(Guid id, string? type, DateTime? from, DateTime? to)
    {
        Contact = await _repo.GetContactAsync(id);
        if (Contact is null) return NotFound();

        Companies = await _repo.GetContactCompaniesAsync(id);

        var timeline = await _repo.GetContactTimelineAsync(id, type, from, to, take: 200);

        // fetch attachments per timeline item (fine for internal + 200 cap)
        var items = new List<TimelineItemVm>(timeline.Count);
        foreach (var t in timeline)
        {
            var atts = await _repo.GetAttachmentsAsync(t.ItemType, t.ItemID);
            items.Add(new TimelineItemVm(t, atts.ToList()));
        }
        Timeline = items;

        return Page();
    }
}
