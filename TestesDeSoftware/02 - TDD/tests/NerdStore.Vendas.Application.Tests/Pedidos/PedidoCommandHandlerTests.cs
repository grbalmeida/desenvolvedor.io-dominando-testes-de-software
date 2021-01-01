﻿using MediatR;
using Moq;
using Moq.AutoMock;
using NerdStore.Core.DomainObjects;
using NerdStore.Vendas.Application.Commands;
using NerdStore.Vendas.Domain;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace NerdStore.Vendas.Application.Tests.Pedidos
{
    public class PedidoCommandHandlerTests
    {
        private readonly Guid _clienteId;
        private readonly Guid _produtoId;
        private readonly Pedido _pedido;
        private readonly AutoMocker _mocker;
        private readonly PedidoCommandHandler _pedidoHandler;

        public PedidoCommandHandlerTests()
        {
            _mocker = new AutoMocker();
            _pedidoHandler = _mocker.CreateInstance<PedidoCommandHandler>();

            _clienteId = Guid.NewGuid();
            _produtoId = Guid.NewGuid();

            _pedido = Pedido.PedidoFactory.NovoPedidoRascunho(_clienteId);
        }

        [Fact(DisplayName = "Adicionar Item Novo Pedido com Sucesso")]
        [Trait("Categoria", "Vendas - Pedido Command Handler")]
        public async Task AdicionarItem_NovoPedido_DeveExecutarComSucesso()
        {
            // Arrange
            var pedidoCommand = new AdicionarItemPedidoCommand(
                _clienteId,
                _produtoId,
                "Produto Teste",
                2,
                100
            );

            _mocker.GetMock<IPedidoRepository>().Setup(r => r.UnitOfWork.Commit()).Returns(Task.FromResult(true));

            // Act
            var result = await _pedidoHandler.Handle(pedidoCommand, CancellationToken.None);

            // Assert
            Assert.True(result);
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.Adicionar(It.IsAny<Pedido>()), Times.Once);
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Once);
        }
    
        [Fact(DisplayName = "Adicionar Novo Item Pedido Rascunho com Sucesso")]
        [Trait("Categoria", "Vendas - Pedido Command Handler")]
        public async Task AdicionarItem_NovoItemAoPedidoRascunho_DeveExecutarComSucesso()
        {
            // Arrange
            var pedidoItemExistente = new PedidoItem(_produtoId, "Produto Xpto", 2, 100);
            _pedido.AdicionarItem(pedidoItemExistente);

            var pedidoCommand = new AdicionarItemPedidoCommand(
                _clienteId,
                Guid.NewGuid(),
                "Produto Teste",
                2,
                100
            );

            _mocker.GetMock<IPedidoRepository>()
                .Setup(r => r.ObterPedidoRascunhoPorClienteId(_clienteId))
                .Returns(Task.FromResult(_pedido));

            _mocker.GetMock<IPedidoRepository>()
                .Setup(r => r.UnitOfWork.Commit())
                .Returns(Task.FromResult(true));

            // Act
            var result = await _pedidoHandler.Handle(pedidoCommand, CancellationToken.None);

            // Assert
            Assert.True(result);
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.AdicionarItem(It.IsAny<PedidoItem>()), Times.Once);
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.Atualizar(It.IsAny<Pedido>()), Times.Once);
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Once);
        }
    
        [Fact(DisplayName = "Adicionar Item Existente ao Pedido Rascunho com Sucesso")]
        [Trait("Categoria", "Vendas - Pedido Command Handler")]
        public async Task AdicionarItem_ItemExistenteAoPedidoRascunho_DeveExecutarComSucesso()
        {
            // Arrange
            var pedidoItemExistente = new PedidoItem(_produtoId, "Produto Xpto", 2, 100);
            _pedido.AdicionarItem(pedidoItemExistente);

            var pedidoCommand = new AdicionarItemPedidoCommand(
                _clienteId,
                _produtoId,
                "Produto Xpto",
                2,
                100
            );

            _mocker.GetMock<IPedidoRepository>()
                .Setup(r => r.ObterPedidoRascunhoPorClienteId(_clienteId))
                .Returns(Task.FromResult(_pedido));

            _mocker.GetMock<IPedidoRepository>()
                .Setup(r => r.UnitOfWork.Commit())
                .Returns(Task.FromResult(true));

            // Act
            var result = await _pedidoHandler.Handle(pedidoCommand, CancellationToken.None);

            // Assert
            Assert.True(result);
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.AtualizarItem(It.IsAny<PedidoItem>()), Times.Once);
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.Atualizar(It.IsAny<Pedido>()), Times.Once);
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Once);
        }
    
        [Fact(DisplayName = "Adicionar Item Command Inválido")]
        [Trait("Categoria", "Vendas - Pedido Command Handler")]
        public async Task AdicionarItem_CommandInvalido_DeveRetornarFalsoELancarEventosDeNotificacao()
        {
            // Arrange
            var pedidoCommand = new AdicionarItemPedidoCommand(Guid.Empty, Guid.Empty, "", 0, 0);

            // Act
            var result = await _pedidoHandler.Handle(pedidoCommand, CancellationToken.None);

            // Assert
            Assert.False(result);
            _mocker.GetMock<IMediator>().Verify(m => m.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Exactly(5));
        }

        [Fact(DisplayName = "Aplicar Voucher com Sucesso")]
        [Trait("Categoria", "Vendas - Pedido Command Handler")]
        public async Task AplicarVoucher_VoucherValido_DeveAplicarComSucesso()
        {
            // Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(_clienteId);
            var pedidoItem1 = new PedidoItem(Guid.NewGuid(), "Produto 1", 10, 10);
            var pedidoItem2 = new PedidoItem(Guid.NewGuid(), "Produto 2", 5, 20);
            pedido.AdicionarItem(pedidoItem1);
            pedido.AdicionarItem(pedidoItem2);

            var codigoVoucher = "PROMO-15-REAIS";
            var voucher = new Voucher(codigoVoucher, null, 15, 10, TipoDescontoVoucher.Valor, DateTime.Now.AddDays(10), true, false);

            var voucherCommand = new AplicarVoucherPedidoCommand(_clienteId, codigoVoucher);

            _mocker.GetMock<IPedidoRepository>()
                .Setup(r => r.ObterPedidoRascunhoPorClienteId(_clienteId))
                .Returns(Task.FromResult(pedido));

            _mocker.GetMock<IPedidoRepository>()
                .Setup(r => r.ObterVoucherPorCodigo(codigoVoucher))
                .Returns(Task.FromResult(voucher));

            _mocker.GetMock<IPedidoRepository>().Setup(r => r.UnitOfWork.Commit()).Returns(Task.FromResult(true));

            // Act
            var result = await _pedidoHandler.Handle(voucherCommand, CancellationToken.None);

            // Assert
            Assert.True(result);
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.Atualizar(It.IsAny<Pedido>()), Times.Once);
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Once);
        }

        [Fact(DisplayName = "Aplicar Voucher Command Inválido")]
        [Trait("Categoria", "Vendas - Pedido Command Handler")]
        public async Task AplicarVoucher_CommandInvalido_DeveRetornarFalsoELancarEventosDeNotificacao()
        {
            // Arrange
            var voucherCommand = new AplicarVoucherPedidoCommand(Guid.Empty, "");

            // Act
            var result = await _pedidoHandler.Handle(voucherCommand, CancellationToken.None);

            // Assert
            Assert.False(result);
            _mocker.GetMock<IMediator>().Verify(m => m.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Exactly(2));
        }

        [Fact(DisplayName = "Aplicar Voucher Pedido Inexistente")]
        [Trait("Categoria", "Vendas - Pedido Command Handler")]
        public async Task AplicarVoucher_PedidoInexistente_DeveRetornarFalsoELancarEventoDeNotificacao()
        {
            // Arrange
            var voucherCommand = new AplicarVoucherPedidoCommand(_clienteId, "PROMO-15-REAIS");

            // Act
            var result = await _pedidoHandler.Handle(voucherCommand, CancellationToken.None);

            // Assert
            Assert.False(result);
            _mocker.GetMock<IMediator>().Verify(m => m.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Once);
        }

        [Fact(DisplayName = "Aplicar Voucher Voucher Inexistente")]
        [Trait("Categoria", "Vendas - Pedido Command Handler")]
        public async Task AplicarVoucher_VoucherInexistente_DeveRetornarFalsoELancarEventoDeNotificacao()
        {
            // Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(_clienteId);
            var pedidoItem = new PedidoItem(_produtoId, "Produto Teste", 1, 10);
            pedido.AdicionarItem(pedidoItem);

            var voucherCommand = new AplicarVoucherPedidoCommand(_clienteId, "PROMO-15-OFF");

            _mocker.GetMock<IPedidoRepository>()
                .Setup(r => r.ObterPedidoRascunhoPorClienteId(_clienteId))
                .Returns(Task.FromResult(pedido));

            // Act
            var result = await _pedidoHandler.Handle(voucherCommand, CancellationToken.None);

            // Assert
            Assert.False(result);
            _mocker.GetMock<IMediator>().Verify(m => m.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Once);
        }

        [Fact(DisplayName = "Aplicar Voucher Inválido")]
        [Trait("Categoria", "Vendas - Pedido Command Handler")]
        public async Task AplicarVoucher_VoucherInvalido_DeveRetornarFalsoELancarEventosDeNotificacao()
        {
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(_clienteId);
            var pedidoItem = new PedidoItem(_produtoId, "Produto Teste", 10, 20);
            pedido.AdicionarItem(pedidoItem);

            var codigoVoucher = "PROMO-15-REAIS";
            var voucher = new Voucher(codigoVoucher, null, 15, 0, TipoDescontoVoucher.Valor, DateTime.Now.AddDays(-1), false, true);

            var voucherCommand = new AplicarVoucherPedidoCommand(_clienteId, codigoVoucher);

            _mocker.GetMock<IPedidoRepository>()
                .Setup(r => r.ObterPedidoRascunhoPorClienteId(_clienteId))
                .Returns(Task.FromResult(pedido));

            _mocker.GetMock<IPedidoRepository>()
                .Setup(r => r.ObterVoucherPorCodigo(codigoVoucher))
                .Returns(Task.FromResult(voucher));

            // Act
            var result = await _pedidoHandler.Handle(voucherCommand, CancellationToken.None);

            // Assert
            Assert.False(result);
            _mocker.GetMock<IMediator>().Verify(m => m.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Exactly(4));
        }
    }
}
