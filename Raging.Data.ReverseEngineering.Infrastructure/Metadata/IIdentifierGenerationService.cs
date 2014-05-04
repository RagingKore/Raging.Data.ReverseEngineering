using Raging.Data.ReverseEngineering.Infrastructure.Pluralization;

namespace Raging.Data.ReverseEngineering.Infrastructure.Metadata
{
    public interface IIdentifierGenerationService
    {
        string Generate(string name, NounForms form, string fullname = "");
    }
}