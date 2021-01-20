using System.ComponentModel.DataAnnotations;

namespace Foody.Models
{
    // Classe de Order
    public class Order
    {
        [Key]
        public int idOrder{ get; set; }
        public int idClient { get; set; }        
        public int state { get; set; }
        //0 - por pagar
        //1 - pago

        public Order()
        {

        }
    }
}
