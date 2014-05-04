using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;

namespace Raging.Data.ReverseEngineering.Infrastructure.Pluralization
{
    public class EnglishPluralizationService : IPluralizationService
    {
        private readonly PluralizationService service = PluralizationService.CreateService(CultureInfo.GetCultureInfo("en"));

        public CultureInfo Culture
        {
            get { return this.service.Culture; }
        }

        public bool IsPlural(string word)
        {
            return this.service.IsPlural(word);
        }

        public bool IsSingular(string word)
        {
            return this.service.IsSingular(word);
        }

        public string Pluralize(string word)
        {
            return this.service.Pluralize(word);
        }

        public string Singularize(string word)
        {
            return this.service.Singularize(word);
        }
    }
}