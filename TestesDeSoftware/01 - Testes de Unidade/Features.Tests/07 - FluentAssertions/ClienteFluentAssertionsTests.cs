using FluentAssertions;
using Xunit;

namespace Features.Tests
{
    [Collection(nameof(ClienteBogusCollection))]
    public class ClienteFluentAssertionsTests
    {
        private readonly ClienteBogusFixture _clienteBogusFixture;

        public ClienteFluentAssertionsTests(ClienteBogusFixture clienteBogusFixture)
        {
            _clienteBogusFixture = clienteBogusFixture;
        }

        [Fact(DisplayName = "Novo Cliente Válido")]
        [Trait("Categoria", "Cliente Fluent Assertions Testes")]
        public void Cliente_NovoCliente_DeveEstarValido()
        {
            // Arrange
            var cliente = _clienteBogusFixture.GerarClienteValido();

            // Act
            var result = cliente.EhValido();

            // Assert
            result.Should().BeTrue();
            cliente.ValidationResult.Errors.Should().HaveCount(0);
        }

        [Fact(DisplayName = "Novo Cliente Inválido")]
        [Trait("Categoria", "Cliente Fluent Assertions Testes")]
        public void Cliente_NovoCliente_DeveEstarInvalido()
        {
            // Arrange
            var cliente = _clienteBogusFixture.GerarClienteInvalido();

            // Act
            var result = cliente.EhValido();

            // Assert
            result.Should().BeFalse();
            cliente.ValidationResult.Errors.Should().HaveCountGreaterOrEqualTo(1);
        }
    }
}
