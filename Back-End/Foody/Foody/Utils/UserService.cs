using System;
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
            var utilizador = db.utilizador.ToArray();
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
                    for (int i = 0; i < utilizador.Length; i++)
                    {
                        if (novoUtilizador.email == utilizador[i].email)
                        {
                            return MessageService.CustomMessage("O utilizador com o email: " + novoUtilizador.email + " já está associado").text;
                        }
                    }

                    //verifica se o número de telemóvel já está associado
                    for (int i = 0; i < utilizador.Length; i++)
                    {
                        if (novoUtilizador.telemovel == utilizador[i].telemovel)
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
                        for (int i = 0; i < utilizador.Length; i++)
                        {
                            if (utilizador[i].telemovel == novoUtilizador.telemovel)
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
                            if (novoUtilizador.nif.ToString().Length == 9 &&  //empresa tem de ter nif
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
                                novoUtilizador.nif.ToString().Length == 1 &&
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
            DbHelper db = new DbHelper();
            //se os campos não exisitirem, irá criar utilizador
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
            else
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

        public static bool VerifyUser(string token, int accessUserId)
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
    }
}
