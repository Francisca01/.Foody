using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Foody.Models
{
    public class Classific
    {
        [Key]
        public int cod_prova{ get; set; }
        [Key]
        public int cod_cavalo { get; set; }
        public int classific { get; set; }

        public Classific()
        {

        }
    }
}
