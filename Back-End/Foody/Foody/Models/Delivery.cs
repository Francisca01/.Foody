using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Foody.Models
{
    // Classe de Delivery
    public class Delivery
    {
        [Key]
        public int idDelivery { get; set; }
        public int idDriver { get; set; }
        public int idOrder { get; set; }
        public string state { get; set; }

        public Delivery()
        {

        }
    }
}
