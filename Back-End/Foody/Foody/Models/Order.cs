using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Foody.Models
{
    // Classe de Order
    public class Order
    {
        [Key]
        public int idOrder{ get; set; }
        public int idClient { get; set; }        

        public Order()
        {

        }
    }
}
