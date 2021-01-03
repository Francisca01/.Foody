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
        }

        // GET api/<EncomendasController>/5
        [HttpGet("{idEncomenda}")]
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

        // POST api/<EncomendasController>
        [HttpPost]
        public string Post([FromBody] Encomenda novaEncomenda)
        {
            if (novaEncomenda != null)
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
            else
            {
                return "Não foi recebido qualquer tipo de dados!";
            }
        }

        /*
        // PUT api/<EncomendasController>/5
        [HttpPut("{idEncomenda}")]
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
        [HttpDelete("{idEncomenda}")]
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
        }*/
    }
}
