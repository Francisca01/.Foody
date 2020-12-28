using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Foody.Models;
using Foody.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using System.IO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Foody.Controllers
{
    [Route("api/[controller]")]
    //[ApiController]
    public class RegistosController : ControllerBase
    {
        // POST api/<RegistosController>
        [HttpPost]
        public string Post([FromBody] Utilizador novoUtilizador, Morada novaMorada, object tipoDeUtilizador)
        {
            using (var db = new DbHelper())
            {
                
                //vai buscar todos os utilizador a base de dados 
                var utilizador = db.utilizador.ToArray();
                //Utilizador novoUtilizador = new Utilizador();
                //Morada novaMorada = new Morada();
                
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

                        //verificar o tipo de utilizador
                        Type cond = typeof(Condutor);
                        Type emp = typeof(Empresa);

                        //cria morada
                        if (createMorada(db, regexNome, novaMorada) && novoUtilizador.telemovel.ToString().Length >= 9)
                        {
                            //verifica se o utilizador é empresa
                            if (tipoDeUtilizador.Equals(emp))
                            {
                                Empresa novaEmpresa = (Empresa)tipoDeUtilizador;

                                if (novaEmpresa.nif.ToString().Length == 9)
                                {
                                    if (createUtilizador(db, novoUtilizador, novaMorada))
                                    {
                                        novaEmpresa.idUtilizador = novoUtilizador.idUtilizador;

                                        db.empresa.Add(novaEmpresa);
                                        db.SaveChanges();
                                    }
                                    else
                                    {
                                        return "Não foi possivel criar o utilizador!";
                                    }

                                    return "Empresa criada!";
                                }
                                else
                                {
                                    return "O nif introduzido não é válido, tem de ter um valor de 9 caracteres";
                                }
                            }

                            if (novoUtilizador.dataNascimento != null)
                            {
                                //verifica se o utilizador é condutor
                                if (tipoDeUtilizador.Equals(cond))
                                {
                                    Condutor novoCondutor = (Condutor)tipoDeUtilizador;

                                    if (novoCondutor.tipoDeVeiculo != null && novoCondutor.numeroCartaConducao.Length >= 11)
                                    {
                                        if (createUtilizador(db, novoUtilizador, novaMorada))
                                        {
                                            novoCondutor.idUtilizador = novoUtilizador.idUtilizador;

                                            db.condutor.Add(novoCondutor);
                                            db.SaveChanges();
                                        }
                                        else
                                        {
                                            return "Não foi possivel criar o utilizador!";
                                        }

                                        return "Condutor criado!";
                                    }
                                    else
                                    {
                                        return "O Tipo de Veiculo ou o número de Carta de condução são inválidos";
                                    }
                                }

                                //caso o utilizador não seja nehuma das entidades a cima então é considerada utilizador
                                else
                                {
                                    Cliente novoCliente = (Cliente)tipoDeUtilizador;
                                    
                                    if (createUtilizador(db, novoUtilizador, novaMorada))
                                    {
                                        novoUtilizador.idMorada = novaMorada.idMorada;

                                        db.utilizador.Add(novoUtilizador);
                                        db.SaveChanges();
                                    }
                                    else
                                    {
                                        return "Não foi possivel criar o utilizador!";
                                    }

                                    novoCliente.idUtilizador = novoUtilizador.idUtilizador;

                                    db.cliente.Add(novoCliente);
                                    db.SaveChanges();

                                    return "Empresa criada!";
                                }
                            }
                        }
                        return "Utilizador Criado";
                    }
                    else
                    {
                        return "Valores inválidos";
                    }

                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
        bool createMorada(DbHelper db, Regex regexNome, Morada novaMorada)
        {
            //verifica a morada
            if (novaMorada != null && novaMorada.codigoPostal != null && novaMorada.localidade != null &&
                               regexNome.IsMatch(novaMorada.localidade) && novaMorada.nomeRua != null &&
                               regexNome.IsMatch(novaMorada.nomeRua) && novaMorada.numeroPorta > 0)
            {
                db.morada.Add(novaMorada);
                db.SaveChanges();

                return true;
            }
            return false;
        }

        bool createUtilizador(DbHelper db, Utilizador novoUtilizador, Morada novaMorada)
        {
            novoUtilizador.idMorada = novaMorada.idMorada;
            db.utilizador.Add(novoUtilizador);
            db.SaveChanges();

            return true;
        }

    }
}
