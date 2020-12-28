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
    public class ProdutosController : ControllerBase
    {
        // GET: api/<ProdutosController>
        [HttpGet]
        public Produto[] Get()
        {
            using (var db = new DbHelper())
            {
                return db.produto.ToArray();
            }

            //HttpContext.Response.StatusCode = (int)

            //return null;
        }

        // GET api/<ProdutosController>/5
        [HttpGet("{id}")]
        public Produto Get(int id)
        {

            using (var db = new DbHelper())
            {
                var produtosDB = db.produto.ToArray();

                for (int i = 0; i <= produtosDB.Length; i++)
                {

                    if (produtosDB[i].idProduto == id)
                    {
                        return produtosDB[i];
                    }
                }

                return null;
            }
        }

        //ou

        /*
         public Produto Get(int id)
        {
            using (var db = new DbHelper())
            {
                return db.produto.Find(id);
            }
        }
         */

        // POST api/<ProdutosController>
        [HttpPost]
        public string Post([FromBody] Produto novoProduto)
        {
            using (var db = new DbHelper())
            {
                var produtosDB = db.produto.ToArray();

                for (int i = 0; i < produtosDB.Length; i++)
                {

                    if (novoProduto.idProduto == produtosDB[i].idProduto)
                    {
                        return "Já existe";
                    }
                }

                db.produto.Add(novoProduto);
                db.SaveChanges();

                return "Criado";
            }
        }
        // ou

        /*
        [HttpPost]
        public string Post([FromBody] Produto novoProduto)
        {
            using (var db = new DbHelper())
            {
                cavalo.cod_cavaço = new Random().Next();
                db.produto.Add(novoProduto);
                db.SaveChanges();
            }
        }
         */

        // PUT api/<ProdutosController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Produto produtoUpdate)
        {
            using (var db = new DbHelper())
            {
                var produtosDB = db.produto.Find(id);

                if (produtosDB == null)
                {
                    Post(produtoUpdate);
                }
                else
                {
                    produtosDB.idProduto = id;

                    db.produto.Update(produtosDB);
                    db.SaveChanges();
                }
            }
        }

        // DELETE api/<ProdutosController>/5
        [HttpDelete("{id}")]
        public string Delete(int id)
        {
            using (var db = new DbHelper())
            {
                var produtosDB = db.produto.Find(id);

                if (produtosDB != null)
                {
                    db.produto.Remove(produtosDB);
                    db.SaveChanges();

                    return "Eliminado!";
                }
                else
                {
                    return "O produto com o id: " + id + " não foi encontrada";
                }
            }
        }
    }
}
