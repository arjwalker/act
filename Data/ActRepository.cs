using Dapper;
using ActViewer.Data;


namespace ActViewer.Data;

public sealed class ActRepository
{
    private readonly ActDb _db;
    public ActRepository(ActDb db) => _db = db;

    public async Task<IReadOnlyList<ContactListRow>> SearchContactsAsync(string? q, int take = 200)
    {
        const string sql = @"
SELECT TOP (@take)
  CONTACTID,
  FULLNAME,
  PrimaryCompanyName,
  PrimaryEmail,
  PrimaryPhone,
  AddrCity,
  AddrPostcode
FROM dbo.vwContacts
WHERE
  (@q IS NULL OR @q = '')
  OR FULLNAME LIKE '%' + @q + '%'
  OR PrimaryCompanyName LIKE '%' + @q + '%'
  OR PrimaryEmail LIKE '%' + @q + '%'
  OR PrimaryPhone LIKE '%' + @q + '%'
ORDER BY FULLNAME;";
        using var conn = _db.OpenConnection();
        var rows = await conn.QueryAsync<ContactListRow>(sql, new { q, take });
        return rows.ToList();
    }

    public async Task<ContactDetailRow?> GetContactAsync(Guid id)
    {
        const string sql = @"
SELECT TOP (1)
  CONTACTID,
  FULLNAME,
  FIRSTNAME,
  LASTNAME,
  JOBTITLE,
  DEPARTMENT,
  CATEGORY,
  PrimaryCompanyID,
  PrimaryCompanyName,
  CONTACTWEBADDRESS,
  ISPRIVATE,
  CREATEDATE,
  EDITDATE,
  PrimaryEmail,
  PrimaryPhone,
  AddrLine1,
  AddrCity,
  AddrPostcode,
  AddrCountry
FROM dbo.vwContacts
WHERE CONTACTID = @id;";
        using var conn = _db.OpenConnection();
        return await conn.QuerySingleOrDefaultAsync<ContactDetailRow>(sql, new { id });
    }

    public async Task<IReadOnlyList<CompanyLinkRow>> GetContactCompaniesAsync(Guid contactId)
    {
        const string sql = @"
SELECT
  COMPANYID,
  CompanyName
FROM dbo.vwContactCompanies
WHERE CONTACTID = @contactId
ORDER BY CompanyName;";
        using var conn = _db.OpenConnection();
        var rows = await conn.QueryAsync<CompanyLinkRow>(sql, new { contactId });
        return rows.ToList();
    }

    public async Task<IReadOnlyList<TimelineRow>> GetContactTimelineAsync(
    Guid contactId, string? type, DateTime? from, DateTime? to, int take = 200)
{
    const string sql = @"
SELECT TOP (@take)
  CONTACTID, ItemType, ItemID, ItemDate, ItemEndDate, REGARDING, DETAILS, SubTypeId
FROM dbo.vwContactTimeline
WHERE CONTACTID = @contactId
  AND (@type IS NULL OR @type = '' OR @type = 'ALL' OR ItemType = @type)
  AND (@from IS NULL OR ItemDate >= @from)
  AND (@to   IS NULL OR ItemDate <  DATEADD(day, 1, @to))
ORDER BY ItemDate DESC;";
    using var conn = _db.OpenConnection();
    var rows = await conn.QueryAsync<TimelineRow>(sql, new { contactId, type, from, to, take });
    return rows.ToList();
}

// -------------------- Companies --------------------

public async Task<IReadOnlyList<CompanyListRow>> SearchCompaniesAsync(string? q, int take = 200)
{
    const string sql = @"
SELECT TOP (@take)
  COMPANYID,
  CompanyName,
  PrimaryEmail,
  PrimaryPhone,
  AddrCity,
  AddrPostcode
FROM dbo.vwCompanies
WHERE
  (@q IS NULL OR @q = '')
  OR CompanyName LIKE '%' + @q + '%'
  OR PrimaryEmail LIKE '%' + @q + '%'
  OR PrimaryPhone LIKE '%' + @q + '%'
ORDER BY CompanyName;";
    using var conn = _db.OpenConnection();
    var rows = await conn.QueryAsync<CompanyListRow>(sql, new { q, take });
    return rows.ToList();
}

public async Task<CompanyDetailRow?> GetCompanyAsync(Guid id)
{
    const string sql = @"
SELECT TOP (1)
  COMPANYID,
  CompanyName,
  DESCRIPTION,
  CATEGORY,
  INDUSTRY,
  WEBADDRESS,
  PrimaryEmail,
  PrimaryPhone,
  AddrLine1,
  AddrCity,
  AddrPostcode,
  AddrCountry
FROM dbo.vwCompanies
WHERE COMPANYID = @id;";
    using var conn = _db.OpenConnection();
    return await conn.QuerySingleOrDefaultAsync<CompanyDetailRow>(sql, new { id });
}

public async Task<IReadOnlyList<ContactMiniRow>> GetCompanyContactsAsync(Guid companyId)
{
    const string sql = @"
SELECT
  CONTACTID,
  FULLNAME,
  JOBTITLE
FROM dbo.vwCompanyContacts
WHERE COMPANYID = @companyId
ORDER BY FULLNAME;";
    using var conn = _db.OpenConnection();
    var rows = await conn.QueryAsync<ContactMiniRow>(sql, new { companyId });
    return rows.ToList();
}

public async Task<IReadOnlyList<TimelineRow>> GetCompanyTimelineAsync(Guid companyId, int take = 200)
{
    // Reuse TimelineRow by mapping COMPANYID -> CONTACTID
    const string sql = @"
SELECT TOP (@take)
  COMPANYID AS CONTACTID,
  ItemType,
  ItemID,
  ItemDate,
  ItemEndDate,
  REGARDING,
  DETAILS,
  SubTypeId
FROM dbo.vwCompanyTimeline
WHERE COMPANYID = @companyId
ORDER BY ItemDate DESC;";
    using var conn = _db.OpenConnection();
    var rows = await conn.QueryAsync<TimelineRow>(sql, new { companyId, take });
    return rows.ToList();
}

// -------------------- Attachments (needed by both) --------------------

public async Task<IReadOnlyList<AttachmentRow>> GetAttachmentsAsync(string itemType, Guid itemId)
{
    const string sql = @"
SELECT
  ATTACHMENTID,
  DISPLAYNAME,
  FILENAME,
  FILEPATH,
  MACHINENAME
FROM dbo.vwTimelineAttachments
WHERE ItemType = @itemType AND ItemID = @itemId
ORDER BY DISPLAYNAME;";
    using var conn = _db.OpenConnection();
    var rows = await conn.QueryAsync<AttachmentRow>(sql, new { itemType, itemId });
    return rows.ToList();
}

}
