using Microsoft.AspNetCore.Identity;


public class ApplicationUser : IdentityUser
{
    [PersonalData] // property is included in download of personal data.
    public string Name { get; set; } = string.Empty;

    [PersonalData]
    public string Location { get; set; } = string.Empty;

    [PersonalData]
    public string FavouriteAlbum { get; set; } = string.Empty;


    [PersonalData]
    public string ImageFilename { get; set; } = string.Empty;

    IFormFile ImageFile { get; set; }
}