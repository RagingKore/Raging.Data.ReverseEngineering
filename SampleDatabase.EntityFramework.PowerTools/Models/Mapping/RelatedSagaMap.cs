using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SampleDatabase.EntityFramework.PowerTools.Models.Mapping
{
    public class RelatedSagaMap : EntityTypeConfiguration<RelatedSaga>
    {
        public RelatedSagaMap()
        {
            // Primary Key
            this.HasKey(t => new { t.PrimarySagaId, t.SecondarySagaId });

            // Properties
            this.Property(t => t.PrimarySagaId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.SecondarySagaId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("RelatedSaga");
            this.Property(t => t.PrimarySagaId).HasColumnName("PrimarySagaId");
            this.Property(t => t.SecondarySagaId).HasColumnName("SecondarySagaId");
            this.Property(t => t.CreatedOn).HasColumnName("CreatedOn");

            // Relationships
            this.HasRequired(t => t.Saga)
                .WithMany(t => t.RelatedSagas)
                .HasForeignKey(d => d.PrimarySagaId);
            this.HasRequired(t => t.Saga1)
                .WithMany(t => t.RelatedSagas1)
                .HasForeignKey(d => d.SecondarySagaId);

        }
    }
}
