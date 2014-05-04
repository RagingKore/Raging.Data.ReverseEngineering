namespace Raging.Data.ReverseEngineering
{
    public interface INavigationPropertyInfoAdapter
    {
        string GetPropertyText();
        string GetConstructorInitializationText();
        string GetMapppingText();
    }
}