using System.ComponentModel.DataAnnotations;

namespace Foody.Models
{
    // Classe de Delivery
    public class Delivery
    {
        [Key]
        public int idDelivery { get; set; }
        public int idDriver { get; set; }
        public int idOrder { get; set; }
        public string state { get; set; }

        public Delivery()
        {

        }
    }
}
