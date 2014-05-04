using System.Linq;
using Raging.Data.ReverseEngineering.Infrastructure.Metadata;
using Raging.Data.ReverseEngineering.Infrastructure.Pluralization;
using Raging.Data.Schema;
using Raging.Toolbox.Extensions;

namespace Raging.Data.ReverseEngineering.EntityFramework.Adapters
{
    public class EntityInfoAdapter : IEntityInfoAdapter
    {
        private readonly ISchemaTable table;
        private readonly IIdentifierGenerationService identifierGenerationService;

        public EntityInfoAdapter(ISchemaTable table, IIdentifierGenerationService identifierGenerationService)
        {
            this.table                = table;
            this.identifierGenerationService = identifierGenerationService;
        }

        public string GetEntityName()
        {
            return this.identifierGenerationService.Generate(this.table.Name, NounForms.Singular, this.table.FullName);
        }

        public string GetDbSetText()
        {
            return @"public DbSet<{0}> {1} {{ get; set; }}".FormatWith(
                this.identifierGenerationService.Generate(this.table.Name, NounForms.Singular, this.table.FullName),
                this.identifierGenerationService.Generate(this.table.Name, NounForms.Plural, this.table.FullName));
        }

        public string GetTableMappingText()
        {
            // this.ToTable("Singular");

            return @"this.ToTable(""{0}"");".FormatWith(this.table.Name);
        }

        public string GetPrimaryKeyMappingText()
        {
            // this.HasKey(t => t.ModelId);
            // this.HasKey(t => new { t.ModelId, t.AnotherModelId, t.SomeOtherModelId });

            //get pk's
            var primaryKeyColumns = this.table.Columns
                .Where(column => column.IsPrimaryKey)
                .Select(column => column.Name)
                .ToList();

            //tables without pk's and views
            if (primaryKeyColumns.Count == 0 || this.table.IsView)
            {
                var requiredColumns = this.table.Columns
                   .Where(column => !column.IsNullable)
                   .Select(column => column.Name)
                   .ToList();

                //default to a key with all columns if none are required
                return @"this.HasKey(t => new {{{0}}});"
                    .FormatWith(
                        requiredColumns.Count > 0
                        ? requiredColumns.Select(column => "t.{0}".FormatWith(column)).Join()
                        : this.table.Columns.Select(column => "t.{0}".FormatWith(column)).Join()
                    );
            }

            //if it's a composite key or single column key
            return primaryKeyColumns.Count > 1
                       ? @"this.HasKey(t => new {{{0}}});"
                            .FormatWith(primaryKeyColumns.Select(column => "t.{0}".FormatWith(column))
                            .Join())
                       : @"this.HasKey(t => t.{0});".FormatWith(primaryKeyColumns[0]);
        }
    }
}