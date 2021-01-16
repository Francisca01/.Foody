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
        public int idEncomendaProduto{ get; set; }
        [Key]
        public int idProduto { get; set; }
        public int quantidade { get; set; }

        public OrderProduct()
        {

        }
    }
}
