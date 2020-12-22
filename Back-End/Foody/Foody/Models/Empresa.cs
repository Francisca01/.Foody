using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Foody.Models
{
    public class Empresa
    {
        [Key]
        public int idEmpresa { get; set; }
        [Key]
        public int idUtilizador { get; set; }
        public int nif { get; set; }
        

        public Empresa()
        {

        }
    }
}
