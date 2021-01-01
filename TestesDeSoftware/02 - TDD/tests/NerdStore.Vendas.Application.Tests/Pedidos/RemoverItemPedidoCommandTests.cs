using NerdStore.Vendas.Application.Commands;
using System;
using System.Linq;
using Xunit;

namespace NerdStore.Vendas.Application.Tests.Pedidos
{
    public class RemoverItemPedidoCommandTests
    {
        [Fact(DisplayName = "Remover Item Command Válido")]
        [Trait("Categoria", "Vendas - Pedido Commands")]
        public void RemoverItemPedidoCommand_CommandEstaValido_DevePassarNaValidacao()
        {
            // Arrange
            var removerCommand = new RemoverItemPedidoCommand(Guid.NewGuid(), Guid.NewGuid());

            // Act
            var result = removerCommand.EhValido();

            // Assert
            Assert.True(result);
        }

        [Fact(DisplayName = "Remover Item Command Inválido")]
        [Trait("Categoria", "Vendas - Pedido Commands")]
        public void RemoverItemPedidoCommand_CommandEstaInvalido_NaoDevePassarNaValidacao()
        {
            // Arrange
            var removerCommand = new RemoverItemPedidoCommand(Guid.Empty, Guid.Empty);

            // Act
            var result = removerCommand.EhValido();

            // Assert
            Assert.False(result);
            Assert.Equal(2, removerCommand.ValidationResult.Errors.Count);
            Assert.Contains("Id do cliente inválido", removerCommand.ValidationResult.Errors.Select(c => c.ErrorMessage));
            Assert.Contains("Id do produto inválido", removerCommand.ValidationResult.Errors.Select(c => c.ErrorMessage));
        }
    }
}
