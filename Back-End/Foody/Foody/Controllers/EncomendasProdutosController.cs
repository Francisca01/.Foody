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
    public class EncomendasProdutosController : ControllerBase
    {
        // GET: api/<EncomendasProdutosController>
        [HttpGet]
        public EncomendaProduto[] Get()
        {
            using (var db = new DbHelper())
            {
                return db.encomendaProduto.ToArray();
            }

            //HttpContext.Response.StatusCode = (int)

            //return null;
        }

        // GET api/<EncomendasProdutosController>/5
        [HttpGet("{id}")]
        public EncomendaProduto Get(int id)
        {

            using (var db = new DbHelper())
            {
                var encomendaProduto = db.encomendaProduto.ToArray();

                for (int i = 0; i <= encomendaProduto.Length; i++)
                {

                    if (encomendaProduto[i].idEncomendaProduto == id)
                    {
                        return encomendaProduto[i];
                    }
                }

                return null;
            }
        }

        //ou

        /*
         public EncomendaProduto Get(int id)
        {
            using (var db = new DbHelper())
            {
                return db.encomendaProduto.Find(id);
            }
        }
         */

        // POST api/<EncomendasProdutosController>
        [HttpPost]
        public string Post([FromBody] EncomendaProduto novaEncomendasProduto)
        {
            using (var db = new DbHelper())
            {
                var encomendaProduto = db.encomendaProduto.ToArray();

                for (int i = 0; i < encomendaProduto.Length; i++)
                {

                    if (novaEncomendasProduto.idEncomendaProduto == encomendaProduto[i].idEncomendaProduto)
                    {
                        return "Já existe";
                    }
                }

                db.encomendaProduto.Add(novaEncomendasProduto);
                db.SaveChanges();

                return "Criado";
            }
        }
        // ou

        /*
        [HttpPost]
        public string Post([FromBody] EncomendaProduto novaEncomendasProduto)
        {
            using (var db = new DbHelper())
            {
                cavalo.cod_cavaço = new Random().Next();
                db.encomendaProduto.Add(novaEncomendasProduto);
                db.SaveChanges();
            }
        }
         */

        // PUT api/<EncomendasProdutosController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] EncomendaProduto encomendaProdutoUpdate)
        {
            using (var db = new DbHelper())
            {
                var encomendaProdutoDB = db.encomendaProduto.Find(id);

                if (encomendaProdutoDB == null)
                {
                    Post(encomendaProdutoUpdate);
                }
                else
                {
                    encomendaProdutoDB.idEncomendaProduto = id;

                    db.encomendaProduto.Update(encomendaProdutoDB);
                    db.SaveChanges();
                }
            }
        }

        // DELETE api/<EncomendasProdutosController>/5
        [HttpDelete("{id}")]
        public string Delete(int id)
        {
            using (var db = new DbHelper())
            {
                var encomendaProdutoDB = db.encomendaProduto.Find(id);

                if (encomendaProdutoDB != null)
                {
                    db.encomendaProduto.Remove(encomendaProdutoDB);
                    db.SaveChanges();

                    return "Eliminado!";
                }
                else
                {
                    return "A encomendaProduto com o id: " + id + " não foi encontrada";
                }
            }
        }
    }
}
