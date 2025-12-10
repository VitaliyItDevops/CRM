using System;
using System.Collections.Generic;

namespace bryx_CRM.Data.Models;

public partial class Purchase
{
    public int Id { get; set; }

    public DateTime PurchaseDate { get; set; }

    public string Category { get; set; } = null!;

    public string Supplier { get; set; } = null!;

    public decimal TotalPriceUsd { get; set; }

    public decimal ExchangeRate { get; set; }

    public decimal TotalPriceUah { get; set; }

    public int Quantity { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public byte[]? RowVersion { get; set; }

    public string? Comment { get; set; }

    public string Status { get; set; } = null!;

    public string? Subcategory { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
