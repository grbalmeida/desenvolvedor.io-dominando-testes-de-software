using MediatR;
using Moq;
using Moq.AutoMock;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace NerdStore.Catalogo.Domain.Tests
{
    public class EstoqueServiceTests
    {
        private readonly AutoMocker _mocker;
        private readonly EstoqueService _estoqueService;

        public EstoqueServiceTests()
        {
            _mocker = new AutoMocker();
            _estoqueService = _mocker.CreateInstance<EstoqueService>();
        }

        [Fact(DisplayName = "Deve debitar o estoque")]
        [Trait("Categoria", "Catálogo - Estoque Service")]
        public async Task EstoqueService_DebitarEstoque_DeveDebitarEstoque()
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

            produto.ReporEstoque(100);

            _mocker.GetMock<IProdutoRepository>()
                .Setup(r => r.ObterPorId(produto.Id))
                .Returns(Task.FromResult(produto));

            _mocker.GetMock<IProdutoRepository>()
                .Setup(r => r.UnitOfWork.Commit())
                .Returns(Task.FromResult(true));

            // Act
            var result = await _estoqueService.DebitarEstoque(produto.Id, 10);

            // Assert
            Assert.True(result);
            _mocker.GetMock<IProdutoRepository>().Verify(r => r.Atualizar(It.IsAny<Produto>()), Times.Once);
        }

        [Fact(DisplayName = "Deve retornar falso se produto não existe ao debitar estoque")]
        [Trait("Categoria", "Catálogo - Estoque Service")]
        public async Task EstoqueService_DebitarEstoque_NaoDeveDebitarDoEstoqueSeProdutoNaoExiste()
        {
            // Act
            var result = await _estoqueService.DebitarEstoque(Guid.NewGuid(), 10);

            // Assert
            Assert.False(result);
        }

        [Fact(DisplayName = "Deve publicar notificação se produto não possui estoque")]
        [Trait("Categoria", "Catálogo - Estoque Service")]
        public async Task EstoqueService_DebitarEstoque_DevePublicarNotificacaoSeNaoPossuiEstoque()
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

            produto.ReporEstoque(10);

            _mocker.GetMock<IProdutoRepository>()
                .Setup(r => r.ObterPorId(produto.Id))
                .Returns(Task.FromResult(produto));

            // Act
            var result = await _estoqueService.DebitarEstoque(produto.Id, 20);

            // Assert
            Assert.False(result);
            _mocker.GetMock<IMediator>().Verify(m => m.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Once);
        }

        [Fact(DisplayName = "Deve repor o estoque")]
        [Trait("Categoria", "Catálogo - Estoque Service")]
        public async Task EstoqueService_ReporEstoque_DeveReporEstoque()
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

            produto.ReporEstoque(100);

            _mocker.GetMock<IProdutoRepository>()
                .Setup(r => r.ObterPorId(produto.Id))
                .Returns(Task.FromResult(produto));

            _mocker.GetMock<IProdutoRepository>()
                .Setup(r => r.UnitOfWork.Commit())
                .Returns(Task.FromResult(true));

            // Act
            var result = await _estoqueService.ReporEstoque(produto.Id, 10);

            // Assert
            Assert.True(result);
            _mocker.GetMock<IProdutoRepository>().Verify(r => r.Atualizar(It.IsAny<Produto>()), Times.Once);
        }

        [Fact(DisplayName = "Deve retornar falso se produto não existe ao repor estoque")]
        [Trait("Categoria", "Catálogo - Estoque Service")]
        public async Task EstoqueService_ReporEstoque_NaoDeveReporDoEstoqueSeProdutoNaoExiste()
        {
            // Act
            var result = await _estoqueService.ReporEstoque(Guid.NewGuid(), 10);

            // Assert
            Assert.False(result);
        }
    }
}
