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
            // obter dados dos utilizadores na base de dados
            using (var db = new DbHelper())
            {
                // devolve-os (dados) num array
                return db.encomendaProduto.ToArray();
            }

            //HttpContext.Response.StatusCode = (int)

            //return null;
        }

        // GET api/<EncomendasProdutosController>/5
        [HttpGet("{idEncomendaProduto}")]
        public EncomendaProduto Get(int id)
        {
            // obter dados do utilizador na base de dados (por id especifico)
            using (var db = new DbHelper())
            {
                // maneira mais simples
                return db.encomendaProduto.Find(id);

                // ou: maneira mais complexa
                /* var encomendaProduto = db.encomendaProduto.ToArray();

                for (int i = 0; i <= encomendaProduto.Length; i++)
                {
                    if (encomendaProduto[i].idEncomendaProduto == id)
                    {
                        return encomendaProduto[i];
                    }
                }

                return null;  */
            }
        }

        // POST api/<EncomendasProdutosController>
        [HttpPost]
        public string Post([FromBody] EncomendaProduto novaEncomendasProduto)
        {
            // verificar se a quantidade de encomendaProduto é maior que 0,
            // e se idProduto não é recebido nulo ou vazio,
            if (novaEncomendasProduto.quantidade > 0 && !string.IsNullOrEmpty(novaEncomendasProduto.idProduto.ToString()))
            {
                // obter dados do utilizador na base de dados (por id especifico)
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
            // obter dados do utilizador na base de dados (por id especifico de Produto)
            using (var db = new DbHelper())
            {
                var encomendaProdutoDB = db.encomendaProduto.Find(encomendaProdutoUpdate.idEncomendaProduto);

                // verificar se os valores da encomendaProduto (DB) não são nulos,
                // se os valores da encomendaProduto (inseridos para update) não são nulos,
                // e se quantidade de encomendaProduto (inseridos para update) é maior que 0
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
            // obter dados do utilizador na base de dados (por id especifico)
            using (var db = new DbHelper())
            {
                // procura o id da encomendaProduto
                var encomendaProdutoDB = db.encomendaProduto.Find(idEncomendaProduto);

                // se id encontrado (diferente de nulo), 
                // remove a encomendaProduto associado
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
