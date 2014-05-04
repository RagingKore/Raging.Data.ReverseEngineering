using System.Collections.Generic;

namespace Raging.Data.ReverseEngineering.Configuration
{
    public class ReverseEngineeringConfiguration : IReverseEngineeringConfiguration
    {
        /// <summary>
        /// Namespace for the generated POCO's.
        /// </summary>
        public string PocoNamespace { get; set; }

        /// <summary>
        /// Namespace for the generated database context, units of work and mappings.
        /// </summary>
        public string DataNamespace { get; set; }

        /// <summary>
        /// Namespace for the generated views.
        /// </summary>
        public string ViewsNamespace { get; set; }

        /// <summary>
        /// Defines if views will be generated.
        /// </summary>
        public bool IncludeViews { get; set; }
  
        /// <summary>
        /// Defines if the template generates files for each item.
        /// </summary>
        public bool GenerateFiles { get; set; }

        /// <summary>
        /// Defines if the POCO's and views will be written such that each word or abbreviation begins with a capital letter.
        /// Underscores and dashes will also be removed.
        /// </summary>
        public bool UsePascalCase  { get; set; }

        /// <summary>
        /// Defines if the POCO's and views will be written such that each word is pluralized.
        /// </summary>
        public bool Pluralize { get; set; }

        /// <summary>
        /// Allows for any given table, view or column name to be overridden by a new name.
        /// </summary>
        public IEnumerable<GlobalNameOverride> GlobalNamingOverrides { get; set; }

        /// <summary>  
        /// Allows removing tables from the generated model
        /// </summary>
        public string TableBlackListFilter { get; set; }

        /// <summary>  
        /// Allows specifying the tables that will be generated.
        /// </summary>
        public string TableWhiteListFilter { get; set; }
    }
}