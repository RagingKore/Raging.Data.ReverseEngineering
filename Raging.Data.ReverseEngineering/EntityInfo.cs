using System.Collections.Generic;

namespace Raging.Data.ReverseEngineering
{
    public struct EntityInfo
    {
        public EntityInfo(
            string tableName, 
            string modelName, 
            string dbSetText, 
            string tableMappingText, 
            string primaryKeyMappingText,
            IReadOnlyList<PropertyInfo> properties, 
            IReadOnlyList<NavigationPropertyInfo> relationships) : this()
        {
            this.TableName             = tableName;
            this.ModelName             = modelName;
            this.DbSetText             = dbSetText;
            this.TableMappingText      = tableMappingText;
            this.PrimaryKeyMappingText = primaryKeyMappingText;
            this.Properties            = properties;
            this.NavigationProperties  = relationships;
        }

        public string TableName { get; private set; }

        public string ModelName { get; private set; }

        public string DbSetText { get; private set; }

        public string TableMappingText { get; private set; }

        public string PrimaryKeyMappingText { get; private set; }

        public IReadOnlyList<PropertyInfo> Properties { get; private set; }

        public IReadOnlyList<NavigationPropertyInfo> NavigationProperties { get; private set; }

        public override string ToString()
        {
            return this.TableName;
        }
    }
}