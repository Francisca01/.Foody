# Quem Somos?
Foody é uma empresa criada tendo em conta os tempos pandémicos atuais, que tem atacado sobretudo a área da restauração.
Temos como principal objetivo, permitir que as pessoas saboreiem do seu prato predileto sem terem que sair de casa, respeitando assim as normas sanitárias.

<br>

# Tecnologias Usadas
<table align="center">
  <tr><th align="center">Parte do Projeto</th><th align="center">Tecnologias Usadas</th></tr>
  <tr><td>Back-end</td><td>C#</td></tr>
  <tr><td>Front-end</td><td>Html, CSS e JavaScript</td></tr>
  <tr><td>Base de Dados</td><td>SQLite</td></tr>
  <tr><td>Desenvolvimento</td><td>SCRUM e GitHub</td></tr>
</table>
 
<br>
  
# Documentação da API
[Clique Aqui para visualizar a documentação completa](https://documenter.getpostman.com/view/12996380/TW6tMVoi)

<br>

# Especificações da API


<table align="center">
<tr><th align="center">Noun</th><th align="center">HHT Action</th><th align="center">REST URL</th><th align="center">Description</th></tr>
  <tr><td> Registar </td><td> POST </td><td> /api/register </td><td> Registar Empresa, Cliente ou Condutor </td></tr>
  <tr><td> Login </td><td> POST </td><td> /api/logins </td><td> Efetuar Login </td></tr>
  <tr><td> Clientes </td><td> GET </td><td> /api/clients </td><td> Lista todos os Clientes </td></tr>
  <tr><td> Clientes </td><td> GET </td><td> /api/clients/{id} </td><td> Lista Cliente pelo ID </td></tr>
  <tr><td> Clientes </td><td> PUT </td><td> /api/clients/{id} </td><td> Edita um Cliente pelo ID </td></tr>
  <tr><td> Clientes </td><td> DELETE </td><td> /api/clients/{id} </td><td> Apaga um Cliente pelo ID </td></tr>
  <tr><td> Condutores </td><td> GET </td><td> /api/drivers </td><td> Lista todos os Condutores </td></tr>
  <tr><td> Condutores </td><td> GET </td><td> /api/drivers/{id} </td><td> Lista Condutor pelo ID </td></tr>
  <tr><td> Condutores </td><td> PUT </td><td> /api/drivers/{id} </td><td> Edita um Condutor pelo ID </td></tr>
  <tr><td> Condutores </td><td> DELETE </td><td> /api/drivers/{id} </td><td> Apaga um Condutor pelo ID </td></tr>
  <tr><td> Empresas </td><td> GET </td><td> /api/companies </td><td> Lista todos as Empresas </td></tr>
  <tr><td> Empresas </td><td> GET </td><td> /api/companies/{id} </td><td> Lista Empresa pelo ID </td></tr>
  <tr><td> Empresas </td><td> PUT </td><td> /api/companies/{id} </td><td> Edita uma Empresa pelo ID </td></tr>
  <tr><td> Empresas </td><td> DELETE </td><td> /api/companies/{id} </td><td> Apaga uma Empresa pelo ID </td></tr>
  <tr><td> Encomendas </td><td> GET </td><td> /api/orders </td><td> Lista todas as Encomendas </td></tr>
  <tr><td> Encomendas </td><td> GET </td><td> /api/orders/{id} </td><td>  Lista Encomenda pelo ID </td></tr>
  <tr><td> Encomendas </td><td> POST </td><td> /api/orders </td><td> Adiciona uma Encomenda </td></tr>
  <tr><td> Encomendas </td><td> POST </td><td> /api/orders/{id} </td><td> Adicionar Produto a Encomenda </td></tr>
  <tr><td> Encomendas </td><td> PUT </td><td> /api/orders/{IdOrder}/{IdProduct} </td><td> Editar Produto em Endomenda </td></tr>
  <tr><td> Encomendas </td><td> PUT </td><td> /api/orders/{id} </td><td> Edita uma Encomenda pelo ID </td></tr>
  <tr><td> Encomendas </td><td> DELETE </td><td> /api/orders/{IdOrder}/{IdProduct} </td><td> Eliminar Produto de Encomenda </td></tr>
  <tr><td> Encomendas </td><td> DELETE </td><td> /api/encomendas/{id} </td><td> Apaga uma Encomenda pelo ID </td></tr>
  <tr><td> Entregas </td><td> GET </td><td> /api/deliveries </td><td> Lista todos as Entregas </td></tr>
  <tr><td> Entregas </td><td> GET </td><td> /api/deliveries/{id} </td><td> Lista Entrega pelo ID </td></tr>
  <tr><td> Entregas </td><td> PUT </td><td> /api/deliveries/{id} </td><td> Edita uma Entrega pelo ID </td></tr>
  <tr><td> Produtos </td><td> GET </td><td> /api/products </td><td> Lista todos os Produtos </td></tr>
  <tr><td> Produtos </td><td> GET </td><td> /api/products/{id} </td><td> Lista Produto pelo ID </td></tr>
  <tr><td> Produtos </td><td> POST </td><td> /api/products </td><td> Adiciona um Produto </td></tr>
  <tr><td> Produtos </td><td> PUT </td><td> /api/products/{id} </td><td> Edita um Produto pelo ID </td></tr>
  <tr><td> Produtos </td><td> DELETE </td><td> /api/products/{id} </td><td> Apaga um Produto pelo ID </td></tr>
</table>

<br>

# Copyright
Código e Documentação © 2020-2021
Francisca Costa & Leonardo Vieira & Tomás Braga
