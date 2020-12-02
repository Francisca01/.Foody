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
    public class CavalosController : ControllerBase
    {
        // GET: api/<CavalosController>
        [HttpGet]
        public Cavalo[] Get()
        {
            using (var db = new DbHelper())
            {
                return db.cavalos.ToArray();
            }

            //HttpContext.Response.StatusCode = (int)

            //return null;
        }

        // GET api/<CavalosController>/5
        [HttpGet("{id}")]
        public Cavalo Get(int id)
        {

            using (var db = new DbHelper())
            {
                var cavalos = db.cavalos.ToArray();

                for (int i = 0; i < cavalos.Length; i++)
                {

                    if (cavalos[i].cod_cavalo == id)
                    {
                        return cavalos[i];
                    }
                }

                return null;
            }
        }

        //ou

        /*
         public Cavalo Get(int id)
        {
            using (var db = new DbHelper())
            {
                return db.cavalos.Find(id);
            }
        }
         */

        // POST api/<CavalosController>
        [HttpPost]
        public string Post([FromBody] Cavalo novoCavalo)
        {
            using (var db = new DbHelper())
            {
                var cavalos = db.cavalos.ToArray();

                for (int i = 0; i < cavalos.Length; i++)
                {

                    if (novoCavalo.cod_cavalo == cavalos[i].cod_cavalo)
                    {
                        return "Já existe";
                    }
                }

                db.cavalos.Add(novoCavalo);
                db.SaveChanges();

                return "Criado";
            }
        }
        // ou

        /*
        [HttpPost]
        public string Post([FromBody] Cavalo novoCavalo)
        {
            using (var db = new DbHelper())
            {
                cavalo.cod_cavaço = new Random().Next();
                db.cavalos.Add(novoCavalo);
                db.SaveChanges();
            }
        }
         */

        // PUT api/<CavalosController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Cavalo cavaloUpdate)
        {
            using (var db = new DbHelper())
            {
                var cavaloDB = db.cavalos.Find(id);

                if (cavaloDB == null)
                {
                    Post(cavaloUpdate);
                }
                else
                {
                    cavaloDB.cod_cavalo = id;

                    if (cavaloUpdate.nome_cavalo != null)
                    {
                        cavaloDB.nome_cavalo = cavaloUpdate.nome_cavalo;
                    }

                    if (cavaloUpdate.cod_coudelaria_nasc != null)
                    {
                        cavaloDB.cod_coudelaria_nasc = cavaloUpdate.cod_coudelaria_nasc;
                    }

                    if (cavaloUpdate.data_nascimento != null)
                    {
                        cavaloDB.data_nascimento = cavaloUpdate.data_nascimento;
                    }

                    if (cavaloUpdate.genero != null)
                    {
                        cavaloDB.genero = cavaloUpdate.genero;
                    }

                    if (cavaloUpdate.mae != null)
                    {
                        cavaloDB.mae = cavaloUpdate.mae;
                    }

                    if (cavaloUpdate.pai != null)
                    {
                        cavaloDB.pai = cavaloUpdate.pai;
                    }

                    db.cavalos.Update(cavaloDB);
                    db.SaveChanges();
                }
            }
        }

        // DELETE api/<CavalosController>/5
        [HttpDelete("{id}")]
        public string Delete(int id)
        {
            using (var db = new DbHelper())
            {
                var cavaloDB = db.cavalos.Find(id);

                if (cavaloDB != null)
                {
                    db.cavalos.Remove(cavaloDB);
                    db.SaveChanges();

                    return "Eliminado!";
                }
                else
                {
                    return "O cavalo com o id: " + id + " não foi encontrado";
                }
            }
        }
    }
}
