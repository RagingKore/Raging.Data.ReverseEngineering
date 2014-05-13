using System.Linq;
using Raging.Data.ReverseEngineering.Infrastructure.Metadata;
using Raging.Data.ReverseEngineering.Infrastructure.Pluralization;
using Raging.Data.Schema;
using Raging.Toolbox.Extensions;

namespace Raging.Data.ReverseEngineering.EntityFramework.Adapters
{
    public class NavigationPropertyInfoAdapter : INavigationPropertyInfoAdapter
    {
        private readonly ISchemaForeignKey foreignKey;
        private readonly IIdentifierGenerationService identifierGenerationService;
        private readonly ISchemaTable table;

        #region . Ctors .

        public NavigationPropertyInfoAdapter(ISchemaForeignKey foreignKey, ISchemaTable table, IIdentifierGenerationService identifierGenerationService)
        {
            this.foreignKey = foreignKey;
            this.table = table;
            this.identifierGenerationService = identifierGenerationService;
        }

        #endregion

        #region . INavigationPropertyInfoAdapter .

        private const string MappingTextTemplate = "this.{0}(a => a.{1})\r\n\t.WithMany(b => b.{2})\r\n\t.HasForeignKey(c => c.{3}); // {4}";
        private const string PropertyTextOneToManyTemplate = "public virtual ICollection<{0}> {1} {{ get; set; }} // {2}.{3}";
        private const string PropertyTextOneToOneTemplate  = "public virtual {0} {1} {{ get; set; }} // {2}";

        public string GetMapppingText()
        {
            string text = null;
            // 1:n
            if(this.table.FullName == this.foreignKey.ForeignKeyTableFullName)
            {
                if(this.foreignKey.PrimaryKeyColumn == this.foreignKey.ForeignKeyColumn)
                {
                    text = MappingTextTemplate
                        .FormatWith(
                            table.Columns.Single(column => column.Name == this.foreignKey.ForeignKeyColumn).IsNullable
                                ? "HasOptional"
                                : "HasRequired",
                            this.foreignKey.PrimaryKeyTableName,
                            this.identifierGenerationService.Generate(
                                this.foreignKey.ForeignKeyTableName, 
                                NounForms.Plural,
                                "{0}.{1}".FormatWith(this.foreignKey.ForeignKeyTableFullName, this.foreignKey.ForeignKeyTableName)),
                            this.foreignKey.ForeignKeyColumn,
                            this.foreignKey.ConstraintName);
                }
                else
                {
                    var propertyName = this.foreignKey.ForeignKeyColumn.ToLowerInvariant().EndsWith("id")
                        ? this.foreignKey.ForeignKeyColumn.Substring(0, this.foreignKey.ForeignKeyColumn.Length - 2)
                        : this.foreignKey.ForeignKeyColumn;

                    text = MappingTextTemplate
                        .FormatWith(
                            table.Columns.Single(column => column.Name == this.foreignKey.ForeignKeyColumn).IsNullable
                                ? "HasOptional"
                                : "HasRequired",
                            propertyName,
                            this.identifierGenerationService.Generate(propertyName, NounForms.Plural, "{0}.{1}".FormatWith(this.foreignKey.ForeignKeyTableFullName, propertyName)),
                            this.foreignKey.ForeignKeyColumn,
                            this.foreignKey.ConstraintName); 
                }
            }

            return text;
        }

        public string GetConstructorInitializationText()
        {

            string template = "this.{0} = new List<{1}>();";

             //one to many
            if(this.table.FullName == this.foreignKey.PrimaryKeyTableFullName)
            {
                if(this.foreignKey.PrimaryKeyColumn == this.foreignKey.ForeignKeyColumn)
                {
                    return template.FormatWith(
                        this.identifierGenerationService.Generate(
                            this.foreignKey.ForeignKeyTableName, 
                            NounForms.Plural, 
                            "{0}.{1}".FormatWith(this.foreignKey.ForeignKeyTableFullName, this.foreignKey.ForeignKeyTableName)),
                       this.foreignKey.ForeignKeyTableName);
                }
                else
                {
                    var propertyName = this.foreignKey.ForeignKeyColumn.ToLowerInvariant().EndsWith("id")
                        ? this.foreignKey.ForeignKeyColumn.Substring(0, this.foreignKey.ForeignKeyColumn.Length - 2)
                        : this.foreignKey.ForeignKeyColumn;

                    return template.FormatWith(
                        this.identifierGenerationService.Generate(
                            propertyName, 
                            NounForms.Plural, 
                            "{0}.{1}".FormatWith(this.foreignKey.ForeignKeyTableFullName, propertyName)),
                        this.foreignKey.ForeignKeyTableName);
                }
               

            }
            
          
            // this.Models = new List<Singular>();

            //throw new NotImplementedException();

            ////one to many
            //if (foreignKey.PrimaryKeySchema == table.Schema && foreignKey.PrimaryKeyTableName == table.Name)
            //{
            //    return "public virtual ICollection<{0}> {1} {{ get; set; }} //{2}"
            //        .FormatWith(
            //            identifierGenerationService.Transform(foreignKey.ForeignKeyTableName, NounForms.Singular),
            //            identifierGenerationService.Transform(foreignKey.ForeignKeyTableName, NounForms.Plural),
            //            foreignKey.ConstraintName
            //        );
            //}

            ////one to many
            //return "public virtual ICollection<{0}> {1} {{ get; set; }} //{2}"
            //    .FormatWith(
            //        foreignKey.ForeignKeyTableName,
            //        foreignKey.ForeignKeyTableName,
            //        foreignKey.ConstraintName
            //    );

            return string.Empty;
        }

        public string GetPropertyText()
        {
            string text = null;

            var propertyName = this.foreignKey.ForeignKeyColumn.ToLowerInvariant().EndsWith("id")
                                   ? this.foreignKey.ForeignKeyColumn.Substring(0, this.foreignKey.ForeignKeyColumn.Length - 2)
                                   : this.foreignKey.ForeignKeyColumn;

            //one to many
            if(this.table.FullName == this.foreignKey.PrimaryKeyTableFullName)
            {
                if(this.foreignKey.PrimaryKeyColumn == this.foreignKey.ForeignKeyColumn)
                {
                    text = PropertyTextOneToManyTemplate
                        .FormatWith(
                            this.identifierGenerationService.Generate(this.foreignKey.ForeignKeyTableName, NounForms.Singular, "{0}.{1}".FormatWith(this.foreignKey.ForeignKeyTableFullName, this.foreignKey.ForeignKeyTableName)),
                            this.identifierGenerationService.Generate(this.foreignKey.ForeignKeyTableName, NounForms.Plural, "{0}.{1}".FormatWith(this.foreignKey.ForeignKeyTableFullName, this.foreignKey.ForeignKeyTableName)),
                            this.foreignKey.ForeignKeyTableName,
                            this.foreignKey.ConstraintName);
                }
                else
                {
                    text = PropertyTextOneToManyTemplate
                        .FormatWith(
                            this.identifierGenerationService.Generate(this.foreignKey.ForeignKeyTableName, NounForms.Singular, "{0}.{1}".FormatWith(this.foreignKey.ForeignKeyTableFullName, this.foreignKey.ForeignKeyTableName)),
                            this.identifierGenerationService.Generate(propertyName, NounForms.Plural, "{0}.{1}".FormatWith(this.foreignKey.ForeignKeyTableFullName, propertyName)),
                            this.foreignKey.ForeignKeyTableName,
                            this.foreignKey.ConstraintName);
                }
            }

            //one to one
            if(this.table.FullName == this.foreignKey.ForeignKeyTableFullName)
            {
                if(text.IsNotBlank()) text += "\r\n";

                text += PropertyTextOneToOneTemplate
                    .FormatWith(
                        //this.identifierGenerationService.Generate(this.foreignKey.PrimaryKeyTableName, NounForms.Singular),
                        this.identifierGenerationService.Generate(this.foreignKey.PrimaryKeyTableName, NounForms.Singular, "{0}.{1}".FormatWith(this.foreignKey.PrimaryKeyTableFullName, this.foreignKey.PrimaryKeyTableName)),
                        this.identifierGenerationService.Generate(propertyName, NounForms.Singular, "{0}.{1}".FormatWith(this.foreignKey.ForeignKeyTableFullName, propertyName)),
                        this.foreignKey.ConstraintName);
            }

            return text;
        }

        #endregion
    }
}

//public string GetPropertyText()
//       {
//           string text = null;

//           //one to many
//           if (this.table.FullName == this.foreignKey.PrimaryKeyTableFullName)
//           {

//               //var text = PropertyTextTemplate
//               //    .FormatWith(
//               //        this.identifierGenerationService.Generate(this.foreignKey.ForeignKeyTableName, NounForms.Singular),
//               //        this.identifierGenerationService.Generate(this.foreignKey.ForeignKeyTableName, NounForms.Plural),
//               //        this.foreignKey.ForeignKeyTableName,
//               //        this.foreignKey.ConstraintName);

//               //return text;

//               //// public virtual ICollection<RelatedSaga> SecondarySagas { get; set; } // RelatedSaga.FK_RelatedSaga_SecondarySaga
//               //if (this.foreignKey.PrimaryKeyTableFullName != this.foreignKey.ForeignKeyTableFullName)
//               //{

//               var propertyName = this.foreignKey.ForeignKeyColumn.ToLowerInvariant().EndsWith("id")
//                  ? this.foreignKey.ForeignKeyColumn.Substring(0, this.foreignKey.ForeignKeyColumn.Length - 2)
//                  : this.foreignKey.ForeignKeyColumn;

//               text = "public virtual ICollection<{0}> {1} {{ get; set; }} // {2}.{3}"
//                   .FormatWith(
//                       this.identifierGenerationService.Generate(this.foreignKey.ForeignKeyTableName, NounForms.Singular),
//                       this.identifierGenerationService.Generate(propertyName, NounForms.Plural),
//                       this.foreignKey.ForeignKeyTableName,
//                       this.foreignKey.ConstraintName);

//               //}

//               //// public virtual ICollection<Person> People { get; set; } // Person.FK_Person_Father
//               //// public virtual ICollection<SagaMessage> SagaMessages { get; set; } // SagaMessage.FK_SagaMessage_Saga
//               //else
//               //{
//               //    var text = PropertyTextTemplate
//               //      .FormatWith(
//               //          this.identifierGenerationService.Generate(this.foreignKey.ForeignKeyTableName, NounForms.Singular),
//               //          this.identifierGenerationService.Generate(this.foreignKey.ForeignKeyTableName, NounForms.Plural),
//               //          this.foreignKey.ForeignKeyTableName,
//               //          this.foreignKey.ConstraintName);

//               //    return text;
//               //}

//               //if (this.foreignKey.PrimaryKeyTableFullName != this.foreignKey.ForeignKeyTableFullName)
//               //{
//               //    var propertyName = this.foreignKey.ForeignKeyColumn.ToLowerInvariant().EndsWith("id")
//               //      ? this.foreignKey.ForeignKeyColumn.Substring(0, this.foreignKey.ForeignKeyColumn.Length - 2)
//               //      : this.foreignKey.ForeignKeyColumn;

//               //    var text = "public virtual ICollection<{0}> {1} {{ get; set; }} // {2}.{3}"
//               //        .FormatWith(
//               //            this.identifierGenerationService.Generate(this.foreignKey.ForeignKeyTableName, NounForms.Singular),
//               //            this.identifierGenerationService.Generate(propertyName, NounForms.Plural),
//               //            this.foreignKey.ForeignKeyTableName,
//               //            this.foreignKey.ConstraintName);

//               //    return text;
//               //}
//               //else
//               //{
//               //    var text = "public virtual ICollection<{0}> {1} {{ get; set; }} // {2}.{3}"
//               //        .FormatWith(
//               //            this.identifierGenerationService.Generate(this.foreignKey.ForeignKeyTableName, NounForms.Singular),
//               //            this.identifierGenerationService.Generate(this.foreignKey.ForeignKeyTableName, NounForms.Plural),
//               //            this.foreignKey.ForeignKeyTableName,
//               //            this.foreignKey.ConstraintName);

//               //    return text;
//               //}
//           }

//           //one to one
//           if (this.table.FullName == this.foreignKey.ForeignKeyTableFullName)
//           {
//               // public virtual Saga PrimarySaga { get; set; } // FK_RelatedSaga_PrimarySaga
//               // public virtual Saga Saga { get; set; } // FK_SagaMessage_Saga

//               var propertyName = this.foreignKey.ForeignKeyColumn.ToLowerInvariant().EndsWith("id")
//                   ? this.foreignKey.ForeignKeyColumn.Substring(0, this.foreignKey.ForeignKeyColumn.Length - 2)
//                   : this.foreignKey.ForeignKeyColumn;

//               text += "\r\npublic virtual {0} {1} {{ get; set; }} // {2}"
//                   .FormatWith(
//                       this.identifierGenerationService.Generate(this.foreignKey.PrimaryKeyTableName, NounForms.Singular),
//                       this.identifierGenerationService.Generate(propertyName, NounForms.Singular),
//                       this.foreignKey.ConstraintName);
//           }

//           return text;
//       }