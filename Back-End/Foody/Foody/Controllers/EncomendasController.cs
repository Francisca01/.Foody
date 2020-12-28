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
    public class EncomendasController : ControllerBase
    {
        // GET: api/<EncomendasController>
        [HttpGet]
        public Encomenda[] Get()
        {
            using (var db = new DbHelper())
            {
                return db.encomenda.ToArray();
            }

            //HttpContext.Response.StatusCode = (int)

            //return null;
        }

        // GET api/<EncomendasController>/5
        [HttpGet("{id}")]
        public Encomenda Get(int id)
        {

            using (var db = new DbHelper())
            {
                var encomendasDB = db.encomenda.ToArray();

                for (int i = 0; i <= encomendasDB.Length; i++)
                {

                    if (encomendasDB[i].idEncomenda == id)
                    {
                        return encomendasDB[i];
                    }
                }

                return null;
            }
        }

        //ou

        /*
         public Encomenda Get(int id)
        {
            using (var db = new DbHelper())
            {
                return db.encomenda.Find(id);
            }
        }
         */

        // POST api/<EncomendasController>
        [HttpPost]
        public string Post([FromBody] Encomenda novaEncomenda)
        {
            using (var db = new DbHelper())
            {
                var encomendasDB = db.encomenda.ToArray();

                for (int i = 0; i < encomendasDB.Length; i++)
                {

                    if (novaEncomenda.idEncomenda == encomendasDB[i].idEncomenda)
                    {
                        return "Já existe";
                    }
                }

                db.encomenda.Add(novaEncomenda);
                db.SaveChanges();

                return "Criado";
            }
        }
        // ou

        /*
        [HttpPost]
        public string Post([FromBody] Encomenda novaEncomenda)
        {
            using (var db = new DbHelper())
            {
                cavalo.cod_cavaço = new Random().Next();
                db.encomenda.Add(novaEncomenda);
                db.SaveChanges();
            }
        }
         */

        // PUT api/<EncomendasController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Encomenda encomendaUpdate)
        {
            using (var db = new DbHelper())
            {
                var encomendasDB = db.encomenda.Find(id);

                if (encomendasDB == null)
                {
                    Post(encomendaUpdate);
                }
                else
                {
                    encomendasDB.idEncomenda = id;

                    db.encomenda.Update(encomendasDB);
                    db.SaveChanges();
                }
            }
        }

        // DELETE api/<EncomendasController>/5
        [HttpDelete("{id}")]
        public string Delete(int id)
        {
            using (var db = new DbHelper())
            {
                var encomendasDB = db.encomenda.Find(id);

                if (encomendasDB != null)
                {
                    db.encomenda.Remove(encomendasDB);
                    db.SaveChanges();

                    return "Eliminado!";
                }
                else
                {
                    return "A encomenda com o id: " + id + " não foi encontrada";
                }
            }
        }
    }
}
