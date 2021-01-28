using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using Foody.Models;
using Microsoft.AspNetCore.Mvc;


namespace Foody.Utils
{
    public class UserService
    {
        public static Message ValidateUser(User newUser, bool edit)
        {
            //vai buscar todos os user à base de dados 
            DbHelper db = new DbHelper();
            var userDB = db.user.ToArray();
            int j = 0;

            //valores aceites para o name
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
                System.Net.Mail.MailAddress email = new System.Net.Mail.MailAddress(newUser.email);
            }
            catch (Exception)
            {
                return MessageService.Custom("Formato de Email inválido");
            }

            try
            {
                //validação de campos do user geral
                if (newUser != null && regexNome.IsMatch(newUser.name) &&
                    numero.IsMatch(newUser.password) && letraMaiuscula.IsMatch(newUser.password) &&
                    tamanho.IsMatch(newUser.password))
                {

                    //verifica se o email já está associado
                    for (int i = 0; i < userDB.Length; i++)
                    {
                        if (edit == false)
                        {
                            if (newUser.email == userDB[i].email)
                            {
                                return MessageService.Custom("O user com o email: "
                                    + newUser.email + " já está associado");
                            }
                        }
                        else
                        {
                            if (newUser.email == userDB[i].email)
                            {
                                j++;

                                if (j > 1)
                                {
                                    return MessageService.Custom("O user com o email: "
                                    + newUser.email + " já está associado");
                                }

                                if (newUser.idUser != userDB[i].idUser)
                                {
                                    return MessageService.Custom("O user com o email: "
                                        + newUser.email + " já está associado");
                                }
                            }
                        }
                    }

                    j = 0;

                    if (edit == false)
                    {
                        //verifica se o número de telemóvel já está associado
                        for (int i = 0; i < userDB.Length; i++)
                        {
                            if (newUser.phone == userDB[i].phone)
                            {
                                //se o user for uma empresa, não pode associar mais do que um número
                                if (newUser.userType == 2)
                                {
                                    return MessageService.Custom("A Empresa com o número de telemóvel: "
                                        + newUser.phone + " já está associado");
                                }

                                j++;

                                //verifica se o telemóvel ja está atribuido a mais do que 2 utilizadores
                                if (j > 1)
                                {
                                    return MessageService.Custom("O User com o telemóvel: "
                                        + newUser.phone + " já está associado");
                                }
                            }
                        }
                    }

                    j = 0;

                    //encripta a password
                    using (SHA256 sha256Hash = SHA256.Create())
                    {
                        newUser.password = HashPassword.GetHash(sha256Hash, newUser.password);
                    }

                    //cria address
                    if (newUser.phone.ToString().Length >= 9)
                    {
                        if (edit == false)
                        {
                            //valida se o número de tlm está atribuido a mais de 2 contas
                            for (int i = 0; i < userDB.Length; i++)
                            {
                                if (userDB[i].phone == newUser.phone)
                                {
                                    j++;
                                    if (j == 2)
                                    {
                                        return MessageService.Custom("O número de telemóvel já se encontra atribuido a 2 contas!");
                                    }
                                }
                            }
                        }

                        //se de alguma forma criarem outro tipo de user diferente dos possiveis ele é automaticamente tornado em cliente
                        if (newUser.userType > 2 || newUser.userType < 0)
                        {
                            newUser.userType = 0;
                        }

                        //verifica se o user é empresa
                        if (newUser.userType == 2)
                        {
                            //verifica se colocou nif
                            if (!string.IsNullOrEmpty(newUser.nif))
                            {
                                //verifica o tamanho do nif
                                if (newUser.nif.Length == 9)  //empresa tem de ter nif
                                {
                                    if (edit == false)
                                    {
                                        newUser.birthDate = null; //empresa nao tem birthDate
                                        newUser.drivingLicense = null; //empresa nao tem drivingLicense
                                        newUser.vehicleType = null;//empresa nao tem vehicleType
                                    }

                                    return CreateUpdate(newUser, edit);
                                }
                                else
                                {
                                    return MessageService.Custom("O nif introduzido não é válido, tem de ter um valor de 9 caracteres!");
                                }
                            }
                            else
                            {
                                return MessageService.Custom("Introduza um nif!");
                            }
                        }

                        //verifica se o user é condutor
                        else if (newUser.userType == 1)
                        {
                            if (!string.IsNullOrEmpty(newUser.vehicleType) && //condutor tem de ter vehicleType
                                !string.IsNullOrEmpty(newUser.drivingLicense) && //condutor tem de ter drivingLicense
                                !string.IsNullOrEmpty(newUser.birthDate)) //condutor tem de ter birthDate
                            {
                                if (newUser.drivingLicense.Length >= 11)//condutor tem de ter carta de condução com pelo menos 11 caracteres
                                {
                                    if (edit == false)
                                    {
                                        newUser.nif = null; //condutor nao tem nif
                                    }

                                    return CreateUpdate(newUser, edit);
                                }
                                else
                                {
                                    return MessageService.Custom("Licença de Condução não está completa!");
                                }
                            }
                            else
                            {
                                return MessageService.Custom("Prencha todos os campos!");
                            }
                        }

                        //caso o user não seja nehuma das entidades a cima então é considerada user
                        else
                        {
                            if (!string.IsNullOrEmpty(newUser.birthDate)) //cliente tem de ter birthDate
                            {
                                if (edit == false)
                                {
                                    newUser.vehicleType = null;
                                    newUser.drivingLicense = null;
                                }
                                return CreateUpdate(newUser, edit);
                            }
                            else
                            {
                                return MessageService.Custom("Introduza a Data de Nascimento");
                            }

                        }
                    }
                    else
                    {
                        return MessageService.Custom("Número de Telemóvel inválido");
                    }
                }
                else
                {
                    return MessageService.Custom("Palavra passe ou Nome inválido");
                }

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static Message CreateUpdate(User newUser, bool edit)
        {
            using (DbHelper db = new DbHelper())
            {
                //criar user
                if (edit == false)
                {
                    db.user.Add(newUser);
                    db.SaveChanges();

                    //verifica o tipo de user:
                    //verifica se é Empresa
                    if (newUser.userType == 2)
                    {
                        return MessageService.Custom("Empresa criada!");
                    }

                    //verifica se é Condutor
                    else if (newUser.userType == 1)
                    {
                        return MessageService.Custom("Condutor criado!");
                    }

                    //verifica se é Cliente
                    else
                    {
                        return MessageService.Custom("Cliente criado!");
                    }
                }
                else //caso contrário edita o user
                {
                    //se os campos já exisitirem, irá edit user (procura pelo id de user)
                    var userDB = db.user.Find(newUser.idUser);

                    if (userDB != null)
                    {
                        userDB.birthDate = newUser.birthDate;
                        userDB.email = newUser.email;
                        userDB.address = newUser.address;
                        userDB.nif = newUser.nif;
                        userDB.name = newUser.name;
                        userDB.drivingLicense = newUser.drivingLicense;
                        userDB.password = newUser.password;
                        userDB.phone = newUser.phone;
                        userDB.vehicleType = newUser.vehicleType;

                        db.user.Update(userDB);
                        db.SaveChanges();

                        //verifica o tipo de user:
                        //verifica se é Empresa
                        if (userDB.userType == 2)
                        {
                            return MessageService.Custom("Empresa editada!");
                        }
                        //verifica se é Condutor
                        else if (userDB.userType == 1)
                        {
                            return MessageService.Custom("Condutor editado!");
                        }
                        //verifica se é Cliente
                        else
                        {
                            return MessageService.Custom("Cliente editado!");
                        }
                    }
                    else
                    {
                        return MessageService.Custom("Cliente inexistente!");
                    }
                }
            }
        }

        public static bool VerifyUserAccess(string token, int accessUserId)
        {
            if (token != null)
            {
                try
                {
                    if (int.TryParse(TokenManager.GetPrincipal(token).Claims.ToArray()[0].Value, out var idUtilizadorLogado) &&
                    int.TryParse(TokenManager.GetPrincipal(token).Claims.ToArray()[1].Value, out var tipoUtilizadorLogado))
                    {
                        DbHelper db = new DbHelper();
                        var user = db.user.Find(accessUserId);

                        if (idUtilizadorLogado == accessUserId)
                        {
                            return true;
                        }
                        else if (tipoUtilizadorLogado == 3)
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
            else
            {
                return false;
            }
        }

        public static int[] UserLoggedIn(string token)
        {
            if (token != null)
            {
                try
                {
                    if (int.TryParse(TokenManager.GetPrincipal(token).Claims.ToArray()[0].Value, out var idUtilizadorLogado) &&
                    int.TryParse(TokenManager.GetPrincipal(token).Claims.ToArray()[1].Value, out var tipoUtilizadorLogado))
                    {
                        int[] userLoggedIn = { idUtilizadorLogado, tipoUtilizadorLogado };
                        return userLoggedIn;
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
            else
            {
                return null;
            }
        }

        #region Get User
        public static List<object> GetUser(string token, int userType)
        {
            //verifica se user Logado pode aceder
            if (VerifyUserAccess(token, 3))
            {
                //cria a variavel para aceder a base de dados
                using (DbHelper db = new DbHelper())
                {
                    //obter dados dos utilizadores na base de dados num array
                    var utilizadoresDB = db.user.ToArray();

                    //array para devolver o resultado
                    List<User> clientes = new List<User>();

                    //incrementador
                    for (int i = 0; i < utilizadoresDB.Length; i++)
                    {
                        if (utilizadoresDB[i].userType == userType)//verifica se o user é cliente
                        {
                            utilizadoresDB[i].password = null;
                            clientes.Add(utilizadoresDB[i]);
                        }
                    }

                    List<object> cls = new List<object>() { clientes };
                    return cls;
                }
            }
            else
            {
                List<object> msg = new List<object>() { MessageService.AccessDenied() };
                return msg;
            }

            //HttpContext.Response.StatusCode = (int);

        }
        #endregion

        #region Get User Id
        public static object GetUserId(string token, int idUser)
        {
            //verifica se user com a Sessão iniciada pode aceder
            if (VerifyUserAccess(token, idUser))
            {
                // obter dados do user na base de dados (por id especifico)
                using (DbHelper db = new DbHelper())
                {
                    var userDB = db.user.Find(idUser);

                    //verifica se o user com o id existe
                    if (db.user.Find(idUser) != null)
                    {
                        userDB.password = null;
                        return userDB;
                    }
                    else
                    {
                        return MessageService.WithoutResults();
                    }
                }
            }
            else
            {
                return MessageService.AccessDenied();
            }
        }
        #endregion

        #region Put User
        public static Message PutUser(string token, User userUpdate, int accessUserId)
        {
            if (userUpdate != null)
            {
                //verifica se user com a Sessão iniciada pode aceder
                if (VerifyUserAccess(token, accessUserId))
                {
                    //obter dados do user na base de dados (por id especifico)
                    using (DbHelper db = new DbHelper())
                    {
                        var userDB = db.user.Find(accessUserId);

                        //se cliente não existir, diz que não foram encontrados resultados
                        if (userDB != null)
                        {
                            userUpdate.idUser = accessUserId;
                            return ValidateUser(userUpdate, true);
                        }

                        //se cliente existir, atualizar dados
                        else
                        {
                            return MessageService.WithoutResults();
                        }
                    }
                }
                else
                {
                    return MessageService.AccessDenied();
                }
            }
            else
            {
                return MessageService.WithoutResults();
            }
        }
        #endregion

        #region Delete User
        public static Message DeleteUser(string token, int idUser)
        {
            //verifica se user com a Sessão iniciada pode aceder
            if (VerifyUserAccess(token, idUser))
            {
                //obter dados do user na base de dados (por id especifico)
                using (DbHelper db = new DbHelper())
                {
                    var userDB = db.user.Find(idUser);

                    //valida se empresa está associada a algum produto
                    foreach (var product in db.product.ToArray())
                    {
                        if (idUser == product.idCompany)
                        {
                            return MessageService.Custom("Não é permitido eliminar uma empresa que tenha um produto associado!");
                        }
                    }

                    //valida se cliente está associada a alguma encomenda
                    foreach (var order in db.order.ToArray())
                    {
                        if (idUser == order.idClient)
                        {
                            return MessageService.Custom("Não é permitido eliminar um cliente que tenha uma encomenda associado!");
                        }
                    }

                    //valida se condutor está associada a alguma entrega
                    foreach (var delivery in db.delivery.ToArray())
                    {
                        if (idUser == delivery.idDriver)
                        {
                            //object ok = HttpStatusCode.BadRequest;
                            return MessageService.Custom("Não é permitido eliminar um condutor que tenha uma entrega associado!");
                        }
                    }

                    //verifica se o user com o id existe
                    if (userDB != null)
                    {                        
                        db.user.Remove(userDB);
                        db.SaveChanges();

                        return MessageService.Custom("Eliminado!");
                    }
                    else
                    {
                        return MessageService.WithoutResults();
                    }
                }
            }
            else
            {
                return MessageService.AccessDenied();
            }
        }
        #endregion
    }
}
