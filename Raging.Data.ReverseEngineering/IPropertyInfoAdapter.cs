namespace Raging.Data.ReverseEngineering
{
    public interface IPropertyInfoAdapter
    {
        string GetPropertyName();
        string GetMappingText();
        string GetColumnMapppingText();
        string GetConstructorInitializationText();
        string GetPropertyText();
    }
}