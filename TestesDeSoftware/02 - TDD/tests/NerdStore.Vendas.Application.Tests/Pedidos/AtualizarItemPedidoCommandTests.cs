using NerdStore.Vendas.Application.Commands;
using System;
using System.Linq;
using Xunit;

namespace NerdStore.Vendas.Application.Tests.Pedidos
{
    public class AtualizarItemPedidoCommandTests
    {
        [Fact(DisplayName = "Atualizar Item Command Válido")]
        [Trait("Categoria", "Vendas - Pedido Commands")]
        public void AtualizarItemPedidoCommand_CommandEstaValido_DevePassarNaValidacao()
        {
            // Arrange
            var atualizarCommand = new AtualizarItemPedidoCommand(Guid.NewGuid(), Guid.NewGuid(), 1);

            // Act
            var result = atualizarCommand.EhValido();

            // Assert
            Assert.True(result);
        }

        [Fact(DisplayName = "Atualizar Item Command Inválido")]
        [Trait("Categoria", "Vendas - Pedido Commands")]
        public void AtualizarItemPedidoCommand_CommandEstaInvalido_NaoDevePassarNaValidacao()
        {
            // Arrange
            var atualizarCommand = new AtualizarItemPedidoCommand(Guid.Empty, Guid.Empty, 15);

            // Act
            var result = atualizarCommand.EhValido();

            // Assert
            Assert.False(result);
            Assert.Equal(2, atualizarCommand.ValidationResult.Errors.Count);
            Assert.Contains("Id do cliente inválido", atualizarCommand.ValidationResult.Errors.Select(c => c.ErrorMessage));
            Assert.Contains("Id do produto inválido", atualizarCommand.ValidationResult.Errors.Select(c => c.ErrorMessage));
        }

        [Fact(DisplayName = "Atualizar Item Command unidades acima do permitido")]
        [Trait("Categoria", "Vendas - Pedido Commands")]
        public void AtualizarItemPedidoCommand_QuantidadeUnidadesSuperiorAoPermitido_NaoDevePassarNaValidacao()
        {
            // Arrange
            var atualizarCommand = new AtualizarItemPedidoCommand(Guid.NewGuid(), Guid.NewGuid(), 16);

            // Act
            var result = atualizarCommand.EhValido();

            // Assert
            Assert.False(result);
            Assert.Equal(1, atualizarCommand.ValidationResult.Errors.Count);
            Assert.Contains("A quantidade máxima de um item é 15", atualizarCommand.ValidationResult.Errors.Select(c => c.ErrorMessage));
        }

        [Fact(DisplayName = "Atualizar Item Command unidades abaixo do permitido")]
        [Trait("Categoria", "Vendas - Pedido Commands")]
        public void AtualizarItemPedidoCommand_QuantidadeUnidadesInferiorAoPermitido_NaoDevePassarNaValidacao()
        {
            // Arrange
            var atualizarCommand = new AtualizarItemPedidoCommand(Guid.NewGuid(), Guid.NewGuid(), 0);

            // Act
            var result = atualizarCommand.EhValido();

            // Assert
            Assert.False(result);
            Assert.Equal(1, atualizarCommand.ValidationResult.Errors.Count);
            Assert.Contains("A quantidade miníma de um item é 1", atualizarCommand.ValidationResult.Errors.Select(c => c.ErrorMessage));
        }
    }
}
