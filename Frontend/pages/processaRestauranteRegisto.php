<!DOCTYPE html>
	<html>
	<head>
		<title>Foody</title>
		<meta charset="UTF-8">
		<meta name="keywords" content="HTML, CSS, JavaScript">
		<meta name="description" content="Restaurantes Register">
		<meta name="author" content="Francisca Costa, Leonardo Vieira, Tomás Braga">

		<!-- Styles - CSS -->
		<link rel="stylesheet" href="../css/style.css">

		<!-- Links Externos -->
		<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.0-beta1/dist/css/bootstrap.min.css" rel="stylesheet"
		integrity="sha384-giJF6kkoqNQ00vy+HMDP7azOuL0xtbfIcaT9wjKHr8RbDVddVHyTfAAsrekwKmP1" crossorigin="anonymous">
		<script src="https://kit.fontawesome.com/739a3f8b60.js" crossorigin="anonymous"></script>
	</head>
	<body>
		<?php
			if($_POST['nomeRestaurante']==null){
				die("<p>É obrigatório indicar o nome do restaurante</p><p><a href='restauranteRegister.html'>Voltar<a/></p>");
			}
			if($_POST['emails']==null){
				die("<p>É obrigatório indicar o email</p><p><a href='restauranteRegister.html'>Voltar<a/></p>");
			}
			if($_POST['password1']==null && $_POST['password2']==null){
				die("<p>É obrigatório indicar a palavra-passe</p><p><a href='clienteRegister.html'>Voltar<a/></p>");
			}
			if($_POST['password1']!=$_POST['password2']){
				die("<p>As palavra-passe devem ser iguais</p><p><a href='clienteRegister.html'>Voltar<a/></p>");
			}
			$telemovels = $_POST['telemovel'];
			if(strlen($telemovels)!=9){
				die("<p>O número de telemóvel é constituído por 9 dígitos. Por favor, indique um número de telemóvel válido</p>
				<p><a href='restauranteRegister.html'>Voltar<a/></p>");
			}
			$NIF = $_POST['nif'];
			if(strlen($NIF)!=9){
				die("<p>O número de identificação fiscal (NIF) é constituído por 9 dígitos. Por favor, indique um NIF válido</p>
				<p><a href='restauranteRegister.html'>Voltar<a/></p>");
			}
			if($_POST['rua']==null){
				die("<p>É obrigatório indicar o nome da rua</p><p><a href='restauranteRegister.html'>Voltar<a/></p>");
			}
			if($_POST['numero']==null){
				die("<p>É obrigatório indicar o nome da porta</p><p><a href='restauranteRegister.html'>Voltar<a/></p>");
			}
			if($_POST['andar']==null){
				die("<p>É obrigatório indicar o andar</p><p><a href='restauranteRegister.html'>Voltar<a/></p>");
			}
			$codigopostal = $_POST['codigoPostal'];
			if(strlen($codigopostal)!=8){
				die("<p>O formato do Código-Postal é XXXX-XXX, em que X representa um número. Por favor, indique um Código-Postal válido</p>
				<p><a href='clienteRegister.html'>Voltar<a/></p>");
			}
			if($_POST['localidade']==null){
				die("<p>É obrigatório indicar o localidade</p><p><a href='restauranteRegister.html'>Voltar<a/></p>");
			}
		?>
		<script>
			window.location.replace("login.html");
		</script>
	</body>
</html>