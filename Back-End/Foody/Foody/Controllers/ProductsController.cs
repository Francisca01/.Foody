using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Foody.Models;
using Foody.Utils;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Foody.Controllers
{
    [Route("api/[controller]")]
    //[ApiController]
    public class ProductsController : ControllerBase
    {
        // GET: api/<ProductsController>
        [HttpGet]
        public object[] Get()
        {
            //obter dados dos produtos na base de dados
            using (DbHelper db = new DbHelper())
            {
                //devolve os produtos da base de dados num array
                var produtosDB = db.product.ToArray();

                if (produtosDB != null)
                {
                    return produtosDB;
                }
                else
                {
                    object[] msg = { MessageService.WithoutResults() };
                    return msg;
                }
            }
        }

        [HttpGet("empresa/{idEmpresa}")]
        public List<object> GetCompanyProduct(int idEmpresa)
        {
            //obter dados dos produtos na base de dados
            using (DbHelper db = new DbHelper())
            {
                //devolve os produtos da base de dados num array
                var produtosDB = db.product.ToArray();

                //lista de produtos a devolver
                List<Product> products = new List<Product>();

                if (produtosDB != null)
                {
                    for (int i = 0; i < produtosDB.Length; i++)
                    {
                        if (produtosDB[i].idUtilizador == idEmpresa)
                        {
                            products.Add(produtosDB[i]);
                        }
                    }

                    List<object> pdts = new List<object>() { products };
                    return pdts;
                }
                else
                {
                    List<object> msg = new List<object>() { MessageService.WithoutResults() };
                    return msg;
                }
            }
        } 

        // GET api/<ProductsController>/5
        [HttpGet("{idProduto}")]
        public object Get(int idProduto)
        {
            // obter dados dos utilizadores na base de dados
            using (DbHelper db = new DbHelper())
            {
                // devolve os dados da base de dados num array
                var produtoDB = db.product.Find(idProduto);

                if (produtoDB != null)
                {
                    return produtoDB;
                }
                else
                {
                    return MessageService.WithoutResults();
                }
            }
        }

        // POST api/<ProductsController>
        [HttpPost]
        public object Post([FromBody] Product product)
        {
            //token do user logado
            string token = Request.Headers["token"][0];

            int[] userLogin = UserService.UserLoggedIn(token);
            //userLogin[0] = Id
            //userLogin[1] = UserType

            if (userLogin != null)
            {
                return ProductService.VerifyProduct(userLogin, product, false, -1);
            }
            else
            {
                return MessageService.AccessDenied();
            }
        }

        // PUT api/<ProductsController>/5
        [HttpPut("{idProduto}")]
        public object Put(int idProduto, [FromBody] Product editarProduto)
        {
                //token do user logado
                string token = Request.Headers["token"][0];

                int[] userLogin = UserService.UserLoggedIn(token);
                //userLogin[0] = Id
                //userLogin[1] = UserType

                return ProductService.VerifyProduct(userLogin, editarProduto, true, idProduto);
        }

        // DELETE api/<ProductsController>/5
        [HttpDelete("{idProduto}")]
        public object Delete(int idProduto)
        {
            using (var db = new DbHelper())
            {
                //token do user logado
                string token = Request.Headers["token"][0];

                int[] userLogin = UserService.UserLoggedIn(token);
                //userLogin[0] = Id
                //userLogin[1] = UserType

                if (userLogin != null)
                {
                    //procura pelo product na base de dados
                    var produtosDB = db.product.Find(idProduto);

                    if (produtosDB != null && produtosDB.idUtilizador == userLogin[0])
                    {
                        db.product.Remove(produtosDB);
                        db.SaveChanges();

                        return MessageService.Custom("Eliminado!");
                    }
                    else
                    {
                        return MessageService.WithoutResults();
                    }
                }
                else
                {
                    return MessageService.AccessDenied();
                }
            }
        }
    }
}
