using System.CodeDom.Compiler;
using System.Collections.Concurrent;
using System.Linq;
using Raging.Data.ReverseEngineering.Configuration;
using Raging.Data.ReverseEngineering.Infrastructure.Pluralization;
using Raging.Toolbox.Extensions;
using Raging.Toolbox.Helpers;

namespace Raging.Data.ReverseEngineering.Infrastructure.Metadata
{
    public class IdentifierGenerationService : IIdentifierGenerationService
    {
        private readonly IPluralizationService pluralizationService;
        private readonly IReverseEngineeringConfiguration configuration;

        private readonly ConcurrentDictionary<string, string> names = new ConcurrentDictionary<string, string>();

        public IdentifierGenerationService(IPluralizationService pluralizationService, IReverseEngineeringConfiguration configuration)
        {
            this.pluralizationService = pluralizationService;
            this.configuration = configuration;
        }

        public string Generate(string name, NounForms form, string fullname = "")
        {
            var key = "{0}.{1}".FormatWith(form, fullname.IsBlank() ? name : fullname).ToBase64();

            //check if the identifier was already generated
            if (this.names.ContainsKey(key))
            {
                //return generated identifier
                return this.names[key];
            }

            //check overrides from configuration
            if (this.configuration.GlobalNamingOverrides != null)
            {
                //get first override for a specific match and if not found, try a global match
                var nameOverride = this.configuration.GlobalNamingOverrides.SingleOrDefault(o => o.From.Like(fullname))
                                   ?? this.configuration.GlobalNamingOverrides.SingleOrDefault(o => o.From.Like(name));

                if (nameOverride != null)
                {
                    this.names.TryAdd(key, nameOverride.To);

                    return nameOverride.To;
                }
            }

            //if requested, identifier shall be removed of all underscores and dashes, and each word or abbreviation will be capitalized.
            if (this.configuration.UsePascalCase)
            {
                name = name.Pascalize();
            }

            switch (form)
            {
                case NounForms.Singular:
                    //if requested, name will be converted to singular if plural
                    if (this.configuration.Pluralize)
                    {
                        name = this.pluralizationService.Singularize(name);
                    }
                    break;

                case NounForms.Plural:
                    //if requested, name will be converted to plural if singular
                    if (this.configuration.Pluralize)
                    {
                        name = this.pluralizationService.Pluralize(name);
                    }
                    break;
            }

            //guarantees that a name will not clash with c# reserved words
            name = CodeDomProvider.CreateProvider("C#").CreateValidIdentifier(name);

            //store generated identifier
            this.names.TryAdd(key, name);

            //return generated identifier
            return name;
        }
    }
}