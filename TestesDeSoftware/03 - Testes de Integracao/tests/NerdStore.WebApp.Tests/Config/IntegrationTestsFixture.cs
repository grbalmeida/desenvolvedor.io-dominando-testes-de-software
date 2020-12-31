using Bogus;
using Microsoft.AspNetCore.Mvc.Testing;
using NerdStore.WebApp.MVC;
using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using Xunit;

namespace NerdStore.WebApp.Tests.Config
{
    [CollectionDefinition(nameof(IntegrationWebTestsFixtureCollection))]
    public class IntegrationWebTestsFixtureCollection : ICollectionFixture<IntegrationTestsFixture<StartupWebTests>>
    {

    }

    [CollectionDefinition(nameof(IntegrationApiTestsFixtureCollection))]
    public class IntegrationApiTestsFixtureCollection : ICollectionFixture<IntegrationTestsFixture<StartupApiTests>>
    {

    }

    public class IntegrationTestsFixture<TStartup> : IDisposable where TStartup : class
    {
        public string AntiForgeryFieldName = "__RequestVerificationToken";

        public string UsuarioEmail;
        public string UsuarioSenha;

        public readonly LojaAppFactory<TStartup> Factory;
        public HttpClient Client;

        public IntegrationTestsFixture()
        {
            var clientOptions = new WebApplicationFactoryClientOptions
            {

            };

            Factory = new LojaAppFactory<TStartup>();
            Client = Factory.CreateClient(clientOptions);
        }

        public void GerarUserSenha()
        {
            var faker = new Faker();
            UsuarioEmail = faker.Internet.Email().ToLower();
            UsuarioSenha = faker.Internet.Password(8, false, "", "@1Ab_");
        }

        public string ObterAntiForgeryToken(string htmlBody)
        {
            var requestVerificationTokenMatch =
                Regex.Match(htmlBody, $@"\<input name=""{AntiForgeryFieldName}"" type=""hidden"" value=""([^""]+)"" \/\>");

            if (requestVerificationTokenMatch.Success)
            {
                return requestVerificationTokenMatch.Groups[1].Captures[0].Value;
            }

            throw new ArgumentException($"Anti forgery token '{AntiForgeryFieldName}' não encontrado no HTML", nameof(htmlBody));
        }

        public void Dispose()
        {
            Client?.Dispose();
            Factory?.Dispose();
        }
    }
}
