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
        public Condutor[] Get()
        {
            // obter dados dos utilizadores na base de dados
            using (var db = new DbHelper())
            {
                // devolve-os (dados) num array
                return db.condutor.ToArray();
            }

            //HttpContext.Response.StatusCode = (int)

            //return null;
        }

        // GET api/<CondutoresController>/5
        [HttpGet("{idCondutor}")]
        public Condutor Get(int id)
        {
            // obter dados do utilizador na base de dados (por id especifico)
            using (var db = new DbHelper())
            {
                // maneira mais simples
                return db.condutor.Find(id);

                // ou: maneira mais complexa
                /*  var condutor = db.condutor.ToArray();

                for (int i = 0; i <= condutor.Length; i++)
                {

                    if (condutor[i].idCondutor == id)
                    {
                        return condutor[i];
                    }
                }

                return null;  */
            }
        }

        // POST api/<CondutoresController>
        [HttpPost]
        public string Post([FromBody] Condutor novoCondutor)
        {
            // obter dados do utilizador na base de dados (por id especifico)
            using (var db = new DbHelper())
            {
                // converte-os (dados) num array
                var condutor = db.condutor.ToArray();

                // verifica se id de condutor já existe na BD
                for (int i = 0; i < condutor.Length; i++)
                {

                    if (novoCondutor.idCondutor == condutor[i].idCondutor)
                    {
                        return "Já existe";
                    }
                }

                // se não existir, adiciona um novo cliente
                db.condutor.Add(novoCondutor);
                db.SaveChanges();

                return "Criado";
            }
        }
        // ou

        /*
        [HttpPost]
        public string Post([FromBody] Condutor novoCondutor)
        {
            using (var db = new DbHelper())
            {
                cavalo.cod_cavaço = new Random().Next();
                db.condutor.Add(novoCondutor);
                db.SaveChanges();
            }
        }
         */

        // PUT api/<CondutoresController>/5
        [HttpPut("{idCondutor}")]
        public void Put(int idCondutor, [FromBody] Condutor condutorUpdate)
        {
            // verificar se utilizado logado é condutor
            if (condutorUpdate != null && condutorUpdate.idCondutor == idCondutor)
            {
                // obter dados do utilizador na base de dados (por id especifico)
                using (var db = new DbHelper())
                {
                    var condutorDB = db.condutor.Find(idCondutor);

                    // se cliente não existir, criar novo
                    if (condutorDB == null)
                    {
                        Post(condutorUpdate);
                    }

                    // se cliente existir, atualizar dados
                    else
                    {
                        condutorDB.idCondutor = idCondutor;

                        db.condutor.Update(condutorDB);
                        db.SaveChanges();
                    }
                }
            }
        }

        // DELETE api/<CondutoresController>/5
        [HttpDelete("{idCondutor}")]
        public string Delete(int idCondutor)
        {
            // obter dados do utilizador na base de dados (por id especifico)
            using (var db = new DbHelper())
            {
                // procura o id do condutor
                var condutorDB = db.condutor.Find(idCondutor);

                // se id encontrado (diferente de nulo), 
                // remove o condutor associado
                if (condutorDB != null)
                {
                    db.condutor.Remove(condutorDB);
                    db.SaveChanges();

                    return "Eliminado!";
                }
                else
                {
                    return "O Condutor com o id: " + idCondutor + " não foi encontrado";
                }
            }
        }
    }
}
