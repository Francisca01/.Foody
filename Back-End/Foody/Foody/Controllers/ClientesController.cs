using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Foody.Models;
using Foody.Utils;
using Microsoft.AspNetCore.Authorization;
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
        public Cliente[] Get()
        {
            using (var db = new DbHelper())
            {
                return db.cliente.ToArray();
            }

            //HttpContext.Response.StatusCode = (int)

            //return null;
        }

        // GET api/<ClientesController>/5
        [HttpGet("{idCliente}")]
        public Cliente Get(int id)
        {

            using (var db = new DbHelper())
            {
                var cliente = db.cliente.ToArray();

                for (int i = 0; i < cliente.Length; i++)
                {

                    if (cliente[i].idCliente == id)
                    {
                        return cliente[i];
                    }
                }

                return null;
            }
        }

        // POST api/<ClientesController>
        [AllowAnonymous]
        [HttpPost]
        public string Post([FromBody] Cliente novoCliente)
        {
            using (var db = new DbHelper())
            {
                var cliente = db.cliente.ToArray();

                for (int i = 0; i < cliente.Length; i++)
                {

                    if (novoCliente.idCliente == cliente[i].idCliente)
                    {
                        return "Já existe";
                    }
                }

                db.cliente.Add(novoCliente);
                db.SaveChanges();

                return "Criado";
            }
        }

        // PUT api/<ClientesController>/5
        [HttpPut("{id}")]
        public void Put(int idCliente, [FromBody] Cliente clienteUpdate)
        {
            if (clienteUpdate != null && clienteUpdate.idCliente == idCliente)
            {
                using (var db = new DbHelper())
                {
                    var clienteDB = db.cliente.Find(idCliente);

                    if (clienteDB == null)
                    {
                        Post(clienteUpdate);
                    }
                    else
                    {
                        clienteDB.idCliente = idCliente;

                        db.cliente.Update(clienteDB);
                        db.SaveChanges();
                    }
                }
            }
        }

        // DELETE api/<ClientesController>/5
        [HttpDelete("{id}")]
        public string Delete(int id)
        {
            using (var db = new DbHelper())
            {
                var clienteDB = db.cliente.Find(id);

                if (clienteDB != null)
                {
                    db.cliente.Remove(clienteDB);
                    db.SaveChanges();

                    return "Eliminado!";
                }
                else
                {
                    return "O cavalo com o id: " + id + " não foi encontrado";
                }
            }
        }
    }
}
