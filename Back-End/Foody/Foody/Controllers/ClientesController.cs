using System.Collections.Generic;
using Foody.Models;
using Foody.Utils;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Foody.Controllers
{
    [Route("api/[controller]")]
    //[ApiController]
    public class ClientesController : ControllerBase
    {
        // GET: api/<ClientesController>
        [HttpGet]
        public List<object> Get()//so pode ser acedido pelo admin
        {
            //token do user logado
            string token = Request.Headers["token"][0];

            //vai buscar os utilizadores
            return UserService.GetUser(token, 0);
        }

        // GET api/<ClientesController>/5
        [HttpGet("{idUtilizador}")]
        public object Get(int idUtilizador)
        {
            //token do user logado
            string token = Request.Headers["token"][0];

            //vai buscar o utilizadore
            return UserService.GetUserId(token, idUtilizador);
        }

        // PUT api/<ClientesController>/5
        [HttpPut("{idUtilizador}")]
        public object Put(int idUtilizador, [FromBody] Utilizador clienteUpdate)
        {
            //token do user logado
            string token = Request.Headers["token"][0];

            return UserService.PutUser(token, clienteUpdate, idUtilizador);
        }

        // DELETE api/<ClientesController>/5
        [HttpDelete("{idUtilizador}")]
        public object Delete(int idUtilizador)
        {
            //token do user logado
            string token = Request.Headers["token"][0];

            return UserService.DeleteUser(token, idUtilizador);
        }
    }
}
