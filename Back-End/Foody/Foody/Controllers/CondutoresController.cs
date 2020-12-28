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
    public class CondutoresController : ControllerBase
    {
        // GET: api/<CondutoresController>
        [HttpGet]
        public Condutor[] Get()
        {
            using (var db = new DbHelper())
            {
                return db.condutor.ToArray();
            }

            //HttpContext.Response.StatusCode = (int)

            //return null;
        }

        // GET api/<CondutoresController>/5
        [HttpGet("{id}")]
        public Condutor Get(int id)
        {

            using (var db = new DbHelper())
            {
                var condutor = db.condutor.ToArray();

                for (int i = 0; i <= condutor.Length; i++)
                {

                    if (condutor[i].idCondutor == id)
                    {
                        return condutor[i];
                    }
                }

                return null;
            }
        }

        //ou

        /*
         public Condutor Get(int id)
        {
            using (var db = new DbHelper())
            {
                return db.condutor.Find(id);
            }
        }
         */

        // POST api/<CondutoresController>
        [HttpPost]
        public string Post([FromBody] Condutor novoCondutor)
        {
            using (var db = new DbHelper())
            {
                var condutor = db.condutor.ToArray();

                for (int i = 0; i < condutor.Length; i++)
                {

                    if (novoCondutor.idCondutor == condutor[i].idCondutor)
                    {
                        return "Já existe";
                    }
                }

                db.condutor.Add(novoCondutor);
                db.SaveChanges();

                return "Criado";
            }
        }
        // ou

        /*
        [HttpPost]
        public string Post([FromBody] Condutor novoCondutor)
        {
            using (var db = new DbHelper())
            {
                cavalo.cod_cavaço = new Random().Next();
                db.condutor.Add(novoCondutor);
                db.SaveChanges();
            }
        }
         */

        // PUT api/<CondutoresController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Condutor condutorUpdate)
        {
            using (var db = new DbHelper())
            {
                var condutorDB = db.condutor.Find(id);

                if (condutorDB == null)
                {
                    Post(condutorUpdate);
                }
                else
                {
                    condutorDB.idCondutor = id;

                    db.condutor.Update(condutorDB);
                    db.SaveChanges();
                }
            }
        }

        // DELETE api/<CondutoresController>/5
        [HttpDelete("{id}")]
        public string Delete(int id)
        {
            using (var db = new DbHelper())
            {
                var condutorDB = db.condutor.Find(id);

                if (condutorDB != null)
                {
                    db.condutor.Remove(condutorDB);
                    db.SaveChanges();

                    return "Eliminado!";
                }
                else
                {
                    return "O Condutor com o id: " + id + " não foi encontrado";
                }
            }
        }
    }
}
