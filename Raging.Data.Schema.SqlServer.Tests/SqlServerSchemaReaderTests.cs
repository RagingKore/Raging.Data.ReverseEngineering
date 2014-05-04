using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Raging.Data.Schema.SqlServer.Tests
{
    [TestFixture]
    public class SqlServerSchemaReaderTests
    {
        ISchemaReader schemaReader;

        [SetUp]
        public void SetUpFixture()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["TestDatabase"].ConnectionString;

            this.schemaReader = new SqlServerSchemaReader(connectionString);
        }

        [Test]
        public void WhenDatabaseHasTables_ShouldReadSchema()
        {
            //Act
            var result = this.schemaReader.Read();

            //Assert
            result.Should().HaveCount(16);
            result.FirstOrDefault(table => table.IsView).Should().NotBeNull();
        }

        [Test]
        public void WhenTableHasForeignKeys_ShouldReadSchema()
        {
            //Arrange
            var tableName = "Media";

            //Act
            var result = this.schemaReader.Read().Single(table => table.Name == tableName);

            //Assert
            result.ForeignKeys.Should().HaveCount(7);
            result.ForeignKeys.Where(key => key.ForeignKeyTableName == tableName).Should().HaveCount(3);
            result.ForeignKeys.Where(key => key.PrimaryKeyTableName == tableName).Should().HaveCount(4);
        }
    }
}
