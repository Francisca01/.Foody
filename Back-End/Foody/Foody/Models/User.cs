using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Foody.Models
{
    // Classe do User
    public class User
    {
        [Key]
        public int idUser { get; set; }
        public string address { get; set; }
        public int phone { get; set; }
        public int userType { get; set; }
        // 0 - cliente 
        // 1 - condutor 
        // 2 - empresa
        public string vehicleType { get; set; }
        public string nif { get; set; }
        public string drivingLicense { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string birthDate { get; set; }

        public User()
        {

        }
    }
}
