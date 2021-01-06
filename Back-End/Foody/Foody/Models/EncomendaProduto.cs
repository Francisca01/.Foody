using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Foody.Models
{
    // Classe de Encomenda - Produto
    public class EncomendaProduto
    {
        [Key]
        public int idEncomendaProduto{ get; set; }
        [Key]
        public int idProduto { get; set; }
        public int quantidade { get; set; }

        public EncomendaProduto()
        {

        }
    }
}
