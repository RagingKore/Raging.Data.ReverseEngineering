using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using Raging.Data.ReverseEngineering.Configuration;
using Raging.Data.ReverseEngineering.EntityFramework.Adapters;
using Raging.Data.ReverseEngineering.Infrastructure;
using Raging.Data.ReverseEngineering.Infrastructure.Metadata;
using Raging.Data.Schema;
using Raging.Toolbox.Testing.NUnit.FakeItEasy;

namespace Raging.Data.ReverseEngineering.EntityFramework.Tests
{
    [TestFixture]
    class PropertyInfoAdapterTests 
    {
        //public class GetName : PropertyInfoAdapterTestsBase
        //{
        //    [Test]
        //    public void WhenUsePascalCaseConfigurationsIsFalse_ShouldReturnUnchangedName()
        //    {
        //        //Arrange
        //        A.CallTo(() => AutoFaker.Resolve<ISchemaColumn>().Name).Returns("_strange-Test_table2");
        //        //A.CallTo(() => AutoFaker.Resolve<IReverseEngineeringConfiguration>().UsePascalCase).Returns(false);
        //        //A.CallTo(() => AutoFaker.Resolve<IReverseEngineeringConfiguration>().ColumnNamingOverrides).Returns(null);

        //        //Act
        //        var result = AutoFaker.Resolve<IPropertyInfoAdapter>().GetPropertyName();

        //        //Assert
        //        result.ShouldBeEquivalentTo(AutoFaker.Resolve<ISchemaColumn>().Name);
        //    }

        //    [Test]
        //    public void WhenUsePascalCaseConfigurationIsTrue_ShouldReturnCleanName()
        //    {
        //        //Arrange
        //        A.CallTo(() => AutoFaker.Resolve<ISchemaColumn>().Name).Returns("_strange-Test_table2");
        //        A.CallTo(() => AutoFaker.Resolve<IReverseEngineeringConfiguration>().UsePascalCase).Returns(true);
        //        A.CallTo(() => AutoFaker.Resolve<IReverseEngineeringConfiguration>().ColumnNamingOverrides).Returns(null);

        //        //Act
        //        var result = AutoFaker.Resolve<IPropertyInfoAdapter>().GetPropertyName();

        //        //Assert
        //        result.ShouldBeEquivalentTo("StrangeTestTable2");
        //    }
        //}

        public class GetMap : PropertyInfoAdapterTestsBase
        {
            [Test]
            public void WhenPropertyNameIsValid_ShouldReturnMap()
            {
                //Arrange
                A.CallTo(() => AutoFaker.Resolve<ISchemaColumn>().Name).Returns("PersonId");
                A.CallTo(() => AutoFaker.Resolve<IReverseEngineeringConfiguration>().UsePascalCase).Returns(true);

                //Act
                var result = AutoFaker.Resolve<IPropertyInfoAdapter>().GetColumnMapppingText();

                //Assert
                result.ShouldBeEquivalentTo(@"this.Plural(t => t.PersonId).HasColumnName(""PersonId"");");
            }
        }

        public class GetMapConfiguration : PropertyInfoAdapterTestsBase
        {
            [Test]
            public void WhenColumnIsNullable_ShouldReturnMapConfiguration()
            {
                //Arrange
                A.CallTo(() => AutoFaker.Resolve<ISchemaColumn>().Name).Returns("ExampleColumn");
                A.CallTo(() => AutoFaker.Resolve<ISchemaColumn>().IsNullable).Returns(true);

                //Act
                var result = AutoFaker.Resolve<IPropertyInfoAdapter>().GetMappingText();

                //Assert
                result.ShouldBeEquivalentTo(@"this.Plural(t => t.ExampleColumn).IsOptional();");
            }

            [Test]
            public void WhenColumnIsNotNullable_ShouldReturnMapConfiguration()
            {
                //Arrange
                A.CallTo(() => AutoFaker.Resolve<ISchemaColumn>().Name).Returns("ExampleColumn");
                A.CallTo(() => AutoFaker.Resolve<ISchemaColumn>().IsNullable).Returns(false);

                //Act
                var result = AutoFaker.Resolve<IPropertyInfoAdapter>().GetMappingText();

                //Assert
                result.ShouldBeEquivalentTo(@"this.Plural(t => t.ExampleColumn).IsRequired();");
            }

            [Test]
            public void WhenColumnIsIdentity_ShouldReturnMapConfiguration()
            {
                //Arrange
                A.CallTo(() => AutoFaker.Resolve<ISchemaColumn>().Name).Returns("ExampleColumn");
                A.CallTo(() => AutoFaker.Resolve<ISchemaColumn>().SystemType).Returns("int");
                A.CallTo(() => AutoFaker.Resolve<ISchemaColumn>().IsIdentity).Returns(true);

                //Act
                var result = AutoFaker.Resolve<IPropertyInfoAdapter>().GetMappingText();

                //Assert
                result.ShouldBeEquivalentTo(
                    @"this.Plural(t => t.ExampleColumn).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);");
            }

            [Test]
            public void WhenColumnIsComputed_ShouldReturnMapConfiguration()
            {
                //Arrange
                A.CallTo(() => AutoFaker.Resolve<ISchemaColumn>().Name).Returns("ExampleColumn");
                A.CallTo(() => AutoFaker.Resolve<ISchemaColumn>().SystemType).Returns("int");
                A.CallTo(() => AutoFaker.Resolve<ISchemaColumn>().IsStoreGenerated).Returns(true);

                //Act
                var result = AutoFaker.Resolve<IPropertyInfoAdapter>().GetMappingText();

                //Assert
                result.ShouldBeEquivalentTo(
                    @"this.Plural(t => t.ExampleColumn).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);");
            }

            [Test]
            public void WhenColumnIsRowVersion_ShouldReturnMapConfiguration()
            {
                //Arrange
                A.CallTo(() => AutoFaker.Resolve<ISchemaColumn>().Name).Returns("ExampleColumn");
                A.CallTo(() => AutoFaker.Resolve<ISchemaColumn>().SystemType).Returns("timestamp");
                A.CallTo(() => AutoFaker.Resolve<ISchemaColumn>().IsStoreGenerated).Returns(true);
                A.CallTo(() => AutoFaker.Resolve<ISchemaColumn>().IsNullable).Returns(false);
                A.CallTo(() => AutoFaker.Resolve<ISchemaColumn>().IsRowVersion).Returns(true);
                A.CallTo(() => AutoFaker.Resolve<ISchemaColumn>().MaxLength).Returns(8);

                //Act
                var result = AutoFaker.Resolve<IPropertyInfoAdapter>().GetMappingText();

                //Assert
                result.ShouldBeEquivalentTo(
                    @"this.Plural(t => t.ExampleColumn).IsRequired().HasMaxLength(8).IsFixedLength().IsRowVersion();");
            }

            [Test]
            public void WhenColumnHasPrecision_ShouldReturnMapConfiguration()
            {
                //Arrange
                A.CallTo(() => AutoFaker.Resolve<ISchemaColumn>().Name).Returns("ExampleColumn");
                A.CallTo(() => AutoFaker.Resolve<ISchemaColumn>().SystemType).Returns("float");
                A.CallTo(() => AutoFaker.Resolve<ISchemaColumn>().Scale).Returns(10);
                A.CallTo(() => AutoFaker.Resolve<ISchemaColumn>().Precision).Returns(10);

                //Act
                var result = AutoFaker.Resolve<IPropertyInfoAdapter>().GetMappingText();

                //Assert
                result.ShouldBeEquivalentTo(@"this.Plural(t => t.ExampleColumn).IsRequired().HasPrecision(10,10);");
            }

            [Test]
            public void WhenColumnHasMaxLengthAndIsRequired_ShouldReturnMapConfiguration()
            {
                //Arrange
                A.CallTo(() => AutoFaker.Resolve<ISchemaColumn>().Name).Returns("ExampleColumn");
                A.CallTo(() => AutoFaker.Resolve<ISchemaColumn>().MaxLength).Returns(250);

                //Act
                var result = AutoFaker.Resolve<IPropertyInfoAdapter>().GetMappingText();

                //Assert
                result.ShouldBeEquivalentTo(@"this.Plural(t => t.ExampleColumn).IsRequired().HasMaxLength(250);");
            }
        }

        public class GetProperty : PropertyInfoAdapterTestsBase
        {
            //// public int ModelId { get; set; } // ratingId (Primary key)
            //// public DateTime? SomeDate { get; set; } // someDate

            //return "public {0}{1} {2} {{ get; set; }} // {3}{4}"
            //    .FormatWith(
            //        column.SystemType,
            //        CheckNullable(column),
            //        GetEntityName(),
            //        column.Name,
            //        column.IsPrimaryKey ? " (Primary key)" : string.Empty
            //    );

            [Test]
            public void WhenColumnIsPrimaryKey_ShouldReturnProperty()
            {
                //Arrange
                A.CallTo(() => AutoFaker.Resolve<ISchemaColumn>().Name).Returns("example_column_id");
                A.CallTo(() => AutoFaker.Resolve<ISchemaColumn>().IsPrimaryKey).Returns(true);
                A.CallTo(() => AutoFaker.Resolve<ISchemaColumn>().SystemType).Returns("int");
                A.CallTo(() => AutoFaker.Resolve<IReverseEngineeringConfiguration>().UsePascalCase).Returns(true);

                //Act
                var result = AutoFaker.Resolve<IPropertyInfoAdapter>().GetPropertyText();

                //Assert
                result.ShouldBeEquivalentTo(@"public int ExampleColumnId { get; set; } // example_column_id (Primary key)");
            }

            [Test]
            public void WhenColumnIsNullable_ShouldReturnProperty()
            {
                //Arrange
                A.CallTo(() => AutoFaker.Resolve<ISchemaColumn>().Name).Returns("example_column");
                A.CallTo(() => AutoFaker.Resolve<ISchemaColumn>().IsNullable).Returns(true);
                A.CallTo(() => AutoFaker.Resolve<ISchemaColumn>().SystemType).Returns("DateTime");
                A.CallTo(() => AutoFaker.Resolve<IReverseEngineeringConfiguration>().UsePascalCase).Returns(true);

                //Act
                var result = AutoFaker.Resolve<IPropertyInfoAdapter>().GetPropertyText();

                //Assert
                result.ShouldBeEquivalentTo(@"public DateTime? ExampleColumn { get; set; } // example_column");
            }

            [Test]
            public void WhenColumnIsNotNullable_ShouldReturnProperty()
            {
                //Arrange
                A.CallTo(() => AutoFaker.Resolve<ISchemaColumn>().Name).Returns("example_column");
                A.CallTo(() => AutoFaker.Resolve<ISchemaColumn>().IsNullable).Returns(false);
                A.CallTo(() => AutoFaker.Resolve<ISchemaColumn>().SystemType).Returns("Guid");
                A.CallTo(() => AutoFaker.Resolve<IReverseEngineeringConfiguration>().UsePascalCase).Returns(true);

                //Act
                var result = AutoFaker.Resolve<IPropertyInfoAdapter>().GetPropertyText();

                //Assert
                result.ShouldBeEquivalentTo(@"public Guid ExampleColumn { get; set; } // example_column");
            }
        }

        public class GetCtor : PropertyInfoAdapterTestsBase
        {
            [Test]
            public void WhenColumnIsNullable_ShouldReturnNull()
            {
                //Arrange
                A.CallTo(() => AutoFaker.Resolve<ISchemaColumn>().IsNullable).Returns(true);

                //Act
                var result = AutoFaker.Resolve<IPropertyInfoAdapter>().GetConstructorInitializationText();

                //Assert
                result.Should().BeNull();
            }

            [Test]
            public void WhenColumnIsNotNullableAndDefaultValueIsBlankAndSystemTypeIsGuid_ShouldReturnCtor()
            {
                //Arrange
                A.CallTo(() => AutoFaker.Resolve<ISchemaColumn>().Name).Returns("ExampleColumn");
                A.CallTo(() => AutoFaker.Resolve<ISchemaColumn>().SystemType).Returns("Guid");
                A.CallTo(() => AutoFaker.Resolve<ISchemaColumn>().IsNullable).Returns(false);

                //Act
                var result = AutoFaker.Resolve<IPropertyInfoAdapter>().GetConstructorInitializationText();

                //Assert
                result.ShouldBeEquivalentTo(@"this.ExampleColumn = Guid.NewGuid();");
            }

            [Test]
            public void WhenColumnIsNotNullableAndDefaultValueIsBlankAndSystemTypeIsDateTime_ShouldReturnCtor()
            {
                //Arrange
                A.CallTo(() => AutoFaker.Resolve<ISchemaColumn>().Name).Returns("ExampleColumn");
                A.CallTo(() => AutoFaker.Resolve<ISchemaColumn>().SystemType).Returns("DateTime");
                A.CallTo(() => AutoFaker.Resolve<ISchemaColumn>().IsNullable).Returns(false);

                //Act
                var result = AutoFaker.Resolve<IPropertyInfoAdapter>().GetConstructorInitializationText();

                //Assert
                result.ShouldBeEquivalentTo(@"this.ExampleColumn = DateTime.UtcNow;");
            }

            [TestCase("long")]
            [TestCase("short")]
            [TestCase("int")]
            [TestCase("double")]
            [TestCase("decimal")]
            [TestCase("float")]
            public void WhenColumnIsNotNullableAndDefaultValueIsBlankAndSystemTypeNotGuidOrDateTime_ShouldReturnNull(string systemType)
            {
                //Arrange
                A.CallTo(() => AutoFaker.Resolve<ISchemaColumn>().Name).Returns("ExampleColumn");
                A.CallTo(() => AutoFaker.Resolve<ISchemaColumn>().SystemType).Returns(systemType);
                A.CallTo(() => AutoFaker.Resolve<ISchemaColumn>().IsNullable).Returns(false);

                //Act
                var result = AutoFaker.Resolve<IPropertyInfoAdapter>().GetConstructorInitializationText();

                //Assert
                result.Should().BeNull();
            }

            [Test]
            public void WhenColumnIsNotNullableAndDefaultValueIsNotBlankAndSystemTypeIsGuid_ShouldReturnCtor()
            {
                //Arrange
                A.CallTo(() => AutoFaker.Resolve<ISchemaColumn>().Name).Returns("ExampleColumn");
                A.CallTo(() => AutoFaker.Resolve<ISchemaColumn>().SystemType).Returns("Guid");
                A.CallTo(() => AutoFaker.Resolve<ISchemaColumn>().DefaultValue).Returns("87EB5533-CE17-4624-A652-A70938F82CB4");
                A.CallTo(() => AutoFaker.Resolve<ISchemaColumn>().IsNullable).Returns(false);

                //Act
                var result = AutoFaker.Resolve<IPropertyInfoAdapter>().GetConstructorInitializationText();

                //Assert
                result.ShouldBeEquivalentTo(@"this.ExampleColumn = Guid.Parse(""87EB5533-CE17-4624-A652-A70938F82CB4"");");
            }

            [Test]
            public void WhenColumnIsNotNullableAndDefaultValueIsNotBlankAndSystemTypeIsDateTime_ShouldReturnCtor()
            {
                //Arrange
                A.CallTo(() => AutoFaker.Resolve<ISchemaColumn>().Name).Returns("ExampleColumn");
                A.CallTo(() => AutoFaker.Resolve<ISchemaColumn>().SystemType).Returns("DateTime");
                A.CallTo(() => AutoFaker.Resolve<ISchemaColumn>().DefaultValue).Returns("01-01-2020");
                A.CallTo(() => AutoFaker.Resolve<ISchemaColumn>().IsNullable).Returns(false);

                //Act
                var result = AutoFaker.Resolve<IPropertyInfoAdapter>().GetConstructorInitializationText();

                //Assert
                result.ShouldBeEquivalentTo(@"this.ExampleColumn = DateTime.Parse(""01-01-2020"");");
            }
        }
    }

    public class PropertyInfoAdapterTestsBase : AutoFakeTestsBase
    {
        public override void SetUp()
        {
            base.SetUp();

            AutoFaker.Provide<IIdentifierGenerationService, IdentifierGenerationService>();
            AutoFaker.Provide<IPropertyInfoAdapter, PropertyInfoAdapter>();
        }
    }
}
