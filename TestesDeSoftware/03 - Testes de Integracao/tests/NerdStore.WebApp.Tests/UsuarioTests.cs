using Bogus;
using NerdStore.WebApp.MVC;
using NerdStore.WebApp.Tests.Config;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace NerdStore.WebApp.Tests
{
    [Collection(nameof(IntegrationWebTestsFixtureCollection))]
    public class UsuarioTests
    {
        private readonly IntegrationTestsFixture<StartupWebTests> _testsFixture;

        public UsuarioTests(IntegrationTestsFixture<StartupWebTests> testsFixture)
        {
            _testsFixture = testsFixture;
        }

        [Fact(DisplayName = "Realizar cadastro com sucesso")]
        [Trait("Categoria", "Integração Web - Usuário")]
        public async Task Usuario_RealizarCadastro_DeveExecutarComSucesso()
        {
            // Arrange
            var initialResponse = await _testsFixture.Client.GetAsync("/Identity/Account/Register");
            initialResponse.EnsureSuccessStatusCode();

            var antiForgeryToken = _testsFixture.ObterAntiForgeryToken(await initialResponse.Content.ReadAsStringAsync());

            _testsFixture.GerarUserSenha();

            var formData = new Dictionary<string, string>
            {
                { _testsFixture.AntiForgeryFieldName, antiForgeryToken },
                { "Input.Email", _testsFixture.UsuarioEmail },
                { "Input.Password", _testsFixture.UsuarioSenha },
                { "Input.ConfirmPassword", _testsFixture.UsuarioSenha }
            };

            var postRequest = new HttpRequestMessage(HttpMethod.Post, "/Identity/Account/Register")
            {
                Content = new FormUrlEncodedContent(formData)
            };

            // Act
            var postResponse = await _testsFixture.Client.SendAsync(postRequest);

            // Assert
            var responseString = await postResponse.Content.ReadAsStringAsync();

            postResponse.EnsureSuccessStatusCode();
            Assert.Contains($"Hello {_testsFixture.UsuarioEmail}!", responseString);
        }
    }
}
