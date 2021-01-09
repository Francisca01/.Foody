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
        public Utilizador[] Get()
        {
            // obter dados dos utilizadores na base de dados
            using (var db = new DbHelper())
            {
                // devolve-os (dados) num array
                return db.utilizador.ToArray();
            }
        }

        // GET api/<AdministradoresController>/5
        [HttpGet("{idUtilizador}/")]
        public Utilizador Get(int id)
        {
            // obter dados do utilizador na base de dados (por id especifico)
            using (var db = new DbHelper())
            {
                return db.utilizador.Find(id);
            }
        }

        // POST api/<AdministradoresController>
        [HttpPost]
        public string Post([FromBody] Utilizador novoAdministrador)
        {
            // obter dados do utilizador na base de dados (por id especifico)
            using (var db = new DbHelper())
            {
                // converte-os (dados) num array
                var utilizador = db.utilizador.ToArray();

                // verifica se id de utilizador já existe na BD
                for (int i = 0; i < utilizador.Length; i++)
                {
                    if (novoAdministrador.idUtilizador == utilizador[i].idUtilizador)
                    {
                        return "Já existe";
                    }
                }

                // se não existir, adiciona um novo utilizador
                db.utilizador.Add(novoAdministrador);
                db.SaveChanges();

                return "Criado";
            }
        }

        // ou: maneira mais complexa

        /*
        [HttpPost]
        public string Post([FromBody] Utilizador novoAdministrador)
        {
            using (var db = new DbHelper())
            {
                Utilizador.cod_cavaço = new Random().Next();
                db.utilizador.Add(novoAdministrador);
                db.SaveChanges();
            }
        } 
        */


        // PUT api/<AdministradoresController>/5
        [HttpPut("{idUtilizador}/")]
        public void Put(int idUtilizador, [FromBody] Utilizador administradorUpdate)
        {
            // verificar se utilizado logado é utilizador
            if (administradorUpdate.idUtilizador == idUtilizador)
            {
                // obter dados do utilizador na base de dados (por id especifico)
                using (var db = new DbHelper())
                {
                    var administradorDB = db.utilizador.Find(idUtilizador);

                    // se utilizador não existir, criar novo
                    if (administradorDB == null)
                    {
                        Post(administradorUpdate);
                    }

                    // se utilizador existir, atualizar dados
                    else
                    {
                        administradorDB.idUtilizador = idUtilizador;

                        db.utilizador.Update(administradorDB);
                        db.SaveChanges();
                    }
                }
            }
        }


        // Por questões de política e privacidade de dados: Utilizador não pode ser eliminado

        /*  // DELETE api/<AdministradoresController>/5
        [HttpDelete("{idUtilizador}")]
        public string Delete(int idUtilizador, int idCavalo)
        {
            using (var db = new DbHelper())
            {
                var administradorDB = db.utilizador.Find(idUtilizador, idCavalo);

                if (administradorDB != null)
                {
                    db.utilizador.Remove(administradorDB);
                    db.SaveChanges();

                    return "Eliminado!";
                }
                else
                {
                    return "O Utilizador com o id de prova: " + idUtilizador + " não foi encontrado";
                }
            }
        }  */
    }
}
