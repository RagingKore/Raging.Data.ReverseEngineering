
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Spatial;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.ComponentModel.DataAnnotations.Schema;
using SampleDatabase.EntityFramework.Models;

namespace SampleDatabase.EntityFramework
{
	public interface IUnitOfWork : IDisposable
    {
        void SaveChanges();
    }

	public partial class DataContext : DbContext
    {
        static DataContext()
        {
            Database.SetInitializer<DataContext>(null);
        }

        public DataContext() : base("Name=DataContext") {}

		public DbSet<Person> People { get; set; }
		public DbSet<RelatedSaga> RelatedSagas { get; set; }
		public DbSet<Saga> Sagas { get; set; }
		public DbSet<SagaMessage> SagaMessages { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.AddFromAssembly(Assembly.GetExecutingAssembly());
		}

	}
}

namespace SampleDatabase.EntityFramework.Models
{
    public partial class Person
    {
        public Person() 
		{
			this.Name = @"John Doe";
			this.Age = 25;
			this.BirthDate = DateTime.Parse("2014-05-01 15:40:47.713");
			this.IsCoolDude = true;
			this.ExternalId = Guid.Parse("37993BAC-E895-4EFA-B726-2FD1405ADC53");
			this.NonNullableDate = DateTime.UtcNow;
			this.DecimalNumber = 67.8m;
			this.FloatingNumber = 456.786778d;
			this.Money = 20m;
			this.Fathers = new List<Person>();
		}

		//Properties
		public int PersonId { get; set; } // PersonId (Primary key)
		public string Name { get; set; } // Name
		public int? FatherId { get; set; } // FatherId
		public int? ComputedColumn { get; private set; } // ComputedColumn
		public short Age { get; set; } // Age
		public DateTime BirthDate { get; set; } // BirthDate
		public DbGeography GeographyValue { get; set; } // GeographyValue
		public DbGeometry GeometryValue { get; set; } // GeometryValue
		public bool? IsCoolDude { get; set; } // IsCoolDude
		public Guid InternalId { get; set; } // InternalId
		public Guid ExternalId { get; set; } // ExternalId
		public DateTime NonNullableDate { get; set; } // NonNullableDate
		public decimal DecimalNumber { get; set; } // DecimalNumber
		public double FloatingNumber { get; set; } // FloatingNumber
		public decimal Money { get; set; } // Money
				
		//Navigation properties
		public virtual ICollection<Person> Fathers { get; set; } // Person.FK_Person_Father
		public virtual Person Father { get; set; } // FK_Person_Father
	}

    public partial class RelatedSaga
    {
        public RelatedSaga() 
		{
		}

		//Properties
		public long PrimarySagaId { get; set; } // PrimarySagaId (Primary key)
		public long SecondarySagaId { get; set; } // SecondarySagaId (Primary key)
		public DateTime CreatedOn { get; set; } // CreatedOn
				
		//Navigation properties
		public virtual Saga PrimarySaga { get; set; } // FK_RelatedSaga_PrimarySaga
		public virtual Saga SecondarySaga { get; set; } // FK_RelatedSaga_SecondarySaga
	}

    public partial class Saga
    {
        public Saga() 
		{
			this.PrimarySagas = new List<RelatedSaga>();
			this.SecondarySagas = new List<RelatedSaga>();
			this.SagaMessages = new List<SagaMessage>();
		}

		//Properties
		public long SagaId { get; set; } // SagaId (Primary key)
		public int? Status { get; set; } // Status
		public byte[] State { get; set; } // State
				
		//Navigation properties
		public virtual ICollection<RelatedSaga> PrimarySagas { get; set; } // RelatedSaga.FK_RelatedSaga_PrimarySaga
		public virtual ICollection<RelatedSaga> SecondarySagas { get; set; } // RelatedSaga.FK_RelatedSaga_SecondarySaga
		public virtual ICollection<SagaMessage> SagaMessages { get; set; } // SagaMessage.FK_SagaMessage_Saga
	}

    public partial class SagaMessage
    {
        public SagaMessage() 
		{
			this.CreatedOn = DateTime.UtcNow;
			this.ModifiedOn = DateTime.UtcNow;
		}

		//Properties
		public long MessageId { get; set; } // MessageId (Primary key)
		public string Name { get; set; } // Name
		public int? Status { get; set; } // Status
		public byte[] State { get; set; } // State
		public byte[] Datum { get; set; } // Data
		public long SagaId { get; set; } // SagaId
		public DateTime CreatedOn { get; set; } // CreatedOn
		public DateTime ModifiedOn { get; set; } // ModifiedOn
				
		//Navigation properties
		public virtual Saga Saga { get; set; } // FK_SagaMessage_Saga
	}

}

namespace SampleDatabase.EntityFramework.Models.Mapping
{
    public class PersonMap : EntityTypeConfiguration<Person>
    {
        public PersonMap()
        {
            // Primary Key
			this.HasKey(t => t.PersonId);

			// Properties
			this.Property(t => t.PersonId)
				.IsRequired()
				.HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

			this.Property(t => t.Name)
				.IsRequired()
				.HasMaxLength(150);

			this.Property(t => t.FatherId)
				.IsOptional();

			this.Property(t => t.ComputedColumn)
				.IsOptional()
				.HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

			this.Property(t => t.Age)
				.IsRequired();

			this.Property(t => t.BirthDate)
				.IsRequired();

			this.Property(t => t.GeographyValue)
				.IsOptional();

			this.Property(t => t.GeometryValue)
				.IsOptional();

			this.Property(t => t.IsCoolDude)
				.IsOptional();

			this.Property(t => t.InternalId)
				.IsRequired();

			this.Property(t => t.ExternalId)
				.IsRequired();

			this.Property(t => t.NonNullableDate)
				.IsRequired();

			this.Property(t => t.DecimalNumber)
				.IsRequired();

			this.Property(t => t.FloatingNumber)
				.IsRequired();

			this.Property(t => t.Money)
				.IsRequired()
				.HasPrecision(19,4);

			// Relationships
			this.HasOptional(a => a.Father)
				.WithMany(b => b.Fathers)
				.HasForeignKey(c => c.FatherId); // FK_Person_Father

            // Table & Column Mappings
			this.ToTable("Person");
			this.Property(t => t.PersonId).HasColumnName("PersonId");
			this.Property(t => t.Name).HasColumnName("Name");
			this.Property(t => t.FatherId).HasColumnName("FatherId");
			this.Property(t => t.ComputedColumn).HasColumnName("ComputedColumn");
			this.Property(t => t.Age).HasColumnName("Age");
			this.Property(t => t.BirthDate).HasColumnName("BirthDate");
			this.Property(t => t.GeographyValue).HasColumnName("GeographyValue");
			this.Property(t => t.GeometryValue).HasColumnName("GeometryValue");
			this.Property(t => t.IsCoolDude).HasColumnName("IsCoolDude");
			this.Property(t => t.InternalId).HasColumnName("InternalId");
			this.Property(t => t.ExternalId).HasColumnName("ExternalId");
			this.Property(t => t.NonNullableDate).HasColumnName("NonNullableDate");
			this.Property(t => t.DecimalNumber).HasColumnName("DecimalNumber");
			this.Property(t => t.FloatingNumber).HasColumnName("FloatingNumber");
			this.Property(t => t.Money).HasColumnName("Money");
        }
    }

    public class RelatedSagaMap : EntityTypeConfiguration<RelatedSaga>
    {
        public RelatedSagaMap()
        {
            // Primary Key
			this.HasKey(t => new {t.PrimarySagaId,t.SecondarySagaId});

			// Properties
			this.Property(t => t.PrimarySagaId)
				.IsRequired()
				.HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

			this.Property(t => t.SecondarySagaId)
				.IsRequired()
				.HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

			this.Property(t => t.CreatedOn)
				.IsRequired();

			// Relationships
			this.HasRequired(a => a.PrimarySaga)
				.WithMany(b => b.PrimarySagas)
				.HasForeignKey(c => c.PrimarySagaId); // FK_RelatedSaga_PrimarySaga

			this.HasRequired(a => a.SecondarySaga)
				.WithMany(b => b.SecondarySagas)
				.HasForeignKey(c => c.SecondarySagaId); // FK_RelatedSaga_SecondarySaga

            // Table & Column Mappings
			this.ToTable("RelatedSaga");
			this.Property(t => t.PrimarySagaId).HasColumnName("PrimarySagaId");
			this.Property(t => t.SecondarySagaId).HasColumnName("SecondarySagaId");
			this.Property(t => t.CreatedOn).HasColumnName("CreatedOn");
        }
    }

    public class SagaMap : EntityTypeConfiguration<Saga>
    {
        public SagaMap()
        {
            // Primary Key
			this.HasKey(t => t.SagaId);

			// Properties
			this.Property(t => t.SagaId)
				.IsRequired()
				.HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

			this.Property(t => t.Status)
				.IsOptional();

			this.Property(t => t.State)
				.IsOptional();

            // Table & Column Mappings
			this.ToTable("Saga");
			this.Property(t => t.SagaId).HasColumnName("SagaId");
			this.Property(t => t.Status).HasColumnName("Status");
			this.Property(t => t.State).HasColumnName("State");
        }
    }

    public class SagaMessageMap : EntityTypeConfiguration<SagaMessage>
    {
        public SagaMessageMap()
        {
            // Primary Key
			this.HasKey(t => t.MessageId);

			// Properties
			this.Property(t => t.MessageId)
				.IsRequired()
				.HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

			this.Property(t => t.Name)
				.IsOptional();

			this.Property(t => t.Status)
				.IsOptional();

			this.Property(t => t.State)
				.IsOptional();

			this.Property(t => t.Datum)
				.IsOptional();

			this.Property(t => t.SagaId)
				.IsRequired();

			this.Property(t => t.CreatedOn)
				.IsRequired();

			this.Property(t => t.ModifiedOn)
				.IsRequired();

			// Relationships
			this.HasRequired(a => a.Saga)
				.WithMany(b => b.SagaMessages)
				.HasForeignKey(c => c.SagaId); // FK_SagaMessage_Saga

            // Table & Column Mappings
			this.ToTable("SagaMessage");
			this.Property(t => t.MessageId).HasColumnName("MessageId");
			this.Property(t => t.Name).HasColumnName("Name");
			this.Property(t => t.Status).HasColumnName("Status");
			this.Property(t => t.State).HasColumnName("State");
			this.Property(t => t.Datum).HasColumnName("Data");
			this.Property(t => t.SagaId).HasColumnName("SagaId");
			this.Property(t => t.CreatedOn).HasColumnName("CreatedOn");
			this.Property(t => t.ModifiedOn).HasColumnName("ModifiedOn");
        }
    }

}

