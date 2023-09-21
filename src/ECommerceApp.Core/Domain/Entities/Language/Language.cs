namespace ECommerceApp.Core.Domain.Entities.Language;

/// <summary>
/// Represents a language entity
/// </summary>
public class Language : DocumentLongTrack
{
    // Represents language name e.g English
    public string Name { get; set; }

    // Represents language code e.g en
    public string Code { get; set; }

    // Represents language direction e.g ltr or rtl
    public string Direction { get; set; }

    // Represents language status e.g true or false
    public bool IsActive { get; set; }
}