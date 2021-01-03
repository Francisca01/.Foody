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
        [HttpGet("{idEncomendaProduto}")]
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

        // POST api/<EncomendasProdutosController>
        [HttpPost]
        public string Post([FromBody] EncomendaProduto novaEncomendasProduto)
        {
            if (novaEncomendasProduto.quantidade > 0 && string.IsNullOrEmpty(novaEncomendasProduto.idProduto.ToString()))
            {
                using (var db = new DbHelper())
                {
                    db.encomendaProduto.Add(novaEncomendasProduto);
                    db.SaveChanges();

                    return "Criado";
                }
            }
            else
            {
                return "Erro: a quatidade do produto tem de ser pelo menos 1";
            }
        }

        // PUT api/<EncomendasProdutosController>/5
        [HttpPut("{idEncomendaProduto}")]
        public void Put(int idEncomenda, [FromBody] EncomendaProduto encomendaProdutoUpdate)
        {
            using (var db = new DbHelper())
            {
                var encomendaProdutoDB = db.encomendaProduto.Find(encomendaProdutoUpdate.idEncomendaProduto);

                if (encomendaProdutoDB != null && encomendaProdutoUpdate != null && encomendaProdutoUpdate.quantidade > 0)
                {
                    db.encomendaProduto.Update(encomendaProdutoUpdate);
                    db.SaveChanges();
                }
            }
        }

        // DELETE api/<EncomendasProdutosController>/5
        [HttpDelete("{idEncomendaProduto}")]
        public string Delete(int idEncomendaProduto)
        {
            using (var db = new DbHelper())
            {
                var encomendaProdutoDB = db.encomendaProduto.Find(idEncomendaProduto);

                if (encomendaProdutoDB != null)
                {
                    db.encomendaProduto.Remove(encomendaProdutoDB);
                    db.SaveChanges();

                    return "Eliminado!";
                }
                else
                {
                    return "A encomendaProduto com o id: " + idEncomendaProduto + " não foi encontrada";
                }
            }
        }
    }
}
