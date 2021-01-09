using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Foody.Models;
using Foody.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Net.Http;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Foody.Controllers
{
    [Route("api/[controller]")]
    //[ApiController]
    public class ClientesController : ControllerBase
    {

        // GET: api/<ClientesController>
        [HttpGet]
        public List<object> Get()
        {
            //token do user logado
            string token = Request.Headers["token"][0];

            //verifica se utilizador Logado pode aceder
            bool canAccess = UserService.VerifyUser(token, 3);

            if (canAccess)
            {
                // obter dados dos utilizadores na base de dados
                using (var db = new DbHelper())
                {
                    //var aaaaaa = HttpContext.;
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

                    List<object> cls = new List<object>() { clientes };
                    return cls;
                }
            }
            else
            {
                List<object> msg = new List<object>() { MessageService.AccessDeniedMessage() };
                return msg;
            }

            //HttpContext.Response.StatusCode = (int)

            //return null;
        }


        // GET api/<ClientesController>/5
        [HttpGet("{idUtilizador}")]
        public object Get(int idUtilizador)
        {
            //token do user logado
            string token = Request.Headers["token"][0];

            //verifica se utilizador Logado pode aceder
            bool canAccess = UserService.VerifyUser(token, idUtilizador);

            if (canAccess)
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
                            return MessageService.WithoutResultsMessage();
                        }
                    }
                    else
                    {
                        return MessageService.WithoutResultsMessage();
                    }
                }
            }
            else
            {
                return MessageService.AccessDeniedMessage();
            }
        }

        // PUT api/<ClientesController>/5
        [HttpPut("{idUtilizador}")]
        public object Put(int idUtilizador, [FromBody] Utilizador clienteUpdate)
        {
            //token do user logado
            string token = Request.Headers["token"][0];

            //verifica se utilizador Logado pode aceder
            bool canAccess = UserService.VerifyUser(token, idUtilizador);

            // verificar se utilizador logado é cliente
            if (clienteUpdate != null)
            {
                if (canAccess)
                {
                    // obter dados do utilizador na base de dados (por id especifico)
                    DbHelper db = new DbHelper();
                    var clienteDB = db.utilizador.Find(idUtilizador);

                    // se cliente não existir, criar novo
                    if (clienteDB == null)
                    {
                        return MessageService.WithoutResultsMessage();
                    }

                    // se cliente existir, atualizar dados
                    else
                    {
                        string msg = UserService.ValidateUser(clienteUpdate, true);
                        return MessageService.CustomMessage(msg);
                    }

                }
                else
                {
                    return MessageService.AccessDeniedMessage();
                }
            }
            else
            {
                return MessageService.WithoutResultsMessage();
            }
        }

        // DELETE api/<ClientesController>/5
        [HttpDelete("{idUtilizador}")]
        public object Delete(int idUtilizador)
        {
            //token do user logado
            string token = Request.Headers["token"][0];

            //verifica se utilizador Logado pode aceder
            bool canAccess = UserService.VerifyUser(token, idUtilizador);

            if (canAccess)
            {
                // obter dados do utilizador na base de dados (por id especifico)
                using (var db = new DbHelper())
                {
                    // procura o id do cliente
                    var clienteDB = db.utilizador.Find(idUtilizador);

                    // se id encontrado (diferente de nulo), 
                    // remove o cliente associado
                    if (clienteDB != null)
                    {
                        db.utilizador.Remove(clienteDB);
                        db.SaveChanges();

                        return MessageService.CustomMessage("Eliminado!");
                    }
                    else
                    {
                        return MessageService.WithoutResultsMessage();
                    }
                }
            }
            else
            {
                return MessageService.AccessDeniedMessage();
            }
        }
    }
}
