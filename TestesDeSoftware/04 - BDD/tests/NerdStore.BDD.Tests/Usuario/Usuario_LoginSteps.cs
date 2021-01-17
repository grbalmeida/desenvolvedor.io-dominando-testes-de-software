using NerdStore.BDD.Tests.Config;
using TechTalk.SpecFlow;
using Xunit;

namespace NerdStore.BDD.Tests.Usuario
{
    [Binding]
    [CollectionDefinition(nameof(AutomacaoWebFixtureCollection))]
    public class LoginDeUsuariosSteps
    {
        private readonly LoginUsuarioTela _loginUsuarioTela;
        private readonly AutomacaoWebTestsFixture _testsFixture;

        public LoginDeUsuariosSteps(AutomacaoWebTestsFixture testsFixture)
        {
            _testsFixture = testsFixture;
            _loginUsuarioTela = new LoginUsuarioTela(testsFixture.BrowserHelper);
        }

        [When(@"Ele clicar em login")]
        public void QuandoEleClicarEmLogin()
        {
            // Act
            _loginUsuarioTela.ClicarNoLinkLogin();

            // Assert
            Assert.Contains(_testsFixture.Configuration.LoginUrl, _loginUsuarioTela.ObterUrl());
        }

        [When(@"Preencher os dados do formulário de login")]
        public void QuandoPreencherOsDadosDoFormularioDeLogin(Table table)
        {
            // Arrange
            var usuario = new Usuario
            {
                Email = "teste@teste.com",
                Senha = "Teste@123"
            };

            _testsFixture.Usuario = usuario;

            // Act
            _loginUsuarioTela.PreencherFormularioLogin(usuario);

            // Assert
            Assert.True(_loginUsuarioTela.ValidarPreenchimentoFormularioLogin(usuario));
        }

        [When(@"Clicar no botão login")]
        public void QuandoClicarNoBotaoLogin()
        {
            _loginUsuarioTela.ClicarNoBotaoLogin();
        }
    }
}
