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
        public static string ValidateUser(User newUser, bool edit)
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
                return MessageService.Custom("Formato de Email inválido").text;
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
                        if (newUser.email == userDB[i].email)
                        {
                            return MessageService.Custom("O user com o email: "
                                + newUser.email + " já está associado").text;
                        }
                    }

                    //verifica se o número de telemóvel já está associado
                    for (int i = 0; i < userDB.Length; i++)
                    {
                        if (newUser.phone == userDB[i].phone)
                        {
                            //se o user for uma empresa, não pode associar mais do que um número
                            if (newUser.userType == 2)
                            {
                                return MessageService.Custom("A Empresa com o número de telemóvel: "
                                    + newUser.phone + " já está associado").text;
                            }

                            j++;

                            //verifica se o telemóvel ja está atribuido a mais do que 2 utilizadores
                            if (j > 1)
                            {
                                return MessageService.Custom("O User com o telemóvel: "
                                    + newUser.phone + " já está associado").text;
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
                        //valida se o número de tlm está atribuido a mais de 2 contas
                        for (int i = 0; i < userDB.Length; i++)
                        {
                            if (userDB[i].phone == newUser.phone)
                            {
                                j++;
                                if (j == 2)
                                {
                                    return MessageService.Custom("O número de telemóvel já se encontra atribuido a 2 contas!").text;
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
                            //verifica o tamanho do nif
                            if (newUser.nif.Length == 9 &&  //empresa tem de ter nif
                                string.IsNullOrEmpty(newUser.vehicleType) && //empresa nao tem vehicleType
                                string.IsNullOrEmpty(newUser.drivingLicense) && //empresa nao tem drivingLicense
                                string.IsNullOrEmpty(newUser.birthDate)) //empresa nao tem birthDate
                            {
                                return CreateUpdate(newUser, edit);
                            }
                            else
                            {
                                return "O nif introduzido não é válido, tem de ter um valor de 9 caracteres!";
                            }
                        }

                        //verifica se o user é condutor
                        else if (newUser.userType == 1)
                        {
                            if (!string.IsNullOrEmpty(newUser.vehicleType) && //condutor tem de ter vehicleType
                                !string.IsNullOrEmpty(newUser.drivingLicense) && //condutor tem de ter drivingLicense
                                !string.IsNullOrEmpty(newUser.birthDate) && //condutor tem de ter birthDate
                                                                                 //condutor nao tem nif
                                newUser.nif.Length == 1 &&
                                newUser.drivingLicense.Length >= 11) //condutor tem de ter carta de condução com pelo menos
                            {                                                    //11 caracteres
                                return CreateUpdate(newUser, edit);
                            }
                            else
                            {
                                return "Prencha todos os campos!";
                            }
                        }


                        //caso o user não seja nehuma das entidades a cima então é considerada user
                        else
                        {
                            if (string.IsNullOrEmpty(newUser.vehicleType) && //cliente não tem vehicleType
                                string.IsNullOrEmpty(newUser.drivingLicense) && //cliente não tem drivingLicense
                                !string.IsNullOrEmpty(newUser.birthDate)) //cliente tem de ter birthDate
                            {
                                return CreateUpdate(newUser, edit);

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

        public static string CreateUpdate(User newUser, bool edit)
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
                        return "Empresa criada!";
                    }

                    //verifica se é Condutor
                    else if (newUser.userType == 1)
                    {
                        return "Condutor criado!";
                    }

                    //verifica se é Cliente
                    else
                    {
                        return "Cliente criado!";
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
                        if (newUser.userType == 2)
                        {
                            return "Empresa editada!";
                        }
                        //verifica se é Condutor
                        else if (newUser.userType == 1)
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
                    var user = db.user.Find(accessUserId);

                    if (idUtilizadorLogado == accessUserId)
                    {
                        return true;
                    }
                    else if (user.userType == 2 && tipoUtilizadorLogado == 3)
                    {
                        return true;
                    }
                    else if (user.userType == 1 && tipoUtilizadorLogado == 3)
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
                    int[] userLogin = { idUtilizadorLogado, tipoUtilizadorLogado };
                    return userLogin;
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
        public static object PutUser(string token, User utilizadorUpdate, int accessUserId)
        {
            if (utilizadorUpdate != null)
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
                            string msg = ValidateUser(utilizadorUpdate, true);
                            return MessageService.Custom(msg);
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
        public static object DeleteUser(string token, int idUser)
        {
            //verifica se user com a Sessão iniciada pode aceder
            if (VerifyUserAccess(token, idUser))
            {
                //obter dados do user na base de dados (por id especifico)
                using (DbHelper db = new DbHelper())
                {
                    var userDB = db.user.Find(idUser);

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
