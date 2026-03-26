namespace Accounts.Domain.Entities
{
    [Table("Account")]
    public class Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        // Basic Info
        [Required]
        [MaxLength(200)]
        public string AccountName { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string AccountNumber { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? AccountOwner { get; set; }

        // Contact & Address
        [Required]
        [MaxLength(20)]
        public string Phone { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? Fax { get; set; }

        [MaxLength(500)]
        [Url]
        public string? Website { get; set; }

        [Required]
        [MaxLength(1000)]
        public string BillingAddress { get; set; } = string.Empty;

        [Required]
        [MaxLength(1000)]
        public string ShippingAddress { get; set; } = string.Empty;

        // Classification
        [Required]
        [MaxLength(50)]
        public string Type { get; set; } = "Customer";

        [MaxLength(200)]
        public string? Industry { get; set; }

        [Required]
        public decimal AnnualRevenue { get; set; }

        [Required]
        public int NumberOfEmployees { get; set; }

        [MaxLength(50)]
        public string? Ownership { get; set; }

        // System Fields
        public DateTime CreatedAtUtc { get; set; }

        public DateTime ModifiedAtUtc { get; set; }
    }

}
