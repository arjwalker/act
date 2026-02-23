namespace ActViewer.Data;

public sealed record ContactListRow(
    Guid CONTACTID,
    string? FULLNAME,
    string? PrimaryCompanyName,
    string? PrimaryEmail,
    string? PrimaryPhone,
    string? AddrCity,
    string? AddrPostcode
);

public sealed record ContactDetailRow(
    Guid CONTACTID,
    string? FULLNAME,
    string? FIRSTNAME,
    string? LASTNAME,
    string? JOBTITLE,
    string? DEPARTMENT,
    string? CATEGORY,
    Guid? PrimaryCompanyID,
    string? PrimaryCompanyName,
    string? CONTACTWEBADDRESS,
    bool? ISPRIVATE,
    DateTime? CREATEDATE,
    DateTime? EDITDATE,
    string? PrimaryEmail,
    string? PrimaryPhone,
    string? AddrLine1,
    string? AddrCity,
    string? AddrPostcode,
    string? AddrCountry
);

public sealed record CompanyLinkRow(
    Guid COMPANYID,
    string? CompanyName
);

public sealed record TimelineRow(
    Guid CONTACTID,
    string ItemType,
    Guid ItemID,
    DateTime? ItemDate,
    DateTime? ItemEndDate,
    string? REGARDING,
    string? DETAILS,
    short? SubTypeId
);

public sealed record AttachmentRow(
    Guid ATTACHMENTID,
    string? DISPLAYNAME,
    string? FILENAME,
    string? FILEPATH,
    string? MACHINENAME
);

public sealed record TimelineItemVm(
    TimelineRow Item,
    List<AttachmentRow> Attachments
);

public sealed record CompanyListRow(
    Guid COMPANYID,
    string? CompanyName,
    string? PrimaryEmail,
    string? PrimaryPhone,
    string? AddrCity,
    string? AddrPostcode
);

public sealed record CompanyDetailRow(
    Guid COMPANYID,
    string? CompanyName,
    string? DESCRIPTION,
    string? CATEGORY,
    string? INDUSTRY,
    string? WEBADDRESS,
    string? PrimaryEmail,
    string? PrimaryPhone,
    string? AddrLine1,
    string? AddrCity,
    string? AddrPostcode,
    string? AddrCountry
);

public sealed record ContactMiniRow(
    Guid CONTACTID,
    string? FULLNAME,
    string? JOBTITLE
);
