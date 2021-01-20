using System.Collections.Generic;
using Foody.Models;
using Foody.Utils;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Foody.Controllers
{
    [Route("api/[controller]")]
    //[ApiController]
    public class ClientsController : ControllerBase
    {
        // GET: api/<ClientsController>
        [HttpGet]
        public List<object> Get()//so pode ser acedido pelo admin
        {
            //token do user logado
            string token = Request.Headers["token"];

            if (token != null)
            {
                //vai buscar os utilizadores
                return UserService.GetUser(token, 0);
            }
            else
            {
                List<object> msg = new List<object>() { MessageService.AccessDenied() };
                return msg;
            }
        }

        // GET api/<ClientsController>/5
        [HttpGet("{idUser}")]
        public object Get(int idUser)
        {
            //token do user logado
            string token = Request.Headers["token"];

            if (token != null)
            {
                //vai buscar o utilizadore
                return UserService.GetUserId(token, idUser);
            }
            else
            {
                List<object> msg = new List<object>() { MessageService.AccessDenied() };
                return msg;
            }
        }

        // PUT api/<ClientsController>/5
        [HttpPut("{idUser}")]
        public object Put(int idUser, [FromBody] User clientUpdate)
        {
            //token do user logado
            string token = Request.Headers["token"];

            if (token != null)
            {
                return UserService.PutUser(token, clientUpdate, idUser);
            }
            else
            {
                List<object> msg = new List<object>() { MessageService.AccessDenied() };
                return msg;
            }
        }

        // DELETE api/<ClientsController>/5
        [HttpDelete("{idUser}")]
        public object Delete(int idUser)
        {
            //token do user logado
            string token = Request.Headers["token"];

            if (token != null)
            {
                return UserService.DeleteUser(token, idUser);
            }
            else
            {
                List<object> msg = new List<object>() { MessageService.AccessDenied() };
                return msg;
            }
        }
    }
}
