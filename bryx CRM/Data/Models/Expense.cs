using System;
using System.Collections.Generic;

namespace bryx_CRM.Data.Models;

public partial class Expense
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Category { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Amount { get; set; }

    public DateTime ExpenseDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public byte[]? RowVersion { get; set; }
}
