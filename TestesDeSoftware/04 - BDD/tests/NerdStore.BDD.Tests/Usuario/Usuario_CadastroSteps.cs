using System;
using TechTalk.SpecFlow;

namespace NerdStore.BDD.Tests.Usuario
{
    [Binding]
    public class Usuario_CadastroSteps
    {        
        [When(@"Ele clicar em registrar")]
        public void QuandoEleClicarEmRegistrar()
        {
            // Arrange

            // Act

            // Assert
        }

        [When(@"Preencher os dados do formulário")]
        public void QuandoPreencherOsDadosDoFormulario(Table table)
        {
            // Arrange

            // Act

            // Assert
        }

        [When(@"Clicar no botão registrar")]
        public void QuandoClicarNoBotaoRegistrar()
        {
            // Arrange

            // Act

            // Assert
        }

        [When(@"Preencher os dados do formulário com uma senha sem maiúsculas")]
        public void QuandoPreencherOsDadosDoFormularioComUmaSenhaSemMaiusculas(Table table)
        {
            // Arrange

            // Act

            // Assert
        }

        [When(@"Preencher os dados do formulário com uma senha sem caractere especial")]
        public void QuandoPreencherOsDadosDoFormularioComUmaSenhaSemCaractereEspecial(Table table)
        {
            // Arrange

            // Act

            // Assert
        }

        [Then(@"Ele receberá uma mensagem de erro que a senha precisa conter uma letra maiúscula")]
        public void EntaoEleReceberaUmaMensagemDeErroQueASenhaPrecisaConterUmaLetraMaiuscula()
        {
            // Arrange

            // Act

            // Assert
        }

        [Then(@"Ele receberá uma mensagem de erro que a senha precisa conter um caractere especial")]
        public void EntaoEleReceberaUmaMensagemDeErroQueASenhaPrecisaConterUmCaractereEspecial()
        {
            // Arrange

            // Act

            // Assert
        }
    }
}
