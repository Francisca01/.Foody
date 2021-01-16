using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using Foody.Models;

namespace Foody.Utils
{
    public class UserService
    {
        public static string ValidateUser(Utilizador novoUtilizador, bool editar)
        {
            //vai buscar todos os utilizador à base de dados 
            DbHelper db = new DbHelper();
            var utilizadorDB = db.utilizador.ToArray();
            int j = 0;

            //valores aceites para o nome
            var regexNome = new Regex("^[a-zA-Z ]*$");

            //valida a password
            //valida se tem pelo menos um numero
            var numero = new Regex(@"[0-9]+");

            //valida se tem pelo menos uma letra Maiuscula
            var letraMaiuscula = new Regex(@"[A-Z]+");

            //valida se tem o tamanho minimo
            var tamanho = new Regex(@".{8,}");

            //valida o email
            try
            {
                System.Net.Mail.MailAddress email = new System.Net.Mail.MailAddress(novoUtilizador.email);
            }
            catch (Exception)
            {
                return MessageService.CustomMessage("Formato de Email inválido").text;
            }

            try
            {
                //validação de campos do utilizador geral
                if (novoUtilizador != null && regexNome.IsMatch(novoUtilizador.nome) &&
                    numero.IsMatch(novoUtilizador.password) && letraMaiuscula.IsMatch(novoUtilizador.password) &&
                    tamanho.IsMatch(novoUtilizador.password))
                {
                    //verifica se o email já está associado
                    for (int i = 0; i < utilizadorDB.Length; i++)
                    {
                        if (novoUtilizador.email == utilizadorDB[i].email)
                        {
                            return MessageService.CustomMessage("O utilizador com o email: " + novoUtilizador.email + " já está associado").text;
                        }
                    }

                    //verifica se o número de telemóvel já está associado
                    for (int i = 0; i < utilizadorDB.Length; i++)
                    {
                        if (novoUtilizador.telemovel == utilizadorDB[i].telemovel)
                        {
                            //se o utilizador for uma empresa, não pode associar mais do que um número
                            if (novoUtilizador.tipoUtilizador == 2)
                            {
                                return MessageService.CustomMessage("A Empresa com o número de telemóvel: "
                                    + novoUtilizador.telemovel + " já está associado").text;
                            }

                            j++;

                            //verifica se o telemóvel ja está atribuido a mais do que 2 utilizadores
                            if (j > 1)
                            {
                                return MessageService.CustomMessage("O Utilizador com o telemóvel: " + novoUtilizador.telemovel + " já está associado").text;
                            }
                        }
                    }

                    j = 0;

                    //encripta a password
                    using (SHA256 sha256Hash = SHA256.Create())
                    {
                        novoUtilizador.password = HashPassword.GetHash(sha256Hash, novoUtilizador.password);
                    }

                    //cria morada
                    if (novoUtilizador.telemovel.ToString().Length >= 9)
                    {
                        //valida se o número de tlm está atribuido a mais de 2 contas
                        for (int i = 0; i < utilizadorDB.Length; i++)
                        {
                            if (utilizadorDB[i].telemovel == novoUtilizador.telemovel)
                            {
                                j++;
                                if (j == 2)
                                {
                                    return MessageService.CustomMessage("O número de telemóvel já se encontra atribuido a 2 contas!").text;
                                }
                            }
                        }

                        //se de alguma forma criarem outro tipo de utilizador diferente dos possiveis ele é automaticamente tornado em cliente
                        if (novoUtilizador.tipoUtilizador > 2 || novoUtilizador.tipoUtilizador < 0)
                        {
                            novoUtilizador.tipoUtilizador = 0;
                        }

                        //verifica se o utilizador é empresa
                        if (novoUtilizador.tipoUtilizador == 2)
                        {
                            //verifica o tamanho do nif
                            if (novoUtilizador.nif.Length == 9 &&  //empresa tem de ter nif
                                string.IsNullOrEmpty(novoUtilizador.tipoVeiculo) && //empresa nao tem tipoVeiculo
                                string.IsNullOrEmpty(novoUtilizador.numeroCartaConducao) && //empresa nao tem numeroCartaConducao
                                string.IsNullOrEmpty(novoUtilizador.dataNascimento)) //empresa nao tem dataNascimento
                            {
                                return CreateUpdate(novoUtilizador, editar);
                            }
                            else
                            {
                                return "O nif introduzido não é válido, tem de ter um valor de 9 caracteres";
                            }
                        }

                        //verifica se o utilizador é condutor
                        else if (novoUtilizador.tipoUtilizador == 1)
                        {
                            if (!string.IsNullOrEmpty(novoUtilizador.tipoVeiculo) && //condutor tem de ter tipoVeiculo
                                !string.IsNullOrEmpty(novoUtilizador.numeroCartaConducao) && //condutor tem de ter numeroCartaConducao
                                !string.IsNullOrEmpty(novoUtilizador.dataNascimento) && //condutor tem de ter dataNascimento
                                                                                        //condutor nao tem nif
                                novoUtilizador.nif.Length == 1 &&
                                novoUtilizador.numeroCartaConducao.Length >= 11) //condutor tem de ter carta de condução com pelo menos
                            {                                                    //11 caracteres
                                return CreateUpdate(novoUtilizador, editar);
                            }
                            else
                            {
                                return "Prencha todos os campos!";
                            }
                        }


                        //caso o utilizador não seja nehuma das entidades a cima então é considerada utilizador
                        else
                        {
                            if (string.IsNullOrEmpty(novoUtilizador.tipoVeiculo) && //cliente não tem tipoVeiculo
                                string.IsNullOrEmpty(novoUtilizador.numeroCartaConducao) && //cliente não tem numeroCartaConducao
                                !string.IsNullOrEmpty(novoUtilizador.dataNascimento)) //cliente tem de ter dataNascimento
                            {
                                return CreateUpdate(novoUtilizador, editar);

                            }
                            else
                            {
                                return "Introduza a Data de Nascimento";
                            }

                        }
                    }
                    else
                    {
                        return "Número de Telemóvel inválido";
                    }
                }
                else
                {
                    return "Palavra passe ou Nome inválido";
                }

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static string CreateUpdate(Utilizador novoUtilizador, bool editar)
        {
            using (DbHelper db = new DbHelper())
            {
                //criar utilizador
                if (editar == false)
                {
                    db.utilizador.Add(novoUtilizador);
                    db.SaveChanges();

                    //verifica o tipo de utilizador:
                    //verifica se é Empresa
                    if (novoUtilizador.tipoUtilizador == 2)
                    {
                        return "Empresa criada!";
                    }

                    //verifica se é Condutor
                    else if (novoUtilizador.tipoUtilizador == 1)
                    {
                        return "Condutor criado!";
                    }

                    //verifica se é Cliente
                    else
                    {
                        return "Cliente criado!";
                    }
                }
                else //caso contrário edita o utilizador
                {
                    //se os campos já exisitirem, irá editar utilizador (procura pelo id de utilizador)
                    var utilizadorDB = db.utilizador.Find(novoUtilizador.idUtilizador);
                    if (utilizadorDB != null)
                    {
                        utilizadorDB.dataNascimento = novoUtilizador.dataNascimento;
                        utilizadorDB.email = novoUtilizador.email;
                        utilizadorDB.idUtilizador = novoUtilizador.idUtilizador;
                        utilizadorDB.morada = novoUtilizador.morada;
                        utilizadorDB.nif = novoUtilizador.nif;
                        utilizadorDB.nome = novoUtilizador.nome;
                        utilizadorDB.numeroCartaConducao = novoUtilizador.numeroCartaConducao;
                        utilizadorDB.password = novoUtilizador.password;
                        utilizadorDB.telemovel = novoUtilizador.telemovel;
                        utilizadorDB.tipoUtilizador = novoUtilizador.tipoUtilizador;
                        utilizadorDB.tipoVeiculo = novoUtilizador.tipoVeiculo;

                        db.utilizador.Update(utilizadorDB);
                        db.SaveChanges();

                        //verifica o tipo de utilizador:
                        //verifica se é Empresa
                        if (novoUtilizador.tipoUtilizador == 2)
                        {
                            return "Empresa editada!";
                        }
                        //verifica se é Condutor
                        else if (novoUtilizador.tipoUtilizador == 1)
                        {
                            return "Condutor editado!";
                        }
                        //verifica se é Cliente
                        else
                        {
                            return "Cliente editado!";
                        }
                    }
                    else
                    {
                        return "Cliente inexistente!";
                    }
                }
            }
        }

        public static bool VerifyUserAccess(string token, int accessUserId)
        {
            try
            {
                if (int.TryParse(TokenManager.GetPrincipal(token).Claims.ToArray()[0].Value, out var idUtilizadorLogado) &&
                int.TryParse(TokenManager.GetPrincipal(token).Claims.ToArray()[1].Value, out var tipoUtilizadorLogado))
                {
                    DbHelper db = new DbHelper();
                    var user = db.utilizador.Find(accessUserId);

                    if (idUtilizadorLogado == accessUserId)
                    {
                        return true;
                    }
                    else if (user.tipoUtilizador == 2 && tipoUtilizadorLogado == 3)
                    {
                        return true;
                    }
                    else if (user.tipoUtilizador == 1 && tipoUtilizadorLogado == 3)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static int[] UserLoggedIn(string token)
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

        #region Get User
        public static List<object> GetUser(string token, int userType)
        {
            //verifica se utilizador Logado pode aceder
            bool canAccess = VerifyUserAccess(token, 3);

            if (canAccess)
            {
                //cria a variavel para aceder a base de dados
                using (DbHelper db = new DbHelper())
                {
                    //obter dados dos utilizadores na base de dados num array
                    var utilizadoresDB = db.utilizador.ToArray();

                    //array para devolver o resultado
                    List<Utilizador> clientes = new List<Utilizador>();

                    //incrementador
                    for (int i = 0; i < utilizadoresDB.Length; i++)
                    {
                        if (utilizadoresDB[i].tipoUtilizador == userType)//verifica se o utilizador é cliente
                        {
                            clientes.Add(utilizadoresDB[i]);
                        }
                    }

                    List<object> cls = new List<object>() { clientes };
                    return cls;
                }
            }
            else
            {
                List<object> msg = new List<object>() { MessageService.AccessDeniedMessage() };
                return msg;
            }

            //HttpContext.Response.StatusCode = (int);

        }
        #endregion

        #region Get User Id
        public static object GetUserId(string token, int idUtilizador)
        {
            //verifica se utilizador com a Sessão iniciada pode aceder
            if (VerifyUserAccess(token, idUtilizador))
            {
                // obter dados do utilizador na base de dados (por id especifico)
                using (DbHelper db = new DbHelper())
                {
                    var utilizadorDB = db.utilizador.Find(idUtilizador);

                    //verifica se o utilizador com o id existe
                    if (db.utilizador.Find(idUtilizador) != null)
                    {
                        return utilizadorDB;
                    }
                    else
                    {
                        return MessageService.WithoutResultsMessage();
                    }
                }
            }
            else
            {
                return MessageService.AccessDeniedMessage();
            }
        }
        #endregion

        #region Put User
        public static object PutUser(string token, Utilizador utilizadorUpdate, int accessUserId)
        {
            if (utilizadorUpdate != null)
            {
                //verifica se utilizador com a Sessão iniciada pode aceder
                if (VerifyUserAccess(token, accessUserId))
                {
                    //obter dados do utilizador na base de dados (por id especifico)
                    using (DbHelper db = new DbHelper())
                    {
                        var userDB = db.utilizador.Find(accessUserId);

                        //se cliente não existir, diz que não foram encontrados resultados
                        if (userDB != null)
                        {
                            string msg = ValidateUser(utilizadorUpdate, true);
                            return MessageService.CustomMessage(msg);
                        }

                        //se cliente existir, atualizar dados
                        else
                        {
                            return MessageService.WithoutResultsMessage();
                        }
                    }
                }
                else
                {
                    return MessageService.AccessDeniedMessage();
                }
            }
            else
            {
                return MessageService.WithoutResultsMessage();
            }
        }
        #endregion

        #region Delete User
        public static object DeleteUser(string token, int idUtilizador)
        {
            //verifica se utilizador com a Sessão iniciada pode aceder
            bool canAccess = VerifyUserAccess(token, idUtilizador);

            if (canAccess)
            {
                //obter dados do utilizador na base de dados (por id especifico)
                using (DbHelper db = new DbHelper())
                {
                    var userDB = db.utilizador.Find(idUtilizador);

                    //verifica se o utilizador com o id existe
                    if (db.utilizador.Find(idUtilizador) != null)
                    {
                        db.utilizador.Remove(userDB);
                        db.SaveChanges();

                        return MessageService.CustomMessage("Eliminado!");
                    }
                    else
                    {
                        return MessageService.WithoutResultsMessage();
                    }
                }
            }
            else
            {
                return MessageService.AccessDeniedMessage();
            }
        }
        #endregion
    }
}
