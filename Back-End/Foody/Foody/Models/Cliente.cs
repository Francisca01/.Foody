using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Foody.Models
{
    public class Cliente
    {
        [Key]
        public int idCliente { get; set; }
        [Key]
        public int idUtilizador { get; set; }

        public Cliente()
        {

        }
    }
}
