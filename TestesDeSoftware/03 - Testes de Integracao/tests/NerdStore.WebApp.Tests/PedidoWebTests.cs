using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AngleSharp.Html.Parser;
using Features.Tests;
using NerdStore.WebApp.MVC;
using NerdStore.WebApp.Tests.Config;
using Xunit;

namespace NerdStore.WebApp.Tests
{
    [TestCaseOrderer("Features.Tests.PriorityOrder", "Features.Tests")]
    [Collection(nameof(IntegrationWebTestsFixtureCollection))]
    public class PedidoWebTests
    {
        private readonly IntegrationTestsFixture<StartupWebTests> _testsFixture;

        public PedidoWebTests(IntegrationTestsFixture<StartupWebTests> testsFixture)
        {
            _testsFixture = testsFixture;
        }

        [Fact(DisplayName = "Adicionar item em novo pedido"), TestPriority(1)]
        [Trait("Categoria", "Integração Web - Pedido")]
        public async Task AdicionarItem_NovoPedido_DeveAtualizarValorTotal()
        {
            // Arrange
            var produtoId = new Guid("e83bde8d-7205-4c58-b491-51f153e1cf83");
            const int quantidade = 2;

            var initialResponse = await _testsFixture.Client.GetAsync($"/produto-detalhe/{produtoId}");
            initialResponse.EnsureSuccessStatusCode();

            var formData = new Dictionary<string, string>
            {
                { "Id", produtoId.ToString() },
                { "quantidade", quantidade.ToString() }
            };

            await _testsFixture.RealizarLoginWeb();

            var postRequest = new HttpRequestMessage(HttpMethod.Post, "/meu-carrinho")
            {
                Content = new FormUrlEncodedContent(formData)
            };

            // Act
            var postResponse = await _testsFixture.Client.SendAsync(postRequest);

            // Assert
            postResponse.EnsureSuccessStatusCode();

            var html = new HtmlParser()
                .ParseDocumentAsync(await postResponse.Content.ReadAsStringAsync())
                .Result
                .All;

            var formQuantidade = html?.FirstOrDefault(c => c.Id == "quantidade")?.GetAttribute("value")?.ApenasNumeros();
            var valorUnitario = html?.FirstOrDefault(c => c.Id == "valorUnitario")?.TextContent.Split(".")[0]?.ApenasNumeros();
            var valorTotal = html?.FirstOrDefault(c => c.Id == "valorTotal")?.TextContent.Split(".")[0]?.ApenasNumeros();

            Assert.Equal(valorTotal, valorUnitario * formQuantidade);
        }

        [Fact(DisplayName = "Atualizar item em pedido existente"), TestPriority(2)]
        [Trait("Categoria", "Integração Web - Pedido")]
        public async Task AtualizarItem_PedidoExistente_DeveRetornarComSucesso()
        {
            // Arrange
            var produtoId = new Guid("e83bde8d-7205-4c58-b491-51f153e1cf83");
            const int quantidade = 3;

            var formData = new Dictionary<string, string>
            {
                { "Id", produtoId.ToString() },
                { "quantidade", quantidade.ToString() }
            };

            await _testsFixture.RealizarLoginWeb();

            var postRequest = new HttpRequestMessage(HttpMethod.Post, "/atualizar-item")
            {
                Content = new FormUrlEncodedContent(formData)
            };

            // Act
            var postResponse = await _testsFixture.Client.SendAsync(postRequest);

            // Assert
            postResponse.EnsureSuccessStatusCode();
        }

        [Fact(DisplayName = "Remover item em pedido existente"), TestPriority(3)]
        [Trait("Categoria", "Integração Web - Pedido")]
        public async Task RemoverItem_PedidoExistente_DeveRetornarComSucesso()
        {
            // Arrange
            var produtoId = new Guid("e83bde8d-7205-4c58-b491-51f153e1cf83");

            var formData = new Dictionary<string, string>
            {
                { "Id", produtoId.ToString() }
            };

            await _testsFixture.RealizarLoginWeb();

            var postRequest = new HttpRequestMessage(HttpMethod.Post, "/remover-item")
            {
                Content = new FormUrlEncodedContent(formData)
            };

            // Act
            var deleteResponse = await _testsFixture.Client.SendAsync(postRequest);

            // Assert
            deleteResponse.EnsureSuccessStatusCode();
        }
    }
}
