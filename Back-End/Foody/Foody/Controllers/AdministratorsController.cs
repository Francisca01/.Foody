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
    public class AdministratorsController : ControllerBase
    {
        // GET: api/<AdministratorsController>
        [HttpGet]
        public User[] Get()
        {
            // obter dados dos utilizadores na base de dados
            using (var db = new DbHelper())
            {
                // devolve-os (dados) num array
                return db.user.ToArray();
            }
        }

        // GET api/<AdministratorsController>/5
        [HttpGet("{idUtilizador}/")]
        public User Get(int id)
        {
            // obter dados do user na base de dados (por id especifico)
            using (var db = new DbHelper())
            {
                return db.user.Find(id);
            }
        }

        // POST api/<AdministratorsController>
        [HttpPost]
        public string Post([FromBody] User novoAdministrador)
        {
            // obter dados do user na base de dados (por id especifico)
            using (var db = new DbHelper())
            {
                // converte-os (dados) num array
                var user = db.user.ToArray();

                // verifica se id de user já existe na BD
                for (int i = 0; i < user.Length; i++)
                {
                    if (novoAdministrador.idUtilizador == user[i].idUtilizador)
                    {
                        return "Já existe";
                    }
                }

                // se não existir, adiciona um novo user
                db.user.Add(novoAdministrador);
                db.SaveChanges();

                return "Criado";
            }
        }

        // ou: maneira mais complexa

        /*
        [HttpPost]
        public string Post([FromBody] User novoAdministrador)
        {
            using (var db = new DbHelper())
            {
                User.cod_cavaço = new Random().Next();
                db.user.Add(novoAdministrador);
                db.SaveChanges();
            }
        } 
        */


        // PUT api/<AdministratorsController>/5
        [HttpPut("{idUtilizador}/")]
        public void Put(int idUtilizador, [FromBody] User administradorUpdate)
        {
            // verificar se utilizado logado é user
            if (administradorUpdate.idUtilizador == idUtilizador)
            {
                // obter dados do user na base de dados (por id especifico)
                using (var db = new DbHelper())
                {
                    var administradorDB = db.user.Find(idUtilizador);

                    // se user não existir, criar novo
                    if (administradorDB == null)
                    {
                        Post(administradorUpdate);
                    }

                    // se user existir, atualizar dados
                    else
                    {
                        administradorDB.idUtilizador = idUtilizador;

                        db.user.Update(administradorDB);
                        db.SaveChanges();
                    }
                }
            }
        }


        // Por questões de política e privacidade de dados: User não pode ser eliminado

        /*  // DELETE api/<AdministratorsController>/5
        [HttpDelete("{idUtilizador}")]
        public string Delete(int idUtilizador, int idCavalo)
        {
            using (var db = new DbHelper())
            {
                var administradorDB = db.user.Find(idUtilizador, idCavalo);

                if (administradorDB != null)
                {
                    db.user.Remove(administradorDB);
                    db.SaveChanges();

                    return "Eliminado!";
                }
                else
                {
                    return "O User com o id de prova: " + idUtilizador + " não foi encontrado";
                }
            }
        }  */
    }
}
