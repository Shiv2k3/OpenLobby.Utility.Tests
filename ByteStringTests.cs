using OpenLobby.Utility.Utils;

namespace OpenLobby.Utility.Tests;

[TestFixture]
internal class ByteStringTests
{
    private const int Length = 1000;
    private const string TestString = "testing";
    private const string TooLongString = "verylooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooongstringggggggggggggggg";
    private const int TestStringLength = 7;

    private ArraySegment<byte> Body;
    [SetUp]
    public void Setup()
    {
        byte[] payload = new byte[Length];
        Body = new(payload, 0, Length); // body repersents some payload with Length
    }

    [Test]
    [TestCase(TestString, 0)]
    [TestCase(TestString, Length - TestStringLength - ByteString.HEADERSIZE)]
    public void Serialzie_Correct(string value, int start)
    {
        ByteString bs = new(value, Body, start);
        Assert.Multiple(() =>
        {
            Assert.That(bs.Value, Is.EqualTo(value));
            Assert.That(bs.StreamLength, Is.EqualTo(value.Length + ByteString.HEADERSIZE));
        });
    }

    [Test]
    [TestCase(TestString, 0)]
    [TestCase(TestString, Length - TestStringLength - ByteString.HEADERSIZE)]
    public void Deserialzie_Correct(string value, int start)
    {
        ByteString bs = new(value, Body, start);
        ByteString debs = new(Body, start);
        Assert.Multiple(() =>
        {
            Assert.That(debs.Value, Is.EqualTo(value));
            Assert.That(debs.StreamLength, Is.EqualTo(value.Length + ByteString.HEADERSIZE));
        });
    }

    [Test]
    [TestCase("")]
    [TestCase(" ")]
    [TestCase(TooLongString)]
    public void Serialzie_ThrowArgumentException(string value)
    {
        void test() { ByteString s = new(value, Body, 0); }
        Assert.Throws<ArgumentException>(test);
    }

    [Test]
    [TestCase(TestString, -1)]
    [TestCase(TestString, Length)]
    [TestCase(TestString, Length - TestStringLength)]
    public void Serialzie_ThrowRangeException(string value, int start)
    {
        void test() { ByteString s = new(value, Body, start); }
        Assert.Throws<ArgumentOutOfRangeException>(test);
    }

    [Test]
    [TestCase(TestString, Length - TestStringLength - ByteString.HEADERSIZE)]
    public void Deserialzie_ThrowRangeException(string value, int start)
    {
        ByteString s = new(value, Body, start);
        void test()
        {
            ByteString bs = new(Body, start + 1);
        }
        Assert.Throws<ArgumentOutOfRangeException>(test);
    }


}