using NerdStore.Vendas.Application.Commands;
using System;
using System.Linq;
using Xunit;

namespace NerdStore.Vendas.Application.Tests.Pedidos
{
    public class AplicarVoucherPedidoCommandTests
    {
        [Fact(DisplayName = "Aplicar Voucher Command Válido")]
        [Trait("Categoria", "Vendas - Pedido Commands")]
        public void AplicarVoucherPedidoCommand_CommandEstaValido_DevePassarNaValidacao()
        {
            // Arrange
            var voucherCommand = new AplicarVoucherPedidoCommand(Guid.NewGuid(), "PROMO-15-OFF");

            // Act
            var result = voucherCommand.EhValido();

            // Assert
            Assert.True(result);
        }

        [Fact(DisplayName = "Aplicar Voucher Command Inválido")]
        [Trait("Categoria", "Vendas - Pedido Commands")]
        public void AplicarVoucherPedidoCommand_CommandEstaInvalido_NaoDevePassarNaValidacao()
        {
            // Arrange
            var voucherCommand = new AplicarVoucherPedidoCommand(Guid.Empty, "");

            // Act
            var result = voucherCommand.EhValido();

            // Assert
            Assert.False(result);
            Assert.Equal(2, voucherCommand.ValidationResult.Errors.Count);
            Assert.Contains("Id do cliente inválido", voucherCommand.ValidationResult.Errors.Select(c => c.ErrorMessage));
            Assert.Contains("O código do voucher não pode ser vazio", voucherCommand.ValidationResult.Errors.Select(c => c.ErrorMessage));
        }
    }
}
