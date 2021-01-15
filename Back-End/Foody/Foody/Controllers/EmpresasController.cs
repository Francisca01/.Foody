using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Foody.Models;
using Foody.Utils;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Foody.Controllers
{
    [Route("api/[controller]")]
    //[ApiController]
    public class EmpresasController : ControllerBase
    {
        // GET: api/<EmpresasController>
        [HttpGet]
        public List<object> Get()//so pode ser acedido pelo admin
        {
            //token do user logado
            string token = Request.Headers["token"][0];

            //vai buscar os utilizadores
            return UserService.GetUser(token, 2);
        }

        // GET api/<EmpresasController>/5
        [HttpGet("{idUtilizador}")]
        public object Get(int idUtilizador)
        {
            //token do user logado
            string token = Request.Headers["token"][0];

            //vai buscar o utilizadore
            return UserService.GetUserId(token, idUtilizador);
        }
        
        // PUT api/<EmpresasController>/5
        [HttpPut("{idUtilizador}")]
        public object Put(int idUtilizador, [FromBody] Utilizador empresaUpdate)
        {
            //token do user logado
            string token = Request.Headers["token"][0];

            return UserService.PutUser(token, empresaUpdate, idUtilizador);
        }

        // DELETE api/<EmpresasController>/5
        [HttpDelete("{idUtilizador}")]
        public object Delete(int idUtilizador)
        {
            //token do user logado
            string token = Request.Headers["token"][0];

            return UserService.DeleteUser(token, idUtilizador);
        }
    }
}
