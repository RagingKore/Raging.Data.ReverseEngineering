using Raging.Toolbox.Extensions;

namespace Raging.Data.Schema.SqlServer
{
    public class SqlSchemaForeignKey : ISchemaForeignKey
    {
        public SqlSchemaForeignKey(
            string foreignKeySchema, string foreignKeyTableName, string foreignKeyColumn, 
            string primaryKeySchema, string primaryKeyTableName, string primaryKeyColumn, string constraintName)
        {
            this.ForeignKeySchema    = foreignKeySchema;
            this.ForeignKeyTableName = foreignKeyTableName;
            this.ForeignKeyColumn    = foreignKeyColumn;
            this.PrimaryKeySchema    = primaryKeySchema;
            this.PrimaryKeyTableName = primaryKeyTableName;
            this.PrimaryKeyColumn    = primaryKeyColumn;
            this.ConstraintName      = constraintName;
        }

        public string ForeignKeySchema    { get; private set; }
        public string ForeignKeyTableName { get; private set; }
        public string ForeignKeyColumn    { get; private set; }
        public string PrimaryKeySchema    { get; private set; }
        public string PrimaryKeyTableName { get; private set; }
        public string PrimaryKeyColumn    { get; private set; }
        public string ConstraintName      { get; private set; }

        public string ForeignKeyTableFullName
        {
            get { return "{0}.{1}".FormatWith(this.ForeignKeySchema, this.ForeignKeyTableName); }
        }

        public string PrimaryKeyTableFullName
        {
            get { return "{0}.{1}".FormatWith(this.PrimaryKeySchema, this.PrimaryKeyTableName); }
        }
    }
}