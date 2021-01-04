using Features.Tests;
using NerdStore.WebApp.MVC;
using NerdStore.WebApp.MVC.Models;
using NerdStore.WebApp.Tests.Config;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace NerdStore.WebApp.Tests
{
    [TestCaseOrderer("Features.Tests.PriorityOrder", "Features.Tests")]
    [Collection(nameof(IntegrationApiTestsFixtureCollection))]
    public class PedidoApiTests
    {
        private readonly IntegrationTestsFixture<StartupApiTests> _testsFixture;

        public PedidoApiTests(IntegrationTestsFixture<StartupApiTests> testsFixture)
        {
            _testsFixture = testsFixture;
        }

        [Fact(DisplayName = "Adicionar item em novo pedido"), TestPriority(5)]
        [Trait("Categoria", "Integração API - Pedido")]
        public async Task AdicionarItem_NovoPedido_DeveRetornarComSucesso()
        {
            // Arrange
            var itemInfo = new ItemViewModel
            {
                Id = new Guid("e83bde8d-7205-4c58-b491-51f153e1cf82"),
                Quantidade = 2
            };

            await _testsFixture.RealizarLoginApi();
            _testsFixture.Client.AtribuirToken(_testsFixture.UsuarioToken);

            // Act
            var postResponse = await _testsFixture.Client.PostAsJsonAsync("api/carrinho", itemInfo);

            // Assert
            postResponse.EnsureSuccessStatusCode();
        }

        [Fact(DisplayName = "Deve retornar o carrinho"), TestPriority(6)]
        [Trait("Categoria", "Integração API - Pedido")]
        public async Task ObterCarrinhoCliente_PedidoExistente_DeveRetornarComSucesso()
        {
            // Arrange
            await _testsFixture.RealizarLoginApi();
            _testsFixture.Client.AtribuirToken(_testsFixture.UsuarioToken);

            // Act
            var response = await _testsFixture.Client.GetAsync("api/carrinho");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact(DisplayName = "Atualizar item em pedido existente"), TestPriority(7)]
        [Trait("Categoria", "Integração API - Pedido")]
        public async Task AtualizarItem_PedidoExistente_DeveRetornarComSucesso()
        {
            // Arrange
            var produtoId = new Guid("e83bde8d-7205-4c58-b491-51f153e1cf82");
            await _testsFixture.RealizarLoginApi();
            _testsFixture.Client.AtribuirToken(_testsFixture.UsuarioToken);

            var itemInfo = new ItemViewModel
            {
                Id = produtoId,
                Quantidade = 3
            };

            // Act
            var putResponse = await _testsFixture.Client.PutAsJsonAsync($"api/carrinho/{produtoId}", itemInfo);

            // Assert
            putResponse.EnsureSuccessStatusCode();
        }

        [Fact(DisplayName = "Remover item em pedido existente"), TestPriority(8)]
        [Trait("Categoria", "Integração API - Pedido")]
        public async Task RemoverItem_PedidoExistente_DeveRetornarComSucesso()
        {
            // Arrange
            var produtoId = new Guid("e83bde8d-7205-4c58-b491-51f153e1cf82");
            await _testsFixture.RealizarLoginApi();
            _testsFixture.Client.AtribuirToken(_testsFixture.UsuarioToken);

            // Act
            var deleteResponse = await _testsFixture.Client.DeleteAsync($"api/carrinho/{produtoId}");

            // Assert
            deleteResponse.EnsureSuccessStatusCode();
        }
    }
}
