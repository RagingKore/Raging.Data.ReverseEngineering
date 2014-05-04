namespace Raging.Data.ReverseEngineering
{
    public struct NavigationPropertyInfo
    {
        public NavigationPropertyInfo(
            string relationshipName, 
            string propertyText, 
            string constructorInitializationText, 
            string mappingText) : this()
        {
            this.RelationshipName              = relationshipName;
            this.PropertyText                  = propertyText;
            this.ConstructorInitializationText = constructorInitializationText;
            this.MappingText                   = mappingText;
        }

        public string RelationshipName { get; private set; }

        /// <summary>
        /// public virtual ICollection<Singular> Models { get; set; }
        /// public virtual ModelType Singular { get; set; }
        /// </summary>
        public string PropertyText { get; private set; }

        /// <summary>
        /// this.Models = new List<Singular>();
        /// </summary>
        public string ConstructorInitializationText { get; private set; }

        /// <summary>
        ///  this.HasRequired(t => t.User)
        ///      .WithMany(t => t.Singular)
        ///      .HasForeignKey(d => d.UserId);
        /// </summary>
        public string MappingText { get; private set; }

        public override string ToString()
        {
            return this.RelationshipName;
        }
    }
}