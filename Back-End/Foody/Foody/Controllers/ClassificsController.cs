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
    public class ClassificsController : ControllerBase
    {
        // GET: api/<ClassificsController>
        [HttpGet]
        public Classific[] Get()
        {
            using (var db = new DbHelper())
            {
                return db.classifics.ToArray();
            }
        }

        // GET api/<ClassificsController>/5
        [HttpGet("{idProva}/{idCavalo}")]
        public Classific GetCodProva(int idProva, int idCavalo)
        {
            using (var db = new DbHelper())
            {
                var classifics = db.classifics.ToArray();

                for (int i = 0; i < classifics.Length; i++)
                {

                    if (classifics[i].cod_prova == idProva && classifics[i].cod_cavalo == idCavalo)
                    {
                        return classifics[i];
                    }
                }

                return null;
            }
        }

        //ou

        /*
         public Classific Get(int id)
        {
            using (var db = new DbHelper())
            {
                return db.classifics.Find(id);
            }
        }
         */

        // POST api/<ClassificsController>
        [HttpPost]
        public string Post([FromBody] Classific novoClassific)
        {
            using (var db = new DbHelper())
            {
                var classifics = db.classifics.ToArray();

                for (int i = 0; i < classifics.Length; i++)
                {
                    if (novoClassific.cod_cavalo == classifics[i].cod_cavalo && novoClassific.cod_prova == classifics[i].cod_prova)
                    {
                        return "Já existe";
                    }
                }

                db.classifics.Add(novoClassific);
                db.SaveChanges();

                return "Criado";
            }

        }
        // ou

        /*
        [HttpPost]
        public string Post([FromBody] Classific novoClassific)
        {
            using (var db = new DbHelper())
            {
                Classific.cod_cavaço = new Random().Next();
                db.classifics.Add(novoClassific);
                db.SaveChanges();
            }
        }
         */

        // PUT api/<ClassificsController>/5
        [HttpPut("{idProva}/{idCavalo}")]
        public void Put(int idProva, int idCavalo, [FromBody] Classific classificUpdate)
        {
            using (var db = new DbHelper())
            {
                var classificDB = db.classifics.Find(idProva, idCavalo);

                if (classificDB == null)
                {
                    Post(classificUpdate);
                }

                else
                {
                    classificDB.cod_prova = idProva;
                    classificDB.cod_cavalo = idCavalo;

                    classificDB.classific = classificUpdate.classific;

                    db.classifics.Update(classificDB);
                    db.SaveChanges();
                }
            }

        }

        // DELETE api/<ClassificsController>/5
        [HttpDelete("{idProva}/{idCavalo}")]
        public string Delete(int idProva, int idCavalo)
        {
            using (var db = new DbHelper())
            {
                var classificDB = db.classifics.Find(idProva, idCavalo);

                if (classificDB != null)
                {
                    db.classifics.Remove(classificDB);
                    db.SaveChanges();

                    return "Eliminado!";
                }
                else
                {
                    return "O Classific com o id de prova: " + idProva + 
                        " e o Cavalo com o id: " + idCavalo + " não foi encontrado";
                }
            }
        }
    }
}
