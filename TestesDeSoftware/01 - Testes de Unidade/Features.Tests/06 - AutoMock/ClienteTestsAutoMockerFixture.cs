using Bogus;
using Bogus.DataSets;
using Features.Clientes;
using Moq.AutoMock;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Features.Tests
{
    [CollectionDefinition(nameof(ClienteAutoMockerCollection))]
    public class ClienteAutoMockerCollection : ICollectionFixture<ClienteTestsAutoMockerFixture>
    {

    }

    public class ClienteTestsAutoMockerFixture : IDisposable
    {
        public ClienteService ClienteService;
        public AutoMocker Mocker;

        public Cliente GerarClienteValido()
        {
            return GerarClientes(1, true).FirstOrDefault();
        }

        public IEnumerable<Cliente> ObterClientesVariados()
        {
            var clientes = new List<Cliente>();

            clientes.AddRange(GerarClientes(50, true).ToList());
            clientes.AddRange(GerarClientes(50, false).ToList());

            return clientes;
        }

        public IEnumerable<Cliente> GerarClientes(int quantidade, bool ativo)
        {
            var genero = new Faker().PickRandom<Name.Gender>();

            var clientes = new Faker<Cliente>()
                .CustomInstantiator(f => new Cliente(
                        Guid.NewGuid(),
                        f.Name.FirstName(),
                        f.Name.LastName(),
                        f.Date.Past(80, DateTime.Now.AddYears(-18)),
                        "",
                        ativo,
                        DateTime.Now
                    ))
                .RuleFor(c => c.Email, (f, c) => f.Internet.Email(c.Nome.ToLower(), c.Sobrenome.ToLower()));

            return clientes.Generate(quantidade);
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

        public ClienteService ObterClienteService()
        {
            Mocker = new AutoMocker();
            ClienteService = Mocker.CreateInstance<ClienteService>();

            return ClienteService;
        }

        public void Dispose()
        {

        }
    }
}
