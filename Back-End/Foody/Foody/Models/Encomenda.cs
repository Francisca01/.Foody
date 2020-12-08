using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Foody.Models
{
    public class Encomenda
    {
        [Key]
        public int idEncomenda{ get; set; }
        [Key]
        public int idEncomendaProduto{ get; set; }
        [Key]
        public int idCliente { get; set; }
        public int estado { get; set; }

        public Encomenda()
        {

        }
    }
}
