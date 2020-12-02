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
                return db.utilizadores.ToArray();
            }

            //HttpContext.Response.StatusCode = (int)

            //return null;
        }

        // GET api/<UtilizadoresController>/5
        [HttpGet("{id}")]
        public Utilizador Get(int id)
        {

            using (var db = new DbHelper())
            {
                var utilizadores = db.utilizadores.ToArray();

                for (int i = 0; i <= utilizadores.Length; i++)
                {

                    if (utilizadores[i].id == id)
                    {
                        return utilizadores[i];
                    }
                }

                return null;
            }
        }

        //ou

        /*
         public Utilizador Get(int id)
        {
            using (var db = new DbHelper())
            {
                return db.utilizadores.Find(id);
            }
        }
         */

        // POST api/<UtilizadoresController>
        [HttpPost]
        public string Post([FromBody] Utilizador novoUtilizador)
        {
            using (var db = new DbHelper())
            {
                var utilizadores = db.utilizadores.ToArray();

                for (int i = 0; i < utilizadores.Length; i++)
                {

                    if (novoUtilizador.id == utilizadores[i].id)
                    {
                        return "Já existe";
                    }
                }

                db.utilizadores.Add(novoUtilizador);
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
                db.utilizadores.Add(novoUtilizador);
                db.SaveChanges();
            }
        }
         */

        // PUT api/<UtilizadoresController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Utilizador utilizadorUpdate)
        {
            using (var db = new DbHelper())
            {
                var utilizadorDB = db.utilizadores.Find(id);

                if (utilizadorDB == null)
                {
                    Post(utilizadorUpdate);
                }
                else
                {
                    utilizadorDB.id = id;

                    if (utilizadorUpdate.email != null)
                    {
                        utilizadorDB.email = utilizadorUpdate.email;
                    }

                    if (utilizadorUpdate.nome != null)
                    {
                        utilizadorDB.nome = utilizadorUpdate.nome;
                    }

                    if (utilizadorUpdate.palavraPasse != null)
                    {
                        utilizadorDB.palavraPasse = utilizadorUpdate.palavraPasse;
                    }

                    db.utilizadores.Update(utilizadorDB);
                    db.SaveChanges();
                }
            }
        }

        // DELETE api/<UtilizadoresController>/5
        [HttpDelete("{id}")]
        public string Delete(int id)
        {
            using (var db = new DbHelper())
            {
                var utilizadorDB = db.utilizadores.Find(id);

                if (utilizadorDB != null)
                {
                    db.utilizadores.Remove(utilizadorDB);
                    db.SaveChanges();

                    return "Eliminado!";
                }
                else
                {
                    return "O Utilizador com o id: " + id + " não foi encontrado";
                }
            }
        }
    }
}
