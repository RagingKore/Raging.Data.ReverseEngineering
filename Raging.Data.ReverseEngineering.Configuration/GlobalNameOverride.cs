namespace Raging.Data.ReverseEngineering.Configuration
{
    public class GlobalNameOverride : INameOverride
    {
        public GlobalNameOverride(string from, string to)
        {
            this.From = from;
            this.To   = to;
        }

        /// <summary>
        /// The original name.
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// The new name.
        /// </summary>
        public string To { get; set; }
    }
}