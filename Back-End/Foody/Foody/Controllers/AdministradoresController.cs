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
            // obter dados dos utilizadores na base de dados
            using (var db = new DbHelper())
            {
                // devolve-os (dados) num array
                return db.administrador.ToArray();
            }
        }

        // GET api/<AdministradoresController>/5
        [HttpGet("{idAdministrador}/")]
        public Administrador Get(int id)
        {
            // obter dados do utilizador na base de dados (por id especifico)
            using (var db = new DbHelper())
            {
                return db.administrador.Find(id);
            }
        }

        // POST api/<AdministradoresController>
        [HttpPost]
        public string Post([FromBody] Administrador novoAdministrador)
        {
            // obter dados do utilizador na base de dados (por id especifico)
            using (var db = new DbHelper())
            {
                // converte-os (dados) num array
                var administrador = db.administrador.ToArray();

                // verifica se id de administrador já existe na BD
                for (int i = 0; i < administrador.Length; i++)
                {
                    if (novoAdministrador.idAdministrador == administrador[i].idAdministrador)
                    {
                        return "Já existe";
                    }
                }

                // se não existir, adiciona um novo administrador
                db.administrador.Add(novoAdministrador);
                db.SaveChanges();

                return "Criado";
            }
        }

        // ou: maneira mais complexa

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
            // verificar se utilizado logado é administrador
            if (administradorUpdate.idAdministrador == idAdministrador)
            {
                // obter dados do utilizador na base de dados (por id especifico)
                using (var db = new DbHelper())
                {
                    var administradorDB = db.administrador.Find(idAdministrador);

                    // se administrador não existir, criar novo
                    if (administradorDB == null)
                    {
                        Post(administradorUpdate);
                    }

                    // se administrador existir, atualizar dados
                    else
                    {
                        administradorDB.idAdministrador = idAdministrador;

                        db.administrador.Update(administradorDB);
                        db.SaveChanges();
                    }
                }
            }
        }


        // Por questões de política e privacidade de dados: Administrador não pode ser eliminado

        /*  // DELETE api/<AdministradoresController>/5
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
        }  */
    }
}
