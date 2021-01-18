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
    public class DriversController : ControllerBase
    {
        // GET: api/<DriversController>
        [HttpGet]
        public List<object> Get()//so pode ser acedido pelo admin
        {
            //token do user logado
            string token = Request.Headers["token"][0];

            //vai buscar os utilizadores
            return UserService.GetUser(token, 1);
        }

        // GET api/<DriversController>/5
        [HttpGet("{idUser}")]
        public object Get(int idUser)
        {
            //token do user logado
            string token = Request.Headers["token"][0];

            //vai buscar o utilizadore
            return UserService.GetUserId(token, idUser);
        }

        // PUT api/<DriversController>/5
        [HttpPut("{idUser}")]
        public object Put(int idUser, [FromBody] User condutorUpdate)
        {
            //token do user logado
            string token = Request.Headers["token"][0];

            return UserService.PutUser(token, condutorUpdate, idUser);
        }

        // DELETE api/<DriversController>/5
        [HttpDelete("{idUser}")]
        public object Delete(int idUser)
        {
            //token do user logado
            string token = Request.Headers["token"][0];

            return UserService.DeleteUser(token, idUser);
        }
    }
}
