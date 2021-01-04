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
        public int idUtilizador { get; set; }
        public string morada { get; set; }
        public int telemovel { get; set; }
        public int tipoUtilizador { get; set; }
        // 0 - cliente 
        // 1 - condutor 
        // 2 - empresa
        public string tipoVeiculo { get; set; }
        public int nif { get; set; }
        public string numeroCartaConducao { get; set; }
        public string nome { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string dataNascimento { get; set; }

        public Utilizador()
        {

        }
    }
}
