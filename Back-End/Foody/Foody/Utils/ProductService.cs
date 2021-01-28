using Foody.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Foody.Utils
{
    public class ProductService
    {
        public static Message VerifyProduct(int[] userLoggedIn, Product product, bool edit, int idProduto)
        {
            int j = 0;

            if (userLoggedIn != null && userLoggedIn[1] == 2)
            {

                //lista para guardar o name de todos os produtos da empresa
                List<Product> companyProducts = new List<Product>();

                using (var db = new DbHelper())
                {
                    //array de produtos da base de dados
                    var produtos = db.product.ToArray();

                    //criação do array dos produtos da empresa
                    for (int i = 0; i < produtos.Length; i++)
                    {
                        if (produtos[i].idCompany == userLoggedIn[0])
                        {
                            companyProducts.Add(produtos[i]);
                        }
                    }
                }

                //valida se o name do product introduzido já exista na empresa
                for (int i = 0; i < companyProducts.Count; i++)
                {
                    if (edit == false)
                    {
                        if (companyProducts[i].name == product.name)
                        {
                            return MessageService.Custom("O Produto com o nome: " + product.name + " já existe na sua empresa!");
                        }
                    }
                    else
                    {
                        if (companyProducts[i].name == product.name)
                        {
                            j++;

                            if (j > 1)
                            {
                                return MessageService.Custom("O Produto com o nome: " + product.name + " já existe na sua empresa!");
                            }

                            if (companyProducts[i].idProduct != idProduto)
                            {
                                return MessageService.Custom("O Produto com o nome: " + product.name + " já existe na sua empresa!");
                            }
                        }
                    }
                }

                using (var db = new DbHelper())
                {
                    if (edit == true)
                    {
                        var productUpdate = db.product.Find(idProduto);

                        if (productUpdate == null)
                        {
                            return MessageService.WithoutResults();
                        }
                        if (product.name != null)
                        {
                            productUpdate.name = product.name;
                        }
                        if (product.name != null)
                        {
                            productUpdate.ingredients = product.ingredients;

                        }
                        if (product.name != null)
                        {
                            productUpdate.unitPrice = product.unitPrice;

                        }
                        if (product.name != null)
                        {
                            productUpdate.category = product.category;
                        }

                        db.product.Update(productUpdate);
                        db.SaveChanges();

                        return MessageService.Custom("Produto Editado!");
                    }
                    else
                    {
                        //valida os campos de product
                        if (product != null && !string.IsNullOrEmpty(product.name) &&
                            product.unitPrice > 0.00)
                        {
                            product.idCompany = userLoggedIn[0];

                            db.product.Add(product);
                            db.SaveChanges();

                            return MessageService.Custom("Produto Criado!");
                        }
                        else
                        {
                            return MessageService.Custom("Os campos obrigatórios não foram preenchidos ou são inválidos");
                        }
                    }
                }
            }
            else
            {
                return MessageService.AccessDenied();
            }
        }
    }
}
