using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SampleDatabase.EntityFramework.PowerTools.Models.Mapping
{
    public class SagaMap : EntityTypeConfiguration<Saga>
    {
        public SagaMap()
        {
            // Primary Key
            this.HasKey(t => t.SagaId);

            // Properties
            this.Property(t => t.SagaId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("Saga");
            this.Property(t => t.SagaId).HasColumnName("SagaId");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.State).HasColumnName("State");
        }
    }
}
