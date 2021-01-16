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
        public int idEntrega { get; set; }
        [Key]
        public int idCondutor { get; set; }
        [Key]
        public int idEncomenda { get; set; }
        public string Estado { get; set; }

        public Delivery()
        {

        }
    }
}
