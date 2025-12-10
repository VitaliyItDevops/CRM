using System;
using System.Collections.Generic;

namespace bryx_CRM.Data.Models;

public partial class Supplier
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
}
