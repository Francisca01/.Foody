<!DOCTYPE HTML>
<html>
	<head>
		<title>Foody</title>
		<meta charset="UTF-8">
		<meta name="keywords" content="HTML, CSS, JavaScript">
		<meta name="description" content="Login">
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
			if($_POST['exampleInputEmail1']==null){
				die("<p>É obrigatório indicar o email</p><p><a href='login.html'>Voltar<a/></p>");
			}
			if($_POST['exampleInputPassword1']==null){
				die("<p>É obrigatório indicar a password</p><p><a href='login.html'>Voltar<a/></p>");
			}
		?>
		<script>
			window.location.replace("../index.html");
		</script>
	</body>
</html>