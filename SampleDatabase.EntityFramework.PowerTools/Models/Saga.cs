using System;
using System.Collections.Generic;

namespace SampleDatabase.EntityFramework.PowerTools.Models
{
    public partial class Saga
    {
        public Saga()
        {
            this.RelatedSagas = new List<RelatedSaga>();
            this.RelatedSagas1 = new List<RelatedSaga>();
            this.SagaMessages = new List<SagaMessage>();
        }

        public long SagaId { get; set; }
        public Nullable<int> Status { get; set; }
        public byte[] State { get; set; }
        public virtual ICollection<RelatedSaga> RelatedSagas { get; set; }
        public virtual ICollection<RelatedSaga> RelatedSagas1 { get; set; }
        public virtual ICollection<SagaMessage> SagaMessages { get; set; }
    }
}
