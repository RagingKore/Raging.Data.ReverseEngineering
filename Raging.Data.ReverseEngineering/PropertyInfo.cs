namespace Raging.Data.ReverseEngineering
{
    public struct PropertyInfo
    {
        public PropertyInfo(
            string columnName,
            string propertyText,
            string constructorInitializationText,
            string mappingText,
            string columnMappingText) : this()
        {
            this.ColumnName                    = columnName;
            this.PropertyText                  = propertyText;
            this.ConstructorInitializationText = constructorInitializationText;
            this.MappingText                   = mappingText;
            this.ColumnMappingText             = columnMappingText;
        }

        public string ColumnName { get; private set; }

        public string PropertyText { get; private set; }

        public string ConstructorInitializationText { get; private set; }

        /// <summary>
        /// this.Plural(t => t.Name).IsRequired().HasMaxLength(250);
        /// this.Plural(t => t.Name).IsRequired().IsFixedLength().HasMaxLength(10);
        /// this.Plural(t => t.ArtworkTypeId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
        /// </summary>
        public string MappingText { get; private set; }

        /// <summary>
        /// this.Plural(t => t.ModelId).HasColumnName("ModelId");
        /// </summary>
        public string ColumnMappingText { get; private set; }

        public override string ToString()
        {
            return this.ColumnName;
        }
    }
}