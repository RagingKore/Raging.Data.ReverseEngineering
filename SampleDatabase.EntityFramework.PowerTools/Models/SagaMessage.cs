using System;
using System.Collections.Generic;

namespace SampleDatabase.EntityFramework.PowerTools.Models
{
    public partial class SagaMessage
    {
        public long MessageId { get; set; }
        public string Name { get; set; }
        public Nullable<int> Status { get; set; }
        public byte[] State { get; set; }
        public byte[] Data { get; set; }
        public long SagaId { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public System.DateTime ModifiedOn { get; set; }
        public virtual Saga Saga { get; set; }
    }
}
