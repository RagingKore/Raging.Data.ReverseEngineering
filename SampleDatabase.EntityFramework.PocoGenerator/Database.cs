

// This file was automatically generated.
// Do not make changes directly to this file - edit the template instead.
// 
// The following connection settings were used to generate this file
// 
//     Configuration file:     "SampleDatabase.EntityFramework.PocoGenerator\App.config"
//     Connection String Name: "MyDbContext"
//     Connection String:      "Data Source=.;Initial Catalog=SagaStore;Integrated Security=True;MultipleActiveResultSets=True"

// ReSharper disable RedundantUsingDirective
// ReSharper disable DoNotCallOverridableMethodsInConstructor
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable PartialMethodWithSinglePart
// ReSharper disable RedundantNameQualifier

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
//using DatabaseGeneratedOption = System.ComponentModel.DataAnnotations.DatabaseGeneratedOption;

namespace SampleDatabase.EntityFramework.PocoGenerator
{
    // ************************************************************************
    // Unit of work
    public interface IMyDbContext : IDisposable
    {
        IDbSet<Person> People { get; set; } // Person
        IDbSet<RelatedSaga> RelatedSagas { get; set; } // RelatedSaga
        IDbSet<Saga> Sagas { get; set; } // Saga
        IDbSet<SagaMessage> SagaMessages { get; set; } // SagaMessage

        int SaveChanges();
    }

    // ************************************************************************
    // Database context
    public class MyDbContext : DbContext, IMyDbContext
    {
        public IDbSet<Person> People { get; set; } // Person
        public IDbSet<RelatedSaga> RelatedSagas { get; set; } // RelatedSaga
        public IDbSet<Saga> Sagas { get; set; } // Saga
        public IDbSet<SagaMessage> SagaMessages { get; set; } // SagaMessage

        static MyDbContext()
        {
            Database.SetInitializer<MyDbContext>(null);
        }

        public MyDbContext()
            : base("Name=MyDbContext")
        {
        }

        public MyDbContext(string connectionString) : base(connectionString)
        {
        }

        public MyDbContext(string connectionString, System.Data.Entity.Infrastructure.DbCompiledModel model) : base(connectionString, model)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new PersonConfiguration());
            modelBuilder.Configurations.Add(new RelatedSagaConfiguration());
            modelBuilder.Configurations.Add(new SagaConfiguration());
            modelBuilder.Configurations.Add(new SagaMessageConfiguration());
        }

        public static DbModelBuilder CreateModel(DbModelBuilder modelBuilder, string schema)
        {
            modelBuilder.Configurations.Add(new PersonConfiguration(schema));
            modelBuilder.Configurations.Add(new RelatedSagaConfiguration(schema));
            modelBuilder.Configurations.Add(new SagaConfiguration(schema));
            modelBuilder.Configurations.Add(new SagaMessageConfiguration(schema));
            return modelBuilder;
        }
    }

    // ************************************************************************
    // POCO classes

    // Person
    public class Person
    {
        public int PersonId { get; set; } // PersonId (Primary key)
        public string Name { get; set; } // Name
        public int? FatherId { get; set; } // FatherId
        public int? ComputedColumn { get; internal set; } // ComputedColumn
        public short Age { get; set; } // Age
        public DateTime BirthDate { get; set; } // BirthDate
        public Guid InternalId { get; set; } // InternalId
        public Guid ExternalId { get; set; } // ExternalId
        public DateTime NonNullableDate { get; set; } // NonNullableDate
        public decimal DecimalNumber { get; set; } // DecimalNumber
        public double FloatingNumber { get; set; } // FloatingNumber
        public decimal Money { get; set; } // Money
        public System.Data.Entity.Spatial.DbGeography GeographyValue { get; set; } // GeographyValue
        public System.Data.Entity.Spatial.DbGeometry GeometryValue { get; set; } // GeometryValue
        public bool? IsCoolDude { get; set; } // IsCoolDude

        // Reverse navigation
        public virtual ICollection<Person> People { get; set; } // Person.FK_Person_Father

        // Foreign keys
        public virtual Person Person_FatherId { get; set; } // FK_Person_Father

        public Person()
        {
            Name = "John Doe";
            Age = 25;
            InternalId = System.Guid.NewGuid();
            ExternalId = Guid.Parse("37993BAC-E895-4EFA-B726-2FD1405ADC53");
            Money = 20m;
            IsCoolDude = true;
            People = new List<Person>();
        }
    }

    // RelatedSaga
    public class RelatedSaga
    {
        public long PrimarySagaId { get; set; } // PrimarySagaId (Primary key)
        public long SecondarySagaId { get; set; } // SecondarySagaId (Primary key)
        public DateTime CreatedOn { get; set; } // CreatedOn

        // Foreign keys
        public virtual Saga Saga_PrimarySagaId { get; set; } // FK_RelatedSaga_PrimarySaga
        public virtual Saga Saga_SecondarySagaId { get; set; } // FK_RelatedSaga_SecondarySaga

        public RelatedSaga()
        {
            CreatedOn = System.DateTime.Now;
        }
    }

    // Saga
    public class Saga
    {
        public long SagaId { get; set; } // SagaId (Primary key)
        public int? Status { get; set; } // Status
        public byte[] State { get; set; } // State

        // Reverse navigation
        public virtual ICollection<RelatedSaga> RelatedSagas_PrimarySagaId { get; set; } // Many to many mapping
        public virtual ICollection<RelatedSaga> RelatedSagas_SecondarySagaId { get; set; } // Many to many mapping
        public virtual ICollection<SagaMessage> SagaMessages { get; set; } // SagaMessage.FK_SagaMessage_Saga

        public Saga()
        {
            RelatedSagas_PrimarySagaId = new List<RelatedSaga>();
            RelatedSagas_SecondarySagaId = new List<RelatedSaga>();
            SagaMessages = new List<SagaMessage>();
        }
    }

    // SagaMessage
    public class SagaMessage
    {
        public long MessageId { get; set; } // MessageId (Primary key)
        public string Name { get; set; } // Name
        public int? Status { get; set; } // Status
        public byte[] State { get; set; } // State
        public byte[] Data { get; set; } // Data
        public long SagaId { get; set; } // SagaId
        public DateTime CreatedOn { get; set; } // CreatedOn
        public DateTime ModifiedOn { get; set; } // ModifiedOn

        // Foreign keys
        public virtual Saga Saga { get; set; } // FK_SagaMessage_Saga
    }


    // ************************************************************************
    // POCO Configuration

    // Person
    internal class PersonConfiguration : EntityTypeConfiguration<Person>
    {
        public PersonConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".Person");
            HasKey(x => x.PersonId);

            Property(x => x.PersonId).HasColumnName("PersonId").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Name).HasColumnName("Name").IsRequired().HasMaxLength(150);
            Property(x => x.FatherId).HasColumnName("FatherId").IsOptional();
            Property(x => x.ComputedColumn).HasColumnName("ComputedColumn").IsOptional().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.Age).HasColumnName("Age").IsRequired();
            Property(x => x.BirthDate).HasColumnName("BirthDate").IsRequired();
            Property(x => x.InternalId).HasColumnName("InternalId").IsRequired();
            Property(x => x.ExternalId).HasColumnName("ExternalId").IsRequired();
            Property(x => x.NonNullableDate).HasColumnName("NonNullableDate").IsRequired();
            Property(x => x.DecimalNumber).HasColumnName("DecimalNumber").IsRequired();
            Property(x => x.FloatingNumber).HasColumnName("FloatingNumber").IsRequired();
            Property(x => x.Money).HasColumnName("Money").IsRequired().HasPrecision(19,4);
            Property(x => x.GeographyValue).HasColumnName("GeographyValue").IsOptional();
            Property(x => x.GeometryValue).HasColumnName("GeometryValue").IsOptional();
            Property(x => x.IsCoolDude).HasColumnName("IsCoolDude").IsOptional();

            // Foreign keys
            HasOptional(a => a.Person_FatherId).WithMany(b => b.People).HasForeignKey(c => c.FatherId); // FK_Person_Father
        }
    }

    // RelatedSaga
    internal class RelatedSagaConfiguration : EntityTypeConfiguration<RelatedSaga>
    {
        public RelatedSagaConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".RelatedSaga");
            HasKey(x => new { x.PrimarySagaId, x.SecondarySagaId });

            Property(x => x.PrimarySagaId).HasColumnName("PrimarySagaId").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.SecondarySagaId).HasColumnName("SecondarySagaId").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.CreatedOn).HasColumnName("CreatedOn").IsRequired();

            // Foreign keys
            HasRequired(a => a.Saga_PrimarySagaId).WithMany(b => b.RelatedSagas_PrimarySagaId).HasForeignKey(c => c.PrimarySagaId); // FK_RelatedSaga_PrimarySaga
            HasRequired(a => a.Saga_SecondarySagaId).WithMany(b => b.RelatedSagas_SecondarySagaId).HasForeignKey(c => c.SecondarySagaId); // FK_RelatedSaga_SecondarySaga
        }
    }

    // Saga
    internal class SagaConfiguration : EntityTypeConfiguration<Saga>
    {
        public SagaConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".Saga");
            HasKey(x => x.SagaId);

            Property(x => x.SagaId).HasColumnName("SagaId").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.Status).HasColumnName("Status").IsOptional();
            Property(x => x.State).HasColumnName("State").IsOptional();
        }
    }

    // SagaMessage
    internal class SagaMessageConfiguration : EntityTypeConfiguration<SagaMessage>
    {
        public SagaMessageConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".SagaMessage");
            HasKey(x => x.MessageId);

            Property(x => x.MessageId).HasColumnName("MessageId").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.Name).HasColumnName("Name").IsOptional();
            Property(x => x.Status).HasColumnName("Status").IsOptional();
            Property(x => x.State).HasColumnName("State").IsOptional();
            Property(x => x.Data).HasColumnName("Data").IsOptional();
            Property(x => x.SagaId).HasColumnName("SagaId").IsRequired();
            Property(x => x.CreatedOn).HasColumnName("CreatedOn").IsRequired();
            Property(x => x.ModifiedOn).HasColumnName("ModifiedOn").IsRequired();

            // Foreign keys
            HasRequired(a => a.Saga).WithMany(b => b.SagaMessages).HasForeignKey(c => c.SagaId); // FK_SagaMessage_Saga
        }
    }

}

