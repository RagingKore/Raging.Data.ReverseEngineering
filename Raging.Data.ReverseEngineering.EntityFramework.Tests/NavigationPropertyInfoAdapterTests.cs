using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using Raging.Data.ReverseEngineering.Configuration;
using Raging.Data.ReverseEngineering.EntityFramework.Adapters;
using Raging.Data.ReverseEngineering.Infrastructure;
using Raging.Data.ReverseEngineering.Infrastructure.Metadata;
using Raging.Data.Schema;
using Raging.Data.Schema.SqlServer;
using Raging.Toolbox.Testing.NUnit.FakeItEasy;

namespace Raging.Data.ReverseEngineering.EntityFramework.Tests
{
    [TestFixture]
    class NavigationPropertyInfoAdapterTests
    {
        public class GetProperty : NavigationPropertyInfoAdapterTestsBase
        {
            [Test]
            public void WhenTableIsPrimaryKeyTable_ShouldReturnReverseNavigationProperty()
            {
                //Arrange
                ISchemaTable table = new SqlSchemaTable(
                    schema: "dbo",
                    name  : "Saga",
                    isView: false);

                ISchemaForeignKey foreignKey = new SqlSchemaForeignKey(
                    foreignKeySchema   : "dbo",
                    foreignKeyTableName: "SagaMessage",
                    foreignKeyColumn   : "SagaId",
                    primaryKeySchema   : "dbo",
                    primaryKeyTableName: "Saga",
                    primaryKeyColumn   : "SagaId",
                    constraintName     : "FK_SagaMessage_Saga_SagaId");

                AutoFaker.Provide(table);
                AutoFaker.Provide(foreignKey);
                AutoFaker.Provide<INavigationPropertyInfoAdapter, NavigationPropertyInfoAdapter>();

                A.CallTo(() => AutoFaker.Resolve<IReverseEngineeringConfiguration>().Pluralize).Returns(true);

                //Act
                var result = AutoFaker.Resolve<INavigationPropertyInfoAdapter>().GetPropertyText();

                //Assert
                result.ShouldBeEquivalentTo(
                    "public virtual ICollection<SagaMessage> SagaMessages { get; set; } // SagaMessage.FK_SagaMessage_Saga_SagaId");
            }

            [Test]
            public void WhenTableIsForeignKeyTable_ShouldReturnNavigationProperty()
            {
                //Arrange
                ISchemaTable table = new SqlSchemaTable(
                    schema: "dbo",
                    name  : "SagaMessage",
                    isView: false);

                ISchemaForeignKey foreignKey = new SqlSchemaForeignKey(
                    foreignKeySchema   : "dbo",
                    foreignKeyTableName: "SagaMessage",
                    foreignKeyColumn   : "SagaId",
                    primaryKeySchema   : "dbo",
                    primaryKeyTableName: "Saga",
                    primaryKeyColumn   : "SagaId",
                    constraintName     : "FK_SagaMessage_Saga_SagaId");

                AutoFaker.Provide(table);
                AutoFaker.Provide(foreignKey);
                AutoFaker.Provide<INavigationPropertyInfoAdapter, NavigationPropertyInfoAdapter>();

                A.CallTo(() => AutoFaker.Resolve<IReverseEngineeringConfiguration>().Pluralize).Returns(true);

                //Act
                var result = AutoFaker.Resolve<INavigationPropertyInfoAdapter>().GetPropertyText();

                //Assert
                result.ShouldBeEquivalentTo(
                    "public virtual Saga Saga { get; set; } // FK_SagaMessage_Saga_SagaId");
            }          
        }

        public class GetMap : NavigationPropertyInfoAdapterTestsBase
        {
            [Test]
            public void WhenTableIsPrimaryKeyTable_ShouldReturnNull()
            {
                //Arrange
                ISchemaTable table = new SqlSchemaTable(
                    schema: "dbo",
                    name  : "Saga",
                    isView: false);

                ISchemaForeignKey foreignKey = new SqlSchemaForeignKey(
                    foreignKeySchema   : "dbo",
                    foreignKeyTableName: "SagaMessage",
                    foreignKeyColumn   : "SagaId",
                    primaryKeySchema   : "dbo",
                    primaryKeyTableName: "Saga",
                    primaryKeyColumn   : "SagaId",
                    constraintName     : "FK_SagaMessage_Saga_SagaId");

                AutoFaker.Provide(table);
                AutoFaker.Provide(foreignKey);
                AutoFaker.Provide<INavigationPropertyInfoAdapter, NavigationPropertyInfoAdapter>();

                A.CallTo(() => AutoFaker.Resolve<IReverseEngineeringConfiguration>().Pluralize).Returns(true);

                //Act
                var result = AutoFaker.Resolve<INavigationPropertyInfoAdapter>().GetMapppingText();

                //Assert
                result.Should().BeNull();
            }

            [Test]
            public void WhenTableIsForeignKeyTable_ShouldReturnMap()
            {
                //Arrange
                ISchemaTable table = new SqlSchemaTable(
                    schema: "dbo",
                    name  : "SagaMessage",
                    isView: false);

                ISchemaForeignKey foreignKey = new SqlSchemaForeignKey(
                    foreignKeySchema   : "dbo",
                    foreignKeyTableName: "SagaMessage",
                    foreignKeyColumn   : "SagaId",
                    primaryKeySchema   : "dbo",
                    primaryKeyTableName: "Saga",
                    primaryKeyColumn   : "SagaId",
                    constraintName     : "FK_SagaMessage_Saga_SagaId");

                AutoFaker.Provide(table);
                AutoFaker.Provide(foreignKey);
                AutoFaker.Provide<INavigationPropertyInfoAdapter, NavigationPropertyInfoAdapter>();

                A.CallTo(() => AutoFaker.Resolve<IReverseEngineeringConfiguration>().Pluralize).Returns(true);

                //Act
                var result = AutoFaker.Resolve<INavigationPropertyInfoAdapter>().GetMapppingText();

                //Assert
                result.ShouldBeEquivalentTo(
                    @"HasRequired(a => a.Saga).WithMany(b => b.SagaMessages).HasForeignKey(c => c.SagaId); // FK_SagaMessage_Saga_SagaId");
            }    
        }
    }

    public class NavigationPropertyInfoAdapterTestsBase : AutoFakeTestsBase
    {
        public override void SetUp()
        {
            base.SetUp();

            AutoFaker.Provide<IIdentifierGenerationService, IdentifierGenerationService>();
        }
    }
}