using System;
using System.Collections.Generic;

namespace SampleDatabase.EntityFramework.PowerTools.Models
{
    public partial class RelatedSaga
    {
        public long PrimarySagaId { get; set; }
        public long SecondarySagaId { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public virtual Saga Saga { get; set; }
        public virtual Saga Saga1 { get; set; }
    }
}
