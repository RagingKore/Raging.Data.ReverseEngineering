using System.Globalization;

namespace Raging.Data.ReverseEngineering.Infrastructure.Pluralization
{
    public interface IPluralizationService
    {
        CultureInfo Culture { get; }
        bool IsPlural(string word);
        bool IsSingular(string word);
        string Pluralize(string word);
        string Singularize(string word);
    }
}