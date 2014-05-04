using Autofac.Extras.FakeItEasy;
using NUnit.Framework;

namespace Raging.Toolbox.Testing.NUnit.FakeItEasy
{
    public abstract class AutoFakeTestsBase
    {
        public AutoFake AutoFaker;

        [SetUp]
        public virtual void SetUp()
        {
            this.AutoFaker = new AutoFake();
        }

        [TearDown]
        public void TearDown()
        {
            if(this.AutoFaker != null)
                this.AutoFaker.Dispose();
        }
    }
}