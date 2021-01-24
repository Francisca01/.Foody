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
            if (userLoggedIn != null && userLoggedIn[1] == 2)
            {
                //valida os campos de product
                if (product != null && !string.IsNullOrEmpty(product.name) &&
                    product.unitPrice > 0.00)
                {
                    //lista para guardar o name de todos os produtos da empresa
                    List<string> productsName = new List<string>();

                    using (var db = new DbHelper())
                    {
                        //array de produtos da base de dados
                        var produtos = db.product.ToArray();

                        //criação do array dos produtos da empresa
                        for (int i = 0; i < produtos.Length; i++)
                        {
                            if (produtos[i].idCompany == userLoggedIn[0])
                            {
                                productsName.Add(produtos[i].name);
                            }
                        }
                    }

                    //valida se o name do product introduzido já exista na empresa
                    for (int i = 0; i < productsName.Count; i++)
                    {
                        if (productsName[i] == product.name)
                        {
                            return MessageService.Custom("O Product com o name: " + product.name + " já existe na sua empresa!");
                        }
                    }

                    using (var db = new DbHelper())
                    {
                        if (edit == true)
                        {
                            var productUpdate = db.product.Find(idProduto);

                            productUpdate.name = product.name;
                            productUpdate.ingredients = product.ingredients;
                            productUpdate.unitPrice = product.unitPrice;
                            productUpdate.category = product.category;

                            db.product.Update(productUpdate);
                            db.SaveChanges();

                            return MessageService.Custom("Product Editado!");
                        }
                        else
                        {
                            product.idCompany = userLoggedIn[0];

                            db.product.Add(product);
                            db.SaveChanges();

                            return MessageService.Custom("Product Criado!");
                        }
                    }
                }
                else
                {
                    return MessageService.Custom("Os campos obrigatórios não foram preenchidos ou são inválidos");
                }
            }
            else
            {
                return MessageService.AccessDenied();
            }
        }
    }
}
