using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SampleDatabase.EntityFramework.PowerTools.Models.Mapping
{
    public class SagaMessageMap : EntityTypeConfiguration<SagaMessage>
    {
        public SagaMessageMap()
        {
            // Primary Key
            this.HasKey(t => t.MessageId);

            // Properties
            this.Property(t => t.MessageId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("SagaMessage");
            this.Property(t => t.MessageId).HasColumnName("MessageId");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.State).HasColumnName("State");
            this.Property(t => t.Data).HasColumnName("Data");
            this.Property(t => t.SagaId).HasColumnName("SagaId");
            this.Property(t => t.CreatedOn).HasColumnName("CreatedOn");
            this.Property(t => t.ModifiedOn).HasColumnName("ModifiedOn");

            // Relationships
            this.HasRequired(t => t.Saga)
                .WithMany(t => t.SagaMessages)
                .HasForeignKey(d => d.SagaId);

        }
    }
}
