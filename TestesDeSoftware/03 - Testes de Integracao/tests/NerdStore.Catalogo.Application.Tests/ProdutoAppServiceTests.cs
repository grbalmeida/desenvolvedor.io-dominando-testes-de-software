using AutoMapper;
using Moq;
using Moq.AutoMock;
using NerdStore.Catalogo.Application.Services;
using NerdStore.Catalogo.Application.ViewModels;
using NerdStore.Catalogo.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace NerdStore.Catalogo.Application.Tests
{
    public class ProdutoAppServiceTests
    {
        private readonly AutoMocker _mocker;
        private readonly ProdutoAppService _produtoAppService;

        public ProdutoAppServiceTests()
        {
            _mocker = new AutoMocker();
            _produtoAppService = _mocker.CreateInstance<ProdutoAppService>();
        }

        [Fact(DisplayName = "Deve retornar lista de produtos por categoria")]
        [Trait("Categoria", "Catálogo - Produto App Service")]
        public async Task ProdutoAppService_ObterPorCategoria_DeveRetornarListaDeProdutosPorCategoria()
        {
            // Arrange
            var produto = new Produto("Produto", "Desc", true, 10, Guid.NewGuid(), DateTime.Now, "produto", new Dimensoes(1, 1, 1));

            var produtoViewModel = new ProdutoViewModel
            {
                Id = produto.Id,
                Nome = produto.Nome,
                Descricao = produto.Descricao,
                Ativo = produto.Ativo,
                CategoriaId = produto.CategoriaId,
                DataCadastro = produto.DataCadastro,
                Imagem = produto.Imagem,
                QuantidadeEstoque = produto.QuantidadeEstoque,
                Valor = produto.Valor,
                Altura = (int)produto.Dimensoes.Altura,
                Largura = (int)produto.Dimensoes.Largura,
                Profundidade = (int)produto.Dimensoes.Profundidade
            };
            
            var produtos = new List<Produto> { produto } as IEnumerable<Produto>;
            var produtosViewModel = new List<ProdutoViewModel> { produtoViewModel } as IEnumerable<ProdutoViewModel>;

            var codigoCategoria = 1;

            _mocker.GetMock<IProdutoRepository>()
                .Setup(r => r.ObterPorCategoria(codigoCategoria))
                .Returns(Task.FromResult(produtos));

            _mocker.GetMock<IMapper>()
                .Setup(m => m.Map<IEnumerable<Produto>, IEnumerable<ProdutoViewModel>>(It.IsAny<IEnumerable<Produto>>()))
                .Returns(produtosViewModel);

            // Act
            var result = await _produtoAppService.ObterPorCategoria(codigoCategoria) as List<ProdutoViewModel>;

            // Assert
            Assert.Equal(2, result.Count);
        }
    }
}
