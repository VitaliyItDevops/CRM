using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bryx_CRM.Data.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Category { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal PurchasePrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal SalePrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? PlannedPrice { get; set; }

        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = "В наличии";

        [Required]
        [MaxLength(200)]
        public string Supplier { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Color { get; set; }

        public bool IsDefective { get; set; } = false;

        public bool IsFavorite { get; set; } = false;

        // Поля для работы с покупками и продажами
        [MaxLength(100)]
        public string? TTN { get; set; }

        [MaxLength(200)]
        public string? Buyer { get; set; }

        [MaxLength(200)]
        public string? SoldFor { get; set; }

        [MaxLength(200)]
        public string? SoldThrough { get; set; }

        [MaxLength(300)]
        public string? AdditionalService { get; set; }

        public DateTime? ArrivalDate { get; set; }

        public DateTime? SaleDate { get; set; }

        public DateTime? DeliveryDate { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        // Оптимистическая блокировка для многопользовательского режима
        [Timestamp]
        public byte[]? RowVersion { get; set; }

        // Связь с продажей
        public int? SaleId { get; set; }
        public Sale? Sale { get; set; }
    }
}