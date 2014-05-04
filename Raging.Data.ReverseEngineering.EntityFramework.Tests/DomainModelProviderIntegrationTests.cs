using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using Raging.Data.ReverseEngineering.Configuration;
using Raging.Data.ReverseEngineering.EntityFramework.Adapters;
using Raging.Data.ReverseEngineering.EntityFramework.Providers;
using Raging.Data.ReverseEngineering.Infrastructure.Metadata;
using Raging.Data.ReverseEngineering.Infrastructure.Pluralization;
using Raging.Data.Schema.SqlServer;

namespace Raging.Data.ReverseEngineering.EntityFramework.Tests
{
    [TestFixture]
    internal class DomainModelProviderIntegrationTests
    {
        [Test]
        public void WhenAllIsFine()
        {
            //Arrange;
            var connString = "Data Source=.;Initial Catalog=SagaStore;Integrated Security=True;MultipleActiveResultSets=True";
            var schemaReader = new SqlServerSchemaReader(connString);
            var modelInfoAdapterFactory = new EntityInfoAdapterFactory();
            var propertyInfoAdapterFactory = new PropertyInfoAdapterFactory();
            var navigationPropertyInfoAdapterFactory = new NavigationPropertyInfoAdapterFactory();
            var identifierGenerationServiceFactory = new IdentifierGenerationServiceFactory();
            var pluralizationService = new EnglishPluralizationService();
            var reverseEngineeringConfiguration = new ReverseEngineeringConfiguration();

            reverseEngineeringConfiguration.Pluralize = true;
            reverseEngineeringConfiguration.UsePascalCase = true;

            //reverseEngineeringConfiguration.TableWhiteListFilter = "dbo.Saga;";

            //reverseEngineeringConfiguration.TableWhiteListFilter = "dbo.Person;dbo.SagaMessage";
            //reverseEngineeringConfiguration.TableBlackListFilter = "dbo.RelatedSaga;dbo.Person";

            var provider = new DomainModelProvider(
                schemaReader,
                modelInfoAdapterFactory,
                propertyInfoAdapterFactory,
                navigationPropertyInfoAdapterFactory,
                identifierGenerationServiceFactory,
                pluralizationService,
                reverseEngineeringConfiguration);

            //Act
            var result = provider.GetInformation().ToList();

            //Assert
            result.Should().HaveCount(4);
        }
    }
}