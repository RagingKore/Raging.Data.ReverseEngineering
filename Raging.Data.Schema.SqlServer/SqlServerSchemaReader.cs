using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Raging.Toolbox.Extensions;

namespace Raging.Data.Schema.SqlServer
{
    public class SqlServerSchemaReader : ISchemaReader
    {
        private readonly string connectionString;

        #region . Tables Schema Sql .

        private const string TablesSchemaSql = @"
            SELECT
              [Extent1].[SchemaName],
              [Extent1].[Name]                            AS [TableName],
              --[Extent1].[TABLE_TYPE]                      AS TableType,
			  CASE
                WHEN ( [Extent1].[TABLE_TYPE] = 'VIEW' ) THEN CAST(1 AS BIT)
                ELSE CAST(0 AS BIT)
              END                                         AS [IsView],
              [UnionAll1].[Ordinal],
              [UnionAll1].[Name]                          AS [ColumnName],
              [UnionAll1].[IsNullable],
              [UnionAll1].[TypeName],
              ISNULL([UnionAll1].[MaxLength], 0)          AS [MaxLength],
              ISNULL([UnionAll1].[Precision], 0)          AS [Precision],
              ISNULL([UnionAll1].[Default], '')           AS [DefaultValue],
              ISNULL([UnionAll1].[DateTimePrecision], '') AS [DateTimePrecision],
              ISNULL([UnionAll1].[Scale], 0)              AS [Scale],
              [UnionAll1].[IsIdentity],
              [UnionAll1].[IsStoreGenerated],
              CASE
                WHEN ( [Project5].[C2] IS NULL ) THEN CAST(0 AS BIT)
                ELSE [Project5].[C2]
              END                                         AS [IsPrimaryKey]
            FROM   (SELECT
                      QUOTENAME(TABLE_SCHEMA)
                      + QUOTENAME(TABLE_NAME) [Id],
                      TABLE_SCHEMA            [SchemaName],
                      TABLE_NAME              [Name],
                      TABLE_TYPE
                    FROM   INFORMATION_SCHEMA.TABLES
                    WHERE  TABLE_TYPE IN ( 'BASE TABLE', 'VIEW' )) AS [Extent1]
                   INNER JOIN (SELECT
                                 [Extent2].[Id]                AS [Id],
                                 [Extent2].[Name]              AS [Name],
                                 [Extent2].[Ordinal]           AS [Ordinal],
                                 [Extent2].[IsNullable]        AS [IsNullable],
                                 [Extent2].[TypeName]          AS [TypeName],
                                 [Extent2].[MaxLength]         AS [MaxLength],
                                 [Extent2].[Precision]         AS [Precision],
                                 [Extent2].[Default],
                                 [Extent2].[DateTimePrecision] AS [DateTimePrecision],
                                 [Extent2].[Scale]             AS [Scale],
                                 [Extent2].[IsIdentity]        AS [IsIdentity],
                                 [Extent2].[IsStoreGenerated]  AS [IsStoreGenerated],
                                 0                             AS [C1],
                                 [Extent2].[ParentId]          AS [ParentId]
                               FROM   (SELECT
                                         QUOTENAME(c.TABLE_SCHEMA)
                                         + QUOTENAME(c.TABLE_NAME)
                                         + QUOTENAME(c.COLUMN_NAME)                                                                           [Id],
                                         QUOTENAME(c.TABLE_SCHEMA)
                                         + QUOTENAME(c.TABLE_NAME)                                                                            [ParentId],
                                         c.COLUMN_NAME                                                                                        [Name],
                                         c.ORDINAL_POSITION                                                                                   [Ordinal],
                                         CAST(CASE c.IS_NULLABLE
                                                WHEN 'YES' THEN 1
                                                WHEN 'NO' THEN 0
                                                ELSE 0
                                              END AS BIT)                                                                                     [IsNullable],
                                         CASE
                                           WHEN c.DATA_TYPE IN ( 'varchar', 'nvarchar', 'varbinary' )
                                                AND c.CHARACTER_MAXIMUM_LENGTH = -1 THEN c.DATA_TYPE + '(max)'
                                           ELSE c.DATA_TYPE
                                         END                                                                                                  AS [TypeName],
                                         c.CHARACTER_MAXIMUM_LENGTH                                                                           [MaxLength],
                                         CAST(c.NUMERIC_PRECISION AS INTEGER)                                                                 [Precision],
                                         CAST(c.DATETIME_PRECISION AS INTEGER)                                                                [DateTimePrecision],
                                         CAST(c.NUMERIC_SCALE AS INTEGER)                                                                     [Scale],
                                         c.COLLATION_CATALOG                                                                                  [CollationCatalog],
                                         c.COLLATION_SCHEMA                                                                                   [CollationSchema],
                                         c.COLLATION_NAME                                                                                     [CollationName],
                                         c.CHARACTER_SET_CATALOG                                                                              [CharacterSetCatalog],
                                         c.CHARACTER_SET_SCHEMA                                                                               [CharacterSetSchema],
                                         c.CHARACTER_SET_NAME                                                                                 [CharacterSetName],
                                         CAST(0 AS BIT)                                                                                       AS [IsMultiSet],
                                         CAST(COLUMNPROPERTY (OBJECT_ID(QUOTENAME(c.TABLE_SCHEMA) + '.'
                                                                        + QUOTENAME(c.TABLE_NAME)), c.COLUMN_NAME, 'IsIdentity') AS BIT)      AS [IsIdentity],
                                         CAST(COLUMNPROPERTY(OBJECT_ID(QUOTENAME(c.TABLE_SCHEMA) + '.'
                                                                       + QUOTENAME(c.TABLE_NAME)), c.COLUMN_NAME, 'IsComputed') | CASE
                                                                                                                                    WHEN c.DATA_TYPE = 'timestamp' THEN 1
                                                                                                                                    ELSE 0
                                                                                                                                  END AS BIT) AS [IsStoreGenerated],
                                         c.COLUMN_DEFAULT                                                                                     AS [Default]
                                       FROM   INFORMATION_SCHEMA.COLUMNS c
                                              INNER JOIN INFORMATION_SCHEMA.TABLES t
                                                      ON c.TABLE_CATALOG = t.TABLE_CATALOG
                                                     AND c.TABLE_SCHEMA = t.TABLE_SCHEMA
                                                     AND c.TABLE_NAME = t.TABLE_NAME
                                                     AND t.TABLE_TYPE IN ( 'BASE TABLE', 'VIEW' )) AS [Extent2]
                               UNION ALL
                               SELECT
                                 [Extent3].[Id]                AS [Id],
                                 [Extent3].[Name]              AS [Name],
                                 [Extent3].[Ordinal]           AS [Ordinal],
                                 [Extent3].[IsNullable]        AS [IsNullable],
                                 [Extent3].[TypeName]          AS [TypeName],
                                 [Extent3].[MaxLength]         AS [MaxLength],
                                 [Extent3].[Precision]         AS [Precision],
                                 [Extent3].[Default],
                                 [Extent3].[DateTimePrecision] AS [DateTimePrecision],
                                 [Extent3].[Scale]             AS [Scale],
                                 [Extent3].[IsIdentity]        AS [IsIdentity],
                                 [Extent3].[IsStoreGenerated]  AS [IsStoreGenerated],
                                 6                             AS [C1],
                                 [Extent3].[ParentId]          AS [ParentId]
                               FROM   (SELECT
                                         QUOTENAME(c.TABLE_SCHEMA)
                                         + QUOTENAME(c.TABLE_NAME)
                                         + QUOTENAME(c.COLUMN_NAME)                                                                           [Id],
                                         QUOTENAME(c.TABLE_SCHEMA)
                                         + QUOTENAME(c.TABLE_NAME)                                                                            [ParentId],
                                         c.COLUMN_NAME                                                                                        [Name],
                                         c.ORDINAL_POSITION                                                                                   [Ordinal],
                                         CAST(CASE c.IS_NULLABLE
                                                WHEN 'YES' THEN 1
                                                WHEN 'NO' THEN 0
                                                ELSE 0
                                              END AS BIT)                                                                                     [IsNullable],
                                         CASE
                                           WHEN c.DATA_TYPE IN ( 'varchar', 'nvarchar', 'varbinary' )
                                                AND c.CHARACTER_MAXIMUM_LENGTH = -1 THEN c.DATA_TYPE + '(max)'
                                           ELSE c.DATA_TYPE
                                         END                                                                                                  AS [TypeName],
                                         c.CHARACTER_MAXIMUM_LENGTH                                                                           [MaxLength],
                                         CAST(c.NUMERIC_PRECISION AS INTEGER)                                                                 [Precision],
                                         CAST(c.DATETIME_PRECISION AS INTEGER)                                                                AS [DateTimePrecision],
                                         CAST(c.NUMERIC_SCALE AS INTEGER)                                                                     [Scale],
                                         c.COLLATION_CATALOG                                                                                  [CollationCatalog],
                                         c.COLLATION_SCHEMA                                                                                   [CollationSchema],
                                         c.COLLATION_NAME                                                                                     [CollationName],
                                         c.CHARACTER_SET_CATALOG                                                                              [CharacterSetCatalog],
                                         c.CHARACTER_SET_SCHEMA                                                                               [CharacterSetSchema],
                                         c.CHARACTER_SET_NAME                                                                                 [CharacterSetName],
                                         CAST(0 AS BIT)                                                                                       AS [IsMultiSet],
                                         CAST(COLUMNPROPERTY(OBJECT_ID(QUOTENAME(c.TABLE_SCHEMA) + '.'
                                                                       + QUOTENAME(c.TABLE_NAME)), c.COLUMN_NAME, 'IsIdentity') AS BIT)       AS [IsIdentity],
                                         CAST(COLUMNPROPERTY(OBJECT_ID(QUOTENAME(c.TABLE_SCHEMA) + '.'
                                                                       + QUOTENAME(c.TABLE_NAME)), c.COLUMN_NAME, 'IsComputed') | CASE
                                                                                                                                    WHEN c.DATA_TYPE = 'timestamp' THEN 1
                                                                                                                                    ELSE 0
                                                                                                                                  END AS BIT) AS [IsStoreGenerated],
                                         c.COLUMN_DEFAULT                                                                                     [Default]
                                       FROM   INFORMATION_SCHEMA.COLUMNS c
                                              INNER JOIN INFORMATION_SCHEMA.VIEWS v
                                                      ON c.TABLE_CATALOG = v.TABLE_CATALOG
                                                     AND c.TABLE_SCHEMA = v.TABLE_SCHEMA
                                                     AND c.TABLE_NAME = v.TABLE_NAME
                                       WHERE  NOT
                                              (
                                                v.TABLE_SCHEMA = 'dbo'
                                                AND v.TABLE_NAME IN ( 'syssegments', 'sysconstraints' )
                                                AND SUBSTRING (CAST(SERVERPROPERTY('productversion') AS VARCHAR(20)), 1, 1) = 8
                                               )
                                      ) AS [Extent3]) AS [UnionAll1]
                           ON
                   (
                     0 = [UnionAll1].[C1]
                   )
                   AND
                   (
                   [Extent1].[Id] = [UnionAll1].[ParentId]
                   )
                   LEFT OUTER JOIN (SELECT
                                      [UnionAll2].[Id] AS [C1],
                                      CAST(1 AS BIT)   AS [C2]
                                    FROM   (SELECT
                                              QUOTENAME(tc.CONSTRAINT_SCHEMA)
                                              + QUOTENAME(tc.CONSTRAINT_NAME) [Id],
                                              QUOTENAME(tc.TABLE_SCHEMA)
                                              + QUOTENAME(tc.TABLE_NAME)      [ParentId],
                                              tc.CONSTRAINT_NAME              [Name],
                                              tc.CONSTRAINT_TYPE              [ConstraintType],
                                              CAST(CASE tc.IS_DEFERRABLE
                                                     WHEN 'NO' THEN 0
                                                     ELSE 1
                                                   END AS BIT)                [IsDeferrable],
                                              CAST(CASE tc.INITIALLY_DEFERRED
                                                     WHEN 'NO' THEN 0
                                                     ELSE 1
                                                   END AS BIT)                [IsInitiallyDeferred]
                                            FROM   INFORMATION_SCHEMA.TABLE_CONSTRAINTS tc
                                            WHERE  tc.TABLE_NAME IS NOT NULL) AS [Extent4]
                                           INNER JOIN (SELECT
                                                         7                        AS [C1],
                                                         [Extent5].[ConstraintId] AS [ConstraintId],
                                                         [Extent6].[Id]           AS [Id]
                                                       FROM   (SELECT
                                                                 QUOTENAME(CONSTRAINT_SCHEMA)
                                                                 + QUOTENAME(CONSTRAINT_NAME) [ConstraintId],
                                                                 QUOTENAME(TABLE_SCHEMA)
                                                                 + QUOTENAME(TABLE_NAME)
                                                                 + QUOTENAME(COLUMN_NAME)     [ColumnId]
                                                               FROM   INFORMATION_SCHEMA.KEY_COLUMN_USAGE) AS [Extent5]
                                                              INNER JOIN (SELECT
                                                                            QUOTENAME(c.TABLE_SCHEMA)
                                                                            + QUOTENAME(c.TABLE_NAME)
                                                                            + QUOTENAME(c.COLUMN_NAME)                                                                           [Id],
                                                                            QUOTENAME(c.TABLE_SCHEMA)
                                                                            + QUOTENAME(c.TABLE_NAME)                                                                            [ParentId],
                                                                            c.COLUMN_NAME                                                                                        [Name],
                                                                            c.ORDINAL_POSITION                                                                                   [Ordinal],
                                                                            CAST(CASE c.IS_NULLABLE
                                                                                   WHEN 'YES' THEN 1
                                                                                   WHEN 'NO' THEN 0
                                                                                   ELSE 0
                                                                                 END AS BIT)                                                                                     [IsNullable],
                                                                            CASE
                                                                              WHEN c.DATA_TYPE IN ( 'varchar', 'nvarchar', 'varbinary' )
                                                                                   AND c.CHARACTER_MAXIMUM_LENGTH = -1 THEN c.DATA_TYPE + '(max)'
                                                                              ELSE c.DATA_TYPE
                                                                            END                                                                                                  AS [TypeName],
                                                                            c.CHARACTER_MAXIMUM_LENGTH                                                                           [MaxLength],
                                                                            CAST(c.NUMERIC_PRECISION AS INTEGER)                                                                 [Precision],
                                                                            CAST(c.DATETIME_PRECISION AS INTEGER)                                                                [DateTimePrecision],
                                                                            CAST(c.NUMERIC_SCALE AS INTEGER)                                                                     [Scale],
                                                                            c.COLLATION_CATALOG                                                                                  [CollationCatalog],
                                                                            c.COLLATION_SCHEMA                                                                                   [CollationSchema],
                                                                            c.COLLATION_NAME                                                                                     [CollationName],
                                                                            c.CHARACTER_SET_CATALOG                                                                              [CharacterSetCatalog],
                                                                            c.CHARACTER_SET_SCHEMA                                                                               [CharacterSetSchema],
                                                                            c.CHARACTER_SET_NAME                                                                                 [CharacterSetName],
                                                                            CAST(0 AS BIT)                                                                                       AS [IsMultiSet],
                                                                            CAST(COLUMNPROPERTY(OBJECT_ID(QUOTENAME(c.TABLE_SCHEMA) + '.'
                                                                                                          + QUOTENAME(c.TABLE_NAME)), c.COLUMN_NAME, 'IsIdentity') AS BIT)       AS [IsIdentity],
                                                                            CAST(COLUMNPROPERTY(OBJECT_ID(QUOTENAME(c.TABLE_SCHEMA) + '.'
                                                                                                          + QUOTENAME(c.TABLE_NAME)), c.COLUMN_NAME, 'IsComputed') | CASE
                                                                                                                                                                       WHEN c.DATA_TYPE = 'timestamp' THEN 1
                                                                                                                                                                       ELSE 0
                                                                                                                                                                     END AS BIT) AS [IsStoreGenerated],
                                                                            c.COLUMN_DEFAULT                                                                                     AS [Default]
                                                                          FROM   INFORMATION_SCHEMA.COLUMNS c
                                                                                 INNER JOIN INFORMATION_SCHEMA.TABLES t
                                                                                         ON c.TABLE_CATALOG = t.TABLE_CATALOG
                                                                                        AND c.TABLE_SCHEMA = t.TABLE_SCHEMA
                                                                                        AND c.TABLE_NAME = t.TABLE_NAME
                                                                                        AND t.TABLE_TYPE IN ( 'BASE TABLE', 'VIEW' )) AS [Extent6]
                                                                      ON [Extent6].[Id] = [Extent5].[ColumnId]
                                                       UNION ALL
                                                       SELECT
                                                         11                       AS [C1],
                                                         [Extent7].[ConstraintId] AS [ConstraintId],
                                                         [Extent8].[Id]           AS [Id]
                                                       FROM   (SELECT
                                                                 CAST(NULL AS NVARCHAR (1))   [ConstraintId],
                                                                 CAST(NULL AS NVARCHAR (MAX)) [ColumnId]
                                                               WHERE  1 = 2) AS [Extent7]
                                                              INNER JOIN (SELECT
                                                                            QUOTENAME(c.TABLE_SCHEMA)
                                                                            + QUOTENAME(c.TABLE_NAME)
                                                                            + QUOTENAME(c.COLUMN_NAME)                                                                           [Id],
                                                                            QUOTENAME(c.TABLE_SCHEMA)
                                                                            + QUOTENAME(c.TABLE_NAME)                                                                            [ParentId],
                                                                            c.COLUMN_NAME                                                                                        [Name],
                                                                            c.ORDINAL_POSITION                                                                                   [Ordinal],
                                                                            CAST(CASE c.IS_NULLABLE
                                                                                   WHEN 'YES' THEN 1
                                                                                   WHEN 'NO' THEN 0
                                                                                   ELSE 0
                                                                                 END AS BIT)                                                                                     [IsNullable],
                                                                            CASE
                                                                              WHEN c.DATA_TYPE IN ( 'varchar', 'nvarchar', 'varbinary' )
                                                                                   AND c.CHARACTER_MAXIMUM_LENGTH = -1 THEN c.DATA_TYPE + '(max)'
                                                                              ELSE c.DATA_TYPE
                                                                            END                                                                                                  AS [TypeName],
                                                                            c.CHARACTER_MAXIMUM_LENGTH                                                                           [MaxLength],
                                                                            CAST(c.NUMERIC_PRECISION AS INTEGER)                                                                 [Precision],
                                                                            CAST(c.DATETIME_PRECISION AS INTEGER)                                                                AS [DateTimePrecision],
                                                                            CAST(c.NUMERIC_SCALE AS INTEGER)                                                                     [Scale],
                                                                            c.COLLATION_CATALOG                                                                                  [CollationCatalog],
                                                                            c.COLLATION_SCHEMA                                                                                   [CollationSchema],
                                                                            c.COLLATION_NAME                                                                                     [CollationName],
                                                                            c.CHARACTER_SET_CATALOG                                                                              [CharacterSetCatalog],
                                                                            c.CHARACTER_SET_SCHEMA                                                                               [CharacterSetSchema],
                                                                            c.CHARACTER_SET_NAME                                                                                 [CharacterSetName],
                                                                            CAST(0 AS BIT)                                                                                       AS [IsMultiSet],
                                                                            CAST(COLUMNPROPERTY(OBJECT_ID(QUOTENAME(c.TABLE_SCHEMA) + '.'
                                                                                                          + QUOTENAME(c.TABLE_NAME)), c.COLUMN_NAME, 'IsIdentity') AS BIT)       AS [IsIdentity],
                                                                            CAST(COLUMNPROPERTY(OBJECT_ID(QUOTENAME(c.TABLE_SCHEMA) + '.'
                                                                                                          + QUOTENAME(c.TABLE_NAME)), c.COLUMN_NAME, 'IsComputed') | CASE
                                                                                                                                                                       WHEN c.DATA_TYPE = 'timestamp' THEN 1
                                                                                                                                                                       ELSE 0
                                                                                                                                                                     END AS BIT) AS [IsStoreGenerated],
                                                                            c.COLUMN_DEFAULT                                                                                     [Default]
                                                                          FROM   INFORMATION_SCHEMA.COLUMNS c
                                                                                 INNER JOIN INFORMATION_SCHEMA.VIEWS v
                                                                                         ON c.TABLE_CATALOG = v.TABLE_CATALOG
                                                                                        AND c.TABLE_SCHEMA = v.TABLE_SCHEMA
                                                                                        AND c.TABLE_NAME = v.TABLE_NAME
                                                                          WHERE  NOT
                                                                                 (
                                                                                   v.TABLE_SCHEMA = 'dbo'
                                                                                   AND v.TABLE_NAME IN ( 'syssegments', 'sysconstraints' )
                                                                                   AND SUBSTRING(CAST(SERVERPROPERTY('productversion') AS VARCHAR(20)), 1, 1) = 8
                                                                                  )
                                                                         ) AS [Extent8]
                                                                      ON [Extent8].[Id] = [Extent7].[ColumnId]) AS [UnionAll2]
                                                   ON
                                           (
                                             7 = [UnionAll2].[C1]
                                            )
                                           AND
                                           (
                                           [Extent4].[Id] = [UnionAll2].[ConstraintId]
                                           )
                                    WHERE  [Extent4].[ConstraintType] = N'PRIMARY KEY') AS [Project5]
                                ON [UnionAll1].[Id] = [Project5].[C1]
            WHERE  NOT
                   (
                     [Extent1].[Name] IN ( 'EdmMetadata', '__MigrationHistory' )
                    ) 
            ";

        #endregion

        #region . Foreign Keys Schema Sql .

        private const string ForeignKeysSchemaSql = @"
            SELECT
              Object_schema_name(f.parent_object_id)                     AS TableSchema,
              Object_name(f.parent_object_id)                            AS TableName,
              Col_name(fc.parent_object_id, fc.parent_column_id)         AS ColumnName,
              Object_schema_name(f.referenced_object_id)                 AS ReferenceTableSchema,
              Object_name (f.referenced_object_id)                       AS ReferenceTableName,
              Col_name(fc.referenced_object_id, fc.referenced_column_id) AS ReferenceColumnName,
              f.name                                                     AS ForeignKey
            FROM   sys.foreign_keys AS f
                   INNER JOIN sys.foreign_key_columns AS fc
                           ON f.OBJECT_ID = fc.constraint_object_id
            --WHERE  Object_name(f.parent_object_id) = 'Media'
            --       OR Object_name (f.referenced_object_id) = 'Media'
            ORDER  BY
              TableName,
              ColumnName
            ";

        #endregion

        public SqlServerSchemaReader(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public IReadOnlyList<ISchemaTable> Read()
        {
            using (var connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                return this.InternalRead(connection);
            }
        }

        private IReadOnlyList<ISchemaTable> InternalRead(SqlConnection connection)
        {
            var foreignKeys = this.LoadForeignKeySchemas(connection).ToList();

            var tables = new List<SqlSchemaTable>();

            using(var cmd = connection.CreateCommand())
            {
                cmd.CommandText = TablesSchemaSql;

                SqlSchemaTable table = null;

                using(var reader = cmd.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        var tableName = reader["TableName"].ToString().Trim();
                        var schemaName = reader["SchemaName"].ToString().Trim();

                        //create table if one was not created before, and add the foreign keys plus the related keys
                        if(table == null || !table.Schema.Equals(schemaName) || !table.Name.Equals(tableName))
                        {
                            table = new SqlSchemaTable(
                                tableName,
                                schemaName,
                                bool.Parse(reader["IsView"].ToString()));

                            table.ForeignKeys.AddRange(
                                foreignKeys.Where(
                                    key =>
                                    key.ForeignKeySchema.Equals(table.Schema) && key.ForeignKeyTableName.Equals(table.Name) ||
                                    key.PrimaryKeySchema.Equals(table.Schema) && key.PrimaryKeyTableName.Equals(table.Name)));

                            tables.Add(table);
                        }

                        var columnSchema = new SqlSchemaColumn(
                            reader["ColumnName"].ToString().Trim(),
                            reader["TypeName"].ToString().Trim(),
                            this.GetPropertyTypeFromSqlType(reader["TypeName"].ToString().Trim()),
                            (int) reader["DateTimePrecision"],
                            reader["DefaultValue"].ToString().Trim(),
                            (int) reader["MaxLength"],
                            (int) reader["Precision"],
                            (int) reader["Scale"],
                            (int) reader["Ordinal"],
                            reader["IsIdentity"].ToString().Trim().ToLower() == "true",
                            reader["IsNullable"].ToString().Trim().ToLower() == "true",
                            reader["IsPrimaryKey"].ToString().Trim().ToLower() == "true",
                            reader["IsStoreGenerated"].ToString().Trim().ToLower() == "true");

                        table.Columns.Add(columnSchema);
                    }
                }
            }

            return tables.AsReadOnly();
        }

        #region . Helpers .

        private IEnumerable<ISchemaForeignKey> LoadForeignKeySchemas(SqlConnection connection)
        {
            using(var cmd = connection.CreateCommand())
            {
                cmd.CommandText = ForeignKeysSchemaSql;

                using(var reader = cmd.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        yield return new SqlSchemaForeignKey(
                            reader["TableSchema"].ToString(),
                            reader["TableName"].ToString(),
                            reader["ColumnName"].ToString(),
                            reader["ReferenceTableSchema"].ToString(),
                            reader["ReferenceTableName"].ToString(),
                            reader["ReferenceColumnName"].ToString(),
                            reader["ForeignKey"].ToString());
                    }
                }
            }
        }

        private string GetPropertyTypeFromSqlType(string sqlType)
        {
            var sysTypes = new Dictionary<string, string>
            {
                {"bigint", "long"},
                {"smallint", "short"},
                {"int", "int"},
                {"uniqueidentifier", "Guid"},
                {"smalldatetime", "DateTime"},
                {"datetime", "DateTime"},
                {"datetime2", "DateTime"},
                {"date", "DateTime"},
                {"datetimeoffset", "DateTimeOffset"},
                {"time", "TimeSpan"},
                {"float", "double"},
                {"real", "float"},
                {"numeric", "decimal"},
                {"smallmoney", "decimal"},
                {"decimal", "decimal"},
                {"money", "decimal"},
                {"tinyint", "byte"},
                {"bit", "bool"},
                {"binary", "byte[]"},
                {"varbinary", "byte[]"},
                {"varbinary(max)", "byte[]"},
                {"geography", "DbGeography"},
                {"geometry", "DbGeometry"},
            };

            var sysType = "";

            sysTypes.TryGetValue(sqlType, out sysType);

            if (sysType.IsBlank())
            {
                sysType = "string";
            }

            return sysType;
        }

        #endregion
    }
}