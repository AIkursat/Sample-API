using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HPlusSport.API.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Sku { get; set; } // Stock Keeping Unit.
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }

        public int CategoryId { get; set; }

        // Relations.
        [JsonIgnore]
        public virtual Category Category { get; set; }
    }
}
