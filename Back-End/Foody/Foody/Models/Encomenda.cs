using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Foody.Models
{
    // Classe de Encomenda
    public class Encomenda
    {
        [Key]
        public int idEncomenda{ get; set; }
        [Key]
        public int idCliente { get; set; }
        [Key]
        public int idEncomendaProduto{ get; set; }
        

        public Encomenda()
        {

        }
    }
}
