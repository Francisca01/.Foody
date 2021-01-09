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
    public class CondutoresController : ControllerBase
    {
        // GET: api/<CondutoresController>
        [HttpGet]
        public Utilizador[] Get()
        {
            // obter dados dos utilizadores na base de dados
            using (var db = new DbHelper())
            {
                // devolve-os (dados) num array
                return db.utilizador.ToArray();
            }

            //HttpContext.Response.StatusCode = (int)

            //return null;
        }

        // GET api/<CondutoresController>/5
        [HttpGet("{idUtilizador}")]
        public Utilizador Get(int id)
        {
            // obter dados do utilizador na base de dados (por id especifico)
            using (var db = new DbHelper())
            {
                // maneira mais simples
                return db.utilizador.Find(id);

                // ou: maneira mais complexa
                /*  var utilizador = db.utilizador.ToArray();

                for (int i = 0; i <= utilizador.Length; i++)
                {

                    if (utilizador[i].idUtilizador == id)
                    {
                        return utilizador[i];
                    }
                }

                return null;  */
            }
        }

        // POST api/<CondutoresController>
        [HttpPost]
        public string Post([FromBody] Utilizador novoCondutor)
        {
            // obter dados do utilizador na base de dados (por id especifico)
            using (var db = new DbHelper())
            {
                // converte-os (dados) num array
                var utilizador = db.utilizador.ToArray();

                // verifica se id de utilizador já existe na BD
                for (int i = 0; i < utilizador.Length; i++)
                {

                    if (novoCondutor.idUtilizador == utilizador[i].idUtilizador)
                    {
                        return "Já existe";
                    }
                }

                // se não existir, adiciona um novo cliente
                db.utilizador.Add(novoCondutor);
                db.SaveChanges();

                return "Criado";
            }
        }
        // ou

        /*
        [HttpPost]
        public string Post([FromBody] Utilizador novoCondutor)
        {
            using (var db = new DbHelper())
            {
                cavalo.cod_cavaço = new Random().Next();
                db.utilizador.Add(novoCondutor);
                db.SaveChanges();
            }
        }
         */

        // PUT api/<CondutoresController>/5
        [HttpPut("{idUtilizador}")]
        public void Put(int idUtilizador, [FromBody] Utilizador condutorUpdate)
        {
            // verificar se utilizado logado é utilizador
            if (condutorUpdate != null && condutorUpdate.idUtilizador == idUtilizador)
            {
                // obter dados do utilizador na base de dados (por id especifico)
                using (var db = new DbHelper())
                {
                    var condutorDB = db.utilizador.Find(idUtilizador);

                    // se cliente não existir, criar novo
                    if (condutorDB == null)
                    {
                        Post(condutorUpdate);
                    }

                    // se cliente existir, atualizar dados
                    else
                    {
                        condutorDB.idUtilizador = idUtilizador;

                        db.utilizador.Update(condutorDB);
                        db.SaveChanges();
                    }
                }
            }
        }

        // DELETE api/<CondutoresController>/5
        [HttpDelete("{idUtilizador}")]
        public string Delete(int idUtilizador)
        {
            // obter dados do utilizador na base de dados (por id especifico)
            using (var db = new DbHelper())
            {
                // procura o id do utilizador
                var condutorDB = db.utilizador.Find(idUtilizador);

                // se id encontrado (diferente de nulo), 
                // remove o utilizador associado
                if (condutorDB != null)
                {
                    db.utilizador.Remove(condutorDB);
                    db.SaveChanges();

                    return "Eliminado!";
                }
                else
                {
                    return "O Utilizador com o id: " + idUtilizador + " não foi encontrado";
                }
            }
        }
    }
}
