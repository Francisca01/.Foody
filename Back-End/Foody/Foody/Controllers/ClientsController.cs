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
            string token = Request.Headers["token"][0];

            //vai buscar os utilizadores
            return UserService.GetUser(token, 0);
        }

        // GET api/<ClientsController>/5
        [HttpGet("{idUser}")]
        public object Get(int idUser)
        {
            //token do user logado
            string token = Request.Headers["token"][0];

            //vai buscar o utilizadore
            return UserService.GetUserId(token, idUser);
        }

        // PUT api/<ClientsController>/5
        [HttpPut("{idUser}")]
        public object Put(int idUser, [FromBody] User clienteUpdate)
        {
            //token do user logado
            string token = Request.Headers["token"][0];

            return UserService.PutUser(token, clienteUpdate, idUser);
        }

        // DELETE api/<ClientsController>/5
        [HttpDelete("{idUser}")]
        public object Delete(int idUser)
        {
            //token do user logado
            string token = Request.Headers["token"][0];

            return UserService.DeleteUser(token, idUser);
        }
    }
}
