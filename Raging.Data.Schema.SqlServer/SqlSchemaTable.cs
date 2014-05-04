using System.Collections.Generic;
using System.Linq;
using Raging.Toolbox.Extensions;

namespace Raging.Data.Schema.SqlServer
{
    public class SqlSchemaTable : ISchemaTable
    {
        public SqlSchemaTable(string name, string schema, bool isView)
        {
            this.Name        = name;
            this.Schema      = schema;
            this.IsView      = isView;
            this.Columns     = new List<ISchemaColumn>();
            this.ForeignKeys = new List<ISchemaForeignKey>();
        }

        public string Name                         { get; private set; }
        public string Schema                       { get; private set; }
        public bool IsView                         { get; private set; }
        public List<ISchemaColumn> Columns         { get; private set; }
        public List<ISchemaForeignKey> ForeignKeys { get; private set; }

        public IEnumerable<ISchemaColumn> PrimaryKeys
        {
            get { return this.Columns.Where(x => x.IsPrimaryKey).ToList(); }
        }

        public ISchemaColumn this[string columnName]
        {
            get { return this.Columns.SingleOrDefault(x => x.Name.Like(columnName)); }
        }

        public string FullName
        {
            get { return "{0}.{1}".FormatWith(this.Schema, this.Name); }
        }
    }
}