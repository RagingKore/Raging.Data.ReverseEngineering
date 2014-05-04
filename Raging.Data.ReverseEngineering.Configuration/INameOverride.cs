namespace Raging.Data.ReverseEngineering.Configuration
{
    public interface INameOverride
    {
        /// <summary>
        /// The original name.
        /// </summary>
        string From { get; set; }

        /// <summary>
        /// The new name.
        /// </summary>
        string To { get; set; }
    }
}