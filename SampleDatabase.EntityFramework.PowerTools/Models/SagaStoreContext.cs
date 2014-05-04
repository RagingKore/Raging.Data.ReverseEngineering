using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using SampleDatabase.EntityFramework.PowerTools.Models.Mapping;

namespace SampleDatabase.EntityFramework.PowerTools.Models
{
    public partial class SagaStoreContext : DbContext
    {
        static SagaStoreContext()
        {
            Database.SetInitializer<SagaStoreContext>(null);
        }

        public SagaStoreContext()
            : base("Name=SagaStoreContext")
        {
        }

        public DbSet<Person> People { get; set; }
        public DbSet<RelatedSaga> RelatedSagas { get; set; }
        public DbSet<Saga> Sagas { get; set; }
        public DbSet<SagaMessage> SagaMessages { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new PersonMap());
            modelBuilder.Configurations.Add(new RelatedSagaMap());
            modelBuilder.Configurations.Add(new SagaMap());
            modelBuilder.Configurations.Add(new SagaMessageMap());
        }
    }
}
