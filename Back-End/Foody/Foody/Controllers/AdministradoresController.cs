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
    public class AdministradoresController : ControllerBase
    {
        // GET: api/<AdministradoresController>
        [HttpGet]
        public Administrador[] Get()
        {
            using (var db = new DbHelper())
            {
                return db.administrador.ToArray();
            }
        }

        // GET api/<AdministradoresController>/5
        [HttpGet("{idAdministrador}/")]
         public Administrador Get(int id)
        {
            using (var db = new DbHelper())
            {
                return db.administrador.Find(id);
            }
        }

        // POST api/<AdministradoresController>
        [HttpPost]
        public string Post([FromBody] Administrador novoAdministrador)
        {
            using (var db = new DbHelper())
            {
                var administrador = db.administrador.ToArray();

                for (int i = 0; i < administrador.Length; i++)
                {
                    if (novoAdministrador.idAdministrador == administrador[i].idAdministrador)
                    {
                        return "Já existe";
                    }
                }

                db.administrador.Add(novoAdministrador);
                db.SaveChanges();

                return "Criado";
            }

        }
        // ou

        /*
        [HttpPost]
        public string Post([FromBody] Administrador novoAdministrador)
        {
            using (var db = new DbHelper())
            {
                Administrador.cod_cavaço = new Random().Next();
                db.administrador.Add(novoAdministrador);
                db.SaveChanges();
            }
        }
         */

        // PUT api/<AdministradoresController>/5
        [HttpPut("{idAdministrador}/")]
        public void Put(int idAdministrador, [FromBody] Administrador administradorUpdate)
        {
            if (administradorUpdate.idAdministrador == idAdministrador)
            {
                using (var db = new DbHelper())
                {
                    var administradorDB = db.administrador.Find(idAdministrador);

                    if (administradorDB == null)
                    {
                        Post(administradorUpdate);
                    }
                    else
                    {
                        administradorDB.idAdministrador = idAdministrador;

                        db.administrador.Update(administradorDB);
                        db.SaveChanges();
                    }
                }
            }

        }

        /*
        // DELETE api/<AdministradoresController>/5
        [HttpDelete("{idAdministrador}")]
        public string Delete(int idAdministrador, int idCavalo)
        {
            using (var db = new DbHelper())
            {
                var administradorDB = db.administrador.Find(idAdministrador, idCavalo);

                if (administradorDB != null)
                {
                    db.administrador.Remove(administradorDB);
                    db.SaveChanges();

                    return "Eliminado!";
                }
                else
                {
                    return "O Administrador com o id de prova: " + idAdministrador + " não foi encontrado";
                }
            }
        }*/
    }
}
