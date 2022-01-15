using System.Collections.Generic;

namespace HPlusSport.API.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // Relations.
        public virtual List<Product> Products { get; set; } 

    }
}
