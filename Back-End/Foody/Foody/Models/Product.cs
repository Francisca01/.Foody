using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Foody.Models
{
    // Classe do Product
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idProduct { get; set; }
        public int idCompany { get; set; }
        public string name { get; set; }
        public float unitPrice { get; set; }
        public string ingredients { get; set; }
        public int category { get; set; }

        public Product()
        {

        }
    }
}
