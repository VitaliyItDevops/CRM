using System;
using System.Collections.Generic;

namespace bryx_CRM.Data.Models;

public partial class Sale
{
    public int Id { get; set; }

    public string Buyer { get; set; } = null!;

    public DateTime SaleDate { get; set; }

    public string? TTN { get; set; }

    public string? SoldThrough { get; set; }

    public string? AdditionalService { get; set; }

    public decimal TotalAmount { get; set; }

    public string Status { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public byte[]? RowVersion { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
