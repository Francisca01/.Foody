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
    public class ProdutosController : ControllerBase
    {
        // GET: api/<ProdutosController>
        [HttpGet]
        public object[] Get()
        {
            //obter dados dos produtos na base de dados
            using (DbHelper db = new DbHelper())
            {
                //devolve os produtos da base de dados num array
                var produtosDB = db.produto.ToArray();

                if (produtosDB != null)
                {
                    return produtosDB;
                }
                else
                {
                    object[] msg = { MessageService.WithoutResultsMessage() };
                    return msg;
                }
            }
        }

        /* [HttpGet("{idEmpresa}")]
        public List<object> GetCompanyProduct(int idEmpresa)
        {
            //obter dados dos produtos na base de dados
            using (DbHelper db = new DbHelper())
            {
                //devolve os produtos da base de dados num array
                var produtosDB = db.produto.ToArray();

                //lista de produtos a devolver
                List<Produto> products = new List<Produto>();

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
                    List<object> msg = new List<object>() { MessageService.WithoutResultsMessage() };
                    return msg;
                }
            }
        } */

        // GET api/<ProdutosController>/5
        [HttpGet("{idProduto}")]
        public object Get(int idProduto)
        {
            // obter dados dos utilizadores na base de dados
            using (DbHelper db = new DbHelper())
            {
                // devolve os dados da base de dados num array
                var produtoDB = db.produto.Find(idProduto);

                if (produtoDB != null)
                {
                    return produtoDB;
                }
                else
                {
                    return MessageService.WithoutResultsMessage();
                }
            }
        }

        // POST api/<ProdutosController>
        [HttpPost]
        public object Post([FromBody] Produto produto)
        {
            //token do user logado
            string token = Request.Headers["token"][0];

            int[] userLogin = UserService.UserLoggedIn(token);
            //userLogin[0] = Id
            //userLogin[1] = UserType

            if (userLogin != null)
            {
                return ProductService.VerifyProduct(userLogin, produto, false, -1);
            }
            else
            {
                return MessageService.AccessDeniedMessage();
            }
        }

        // PUT api/<ProdutosController>/5
        [HttpPut("{idProduto}")]
        public object Put(int idProduto, [FromBody] Produto editarProduto)
        {
                //token do user logado
                string token = Request.Headers["token"][0];

                int[] userLogin = UserService.UserLoggedIn(token);
                //userLogin[0] = Id
                //userLogin[1] = UserType

                return ProductService.VerifyProduct(userLogin, editarProduto, true, idProduto);
        }

        // DELETE api/<ProdutosController>/5
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
                    //procura pelo produto na base de dados
                    var produtosDB = db.produto.Find(idProduto);

                    if (produtosDB != null && produtosDB.idUtilizador == userLogin[0])
                    {
                        db.produto.Remove(produtosDB);
                        db.SaveChanges();

                        return MessageService.CustomMessage("Eliminado!");
                    }
                    else
                    {
                        return MessageService.WithoutResultsMessage();
                    }
                }
                else
                {
                    return MessageService.AccessDeniedMessage();
                }
            }
        }
    }
}
