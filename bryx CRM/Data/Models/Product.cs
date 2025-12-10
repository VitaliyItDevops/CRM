using System;
using System.Collections.Generic;

namespace bryx_CRM.Data.Models;

public partial class Product
{
    public int Id { get; set; }

    public string Category { get; set; } = null!;

    public string Name { get; set; } = null!;

    public decimal PurchasePrice { get; set; }

    public decimal SalePrice { get; set; }

    public string Status { get; set; } = null!;

    public string Supplier { get; set; } = null!;

    public bool IsDefective { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool IsFavorite { get; set; }

    public string? AdditionalService { get; set; }

    public string? Buyer { get; set; }

    public string? SoldFor { get; set; }

    public string? SoldThrough { get; set; }

    public string? Ttn { get; set; }

    public decimal? PlannedPrice { get; set; }

    public DateTime? ArrivalDate { get; set; }

    public DateTime? SaleDate { get; set; }

    public DateTime? DeliveryDate { get; set; }

    public int? SaleId { get; set; }

    public string? Color { get; set; }

    public byte[]? RowVersion { get; set; }

    public decimal? ExchangeRate { get; set; }

    public decimal? PriceInUsd { get; set; }

    public string? Subcategory { get; set; }

    public int? PurchaseId { get; set; }

    public int? OriginalSaleId { get; set; }

    public int? ServiceId { get; set; }

    public virtual Purchase? Purchase { get; set; }

    public virtual Sale? Sale { get; set; }

    public virtual Service? Service { get; set; }
}
