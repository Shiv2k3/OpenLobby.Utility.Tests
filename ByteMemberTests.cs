using NUnit.Framework.Internal;
using OpenLobby.Utility.Utils;

namespace OpenLobby.Utility.Tests;

[TestFixture]
internal class ByteMemberTests
{
    private const int Length = 1000;
    private ArraySegment<byte> Body;

    [SetUp]
    public void Setup()
    {
        byte[] payload = new byte[Length];
        Body = new(payload, 0, Length); // body repersents some payload with Length
    }

    [Test]
    [TestCase(0, 255)]
    [TestCase(Length - 1, 255)]
    public void Serialize_Correct(int index, byte given)
    {
        ByteMember bm = new(Body, index, given);
        Assert.That(bm.AsBool, Is.EqualTo(given > 0));
        Assert.That(bm.Value, Is.EqualTo(given));
    }

    [Test]
    [TestCase(0, 255)]
    [TestCase(Length - 1, 255)]
    public void Deserialize_Correct(int index, byte given)
    {
        ByteMember bm = new(Body, index, given);
        Assert.That(bm.Value, Is.EqualTo(given));
        Assert.That(bm.AsBool, Is.EqualTo(given > 0));
    }
}