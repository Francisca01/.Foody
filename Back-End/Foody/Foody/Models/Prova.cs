using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Foody.Models
{
    public class Prova
    {
        [Key]
        public int cod_prova { get; set; }
        public string nome_prova { get; set; }
        public DateTime data { get; set; }
        

        public Prova()
        {

        }
    }
}
