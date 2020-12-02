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
    public class ProvasController : ControllerBase
    {
        // GET: api/<ProvasController>
        [HttpGet]
        public Prova[] Get()
        {
            using (var db = new DbHelper())
            {
                return db.provas.ToArray();
            }

            //HttpContext.Response.StatusCode = (int)

            //return null;
        }

        // GET api/<ProvasController>/5
        [HttpGet("{id}")]
        public Prova Get(int id)
        {

            using (var db = new DbHelper())
            {
                var provas = db.provas.ToArray();

                for (int i = 0; i <= provas.Length; i++)
                {

                    if (provas[i].cod_prova == id)
                    {
                        return provas[i];
                    }
                }

                return null;
            }
        }

        //ou

        /*
         public Prova Get(int id)
        {
            using (var db = new DbHelper())
            {
                return db.provas.Find(id);
            }
        }
         */

        // POST api/<ProvasController>
        [HttpPost]
        public string Post([FromBody] Prova novaProva)
        {
            using (var db = new DbHelper())
            {
                var provas = db.provas.ToArray();

                for (int i = 0; i < provas.Length; i++)
                {

                    if (novaProva.cod_prova == provas[i].cod_prova)
                    {
                        return "Já existe";
                    }
                }

                db.provas.Add(novaProva);
                db.SaveChanges();

                return "Criado";
            }
        }
        // ou

        /*
        [HttpPost]
        public string Post([FromBody] Prova novaProva)
        {
            using (var db = new DbHelper())
            {
                cavalo.cod_cavaço = new Random().Next();
                db.provas.Add(novaProva);
                db.SaveChanges();
            }
        }
         */

        // PUT api/<ProvasController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Prova provaUpdate)
        {
            using (var db = new DbHelper())
            {
                var provaDB = db.provas.Find(id);

                if (provaDB == null)
                {
                    Post(provaUpdate);
                }
                else
                {
                    provaDB.cod_prova = id;

                    if (provaUpdate.data != null)
                    {
                        provaDB.data = provaUpdate.data;
                    }

                    if (provaUpdate.nome_prova != null)
                    {
                        provaDB.nome_prova = provaUpdate.nome_prova;
                    }

                    db.provas.Update(provaDB);
                    db.SaveChanges();
                }
            }
        }

        // DELETE api/<ProvasController>/5
        [HttpDelete("{id}")]
        public string Delete(int id)
        {
            using (var db = new DbHelper())
            {
                var provaDB = db.provas.Find(id);

                if (provaDB != null)
                {
                    db.provas.Remove(provaDB);
                    db.SaveChanges();

                    return "Eliminado!";
                }
                else
                {
                    return "A prova com o id: " + id + " não foi encontrada";
                }
            }
        }
    }
}
