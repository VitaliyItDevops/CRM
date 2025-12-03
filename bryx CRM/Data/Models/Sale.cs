using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bryx_CRM.Data.Models
{
    public class Sale
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Buyer { get; set; } = string.Empty;

        [Required]
        public DateTime SaleDate { get; set; }

        [MaxLength(100)]
        public string? TTN { get; set; }

        [MaxLength(200)]
        public string? SoldThrough { get; set; }

        [MaxLength(300)]
        public string? AdditionalService { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = "Отправлено";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        [Timestamp]
        public byte[]? RowVersion { get; set; }

        // Навигационное свойство для связи с товарами
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
