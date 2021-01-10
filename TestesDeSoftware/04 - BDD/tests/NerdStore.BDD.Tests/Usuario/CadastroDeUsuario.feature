Funcionalidade: Usuário - Cadastro
	Como um visitante da loja
	Eu desejo me cadastrar como usuário
	Para que eu possa realizar compras na loja

Cenário: Cadastro de usuário com sucesso
Dado Que o visitante está acessando o site da loja
Quando Ele clicar em registrar
E Preencher os dados do formulário
	| Dados                |
	| E-mail               |
	| Senha                |
	| Confirmação da Senha |
E Clicar no botão registrar
Então Ele será redirecionado para a vitrine
E Uma saudação com seu e-mail será exibida no menu superior

Cenário: Cadastro com senha sem maiúsculas
Dado Que o visitante está acessando o site da loja
Quando Ele clicar em registrar
E Preencher os dados do formulário com uma senha sem maiúsculas
	| Dados                |
	| E-mail               |
	| Senha                |
	| Confirmação da Senha |
E Clicar no botão registrar
Então Ele receberá uma mensagem de erro que a senha precisa conter uma letra maiúscula

Cenário: Cadastro com senha sem caractere especial
Dado Que o visitante está acessando o site da loja
Quando Ele clicar em registrar
E Preencher os dados do formulário com uma senha sem caractere especial
	| Dados                |
	| E-mail               |
	| Senha                |
	| Confirmação da Senha |
E Clicar no botão registrar
Então Ele receberá uma mensagem de erro que a senha precisa conter um caractere especial
