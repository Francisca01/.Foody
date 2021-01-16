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
    public class UsersController : ControllerBase
    {
        // GET: api/<UsersController>
        [HttpGet]
        public User[] Get()
        {
            using (var db = new DbHelper())
            {
                return db.user.ToArray();
            }

            //HttpContext.Response.StatusCode = (int)

            //return null;
        }

        // GET api/<UsersController>/5
        [HttpGet("{idUtilizador}")]
        public User Get(int idUtilizador)
        {

            using (var db = new DbHelper())
            {
                var user = db.user.ToArray();

                for (int i = 0; i <= user.Length; i++)
                {

                    if (user[i].idUtilizador == idUtilizador)
                    {
                        return user[i];
                    }
                }

                return null;
            }
        }

        //ou

        /*
         public User Get(int idUtilizador)
        {
            using (var db = new DbHelper())
            {
                return db.user.Find(idUtilizador);
            }
        }
         */

        // POST api/<UsersController>
        [HttpPost]
        public string Post([FromBody] User novoUtilizador)
        {
            using (var db = new DbHelper())
            {
                var user = db.user.ToArray();

                for (int i = 0; i < user.Length; i++)
                {

                    if (novoUtilizador.idUtilizador == user[i].idUtilizador)
                    {
                        return "Já existe";
                    }
                }

                db.user.Add(novoUtilizador);
                db.SaveChanges();

                return "Criado";
            }
        }
        // ou

        /*
        [HttpPost]
        public string Post([FromBody] User novoUtilizador)
        {
            using (var db = new DbHelper())
            {
                cavalo.cod_cavaço = new Random().Next();
                db.user.Add(novoUtilizador);
                db.SaveChanges();
            }
        }
         */

        // PUT api/<UsersController>/5
        [HttpPut("{idUtilizador}")]
        public void Put(int idUtilizador, [FromBody] User utilizadorUpdate)
        {
            using (var db = new DbHelper())
            {
                var utilizadorDB = db.user.Find(idUtilizador);

                if (utilizadorDB == null)
                {
                    Post(utilizadorUpdate);
                }
                else
                {
                    utilizadorDB.idUtilizador = idUtilizador;

                    if (utilizadorUpdate.email != null)
                    {
                        utilizadorDB.email = utilizadorUpdate.email;
                    }

                    if (utilizadorUpdate.nome != null)
                    {
                        utilizadorDB.nome = utilizadorUpdate.nome;
                    }

                    if (utilizadorUpdate.password != null)
                    {
                        utilizadorDB.password = utilizadorUpdate.password;
                    }

                    db.user.Update(utilizadorDB);
                    db.SaveChanges();
                }
            }
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{idUtilizador}")]
        public string Delete(int idUtilizador)
        {
            using (var db = new DbHelper())
            {
                var utilizadorDB = db.user.Find(idUtilizador);

                if (utilizadorDB != null)
                {
                    db.user.Remove(utilizadorDB);
                    db.SaveChanges();

                    return "Eliminado!";
                }
                else
                {
                    return "O User com o idUtilizador: " + idUtilizador + " não foi encontrado";
                }
            }
        }
    }
}
