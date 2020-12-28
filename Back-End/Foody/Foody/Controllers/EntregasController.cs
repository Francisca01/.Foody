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
    public class EntregasController : ControllerBase
    {
        // GET: api/<EntregasController>
        [HttpGet]
        public Entrega[] Get()
        {
            using (var db = new DbHelper())
            {
                return db.entrega.ToArray();
            }

            //HttpContext.Response.StatusCode = (int)

            //return null;
        }

        // GET api/<EntregasController>/5
        [HttpGet("{id}")]
        public Entrega Get(int id)
        {

            using (var db = new DbHelper())
            {
                var entregasDB = db.entrega.ToArray();

                for (int i = 0; i <= entregasDB.Length; i++)
                {

                    if (entregasDB[i].idEntrega == id)
                    {
                        return entregasDB[i];
                    }
                }

                return null;
            }
        }

        //ou

        /*
         public Entrega Get(int id)
        {
            using (var db = new DbHelper())
            {
                return db.entrega.Find(id);
            }
        }
         */

        // POST api/<EntregasController>
        [HttpPost]
        public string Post([FromBody] Entrega novaEntrega)
        {
            using (var db = new DbHelper())
            {
                var entregasDB = db.entrega.ToArray();

                for (int i = 0; i < entregasDB.Length; i++)
                {

                    if (novaEntrega.idEntrega == entregasDB[i].idEntrega)
                    {
                        return "Já existe";
                    }
                }

                db.entrega.Add(novaEntrega);
                db.SaveChanges();

                return "Criado";
            }
        }
        // ou

        /*
        [HttpPost]
        public string Post([FromBody] Entrega novaEntrega)
        {
            using (var db = new DbHelper())
            {
                cavalo.cod_cavaço = new Random().Next();
                db.entrega.Add(novaEntrega);
                db.SaveChanges();
            }
        }
         */

        // PUT api/<EntregasController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Entrega entregaUpdate)
        {
            using (var db = new DbHelper())
            {
                var entregasDB = db.entrega.Find(id);

                if (entregasDB == null)
                {
                    Post(entregaUpdate);
                }
                else
                {
                    entregasDB.idEntrega = id;

                    db.entrega.Update(entregasDB);
                    db.SaveChanges();
                }
            }
        }

        // DELETE api/<EntregasController>/5
        [HttpDelete("{id}")]
        public string Delete(int id)
        {
            using (var db = new DbHelper())
            {
                var entregasDB = db.entrega.Find(id);

                if (entregasDB != null)
                {
                    db.entrega.Remove(entregasDB);
                    db.SaveChanges();

                    return "Eliminado!";
                }
                else
                {
                    return "A entrega com o id: " + id + " não foi encontrada";
                }
            }
        }
    }
}
