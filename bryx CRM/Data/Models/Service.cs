using System;
using System.Collections.Generic;

namespace bryx_CRM.Data.Models;

public partial class Service
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Category { get; set; } = null!;

    public string? Subcategory { get; set; }

    public decimal Price { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public byte[]? RowVersion { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
