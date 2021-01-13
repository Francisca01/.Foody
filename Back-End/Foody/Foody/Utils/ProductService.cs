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
        public static object VerifyProduct(int[] userLogin, Produto produto, bool editar)
        {
            if (userLogin != null && userLogin[1] == 2)
            {
                using (var db = new DbHelper())
                {
                    //valores aceites para o nome
                    var regexNome = new Regex("^[a-zA-Z ]*$");

                    //array de produtos da base de dados
                    var produtos = db.produto.ToArray();

                    //valida os campos de produto
                    if (produto != null && !string.IsNullOrEmpty(produto.nome) && regexNome.IsMatch(produto.nome) && 
                        produto.precoUnitario > 0.00)
                    {
                        //lista para guardar o nome de todos os produtos da empresa
                        List<string> nomeProdutos = new List<string>();

                        int j = 0;

                        //criação do array dos produtos da empresa
                        for (int i = 0; i < produtos.Length; i++)
                        {
                            if (produtos[i].idUtilizador == userLogin[0])
                            {
                                nomeProdutos.Add(produtos[i].nome);
                                j++;
                            }
                        }

                        //valida se o nome do produto introduzido já exista na empresa
                        for (int i = 0; i < nomeProdutos.Count; i++)
                        {
                            if (nomeProdutos[i] == produto.nome)
                            {
                                return MessageService.CustomMessage("O Produto com o nome: " + produto.nome + " já existe na sua empresa!");
                            }
                        }

                        if (editar == true)
                        {
                            db.produto.Update(produto);
                            db.SaveChanges();
                            
                            return MessageService.CustomMessage("Produto Editado!");
                        }
                        else
                        {
                            produto.idUtilizador = userLogin[0];

                            db.produto.Add(produto);
                            db.SaveChanges();

                            return MessageService.CustomMessage("Produto Criado!");
                        }
                    }
                    else
                    {
                        return MessageService.CustomMessage("Os campos obrigatórios não foram preenchidos");
                    }
                }
            }
            else
            {
                return MessageService.AccessDeniedMessage();
            }
        }

        public static int[] VerifyProductAccess(string token, int userIdAccess)
        {
            try
            {
                if (int.TryParse(TokenManager.GetPrincipal(token).Claims.ToArray()[0].Value, out var idUtilizadorLogado) &&
                int.TryParse(TokenManager.GetPrincipal(token).Claims.ToArray()[1].Value, out var tipoUtilizadorLogado))
                {
                    if (tipoUtilizadorLogado == 2)
                    {
                        int[] userLogin = { idUtilizadorLogado, tipoUtilizadorLogado };
                        return userLogin;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

    }
}
