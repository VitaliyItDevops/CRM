using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bryx_CRM.Data.Models
{
    public class Purchase
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime PurchaseDate { get; set; }

        [Required]
        [MaxLength(100)]
        public string Category { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Subcategory { get; set; }

        [Required]
        [MaxLength(200)]
        public string Supplier { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPriceUSD { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal ExchangeRate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPriceUAH { get; set; }

        public int Quantity { get; set; }

        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = "Ожидается";

        [MaxLength(500)]
        public string? Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        [Timestamp]
        public byte[]? RowVersion { get; set; }

        // Навигационное свойство для связи с товарами
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
