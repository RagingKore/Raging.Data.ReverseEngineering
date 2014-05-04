using System.Collections.Generic;

namespace Raging.Data.ReverseEngineering.Configuration
{
    public interface IReverseEngineeringConfiguration
    {
        /// <summary>
        /// Namespace for the generated POCO's.
        /// </summary>
        string PocoNamespace { get; set; }

        /// <summary>
        /// Namespace for the generated database context, units of work and mappings.
        /// </summary>
        string DataNamespace { get; set; }

        /// <summary>
        /// Namespace for the generated views.
        /// </summary>
        string ViewsNamespace { get; set; }

        /// <summary>
        /// Defines if views will be generated.
        /// </summary>
        bool IncludeViews { get; set; }

        /// <summary>
        /// Defines if the template generates files for each item.
        /// </summary>
        bool GenerateFiles { get; set; }

        /// <summary>
        /// Defines if the POCO's and views will be written such that each word or abbreviation begins with a capital letter.
        /// </summary>
        bool UsePascalCase { get; set; }

        /// <summary>
        /// Defines if the POCO's and views will be written such that each word is pluralized.
        /// </summary>
        bool Pluralize { get; set; }

        /// <summary>
        /// Allows for any given table, view or column name to be overridden by a new name.
        /// </summary>
        IEnumerable<GlobalNameOverride> GlobalNamingOverrides { get; set; }

        /// <summary>  
        /// Allows removing tables from the generated model
        /// </summary>
        string TableBlackListFilter { get; set; }

        /// <summary>  
        /// Allows specifying the tables that will be generated.
        /// </summary>
        string TableWhiteListFilter { get; set; }
    }
}