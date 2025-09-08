using Microsoft.AspNetCore.Identity;

namespace Conduit.Api.Models;

public sealed class ApplicationUser() : IdentityUser
{
    public bool EnableNotifications { get; set; } = false;
    public string Initials { get; set; } = string.Empty;
}
