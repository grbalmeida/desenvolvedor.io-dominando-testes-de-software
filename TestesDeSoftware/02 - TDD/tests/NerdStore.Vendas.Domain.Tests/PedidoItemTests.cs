using NerdStore.Core.DomainObjects;
using System;
using Xunit;

namespace NerdStore.Vendas.Domain.Tests
{
    public class PedidoItemTests
    {
        [Fact(DisplayName = "Novo Item Pedido com unidades abaixo do permitido")]
        [Trait("Categoria", "Vendas - Pedido Item")]
        public void AdicionarItemPedido_UnidadesItemAbaixoDoPermitido_DeveRetornarException()
        {
            // Arrange & Act & Assert
            var domainException = Assert.Throws<DomainException>(() => new PedidoItem(Guid.NewGuid(), "Produto Teste", Pedido.MIN_UNIDADES_ITEM - 1, 100));
            Assert.Equal($"Mínimo de {Pedido.MIN_UNIDADES_ITEM} unidades por produto", domainException.Message);
        }
    }
}
