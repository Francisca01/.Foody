using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Foody.Models
{
    // Classe de OrderProduct
    public class OrderProduct
    {
        [Key]
        public int idOrder{ get; set; }
        [Key]
        public int idProduct { get; set; }
        public int quantity { get; set; }

        public OrderProduct()
        {

        }
    }
}
