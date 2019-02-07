namespace NServiceBus.Hosting.Azure.Tests
{
    using NUnit.Framework;
    using Particular.Approvals;
    using PublicApiGenerator;

    [TestFixture]
    public class APIApprovals
    {
        [Test]
        public void Approve()
        {
            var publicApi = ApiGenerator.GeneratePublicApi(typeof(GenericHost).Assembly);
            Approver.Verify(publicApi);
        }
    }
}