using System.Collections.Generic;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using Raging.Data.ReverseEngineering.EntityFramework.Adapters;
using Raging.Data.Schema;
using Raging.Data.Schema.SqlServer;

namespace Raging.Data.ReverseEngineering.EntityFramework.Tests
{
    [TestFixture]
    internal class EntityInfoAdapterTests
    {
        public static readonly IEntityInfoAdapterFactory AdapterFactory = new EntityInfoAdapterFactory();

        public class GetTableMappingText
        {
            [Test]
            public void WhenTableNameIsValid_ShouldReturnMap()
            {
                //Arrange;
                var table = new SqlSchemaTable("Saga", "dbo", false);

                var provider = AdapterFactory.Create(table, null);

                //Act
                var result = provider.GetTableMappingText();

                //Assert
                result.ShouldBeEquivalentTo(@"this.ToTable(""Saga"");");
            }
        }

        public class GetPrimaryKeyMappingText
        {
            [Test]
            public void WhenTableHasCompositeKey_ShouldReturnCompositeKeyMap()
            {
                //Arrange
                var fakeColumnOne = A.Fake<ISchemaColumn>();

                A.CallTo(() => fakeColumnOne.Name).Returns("KeyOne");
                A.CallTo(() => fakeColumnOne.IsPrimaryKey).Returns(true);

                var fakeColumnTwo = A.Fake<ISchemaColumn>();

                A.CallTo(() => fakeColumnTwo.Name).Returns("KeyTwo");
                A.CallTo(() => fakeColumnTwo.IsPrimaryKey).Returns(true);

                var fakeColumnThree = A.Fake<ISchemaColumn>();

                A.CallTo(() => fakeColumnThree.Name).Returns("NotKeyColumn");

                var fakeTable = A.Fake<ISchemaTable>();

                A.CallTo(() => fakeTable.Columns).Returns(
                    new List<ISchemaColumn>
                    {
                        fakeColumnOne,
                        fakeColumnTwo,
                        fakeColumnThree
                    });

                var provider = AdapterFactory.Create(fakeTable, null);

                //Act
                var result = provider.GetPrimaryKeyMappingText();

                //Assert
                result.ShouldBeEquivalentTo("this.HasKey(t => new {t.KeyOne,t.KeyTwo});");
            }

            [Test]
            public void WhenTableHasSimpleKey_ShouldReturnSimpleKeyMap()
            {
                //Arrange
                var fakeColumnOne = A.Fake<ISchemaColumn>();

                A.CallTo(() => fakeColumnOne.Name).Returns("KeyOne");
                A.CallTo(() => fakeColumnOne.IsPrimaryKey).Returns(true);

                var fakeColumnTwo = A.Fake<ISchemaColumn>();

                A.CallTo(() => fakeColumnTwo.Name).Returns("NotKeyColumn");

                var fakeTable = A.Fake<ISchemaTable>();

                A.CallTo(() => fakeTable.Columns).Returns(
                    new List<ISchemaColumn>
                    {
                        fakeColumnOne,
                        fakeColumnTwo
                    });

                var provider = AdapterFactory.Create(fakeTable, null);

                //Act
                var result = provider.GetPrimaryKeyMappingText();

                //Assert
                result.ShouldBeEquivalentTo("this.HasKey(t => t.KeyOne);");
            }

            [Test]
            public void WhenTableIsView_ShouldReturnCompositeKeyMap()
            {
                //Arrange
                var fakeColumnOne = A.Fake<ISchemaColumn>();

                A.CallTo(() => fakeColumnOne.Name).Returns("ColumnOne");

                var fakeColumnTwo = A.Fake<ISchemaColumn>();

                A.CallTo(() => fakeColumnTwo.Name).Returns("ColumnTwo");

                var fakeTable = A.Fake<ISchemaTable>();

                A.CallTo(() => fakeTable.IsView).Returns(true);

                A.CallTo(() => fakeTable.Columns).Returns(
                    new List<ISchemaColumn>
                    {
                        fakeColumnOne,
                        fakeColumnTwo
                    });

                var provider = AdapterFactory.Create(fakeTable, null);

                //Act
                var result = provider.GetPrimaryKeyMappingText();

                //Assert
                result.ShouldBeEquivalentTo("this.HasKey(t => new {t.ColumnOne,t.ColumnTwo});");
            }

            [Test]
            public void WhenTableDoesNotHaveAnyKey_ShouldReturnCompositeKeyMap()
            {
                //Arrange
                var fakeColumnOne = A.Fake<ISchemaColumn>();

                A.CallTo(() => fakeColumnOne.Name).Returns("ColumnOne");

                var fakeColumnTwo = A.Fake<ISchemaColumn>();

                A.CallTo(() => fakeColumnTwo.Name).Returns("ColumnTwo");

                var fakeTable = A.Fake<ISchemaTable>();

                A.CallTo(() => fakeTable.Columns).Returns(
                    new List<ISchemaColumn>
                    {
                        fakeColumnOne,
                        fakeColumnTwo
                    });

                var provider = AdapterFactory.Create(fakeTable, null);

                //Act
                var result = provider.GetPrimaryKeyMappingText();

                //Assert
                result.ShouldBeEquivalentTo("this.HasKey(t => new {t.ColumnOne,t.ColumnTwo});");
            }
        }
    }
}