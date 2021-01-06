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
            // obter dados dos utilizadores na base de dados
            using (var db = new DbHelper())
            {
                // devolve-os (dados) num array
                return db.cliente.ToArray();
            }

            //HttpContext.Response.StatusCode = (int)

            //return null;
        }

        // GET api/<ClientesController>/5
        [HttpGet("{idCliente}")]
        public Cliente Get(int id)
        {
            // obter dados do utilizador na base de dados (por id especifico)
            using (var db = new DbHelper())
            {
                // maneira mais simples
                return db.cliente.Find(id);

                // ou: maneira mais complexa
                /*  var cliente = db.cliente.ToArray();

                for (int i = 0; i < cliente.Length; i++)
                {

                    if (cliente[i].idCliente == id)
                    {
                        return cliente[i];
                    }
                }

                return null;  */
            }
        }

        // POST api/<ClientesController>
        [AllowAnonymous]
        [HttpPost]
        public string Post([FromBody] Cliente novoCliente)
        {
            // obter dados do utilizador na base de dados (por id especifico)
            using (var db = new DbHelper())
            {
                // converte-os (dados) num array
                var cliente = db.cliente.ToArray();

                // verifica se id de cliente já existe na BD
                for (int i = 0; i < cliente.Length; i++)
                {
                    if (novoCliente.idCliente == cliente[i].idCliente)
                    {
                        return "Já existe";
                    }
                }

                // se não existir, adiciona um novo cliente
                db.cliente.Add(novoCliente);
                db.SaveChanges();

                return "Criado";
            }
        }

        // PUT api/<ClientesController>/5
        [HttpPut("{id}")]
        public void Put(int idCliente, [FromBody] Cliente clienteUpdate)
        {
            // verificar se utilizador logado é cliente
            if (clienteUpdate != null && clienteUpdate.idCliente == idCliente)
            {
                // obter dados do utilizador na base de dados (por id especifico)
                using (var db = new DbHelper())
                {
                    var clienteDB = db.cliente.Find(idCliente);

                    // se cliente não existir, criar novo
                    if (clienteDB == null)
                    {
                        Post(clienteUpdate);
                    }

                    // se cliente existir, atualizar dados
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
            // obter dados do utilizador na base de dados (por id especifico)
            using (var db = new DbHelper())
            {
                // procura o id do cliente
                var clienteDB = db.cliente.Find(id);

                // se id encontrado (diferente de nulo), 
                // remove o cliente associado
                if (clienteDB != null)
                {
                    db.cliente.Remove(clienteDB);
                    db.SaveChanges();

                    return "Eliminado!";
                }
                else
                {
                    return "O Cliente com o id: " + id + " não foi encontrado";
                }
            }
        }
    }
}
