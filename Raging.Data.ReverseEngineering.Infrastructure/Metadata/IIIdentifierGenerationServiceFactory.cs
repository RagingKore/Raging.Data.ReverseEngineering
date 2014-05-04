using Raging.Data.ReverseEngineering.Configuration;
using Raging.Data.ReverseEngineering.Infrastructure.Pluralization;

namespace Raging.Data.ReverseEngineering.Infrastructure.Metadata
{
    public interface IIIdentifierGenerationServiceFactory
    {
        IIdentifierGenerationService Create(IPluralizationService pluralizationService, IReverseEngineeringConfiguration configuration);
    }
}