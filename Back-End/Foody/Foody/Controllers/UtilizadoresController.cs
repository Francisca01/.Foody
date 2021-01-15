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
    public class UtilizadoresController : ControllerBase
    {
        // GET: api/<UtilizadoresController>
        [HttpGet]
        public Utilizador[] Get()
        {
            using (var db = new DbHelper())
            {
                return db.utilizador.ToArray();
            }

            //HttpContext.Response.StatusCode = (int)

            //return null;
        }

        // GET api/<UtilizadoresController>/5
        [HttpGet("{idUtilizador}")]
        public Utilizador Get(int idUtilizador)
        {

            using (var db = new DbHelper())
            {
                var utilizador = db.utilizador.ToArray();

                for (int i = 0; i <= utilizador.Length; i++)
                {

                    if (utilizador[i].idUtilizador == idUtilizador)
                    {
                        return utilizador[i];
                    }
                }

                return null;
            }
        }

        //ou

        /*
         public Utilizador Get(int idUtilizador)
        {
            using (var db = new DbHelper())
            {
                return db.utilizador.Find(idUtilizador);
            }
        }
         */

        // POST api/<UtilizadoresController>
        [HttpPost]
        public string Post([FromBody] Utilizador novoUtilizador)
        {
            using (var db = new DbHelper())
            {
                var utilizador = db.utilizador.ToArray();

                for (int i = 0; i < utilizador.Length; i++)
                {

                    if (novoUtilizador.idUtilizador == utilizador[i].idUtilizador)
                    {
                        return "Já existe";
                    }
                }

                db.utilizador.Add(novoUtilizador);
                db.SaveChanges();

                return "Criado";
            }
        }
        // ou

        /*
        [HttpPost]
        public string Post([FromBody] Utilizador novoUtilizador)
        {
            using (var db = new DbHelper())
            {
                cavalo.cod_cavaço = new Random().Next();
                db.utilizador.Add(novoUtilizador);
                db.SaveChanges();
            }
        }
         */

        // PUT api/<UtilizadoresController>/5
        [HttpPut("{idUtilizador}")]
        public void Put(int idUtilizador, [FromBody] Utilizador utilizadorUpdate)
        {
            using (var db = new DbHelper())
            {
                var utilizadorDB = db.utilizador.Find(idUtilizador);

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

                    db.utilizador.Update(utilizadorDB);
                    db.SaveChanges();
                }
            }
        }

        // DELETE api/<UtilizadoresController>/5
        [HttpDelete("{idUtilizador}")]
        public string Delete(int idUtilizador)
        {
            using (var db = new DbHelper())
            {
                var utilizadorDB = db.utilizador.Find(idUtilizador);

                if (utilizadorDB != null)
                {
                    db.utilizador.Remove(utilizadorDB);
                    db.SaveChanges();

                    return "Eliminado!";
                }
                else
                {
                    return "O Utilizador com o idUtilizador: " + idUtilizador + " não foi encontrado";
                }
            }
        }
    }
}
