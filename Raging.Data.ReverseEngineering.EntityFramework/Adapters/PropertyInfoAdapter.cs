using Raging.Data.ReverseEngineering.Infrastructure.Metadata;
using Raging.Data.ReverseEngineering.Infrastructure.Pluralization;
using Raging.Data.Schema;
using Raging.Toolbox.Extensions;

namespace Raging.Data.ReverseEngineering.EntityFramework.Adapters
{
    public class PropertyInfoAdapter : IPropertyInfoAdapter
    {
        private readonly ISchemaTable table;
        private readonly IIdentifierGenerationService identifierGenerationService;
        private readonly ISchemaColumn column;

        #region . Ctors .

        public PropertyInfoAdapter(ISchemaColumn column, ISchemaTable table, IIdentifierGenerationService identifierGenerationService)
        {
            this.column               = column;
            this.table                = table;
            this.identifierGenerationService = identifierGenerationService;
        }

        #endregion

        #region . IPropertyInfoAdapter .

        public string GetMappingText()
        {
            // this.Property(t => t.Name).IsRequired().HasMaxLength(250);
            // this.Property(t => t.Name).IsRequired().IsFixedLength().HasMaxLength(10);
            // this.Property(t => t.ArtworkTypeId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            return @"this.Property(t => t.{0}){1}{2}{3}{4}{5};"
                .FormatWith(
                    this.GetPropertyName(),
                    this.column.IsNullable ? "\r\n\t.IsOptional()" : "\r\n\t.IsRequired()",
                    this.column.MaxLength > 0 ? "\r\n\t.HasMaxLength({0})".FormatWith(this.column.MaxLength) : string.Empty,
                    this.column.Scale > 0 ? "\r\n\t.HasPrecision({0},{1})".FormatWith(this.column.Precision, this.column.Scale) : string.Empty,
                    //column.IsStoreGenerated && column.SystemType.Like("timestamp") ? ".IsFixedLength().IsRowVersion()" : string.Empty,
                    this.column.IsRowVersion ? "\r\n\t.IsFixedLength()\r\n\t.IsRowVersion()" : string.Empty,
                    GetDatabaseGeneratedOption(this.column)
                );
        }

        public string GetColumnMapppingText()
        {
            // this.Plural(t => t.ModelId).HasColumnName("ModelId");

            return @"this.Property(t => t.{0}).HasColumnName(""{1}"");"
                .FormatWith(this.GetPropertyName(), this.column.Name);
        }

        public string GetPropertyName()
        {
            return this.identifierGenerationService.Generate(this.column.Name, NounForms.Singular, "{0}.{1}".FormatWith(this.table.FullName, this.column.Name));
        }

        public string GetPropertyText()
        {
            // public int ModelId { get; set; } // ratingId (Primary key)
            // public DateTime? SomeDate { get; set; } // someDate
            // public int Computed { get; private set; } // computed

            var text = "public {0}{1} {2} {{ {3} }} // {4}{5}"
                .FormatWith(
                    this.column.SystemType,
                    CheckNullable(this.column),
                    this.GetPropertyName(),
                    this.column.IsStoreGenerated ? "get; private set;" : "get; set;",
                    this.column.Name,
                    this.column.IsPrimaryKey ? " (Primary key)" : string.Empty
                );

            return text;
        }

        const string ConstructorInitializationTextTemplate = @"this.{0} = {1};";

        public string GetConstructorInitializationText()
        {
            //this.Name = @"John Doe";
            //this.Age = 25;
            //this.BirthDate = DateTime.Parse("2014-05-01 15:40:47.713");
            //this.ExternalId = Guid.Parse("37993BAC-E895-4EFA-B726-2FD1405ADC53");

            if(HasComputedDefaultValue(this.column)) return null;

            string defaultValue = null;

            if(this.column.DefaultValue.IsNotBlank())
            {
                defaultValue = CleanDefaultValue(this.column);

                switch (this.column.SystemType)
                {
                    case "Guid":
                        defaultValue = @"Guid.Parse(""{0}"")".FormatWith(defaultValue);
                        break;

                    case "DateTime":
                        defaultValue = @"DateTime.Parse(""{0}"")".FormatWith(defaultValue);
                        break;

                    case "System.Data.Entity.Spatial.DbGeography":
                        defaultValue = @"DbGeography.Parse(""{0}"")".FormatWith(column.DefaultValue);
                        break;

                    case "System.Data.Entity.Spatial.DbGeometry":
                        defaultValue = @"DbGeometry.Parse(""{0}"")".FormatWith(column.DefaultValue);
                        break;

                    case "string":
                        defaultValue = @"@""{0}""".FormatWith(defaultValue);
                        break;

                    //case "long":
                    //case "short":
                    //case "int":
                    //    defaultValue = this.column.DefaultValue;
                    //    break;

                    case "double":
                        defaultValue = @"{0}d".FormatWith(defaultValue);
                        break;

                    case "decimal":
                        defaultValue = @"{0}m".FormatWith(defaultValue);
                        break;

                    case "float":
                        defaultValue = @"{0}f".FormatWith(defaultValue);
                        break;

                    case "bool":
                        defaultValue = @"{0}".FormatWith(defaultValue == "1" ? "true" : "false");
                        break;
                } 
            }
            else if(!this.column.IsNullable)
            {
                //only types requiring initialization
                switch(this.column.SystemType)
                {
                    case "Guid":
                        defaultValue = "Guid.NewGuid()";
                        break;

                    case "DateTime":
                        defaultValue = "DateTime.UtcNow";
                        break;
                }
            }

            return defaultValue.IsBlank()
                ? null 
                : ConstructorInitializationTextTemplate
                    .FormatWith(
                        this.GetPropertyName(),
                        defaultValue);
        }

        #endregion

        #region . Helpers .

        private static readonly string[] NonNullableTypes =
        {
            "byte[]", 
            "string", 
            "DbGeography",
            "DbGeometry"
        };

        private static string CheckNullable(ISchemaColumn column)
        {
            return column.IsNullable && column.SystemType.NotLikeAll(NonNullableTypes) 
                ? "?" 
                : string.Empty;
        }

        private static bool HasComputedDefaultValue(ISchemaColumn column)
        {
            return column.DefaultValue.ContainsAny("getdate", "getutcdate", "newid", "newsequentialid");
        }

        private static string GetDatabaseGeneratedOption(ISchemaColumn column)
        {
            var hasDatabaseGeneratedOption = (column.SystemType.Like("guid")   || column.SystemType.Like("long") ||
                                              column.SystemType.Like("short")  || column.SystemType.Like("int") ||
                                              column.SystemType.Like("double") || column.SystemType.Like("float") ||
                                              column.SystemType.Like("decimal"));

            if (!hasDatabaseGeneratedOption) 
                return string.Empty;

            if (column.IsIdentity)
                return "\r\n\t.HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)";

            if (column.IsStoreGenerated)
                return "\r\n\t.HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed)";

            if (column.IsPrimaryKey && !column.IsIdentity && !column.IsStoreGenerated)
                return "\r\n\t.HasDatabaseGeneratedOption(DatabaseGeneratedOption.None)";

            return string.Empty;
        }

        private static string CleanDefaultValue(ISchemaColumn column)
        {
            return column.SystemType.LikeAny("string","guid","datetime","bool","long","short","int","double","float","decimal") 
                ? column.DefaultValue.Substring(2).Remove(column.DefaultValue.Length - 4, 2)
                : column.DefaultValue;
        }

        #endregion
    }
}