using NerdStore.Core.DomainObjects;
using System;
using Xunit;

namespace NerdStore.Catalogo.Domain.Tests
{
    public class ProdutoTests
    {
        [Fact(DisplayName = "Deve ativar o produto")]
        [Trait("Categoria", "Catálogo - Produto")]
        public void Produto_Ativar_DeveAtivarProduto()
        {
            // Arrange
            var produto = new Produto(
                "Produto Teste",
                "Descrição Teste",
                false,
                10,
                Guid.NewGuid(),
                DateTime.Now,
                "imagem-teste",
                new Dimensoes(1, 1, 1)
            );

            // Act
            produto.Ativar();

            // Assert
            Assert.True(produto.Ativo);
        }

        [Fact(DisplayName = "Deve desativar o produto")]
        [Trait("Categoria", "Catálogo - Produto")]
        public void Produto_Desativar_DeveDesativarProduto()
        {
            // Arrange
            var produto = new Produto(
                "Produto Teste",
                "Descrição Teste",
                true,
                10,
                Guid.NewGuid(),
                DateTime.Now,
                "imagem-teste",
                new Dimensoes(1, 1, 1)
            );

            // Act
            produto.Desativar();

            // Arrange
            Assert.False(produto.Ativo);
        }

        [Fact(DisplayName = "Deve alterar a categoria")]
        [Trait("Categoria", "Catálogo - Produto")]
        public void Produto_AlterarCategoria_DeveAlterarCategoria()
        {
            // Arrange
            var categoriaIdInicial = Guid.NewGuid();

            var produto = new Produto(
                "Produto Teste",
                "Descrição Teste",
                false,
                10,
                categoriaIdInicial,
                DateTime.Now,
                "imagem-teste",
                new Dimensoes(1, 1, 1)
            );

            var categoria = new Categoria("Eletrônicos", 1);

            // Act
            produto.AlterarCategoria(categoria);

            // Assert
            Assert.NotEqual(categoriaIdInicial, produto.CategoriaId);
            Assert.Equal(categoria.Id, produto.CategoriaId);
            Assert.Equal(categoria, produto.Categoria);
        }

        [Fact(DisplayName = "Deve alterar a descrição")]
        [Trait("Categoria", "Catálogo - Produto")]
        public void Produto_AlterarDescricao_DeveAlterarDescricao()
        {
            // Arrange
            var produto = new Produto(
                "Produto Teste",
                "Descrição Teste",
                false,
                10,
                Guid.NewGuid(),
                DateTime.Now,
                "imagem-teste",
                new Dimensoes(1, 1, 1)
            );

            // Act
            produto.AlterarDescricao("Eletrônicos");

            // Assert
            Assert.Equal("Eletrônicos", produto.Descricao);
        }

        [Fact(DisplayName = "Deve repor o estoque")]
        [Trait("Categoria", "Catálogo - Produto")]
        public void Produto_ReporEstoque_DeveReporEstoque()
        {
            // Arrange
            var produto = new Produto(
                "Produto Teste",
                "Descrição Teste",
                false,
                10,
                Guid.NewGuid(),
                DateTime.Now,
                "imagem-teste",
                new Dimensoes(1, 1, 1)
            );

            // Act
            produto.ReporEstoque(10);
            produto.ReporEstoque(20);
            produto.ReporEstoque(100);

            // Assert
            Assert.Equal(130, produto.QuantidadeEstoque);
        }

        [Fact(DisplayName = "Deve debitar o estoque")]
        [Trait("Categoria", "Catálogo - Produto")]
        public void Produto_DebitarEstoque_DeveDebitarEstoque()
        {
            // Arrange
            var produto = new Produto(
                "Produto Teste",
                "Descrição Teste",
                false,
                10,
                Guid.NewGuid(),
                DateTime.Now,
                "imagem-teste",
                new Dimensoes(1, 1, 1)
            );

            produto.ReporEstoque(100);

            // Act
            produto.DebitarEstoque(2);
            produto.DebitarEstoque(3);
            produto.DebitarEstoque(5);

            // Assert
            Assert.Equal(90, produto.QuantidadeEstoque);
        }

        [Fact(DisplayName = "Deve debitar o estoque se quantidade for negativa")]
        [Trait("Categoria", "Catálogo - Produto")]
        public void Produto_DebitarEstoque_DeveDebitarEstoqueSeQuantidadeForNegativa()
        {
            // Arrange
            var produto = new Produto(
                "Produto Teste",
                "Descrição Teste",
                false,
                10,
                Guid.NewGuid(),
                DateTime.Now,
                "imagem-teste",
                new Dimensoes(1, 1, 1)
            );

            produto.ReporEstoque(100);

            // Act
            produto.DebitarEstoque(-1);
            produto.DebitarEstoque(-10);
            produto.DebitarEstoque(-30);

            // Assert
            Assert.Equal(59, produto.QuantidadeEstoque);
        }

        [Fact(DisplayName = "Deve lançar exceção se quantidade em estoque for insuficiente")]
        [Trait("Categoria", "Catálogo - Produto")]
        public void Produto_DebitarEstoque_DeveLancarExcecaoSeQuantidadeEmEstoqueForInsuficiente()
        {
            // Arrange
            var produto = new Produto(
                "Produto Teste",
                "Descrição Teste",
                false,
                10,
                Guid.NewGuid(),
                DateTime.Now,
                "imagem-teste",
                new Dimensoes(1, 1, 1)
            );

            produto.ReporEstoque(14);

            // Act & Assert
            var domainException = Assert.Throws<DomainException>(() => produto.DebitarEstoque(15));
            Assert.Equal("Estoque insuficiente", domainException.Message);
        }

        [Fact(DisplayName = "Deve retornar verdadeiro se possui estoque")]
        [Trait("Categoria", "Catálogo - Produto")]
        public void Produto_PossuiEstoque_DeveRetornarVerdadeiro()
        {
            // Arrange
            var produto = new Produto(
                "Produto Teste",
                "Descrição Teste",
                false,
                10,
                Guid.NewGuid(),
                DateTime.Now,
                "imagem-teste",
                new Dimensoes(1, 1, 1)
            );

            produto.ReporEstoque(100);

            // Act
            var result = produto.PossuiEstoque(50);

            // Assert
            Assert.True(result);
        }

        [Fact(DisplayName = "Deve retornar falso se não possui estoque")]
        [Trait("Categoria", "Catálogo - Produto")]
        public void Produto_PossuiEstoque_DeveRetornarFalso()
        {
            // Arrange
            var produto = new Produto(
                "Produto Teste",
                "Descrição Teste",
                false,
                10,
                Guid.NewGuid(),
                DateTime.Now,
                "imagem-teste",
                new Dimensoes(1, 1, 1)
            );

            produto.ReporEstoque(10);

            // Act
            var result = produto.PossuiEstoque(15);

            // Assert
            Assert.False(result);
        }
    }
}
