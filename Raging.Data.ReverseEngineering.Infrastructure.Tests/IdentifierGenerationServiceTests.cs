using System.Collections.Generic;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using Raging.Data.ReverseEngineering.Configuration;
using Raging.Data.ReverseEngineering.Infrastructure.Metadata;
using Raging.Data.ReverseEngineering.Infrastructure.Pluralization;
using Raging.Toolbox.Testing.NUnit.FakeItEasy;

namespace Raging.Data.ReverseEngineering.Infrastructure.Tests
{
    [TestFixture]
    class IdentifierGenerationServiceTests : AutoFakeTestsBase
    {
        public override void SetUp()
        {
            base.SetUp();

            AutoFaker.Provide<IPluralizationService, EnglishPluralizationService>();
            AutoFaker.Provide<IIdentifierGenerationService, IdentifierGenerationService>();
        }

        #region . Singular .

        [Test]
        public void WhenPluralizeAndUsePascalCaseConfigurationsAreFalse_ShouldReturnUnchangedName()
        {
            //Arrange
            string input = "_strange-Test_table2";
            string output = "_strange-Test_table2";

            //Act
            var result = AutoFaker
                .Resolve<IIdentifierGenerationService>()
                .Generate(input, NounForms.Singular);

            //Assert
            result.ShouldBeEquivalentTo(output);
        }

        [TestCase("_strange-Test_table2", "StrangeTestTable2")]
        [TestCase("i_dont-Care", "IDontCare")]
        public void WhenUsePascalCaseConfigurationIsTrue_ShouldReturnCleanName(string input, string output)
        {
            //Arrange
            A.CallTo(() => AutoFaker.Resolve<IReverseEngineeringConfiguration>().UsePascalCase)
                .Returns(true);

            //Act
            var result = AutoFaker
                .Resolve<IIdentifierGenerationService>()
                .Generate(input, NounForms.Singular);

            //Assert
            result.ShouldBeEquivalentTo(output);
        }

        [TestCase("People", "Person")]
        [TestCase("Companies", "Company")]
        public void WhenPluralizeConfigurationIsTrue_ShouldReturnSingularName(string input, string output)
        {
            //Arrange
            A.CallTo(() => AutoFaker.Resolve<IReverseEngineeringConfiguration>().Pluralize)
                .Returns(true);

            //Act
            var result = AutoFaker
                .Resolve<IIdentifierGenerationService>()
                .Generate(input, NounForms.Singular);

            //Assert
            result.ShouldBeEquivalentTo(output);
        }

        [TestCase("classes", "_class")]
        [TestCase("private", "_private")]
        [TestCase("this", "_this")]
        public void WhenOutputNameIsReservedAndPluralizeConfigurationIsTrue_ShouldReturnValidSingularName(string input, string output)
        {
            //Arrange
            A.CallTo(() => AutoFaker.Resolve<IReverseEngineeringConfiguration>().Pluralize)
                .Returns(true);

            //Act
            var result = AutoFaker
                .Resolve<IIdentifierGenerationService>()
                .Generate(input, NounForms.Singular);

            //Assert
            result.ShouldBeEquivalentTo(output);
        }

        #endregion

        #region . Plural .

        [Test]
        public void WhenUsePascalCaseConfigurationIsTrue_ShouldReturnCleanName()
        {
            //Arrange
            A.CallTo(() => AutoFaker.Resolve<IReverseEngineeringConfiguration>().UsePascalCase)
                .Returns(true);

            //Act
            var result = AutoFaker.Resolve<IIdentifierGenerationService>().Generate("_strange-Test_table2", NounForms.Plural);

            //Assert
            result.ShouldBeEquivalentTo("StrangeTestTable2");
        }

        [TestCase("Person", "People")]
        [TestCase("Companies", "Companies")]
        public void WhenTargetIsPropertyAndPluralizeConfigurationIsTrue_ShouldReturnPluralName(string input, string output)
        {
            //Arrange
            A.CallTo(() => AutoFaker.Resolve<IReverseEngineeringConfiguration>().Pluralize).Returns(true);

            //Act
            var result = AutoFaker.Resolve<IIdentifierGenerationService>().Generate(input, NounForms.Plural);

            //Assert
            result.ShouldBeEquivalentTo(output);
        }

        #endregion

        [TestCase("this", "", "these")]
        [TestCase("Person", "dbo.Person", "Persona")]
        public void WhendOverrideExists_ShouldReturnOverride(string input, string inputFullname, string output)
        {
            //Arrange
            A.CallTo(() => AutoFaker.Resolve<IReverseEngineeringConfiguration>().GlobalNamingOverrides)
                .Returns(
                    new List<GlobalNameOverride>
                    {
                        new GlobalNameOverride(inputFullname,output)
                    });

            //Act
            var result = AutoFaker
                .Resolve<IIdentifierGenerationService>()
                .Generate(input, NounForms.Singular, inputFullname);

            //Assert
            result.ShouldBeEquivalentTo(output);
        }


        [Test]
        public void WhenIdentifierWasAlreadyGenerated_ShouldReturnFromNameStore()
        {
            //Arrange
            string expectedName = "ExampleName";

            //generate and store
            AutoFaker.Resolve<IIdentifierGenerationService>().Generate(expectedName, NounForms.Singular);
            
            //Act
            var result = AutoFaker
                .Resolve<IIdentifierGenerationService>()
                .Generate(expectedName, NounForms.Singular);

            //Assert
            result.ShouldBeEquivalentTo(expectedName);

            A.CallTo(() => AutoFaker.Resolve<IReverseEngineeringConfiguration>().GlobalNamingOverrides)
                .MustHaveHappened(Repeated.Exactly.Times(3));
        }
    }
}