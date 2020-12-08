using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Foody.Models
{
    public class Entrega
    {
        [Key]
        public int idEntrega { get; set; }
        [Key]
        public int idEncomenda { get; set; }
        [Key]
        public int idCliente { get; set; }
        [Key]
        public int idCondutor { get; set; }

        public Entrega()
        {

        }
    }
}
