using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public Cliente[] Get()
        {
            using (var db = new DbHelper())
            {
                return db.clientes.ToArray();
            }

            //HttpContext.Response.StatusCode = (int)

            //return null;
        }

        // GET api/<ClientesController>/5
        [HttpGet("{id}")]
        public Cliente Get(int id)
        {

            using (var db = new DbHelper())
            {
                var clientes = db.clientes.ToArray();

                for (int i = 0; i < clientes.Length; i++)
                {

                    if (clientes[i].idCliente == id)
                    {
                        return clientes[i];
                    }
                }

                return null;
            }
        }

        //ou

        /*
         public Cliente Get(int id)
        {
            using (var db = new DbHelper())
            {
                return db.clientes.Find(id);
            }
        }
         */

        // POST api/<ClientesController>
        [HttpPost]
        public string Post([FromBody] Cliente novoCliente)
        {
            using (var db = new DbHelper())
            {
                var clientes = db.clientes.ToArray();

                for (int i = 0; i < clientes.Length; i++)
                {

                    if (novoCliente.idCliente == clientes[i].idCliente)
                    {
                        return "Já existe";
                    }
                }

                db.clientes.Add(novoCliente);
                db.SaveChanges();

                return "Criado";
            }
        }
        // ou

        /*
        [HttpPost]
        public string Post([FromBody] Cliente novoCliente)
        {
            using (var db = new DbHelper())
            {
                cavalo.cod_cavaço = new Random().Next();
                db.clientes.Add(novoCliente);
                db.SaveChanges();
            }
        }
         */

        // PUT api/<ClientesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Cliente clienteUpdate)
        {
            using (var db = new DbHelper())
            {
                var clienteDB = db.clientes.Find(id);

                if (clienteDB == null)
                {
                    Post(clienteUpdate);
                }
                else
                {
                    clienteDB.idCliente = id;

                    db.clientes.Update(clienteDB);
                    db.SaveChanges();
                }
            }
        }

        // DELETE api/<ClientesController>/5
        [HttpDelete("{id}")]
        public string Delete(int id)
        {
            using (var db = new DbHelper())
            {
                var clienteDB = db.clientes.Find(id);

                if (clienteDB != null)
                {
                    db.clientes.Remove(clienteDB);
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
