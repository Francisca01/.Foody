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
    public class CoudelariasController : ControllerBase
    {
        // GET: api/<CoudelariasController>
        [HttpGet]
        public Coudelaria[] Get()
        {
            using (var db = new DbHelper())
            {
                return db.coudelarias.ToArray();
            }

            //HttpContext.Response.StatusCode = (int)

            //return null;
        }

        // GET api/<CoudelariasController>/5
        [HttpGet("{id}")]
        public Coudelaria Get(int id)
        {

            using (var db = new DbHelper())
            {
                var coudelarias = db.coudelarias.ToArray();

                for (int i = 0; i <= coudelarias.Length; i++)
                {

                    if (coudelarias[i].cod_coudelaria == id)
                    {
                        return coudelarias[i];
                    }
                }

                return null;
            }
        }

        //ou

        /*
         public Coudelaria Get(int id)
        {
            using (var db = new DbHelper())
            {
                return db.coudelarias.Find(id);
            }
        }
         */

        // POST api/<CoudelariasController>
        [HttpPost]
        public string Post([FromBody] Coudelaria novoCoudelaria)
        {
            using (var db = new DbHelper())
            {
                var coudelarias = db.coudelarias.ToArray();

                for (int i = 0; i < coudelarias.Length; i++)
                {

                    if (novoCoudelaria.cod_coudelaria == coudelarias[i].cod_coudelaria)
                    {
                        return "Já existe";
                    }
                }

                db.coudelarias.Add(novoCoudelaria);
                db.SaveChanges();

                return "Criado";
            }
        }
        // ou

        /*
        [HttpPost]
        public string Post([FromBody] Coudelaria novoCoudelaria)
        {
            using (var db = new DbHelper())
            {
                cavalo.cod_cavaço = new Random().Next();
                db.coudelarias.Add(novoCoudelaria);
                db.SaveChanges();
            }
        }
         */

        // PUT api/<CoudelariasController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Coudelaria coudelariaUpdate)
        {
            using (var db = new DbHelper())
            {
                var coudelariaDB = db.coudelarias.Find(id);

                if (coudelariaDB == null)
                {
                    Post(coudelariaUpdate);
                }
                else
                {
                    coudelariaDB.cod_coudelaria = id;

                    if (coudelariaUpdate.nome_coudelaria != null)
                    {
                        coudelariaDB.nome_coudelaria = coudelariaUpdate.nome_coudelaria;
                    }

                    if (coudelariaUpdate.data_inicio_actividade != null)
                    {
                        coudelariaDB.data_inicio_actividade = coudelariaUpdate.data_inicio_actividade;
                    }

                    /*if (coudelariaUpdate.cod_criador != null)
                    {
                        coudelariaDB.cod_criador = coudelariaUpdate.cod_criador;
                    }*/

                    db.coudelarias.Update(coudelariaDB);
                    db.SaveChanges();
                }
            }
        }

        // DELETE api/<CoudelariasController>/5
        [HttpDelete("{id}")]
        public string Delete(int id)
        {
            using (var db = new DbHelper())
            {
                var coudelariaDB = db.coudelarias.Find(id);

                if (coudelariaDB != null)
                {
                    db.coudelarias.Remove(coudelariaDB);
                    db.SaveChanges();

                    return "Eliminado!";
                }
                else
                {
                    return "A coudelaria com o id: " + id + " não foi encontrado";
                }
            }
        }
    }
}
