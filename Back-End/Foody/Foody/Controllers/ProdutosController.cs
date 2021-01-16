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

        [HttpGet("{idEmpresa}")]
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
        }

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
                return ProductService.VerifyProduct(userLogin, produto, false);
            }
            else
            {
                return MessageService.AccessDeniedMessage();
            }
        }

        // PUT api/<ProdutosController>/5
        [HttpPut("{idProduto}")]
        public string Put(int idEmpresa, [FromBody] Produto editarProduto)
        {/*
            using (var db = new DbHelper())
            {
                //procura pelo produto na base de dados
                var produtoDB = db.produto.Find(editarProduto.idProduto);

                if (produtoDB != null)
                {
                    if (Post(idEmpresa, editarProduto, true) == "Ok")
                    {
                        return "Produto Alterado";
                    }
                    else
                    {
                        return "Não foi possivel criar o produto";
                    }
                }
                else
                {
                    return "Produto não encontrado!";
                }
            }*/
            return null;
        }

        // DELETE api/<ProdutosController>/5
        [HttpDelete("{id}")]
        public string Delete(int idEmpresa, int id)
        {
            using (var db = new DbHelper())
            {
                //procura pelo produto na base de dados
                var produtosDB = db.produto.Find(id);

                if (produtosDB != null && produtosDB.idUtilizador == idEmpresa)
                {
                    db.produto.Remove(produtosDB);
                    db.SaveChanges();

                    return "Eliminado!";
                }
                else
                {
                    return "O produto com o id: " + id + " não foi encontrado!";
                }
            }
        }
    }
}
