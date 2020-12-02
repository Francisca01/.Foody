using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Foody.Models
{
    public class Utilizador
    {
        [Key]
        public int id{ get; set; }
        public string nome { get; set; }
        public string email { get; set; }
        public string palavraPasse { get; set; }

        public Utilizador()
        {

        }
    }
}
