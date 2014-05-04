using Raging.Data.ReverseEngineering.Configuration;
using Raging.Data.ReverseEngineering.Infrastructure.Pluralization;

namespace Raging.Data.ReverseEngineering.Infrastructure.Metadata
{
    public class IdentifierGenerationServiceFactory : IIIdentifierGenerationServiceFactory
    {
        public IIdentifierGenerationService Create(IPluralizationService pluralizationService,IReverseEngineeringConfiguration configuration)
        {
            return new IdentifierGenerationService(pluralizationService, configuration);
        }
    }
}