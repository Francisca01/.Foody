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
    public class EncomendasController : ControllerBase
    {
        // GET: api/<EncomendasController>
        [HttpGet]
        public Encomenda[] Get()
        {
            // obter dados dos utilizadores na base de dados
            using (var db = new DbHelper())
            {
                // devolve-os (dados) num array
                return db.encomenda.ToArray();
            }
        }

        // GET api/<EncomendasController>/5
        [HttpGet("{idEncomenda}")]
        public Encomenda Get(int id)
        {
            // obter dados do utilizador na base de dados (por id especifico)
            using (var db = new DbHelper())
            {
                // maneira mais simples
                return db.encomenda.Find(id);

                // maneira mais complesa 
                /*  var encomendasDB = db.encomenda.ToArray();

                for (int i = 0; i <= encomendasDB.Length; i++)
                {
                    if (encomendasDB[i].idEncomenda == id)
                    {
                        return encomendasDB[i];
                    }
                }

                return null;  */
            }
        }

        // POST api/<EncomendasController>
        [HttpPost]
        public string Post([FromBody] Encomenda novaEncomenda)
        {
            if (novaEncomenda != null)
            {
                // obter dados do utilizador na base de dados (por id especifico)
                using (var db = new DbHelper())
                {
                    // converte-os (dados) num array
                    var encomendasDB = db.encomenda.ToArray();

                    // verifica se id de encomenda já existe na BD
                    for (int i = 0; i < encomendasDB.Length; i++)
                    {
                        if (novaEncomenda.idEncomenda == encomendasDB[i].idEncomenda)
                        {
                            return "Já existe";
                        }
                    }

                    // se não existir, adiciona um novo encomenda
                    db.encomenda.Add(novaEncomenda);
                    db.SaveChanges();

                    return "Criado";
                }
            }
            else
            {
                return "Não foi recebido qualquer tipo de dados!";
            }
        }

        // PUT api/<EncomendasController>/5
        [HttpPut("{idEncomenda}")]
        public void Put(int idEncomenda, [FromBody] Encomenda encomendaUpdate)
        {
            // verificar se encomenda não está nula 
            if (encomendaUpdate != null && encomendaUpdate.idEncomenda == idEncomenda)
            {
                // obter dados do utilizador na base de dados (por id especifico)
                using (var db = new DbHelper())
                {
                    var encomendasDB = db.encomenda.Find(idEncomenda);

                    // se encomenda não existir, criar nova
                    if (encomendasDB == null)
                    {
                        Post(encomendaUpdate);
                    }
                    // se encomenda existir, atualizar dados
                    else
                    {
                        encomendasDB.idEncomenda = idEncomenda;

                        db.encomenda.Update(encomendasDB);
                        db.SaveChanges();
                    }
                }
            }
        }

        // DELETE api/<EncomendasController>/5
        [HttpDelete("{idEncomenda}")]
        public string Delete(int id)
        {
            // obter dados do utilizador na base de dados (por id especifico)
            using (var db = new DbHelper())
            {
                // procura o id do encomenda
                var encomendasDB = db.encomenda.Find(id);

                // se id encontrado (diferente de nulo), 
                // remove o cliente associado
                if (encomendasDB != null)
                {
                    db.encomenda.Remove(encomendasDB);
                    db.SaveChanges();

                    return "Eliminado!";
                }
                else
                {
                    return "A encomenda com o id: " + id + " não foi encontrada";
                }
            }
        }
    }
}
