using Moq.AutoMock;
using NerdStore.Vendas.Application.Queries;
using NerdStore.Vendas.Application.Queries.ViewModels;
using NerdStore.Vendas.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace NerdStore.Vendas.Application.Tests.Pedidos
{
    public class PedidoQueriesTests
    {
        private readonly Guid _clienteId;
        private readonly Guid _produtoId;
        private readonly AutoMocker _mocker;
        private readonly PedidoQueries _pedidoQueries;

        public PedidoQueriesTests()
        {
            _clienteId = Guid.NewGuid();
            _produtoId = Guid.NewGuid();
            _mocker = new AutoMocker();
            _pedidoQueries = _mocker.CreateInstance<PedidoQueries>();
        }

        [Fact(DisplayName = "Deve retornar carrinho de compras sem voucher")]
        [Trait("Categoria", "Vendas - Pedido Queries")]
        public async Task ObterCarrinhoCliente_CarrinhoExistenteSemVoucher_DeveRetornarCarrinho()
        {
            // Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(_clienteId);
            var pedidoItem1 = new PedidoItem(Guid.NewGuid(), "Produto 1", 1, 10);
            var pedidoItem2 = new PedidoItem(Guid.NewGuid(), "Produto 2", 2, 20);
            pedido.AdicionarItem(pedidoItem1);
            pedido.AdicionarItem(pedidoItem2);

            _mocker.GetMock<IPedidoRepository>()
                .Setup(r => r.ObterPedidoRascunhoPorClienteId(_clienteId))
                .Returns(Task.FromResult(pedido));

            // Act
            var carrinho = await _pedidoQueries.ObterCarrinhoCliente(_clienteId);

            // Assert
            Assert.Equal(pedido.PedidoItens.Count, carrinho.Itens.Count);
            Assert.Equal(pedido.ClienteId, carrinho.ClienteId);
            Assert.Equal(pedido.ValorTotal, carrinho.ValorTotal);
            Assert.Equal(pedido.Id, carrinho.PedidoId);
            Assert.Equal(pedido.Desconto, carrinho.ValorDesconto);
        }

        [Fact(DisplayName = "Deve retornar carrinho de compras com voucher")]
        [Trait("Categoria", "Vendas - Pedido Queries")]
        public async Task ObterCarrinhoCliente_CarrinhoExistenteComVoucher_DeveRetornarCarrinho()
        {
            // Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(_clienteId);
            var pedidoItem = new PedidoItem(_produtoId, "Produto Teste", 1, 10);
            pedido.AdicionarItem(pedidoItem);
            var voucher = new Voucher("PROMO-5-REAIS", null, 5, 1, TipoDescontoVoucher.Valor, DateTime.Now.AddDays(10), true, false);
            pedido.AplicarVoucher(voucher);

            _mocker.GetMock<IPedidoRepository>()
                .Setup(r => r.ObterPedidoRascunhoPorClienteId(_clienteId))
                .Returns(Task.FromResult(pedido));

            // Act
            var carrinho = await _pedidoQueries.ObterCarrinhoCliente(_clienteId);

            // Assert
            Assert.Equal(pedido.Voucher.Codigo, carrinho.VoucherCodigo);
            Assert.Equal(pedido.PedidoItens.Count, carrinho.Itens.Count);
            Assert.Equal(pedido.ClienteId, carrinho.ClienteId);
            Assert.Equal(pedido.ValorTotal, carrinho.ValorTotal);
            Assert.Equal(pedido.Id, carrinho.PedidoId);
            Assert.Equal(pedido.Desconto, carrinho.ValorDesconto);
        }

        [Fact(DisplayName = "Deve retornar lista de pedidos do cliente")]
        [Trait("Categoria", "Vendas - Pedido Queries")]
        public async Task ObterPedidosCliente_ClienteComPedidos_DeveRetornarPedidos()
        {
            // Arrange
            var pedido1 = Pedido.PedidoFactory.NovoPedidoRascunho(_clienteId);
            var pedido2 = Pedido.PedidoFactory.NovoPedidoRascunho(_clienteId);
            pedido1.GetType().GetProperty("PedidoStatus").SetValue(pedido1, PedidoStatus.Pago);
            pedido2.GetType().GetProperty("PedidoStatus").SetValue(pedido2, PedidoStatus.Cancelado);

            var pedidos = new List<Pedido>
            {
                pedido1,
                pedido2
            } as IEnumerable<Pedido>;

            _mocker.GetMock<IPedidoRepository>()
                .Setup(r => r.ObterListaPorClienteId(_clienteId))
                .Returns(Task.FromResult(pedidos));

            // Act
            var pedidosViewModel = await _pedidoQueries.ObterPedidosCliente(_clienteId) as List<PedidoViewModel>;

            // Assert
            Assert.NotNull(pedidosViewModel);
            Assert.Equal(2, pedidosViewModel.Count);
        }

        [Fact(DisplayName = "Deve retornar nulo se não tiver pedidos pagos ou cancelados")]
        [Trait("Categoria", "Vendas - Pedido Queries")]
        public async Task ObterPedidosCliente_ClienteSemPedidosPagosOuCancelados_DeveRetornarNulo()
        {
            // Arrange
            var pedido1 = Pedido.PedidoFactory.NovoPedidoRascunho(_clienteId);
            var pedido2 = Pedido.PedidoFactory.NovoPedidoRascunho(_clienteId);
            var pedido3 = Pedido.PedidoFactory.NovoPedidoRascunho(_clienteId);
            pedido2.GetType().GetProperty("PedidoStatus").SetValue(pedido1, PedidoStatus.Entregue);
            pedido3.GetType().GetProperty("PedidoStatus").SetValue(pedido1, PedidoStatus.Iniciado);

            var pedidos = new List<Pedido>
            {
                pedido1,
                pedido2,
                pedido3
            } as IEnumerable<Pedido>;

            _mocker.GetMock<IPedidoRepository>()
                .Setup(r => r.ObterListaPorClienteId(_clienteId))
                .Returns(Task.FromResult(pedidos));

            // Act
            var pedidosViewModel = await _pedidoQueries.ObterPedidosCliente(_clienteId);

            // Assert
            Assert.Null(pedidosViewModel);
        }
    }
}
