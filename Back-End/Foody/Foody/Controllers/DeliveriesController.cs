using System.Collections.Generic;
using System.Linq;
using Foody.Models;
using Foody.Utils;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Foody.Controllers
{
    [Route("api/[controller]")]
    //[ApiController]
    public class DeliveriesController : ControllerBase
    {
        // GET: api/<DeliveriesController>
        [HttpGet]
        public List<object> Get()
        {
            //token do user logado
            string token = Request.Headers["token"];

            int[] userLoggedIn = UserService.UserLoggedIn(token);

            if (userLoggedIn != null)
            {
                return DeliveryService.GetDeliveries(userLoggedIn);
            }
            else
            {
                List<object> msg = new List<object>() { MessageService.AccessDenied() };
                return msg;
            }
        }

        // GET api/<DeliveriesController>/5
        [HttpGet("{idDelivery}")]
        public object Get(int idDelivery)
        {
            //token do user logado
            string token = Request.Headers["token"];

            if (token != null)
            {
                return DeliveryService.GetDeliveryId(token, idDelivery);
            }
            else
            {
                List<object> msg = new List<object>() { MessageService.AccessDenied() };
                return msg;
            }            
        }

        // PUT api/<DeliveriesController>/5
        [HttpPut("{idDelivery}")]
        public Message Put(int idDelivery, [FromBody] Delivery deliveryUpdate)
        {
            //token do user logado
            string token = Request.Headers["token"];

            int[] userLoggedIn = UserService.UserLoggedIn(token);

            if (userLoggedIn != null)
            {
                return DeliveryService.ChangeDeliveryState(userLoggedIn[0], deliveryUpdate, idDelivery);
            }
            else
            {
                return MessageService.AccessDenied();
            }
        }

        //NOTA: Podem ser eliminadas, mas após delivery ser concluida
        // Adicionar um status à delivery, quando concluido, eliminada


        // Por questões de política e privacidade de dados: Entregas não podem ser eliminadas
    }
}
