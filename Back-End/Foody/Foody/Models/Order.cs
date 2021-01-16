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
        public int idEncomenda{ get; set; }
        [Key]
        public int idCliente { get; set; }
        [Key]
        public int idEncomendaProduto{ get; set; }
        

        public Order()
        {

        }
    }
}
