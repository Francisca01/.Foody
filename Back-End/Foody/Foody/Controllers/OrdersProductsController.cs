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
    public class OrdersProductsController : ControllerBase
    {
        // GET: api/<OrdersProductsController>
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
                    foreach (var orderProduct in db.orderProduct.ToArray())
                    {
                        encomendasProdutos.Add(orderProduct);
                    }
                    return encomendasProdutos;
                }
            }
            else
            {
                List<object> msg = new List<object>() { MessageService.AccessDenied() };
                return msg;
            }
        }

        // GET api/<OrdersProductsController>/5
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
                    return db.orderProduct.Find(idOrderProduct);
                }
            }
            else
            {
                List<object> msg = new List<object>() { MessageService.AccessDenied() };
                return msg;
            }
        }

        // POST api/<OrdersProductsController>
        [HttpPost]
        public string Post([FromBody] OrderProduct newOrderProduct)
        {
            if (newOrderProduct.quantidade > 0 && !string.IsNullOrEmpty(newOrderProduct.idProduto.ToString()))
            {
                // obter dados do utilizador na base de dados (por id especifico)
                using (var db = new DbHelper())
                {
                    db.orderProduct.Add(newOrderProduct);
                    db.SaveChanges();

                    return "Criado";
                }
            }
            else
            {
                return "Erro: a quatidade do produto tem de ser pelo menos 1";
            }
        }

        // PUT api/<OrdersProductsController>/5
        [HttpPut("{idEncomendaProduto}")]
        public void Put(int idEncomenda, [FromBody] OrderProduct encomendaProdutoUpdate)
        {
            // obter dados do utilizador na base de dados (por id especifico de Produto)
            using (var db = new DbHelper())
            {
                var encomendaProdutoDB = db.orderProduct.Find(encomendaProdutoUpdate.idEncomendaProduto);

                // verificar se os valores da orderProduct (DB) não são nulos,
                // se os valores da orderProduct (inseridos para update) não são nulos,
                // e se quantidade de orderProduct (inseridos para update) é maior que 0
                if (encomendaProdutoDB != null && encomendaProdutoUpdate != null && encomendaProdutoUpdate.quantidade > 0)
                {
                    db.orderProduct.Update(encomendaProdutoUpdate);
                    db.SaveChanges();
                }
            }
        }

        // DELETE api/<OrdersProductsController>/5
        [HttpDelete("{idEncomendaProduto}")]
        public string Delete(int idEncomendaProduto)
        {
            // obter dados do utilizador na base de dados (por id especifico)
            using (var db = new DbHelper())
            {
                // procura o id da orderProduct
                var encomendaProdutoDB = db.orderProduct.Find(idEncomendaProduto);

                // se id encontrado (diferente de nulo), 
                // remove a orderProduct associado
                if (encomendaProdutoDB != null)
                {
                    db.orderProduct.Remove(encomendaProdutoDB);
                    db.SaveChanges();

                    return "Eliminado!";
                }
                else
                {
                    return "A orderProduct com o id: " + idEncomendaProduto + " não foi encontrada";
                }
            }
        }
    }
}
