using Features.Clientes;
using System;
using Xunit;

namespace Features.Tests
{
    public class ClienteTests
    {
        [Fact(DisplayName = "Novo Cliente Válido")]
        [Trait("Categoria", "Cliente Trait Tests")]
        public void Cliente_NovoCliente_DeveEstarValido()
        {
            // Arrange
            var cliente = new Cliente(
                Guid.NewGuid(),
                "Eduardo",
                "Pires",
                DateTime.Now.AddYears(-30),
                "edu@edu.com",
                true,
                DateTime.Now
            );

            // Act
            var result = cliente.EhValido();

            // Assert
            Assert.True(result);
            Assert.Equal(0, cliente.ValidationResult.Errors.Count);
        }

        [Fact(DisplayName = "Novo Cliente Inválido")]
        [Trait("Categoria", "Cliente Trait Tests")]
        public void Cliente_NovoCliente_DeveEstarInvalido()
        {
            // Arrange
            var cliente = new Cliente(
                Guid.NewGuid(),
                "",
                "",
                DateTime.Now,
                "edu2edu.com",
                true,
                DateTime.Now
            );

            // Act
            var result = cliente.EhValido();

            // Assert
            Assert.False(result);
            Assert.NotEqual(0, cliente.ValidationResult.Errors.Count);
        }

        [Fact(DisplayName = "Deve Inativar Cliente")]
        [Trait("Categoria", "Cliente Trait Tests")]
        public void Cliente_NovoCliente_DeveInativar()
        {
            // Arrange
            var cliente = new Cliente(
                Guid.NewGuid(),
                "Eduardo",
                "Pires",
                DateTime.Now.AddYears(-30),
                "edu@edu.com",
                true,
                DateTime.Now
            );

            // Act
            cliente.Inativar();

            // Assert
            Assert.False(cliente.Ativo);
        }

        [Fact(DisplayName = "Deve Retornar Nome Completo")]
        [Trait("Categoria", "Cliente Trait Tests")]
        public void Cliente_NovoCliente_DeveRetornarNomeCompleto()
        {
            // Arrange
            var cliente = new Cliente(
                Guid.NewGuid(),
                "Eduardo",
                "Pires",
                DateTime.Now.AddYears(-30),
                "edu@edu.com",
                true,
                DateTime.Now
            );

            // Act
            var nomeCompleto = cliente.NomeCompleto();

            // Assert
            Assert.Equal("Eduardo Pires", nomeCompleto);
        }

        [Fact(DisplayName = "Deve Retornar que o Cliente é Especial se Tempo de Cadastro Maior que 3 anos")]
        [Trait("Categoria", "Cliente Trait Tests")]
        public void Cliente_NovoCliente_EhEspecialSeTempoDeCadastroMaiorQue3Anos()
        {
            // Arrange
            var cliente = new Cliente(
                Guid.NewGuid(),
                "Eduardo",
                "Pires",
                DateTime.Now.AddYears(-30),
                "edu@edu.com",
                true,
                DateTime.Now.AddYears(-5)
            );

            // Act
            var ehEspecial = cliente.EhEspecial();

            // Assert
            Assert.True(ehEspecial);
        }

        [Fact(DisplayName = "Deve Retornar que Cliente não é Especial Se Tempo de Cadastro é menor que 3 anos")]
        [Trait("Categoria", "Cliente Trait Tests")]
        public void Cliente_NovoCliente_NaoEhEspecialSeTempoDeCadastroMenorQue3Anos()
        {
            // Arrange
            var cliente = new Cliente(
                Guid.NewGuid(),
                "Eduardo",
                "Pires",
                DateTime.Now.AddYears(-30),
                "edu@edu.com",
                true,
                DateTime.Now.AddYears(-2)
            );

            // Act
            var ehEspecial = cliente.EhEspecial();

            // Assert
            Assert.False(ehEspecial);
        }
    }
}
