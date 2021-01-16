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
    public class OrdersController : ControllerBase
    {
        // GET: api/<OrdersController>
        [HttpGet]
        public Order[] Get()
        {
            // obter dados dos utilizadores na base de dados
            using (var db = new DbHelper())
            {
                // devolve-os (dados) num array
                return db.order.ToArray();
            }
        }

        // GET api/<OrdersController>/5
        [HttpGet("{idEncomenda}")]
        public Order Get(int id)
        {
            // obter dados do utilizador na base de dados (por id especifico)
            using (var db = new DbHelper())
            {
                // maneira mais simples
                return db.order.Find(id);

                // maneira mais complesa 
                /*  var encomendasDB = db.order.ToArray();

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

        // POST api/<OrdersController>
        [HttpPost]
        public string Post([FromBody] Order novaEncomenda)
        {
            if (novaEncomenda != null)
            {
                // obter dados do utilizador na base de dados (por id especifico)
                using (var db = new DbHelper())
                {
                    // converte-os (dados) num array
                    var encomendasDB = db.order.ToArray();

                    // verifica se id de order já existe na BD
                    for (int i = 0; i < encomendasDB.Length; i++)
                    {
                        if (novaEncomenda.idEncomenda == encomendasDB[i].idEncomenda)
                        {
                            return "Já existe";
                        }
                    }

                    // se não existir, adiciona um novo order
                    db.order.Add(novaEncomenda);
                    db.SaveChanges();

                    return "Criado";
                }
            }
            else
            {
                return "Não foi recebido qualquer tipo de dados!";
            }
        }

        // PUT api/<OrdersController>/5
        [HttpPut("{idEncomenda}")]
        public void Put(int idEncomenda, [FromBody] Order encomendaUpdate)
        {
            // verificar se order não está nula 
            if (encomendaUpdate != null && encomendaUpdate.idEncomenda == idEncomenda)
            {
                // obter dados do utilizador na base de dados (por id especifico)
                using (var db = new DbHelper())
                {
                    var encomendasDB = db.order.Find(idEncomenda);

                    // se order não existir, criar nova
                    if (encomendasDB == null)
                    {
                        Post(encomendaUpdate);
                    }
                    // se order existir, atualizar dados
                    else
                    {
                        encomendasDB.idEncomenda = idEncomenda;

                        db.order.Update(encomendasDB);
                        db.SaveChanges();
                    }
                }
            }
        }

        // DELETE api/<OrdersController>/5
        [HttpDelete("{idEncomenda}")]
        public string Delete(int id)
        {
            // obter dados do utilizador na base de dados (por id especifico)
            using (var db = new DbHelper())
            {
                // procura o id do order
                var encomendasDB = db.order.Find(id);

                // se id encontrado (diferente de nulo), 
                // remove o cliente associado
                if (encomendasDB != null)
                {
                    db.order.Remove(encomendasDB);
                    db.SaveChanges();

                    return "Eliminado!";
                }
                else
                {
                    return "A order com o id: " + id + " não foi encontrada";
                }
            }
        }
    }
}
