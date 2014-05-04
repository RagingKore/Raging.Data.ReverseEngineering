using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Raging.Data.ReverseEngineering.Configuration;
using Raging.Data.ReverseEngineering.EntityFramework.Adapters;
using Raging.Data.ReverseEngineering.Infrastructure;
using Raging.Data.ReverseEngineering.Infrastructure.Metadata;
using Raging.Data.ReverseEngineering.Infrastructure.Pluralization;
using Raging.Data.Schema;
using Raging.Toolbox.Extensions;

namespace Raging.Data.ReverseEngineering.EntityFramework.Providers
{
    public class DomainModelProvider : IDomainModelProvider
    {
        private readonly IReverseEngineeringConfiguration configuration;
        private readonly ISchemaReader schemaReader;
        private readonly IEntityInfoAdapterFactory entityInfoAdapterFactory;
        private readonly IPropertyInfoAdapterFactory propertyInfoAdapterFactory;      
        private readonly INavigationPropertyInfoAdapterFactory navigationPropertyInfoAdapterFactory;
        private readonly IIdentifierGenerationService identifierGenerationService;

        public DomainModelProvider(
            ISchemaReader schemaReader, 
            IEntityInfoAdapterFactory entityInfoAdapterFactory, 
            IPropertyInfoAdapterFactory propertyInfoAdapterFactory, 
            INavigationPropertyInfoAdapterFactory navigationPropertyInfoAdapterFactory, 
            IIIdentifierGenerationServiceFactory identifierGenerationServiceFactory,
            IPluralizationService pluralizationService,
            IReverseEngineeringConfiguration configuration)
        {
            
            this.schemaReader                         = schemaReader;
            this.entityInfoAdapterFactory             = entityInfoAdapterFactory;
            this.navigationPropertyInfoAdapterFactory = navigationPropertyInfoAdapterFactory;
            this.propertyInfoAdapterFactory           = propertyInfoAdapterFactory;
            this.configuration                        = configuration;
            this.identifierGenerationService          = identifierGenerationServiceFactory.Create(pluralizationService, configuration);
        }

        #region . IContextProvider .

        public IEnumerable<EntityInfo> GetInformation()
        {
            var blackList = this.configuration.TableBlackListFilter.IsNotBlank() ? this.configuration.TableBlackListFilter.Split(';') : new string[] {};
            var whiteList = this.configuration.TableWhiteListFilter.IsNotBlank() ? this.configuration.TableWhiteListFilter.Split(';') : new string[] {};

            var tables = this.schemaReader
                .Read()
                .AsParallel()
                .Where(table => table.FullName.LikeAny(whiteList) || whiteList.Length == 0)
                .Where(table => table.FullName.NotLikeAll(blackList));
 
            var info =
                from table in tables
                let adapter = this.entityInfoAdapterFactory.Create(table, this.identifierGenerationService) 
                orderby table.Name
                select new EntityInfo(
                    tableName             : table.Name, 
                    modelName             : adapter.GetEntityName(),
                    dbSetText             : adapter.GetDbSetText(),
                    tableMappingText      : adapter.GetTableMappingText(), 
                    primaryKeyMappingText : adapter.GetPrimaryKeyMappingText(), 
                    properties            : this.GetPropertiesInfo(table), 
                    relationships         : this.GetNavigationPropertiesInfo(table));

            return info;
        }

        #endregion

        #region . Helpers .

        private List<PropertyInfo> GetPropertiesInfo(ISchemaTable table)
        {
            var info = 
                from column in table.Columns
                let adapter = this.propertyInfoAdapterFactory.Create(column, table, this.identifierGenerationService)
                select new PropertyInfo(
                    columnName                    : column.Name,
                    propertyText                  : adapter.GetPropertyText(),
                    constructorInitializationText : adapter.GetConstructorInitializationText(), 
                    mappingText                   : adapter.GetMappingText(), 
                    columnMappingText             : adapter.GetColumnMapppingText());

            return info.ToList();
        }

        private List<NavigationPropertyInfo> GetNavigationPropertiesInfo(ISchemaTable table)
        {
            var info =
                from foreignKey in table.ForeignKeys
                let adapter = this.navigationPropertyInfoAdapterFactory.Create(foreignKey, table, this.identifierGenerationService)
                select new NavigationPropertyInfo(
                    relationshipName              : foreignKey.ConstraintName,
                    propertyText                  : adapter.GetPropertyText(),
                    constructorInitializationText : adapter.GetConstructorInitializationText(),
                    mappingText                   : adapter.GetMapppingText());

            return info.ToList();
        }

        #endregion
    }
}