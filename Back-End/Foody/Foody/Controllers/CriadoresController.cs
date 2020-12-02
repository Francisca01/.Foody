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
    public class CriadoresController : ControllerBase
    {
        // GET: api/<CriadoresController>
        [HttpGet]
        public Criador[] Get()
        {
            using (var db = new DbHelper())
            {
                return db.criadores.ToArray();
            }

            //HttpContext.Response.StatusCode = (int)

            //return null;
        }

        // GET api/<CriadoresController>/5
        [HttpGet("{id}")]
        public Criador Get(int id)
        {

            using (var db = new DbHelper())
            {
                var criadores = db.criadores.ToArray();

                for (int i = 0; i <= criadores.Length; i++)
                {

                    if (criadores[i].cod_criador == id)
                    {
                        return criadores[i];
                    }
                }

                return null;
            }
        }

        //ou

        /*
         public Criador Get(int id)
        {
            using (var db = new DbHelper())
            {
                return db.criadores.Find(id);
            }
        }
         */

        // POST api/<CriadoresController>
        [HttpPost]
        public string Post([FromBody] Criador novoCriador)
        {
            using (var db = new DbHelper())
            {
                var criadores = db.criadores.ToArray();

                for (int i = 0; i < criadores.Length; i++)
                {

                    if (novoCriador.cod_criador == criadores[i].cod_criador)
                    {
                        return "Já existe";
                    }
                }

                db.criadores.Add(novoCriador);
                db.SaveChanges();

                return "Criado";
            }
        }
        // ou

        /*
        [HttpPost]
        public string Post([FromBody] Criador novoCriador)
        {
            using (var db = new DbHelper())
            {
                cavalo.cod_cavaço = new Random().Next();
                db.criadores.Add(novoCriador);
                db.SaveChanges();
            }
        }
         */

        // PUT api/<CriadoresController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Criador criadorUpdate)
        {
            using (var db = new DbHelper())
            {
                var criadorDB = db.criadores.Find(id);

                if (criadorDB == null)
                {
                    Post(criadorUpdate);
                }
                else
                {
                    criadorDB.cod_criador = id;

                    if (criadorUpdate.nome != null)
                    {
                        criadorDB.nome = criadorUpdate.nome;
                    }

                    db.criadores.Update(criadorDB);
                    db.SaveChanges();
                }
            }
        }

        // DELETE api/<CriadoresController>/5
        [HttpDelete("{id}")]
        public string Delete(int id)
        {
            using (var db = new DbHelper())
            {
                var criadorDB = db.criadores.Find(id);

                if (criadorDB != null)
                {
                    db.criadores.Remove(criadorDB);
                    db.SaveChanges();

                    return "Eliminado!";
                }
                else
                {
                    return "O Criador com o id: " + id + " não foi encontrado";
                }
            }
        }
    }
}
