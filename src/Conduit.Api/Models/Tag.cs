using System.ComponentModel.DataAnnotations;

namespace Conduit.Api.Models;

internal sealed class Tag
{
    public Guid   Id   { get; private set; }

    [MaxLength(255)]
    [Required]
    public string Name { get; private set; }

    public Tag(string name)
    {
        Id   = Guid.CreateVersion7();
        Name = name;
    }
}
