namespace Raging.Data.ReverseEngineering
{
    public interface IEntityInfoAdapter
    {
        string GetEntityName();
        string GetDbSetText();
        string GetTableMappingText();
        string GetPrimaryKeyMappingText();
    }
}