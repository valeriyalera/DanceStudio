using System;
using System.Collections.Generic;

namespace DanceStudio.Domain.Model;

public partial class Style
{
    public int Id { get; set; }

    public string? Name { get; set; }

     public virtual ICollection<Group>? Groups { get; set; }
}
