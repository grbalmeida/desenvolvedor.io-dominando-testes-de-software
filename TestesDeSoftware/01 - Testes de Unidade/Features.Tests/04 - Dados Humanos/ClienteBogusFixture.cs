using Bogus;
using Bogus.DataSets;
using Features.Clientes;
using System;
using Xunit;

namespace Features.Tests
{
    [CollectionDefinition(nameof(ClienteBogusCollection))]
    public class ClienteBogusCollection : ICollectionFixture<ClienteBogusFixture>
    {

    }

    public class ClienteBogusFixture : IDisposable
    {
        public Cliente GerarClienteValido()
        {
            var genero = new Faker().PickRandom<Name.Gender>();

            var cliente = new Faker<Cliente>()
                .CustomInstantiator(f => new Cliente(
                        Guid.NewGuid(),
                        f.Name.FirstName(),
                        f.Name.LastName(),
                        f.Date.Past(80, DateTime.Now.AddYears(-18)),
                        "",
                        true,
                        DateTime.Now
                    ))
                .RuleFor(c => c.Email, (f, c) => f.Internet.Email(c.Nome.ToLower(), c.Sobrenome.ToLower()));

            return cliente;
        }

        public Cliente GerarClienteInvalido()
        {
            var cliente = new Cliente(
                Guid.NewGuid(),
                "",
                "",
                DateTime.Now,
                "edu2edu.com",
                true,
                DateTime.Now
            );

            return cliente;
        }

        public void Dispose()
        {

        }
    }
}
