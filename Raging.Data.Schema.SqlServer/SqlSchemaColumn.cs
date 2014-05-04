namespace Raging.Data.Schema.SqlServer
{
    public class SqlSchemaColumn : ISchemaColumn
    {
        public string Name           { get; private set; } //ColumnName	
        public string Type           { get; private set; } //TypeName
        public string SystemType     { get; private set; }
        public int DateTimePrecision { get; private set; } //DateTimePrecision	
        public string DefaultValue   { get; private set; } //Default	
        public int MaxLength         { get; private set; } //MaxLength	
        public int Precision         { get; private set; } //Precision	
        public int Scale             { get; private set; } //Scale	
        public int Ordinal           { get; private set; } //Ordinal	
        public bool IsIdentity       { get; private set; } //IsIdentity	
        public bool IsNullable       { get; private set; } //IsNullable	
        public bool IsPrimaryKey     { get; private set; } //PrimaryKey
        public bool IsStoreGenerated { get; private set; } //IsStoreGenerated	
        public bool IsRowVersion     { get; private set; }

        public SqlSchemaColumn(
            string name, string type, string systemType, int dateTimePrecision, string defaultValue, int maxLength, int precision,
            int scale, int ordinal, bool isIdentity, bool isNullable, bool isPrimaryKey, bool isStoreGenerated)
        {
            this.Name              = name;
            this.Type              = type;
            this.SystemType        = systemType;
            this.DateTimePrecision = dateTimePrecision;
            this.DefaultValue      = defaultValue;
            this.MaxLength         = maxLength;
            this.Precision         = precision;
            this.Scale             = scale;
            this.Ordinal           = ordinal;
            this.IsIdentity        = isIdentity;
            this.IsNullable        = isNullable;
            this.IsPrimaryKey      = isPrimaryKey;
            this.IsStoreGenerated  = isStoreGenerated;

            this.IsRowVersion = isStoreGenerated && !isNullable && type == "timestamp";

            if (this.IsRowVersion)
                this.MaxLength = 8;
        }
    }
}