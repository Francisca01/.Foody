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
        [Key]
        public int idMorada { get; set; }
        public int telemovel { get; set; }
        public string nome { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public DateTime dataNascimento { get; set; }

        public Utilizador()
        {

        }
    }
}
