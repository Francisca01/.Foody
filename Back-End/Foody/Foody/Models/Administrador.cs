using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Foody.Models
{
    public class Administrador
    {
        [Key]
        public int idAdministrador { get; set; }
        [Key]
        public int idUtilizador { get; set; }

        public Administrador()
        {

        }
    }
}
