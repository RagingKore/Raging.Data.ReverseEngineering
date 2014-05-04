using System;
using System.Collections.Generic;

namespace SampleDatabase.EntityFramework.PowerTools.Models
{
    public partial class Person
    {
        public Person()
        {
            this.Person1 = new List<Person>();
        }

        public int PersonId { get; set; }
        public string Name { get; set; }
        public Nullable<int> FatherId { get; set; }
        public Nullable<int> ComputedColumn { get; set; }
        public short Age { get; set; }
        public System.DateTime BirthDate { get; set; }
        public System.Guid InternalId { get; set; }
        public System.Guid ExternalId { get; set; }
        public System.DateTime NonNullableDate { get; set; }
        public decimal DecimalNumber { get; set; }
        public double FloatingNumber { get; set; }
        public decimal Money { get; set; }
        public System.Data.Entity.Spatial.DbGeography GeographyValue { get; set; }
        public System.Data.Entity.Spatial.DbGeometry GeometryValue { get; set; }
        public virtual ICollection<Person> Person1 { get; set; }
        public virtual Person Person2 { get; set; }
    }
}
