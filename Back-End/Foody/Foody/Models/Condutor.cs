using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Foody.Models
{
    // Classe do Condutor
    public class Condutor
    {
        [Key]
        public int idCondutor { get; set; }
        [Key]
        public int idUtilizador { get; set; }
        public string numeroCartaConducao { get; set; }
        public string tipoDeVeiculo { get; set; }

        public Condutor()
        {

        }
    }
}
