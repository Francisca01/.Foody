# Quem Somos?
Foody é uma empresa criada tendo em conta os tempos pandémicos atuais, que tem atacado sobretudo a área da restauração.
Temos como principal objetivo, permitir que as pessoas saboreiem do seu prato predileto sem terem que sair de casa, respeitando assim as normas sanitárias.

<br>
 
# Tecnologias Usadas TESTE
- | Parte do Projeto | Tecnologias Usadas |
- | --- | --- |
- | **Back-end** | C# |
- | **Front-end** | Html, CSS e JavaScript |
- | **Base de Dados** | SQLite |
- | **Desenvolvimento** | SCRUM e GitHub |

<br>
  
# Documentação da API
[Clique Aqui para visualizar a documentação completa](https://documenter.getpostman.com/view/12996380/TW6tMVoi)

<br>

# Especificações da API
| Noun | HHT Action | REST URL | Description
| --- | --- | --- | --- |
| Registar | POST | /api/register | Registar Empresa, Cliente ou Condutor
| Login | POST | /api/logins | Efetuar Login
| Clientes | GET | /api/clients | Lista todos os Clientes
| Clientes | GET | /api/clients/{id} | Lista Cliente pelo ID
| Clientes | PUT | /api/clients/{id} | Edita um Cliente pelo ID
| Clientes | DELETE | /api/clients/{id} | Apaga um Cliente pelo ID
| Condutores | GET | /api/drivers | Lista todos os Condutores
| Condutores | GET | /api/drivers/{id} | Lista Condutor pelo ID
| Condutores | PUT | /api/drivers/{id} | Edita um Condutor pelo ID
| Condutores | DELETE | /api/drivers/{id} | Apaga um Condutor pelo ID
| Empresas | GET | /api/companies | Lista todos as Empresas
| Empresas | GET | /api/companies/{id} | Lista Empresa pelo ID
| Empresas | PUT | /api/companies/{id} | Edita uma Empresa pelo ID
| Empresas | DELETE | /api/companies/{id} | Apaga uma Empresa pelo ID
| Encomendas | GET | /api/orders | Lista todas as Encomendas
| Encomendas | GET | /api/orders/{id} | Lista Encomenda pelo ID
| Encomendas | POST | /api/orders | Adiciona uma Encomenda
| Encomendas | POST | /api/orders/{id} | Adicionar Produto a Encomenda
| Encomendas | PUT | /api/orders/{IdOrder}/{IdProduct} | Editar Produto em Endomenda 
| Encomendas | PUT | /api/orders/{id} | Edita uma Encomenda pelo ID
| Encomendas | DELETE | /api/orders/{IdOrder}/{IdProduct} | Eliminar Produto de Encomenda
| Encomendas | DELETE | /api/encomendas/{id} | Apaga uma Encomenda pelo ID
| Entregas | GET | /api/deliveries | Lista todos as Entregas
| Entregas | GET | /api/deliveries/{id} | Lista Entrega pelo ID
| Entregas | PUT | /api/deliveries/{id} | Edita uma Entrega pelo ID
| Produtos | GET | /api/products | Lista todos os Produtos
| Produtos | GET | /api/products/{id} | Lista Produto pelo ID
| Produtos | POST | /api/products | Adiciona um Produto
| Produtos | PUT | /api/products/{id} | Edita um Produto pelo ID
| Produtos | DELETE | /api/products/{id} | Apaga um Produto pelo ID

<br>

# Copyright
Código e Documentação © 2020-2021
Francisca Costa & Leonardo Vieira & Tomás Braga
