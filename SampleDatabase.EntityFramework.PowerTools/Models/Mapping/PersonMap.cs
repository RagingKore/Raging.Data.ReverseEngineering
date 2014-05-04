using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SampleDatabase.EntityFramework.PowerTools.Models.Mapping
{
    public class PersonMap : EntityTypeConfiguration<Person>
    {
        public PersonMap()
        {
            // Primary Key
            this.HasKey(t => t.PersonId);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(150);

            // Table & Column Mappings
            this.ToTable("Person");
            this.Property(t => t.PersonId).HasColumnName("PersonId");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.FatherId).HasColumnName("FatherId");
            this.Property(t => t.ComputedColumn).HasColumnName("ComputedColumn");
            this.Property(t => t.Age).HasColumnName("Age");
            this.Property(t => t.BirthDate).HasColumnName("BirthDate");
            this.Property(t => t.InternalId).HasColumnName("InternalId");
            this.Property(t => t.ExternalId).HasColumnName("ExternalId");
            this.Property(t => t.NonNullableDate).HasColumnName("NonNullableDate");
            this.Property(t => t.DecimalNumber).HasColumnName("DecimalNumber");
            this.Property(t => t.FloatingNumber).HasColumnName("FloatingNumber");
            this.Property(t => t.Money).HasColumnName("Money");
            this.Property(t => t.GeographyValue).HasColumnName("GeographyValue");
            this.Property(t => t.GeometryValue).HasColumnName("GeometryValue");

            // Relationships
            this.HasOptional(t => t.Person2)
                .WithMany(t => t.Person1)
                .HasForeignKey(d => d.FatherId);

        }
    }
}
