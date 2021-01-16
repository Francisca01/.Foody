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
        public List<object> Get()
        {
            //token do user logado
            string token = Request.Headers["token"][0];

            if (UserService.VerifyUserAccess(token, 3))
            {
                //obter dados dos utilizadores na base de dados
                using (var db = new DbHelper())
                {
                    List<object> encomendasProdutos = new List<object>();
                    foreach (var encomendaProduto in db.encomendaProduto.ToArray())
                    {
                        encomendasProdutos.Add(encomendaProduto);
                    }
                    return encomendasProdutos;
                }
            }
            else
            {
                List<object> msg = new List<object>() { MessageService.AccessDeniedMessage() };
                return msg;
            }
        }

        // GET api/<EncomendasProdutosController>/5
        [HttpGet("{idEncomendaProduto}")]
        public object Get(int idOrderProduct)
        {
            //token do user logado
            string token = Request.Headers["token"][0];

            int[] userLoggedIn = UserService.UserLoggedIn(token);

            if (OrderProductService.VerifyOrderProductAccess(userLoggedIn[0], idOrderProduct))
            {
                //obter dados dos utilizadores na base de dados
                using (var db = new DbHelper())
                {
                    return db.encomendaProduto.Find(idOrderProduct);
                }
            }
            else
            {
                List<object> msg = new List<object>() { MessageService.AccessDeniedMessage() };
                return msg;
            }
        }

        // POST api/<EncomendasProdutosController>
        [HttpPost]
        public string Post([FromBody] EncomendaProduto newOrderProduct)
        {
            if (newOrderProduct.quantidade > 0 && !string.IsNullOrEmpty(newOrderProduct.idProduto.ToString()))
            {
                // obter dados do utilizador na base de dados (por id especifico)
                using (var db = new DbHelper())
                {
                    db.encomendaProduto.Add(newOrderProduct);
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
