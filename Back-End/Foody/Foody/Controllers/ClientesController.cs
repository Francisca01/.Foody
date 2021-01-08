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
        public List<Utilizador> Get()
        {
            // obter dados dos utilizadores na base de dados
            using (var db = new DbHelper())
            {
                // devolve os dados da base de dados num array
                var clientesDB = db.utilizador.ToArray();

                //array para devolver o resultado
                List<Utilizador> clientes = new List<Utilizador>();

                //incrementador

                for (int i = 0; i < clientesDB.Length; i++)
                {
                    if (clientesDB[i].tipoUtilizador == 0)//verifica se o utilizador é cliente
                    {
                        clientes.Add(clientesDB[i]);
                    }
                }

                return clientes;
            }

            //HttpContext.Response.StatusCode = (int)

            //return null;
        }


        // GET api/<ClientesController>/5
        [HttpGet("{idUtilizador}")]
        public object Get(int idUtilizador)
        {
            // obter dados do utilizador na base de dados (por id especifico)
            using (var db = new DbHelper())
            {
                // maneira mais simples
                var cliente = db.utilizador.Find(idUtilizador);

                if (cliente != null)
                {
                    if (cliente.tipoUtilizador == 0)
                    {
                        return cliente;

                    }
                    else
                    {
                        return MessageService.MessagemSemResultados();
                    }
                }
                else
                {
                    return MessageService.MessagemSemResultados();
                }
            }
        }

        // PUT api/<ClientesController>/5
        [HttpPut("{idUtilizador}")]
        public object Put(int idUtilizador, [FromBody] Utilizador clienteUpdate)//testar
        {
            // verificar se utilizador logado é cliente
            if (clienteUpdate != null)
            {
                if (clienteUpdate.idUtilizador == idUtilizador)
                {
                    // obter dados do utilizador na base de dados (por id especifico)
                    using (var db = new DbHelper())
                    {
                        var clienteDB = db.utilizador.Find(idUtilizador);

                        // se cliente não existir, criar novo
                        if (clienteDB == null)
                        {
                            return MessageService.MessagemSemResultados();
                        }

                        // se cliente existir, atualizar dados
                        else
                        {
                            UserService.CriarEditarUtilizador(db, clienteUpdate, true);
                            return MessageService.MessagemCustomizada("Cliente Atualizado!");

                        }
                    }
                }
                else
                {
                    return MessageService.MessagemCustomizada("Não tem permissão para editar o cliente!"); ;
                }
            }
            else
            {
                return MessageService.MessagemSemResultados();
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
