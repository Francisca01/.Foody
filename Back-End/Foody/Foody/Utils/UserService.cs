using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Foody.Models;

namespace Foody.Utils
{
    public class UserService
    {
        public static string CreateUtilizador(DbHelper db, Utilizador novoUtilizador, bool editar)
        {
            //vai buscar todos os utilizador a base de dados 
            var utilizador = db.utilizador.ToArray();

            //valores aceites para o nome
            var regexNome = new Regex("^[a-zA-Z ]*$");

            //valida o email
            System.Net.Mail.MailAddress email = new System.Net.Mail.MailAddress(novoUtilizador.email);

            //valida a password

            //valida se tem pelo menos um numero
            var numero = new Regex(@"[0-9]+");

            //valida se tem pelo menos uma letra Maiuscula
            var letraMaiuscula = new Regex(@"[A-Z]+");

            //valida se tem o tamanho minimo
            var tamanho = new Regex(@".{8,}");

            try
            {
                //validação de campos do utilizador geral
                if (novoUtilizador != null && regexNome.IsMatch(novoUtilizador.nome) &&
                    numero.IsMatch(novoUtilizador.password) && letraMaiuscula.IsMatch(novoUtilizador.password) &&
                    tamanho.IsMatch(novoUtilizador.password)
                    )
                {
                    //verifica se o email já está associado
                    for (int i = 0; i < utilizador.Length; i++)
                    {
                        if (novoUtilizador.email == utilizador[i].email)
                        {
                            return "O utilizador com o email: " + novoUtilizador.email + " já está associado";
                        }
                    }

                    //encripta a password
                    using (SHA256 sha256Hash = SHA256.Create())
                    {
                        novoUtilizador.password = HashPassword.GetHash(sha256Hash, novoUtilizador.password);
                    }

                    //cria morada
                    if (novoUtilizador.telemovel.ToString().Length >= 9)
                    {
                        //verifica se o utilizador é empresa
                        if (novoUtilizador.tipoUtilizador == 2)
                        {
                            //verifica o tamanho do nif
                            if (novoUtilizador.nif.ToString().Length == 9 &&  //empresa tem de ter nif
                                string.IsNullOrEmpty(novoUtilizador.tipoVeiculo) && //empresa nao tem tipoVeiculo
                                string.IsNullOrEmpty(novoUtilizador.numeroCartaConducao) && //empresa nao tem numeroCartaConducao
                                string.IsNullOrEmpty(novoUtilizador.dataNascimento)) //empresa nao tem dataNascimento
                            {
                                db.utilizador.Add(novoUtilizador);
                                db.SaveChanges();

                                return "Empresa criada!";
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
                                (string.IsNullOrEmpty(novoUtilizador.nif.ToString()) || //condutor nao tem nif
                                novoUtilizador.nif.ToString().Length == 0) &&
                                novoUtilizador.numeroCartaConducao.Length >= 11) //condutor tem de ter carta de condução com pelo menos
                            {                                                    //11 caracteres
                                db.utilizador.Add(novoUtilizador);
                                db.SaveChanges();

                                return "Condutor criado!";
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
                                !string.IsNullOrEmpty(novoUtilizador.dataNascimento) //cliente tem de ter dataNascimento
                                )
                            {
                                db.utilizador.Add(novoUtilizador);
                                db.SaveChanges();

                                return "Cliente criado!";
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
    }
}
