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
                var productsDB = db.product.ToArray();

                if (productsDB != null)
                {
                    return productsDB;
                }
                else
                {
                    object[] msg = { MessageService.WithoutResults() };
                    return msg;
                }
            }
        }

        [HttpGet("company/{idEmpresa}")]
        public List<object> GetCompanyProduct(int idEmpresa)
        {
            //obter dados dos produtos na base de dados
            using (DbHelper db = new DbHelper())
            {
                //devolve os produtos da base de dados num array
                var productsDB = db.product.ToArray();

                //lista de produtos a devolver
                List<Product> products = new List<Product>();

                if (productsDB != null)
                {
                    for (int i = 0; i < productsDB.Length; i++)
                    {
                        if (productsDB[i].idCompany == idEmpresa)
                        {
                            products.Add(productsDB[i]);
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
        [HttpGet("{idProduct}")]
        public object Get(int idProduct)
        {
            // obter dados dos utilizadores na base de dados
            using (DbHelper db = new DbHelper())
            {
                // devolve os dados da base de dados num array
                var produtoDB = db.product.Find(idProduct);

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
            string token = Request.Headers["token"];

            int[] userLoggedIn = UserService.UserLoggedIn(token);
            //userLoggedIn[0] = Id
            //userLoggedIn[1] = UserType

            if (userLoggedIn != null)
            {
                if (userLoggedIn[1] == 2)
                {
                    return ProductService.VerifyProduct(userLoggedIn, product, false, -1);
                }
                else
                {
                    return MessageService.AccessDenied();
                }
            }
            else
            {
                return MessageService.AccessDenied();
            }
        }

        // PUT api/<ProductsController>/5
        [HttpPut("{idProduct}")]
        public object Put(int idProduct, [FromBody] Product productUpdate)
        {
            //token do user logado
            string token = Request.Headers["token"];

            int[] userLoggedIn = UserService.UserLoggedIn(token);
            //userLoggedIn[0] = Id
            //userLoggedIn[1] = UserType

            if (userLoggedIn != null)
            {
                return ProductService.VerifyProduct(userLoggedIn, productUpdate, true, idProduct);
            }
            else
            {
                List<object> msg = new List<object>() { MessageService.AccessDenied() };
                return msg;
            }
        }

        // DELETE api/<ProductsController>/5
        [HttpDelete("{idProduct}")]
        public object Delete(int idProduct)
        {
            //token do user logado
            string token = Request.Headers["token"];

            int[] userLoggedIn = UserService.UserLoggedIn(token);
            //userLoggedIn[0] = Id
            //userLoggedIn[1] = UserType

            if (userLoggedIn != null)
            {
                using (var db = new DbHelper())
                {
                    if (userLoggedIn != null)
                    {
                        //procura pelo product na base de dados
                        var productsDB = db.product.Find(idProduct);

                        if (productsDB != null && productsDB.idCompany == userLoggedIn[0])
                        {
                            db.product.Remove(productsDB);
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
            else
            {
                List<object> msg = new List<object>() { MessageService.AccessDenied() };
                return msg;
            }
        }
    }
}
