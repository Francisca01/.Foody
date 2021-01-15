using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Foody.Models
{
    // Classe do Produto
    public class Produto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idProduto{ get; set; }
        [Key]
        public int idUtilizador { get; set; }
        public string nome { get; set; }
        public float precoUnitario { get; set; }
        public string ingredientes { get; set; }
        public int categoria { get; set; }

        public Produto()
        {

        }
    }
}
