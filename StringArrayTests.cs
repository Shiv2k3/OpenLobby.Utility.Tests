using OpenLobby.Utility.Utils;

namespace OpenLobby.Utility.Tests;

[TestFixture]
internal class StringArrayTests
{
    private const int Length = 10000;
    private ArraySegment<byte> Body;

    private const int ValidStringCount = 10; // it has to be less than 255
    private const int InvalidStringArrayLength = 256;
    private List<string> ValidStrings;
    private List<string> InvalidStrings;
    private List<string> InvalidStringArray;
    private string InvalidString = "VeryLongVeryLongVeryLongVeryLongVeryLongVeryLongVeryLongVeryLongVeryLongVeryLongVeryLongVeryLongVeryLongVeryLongVeryLongVeryLongVeryLongVeryLongVeryLongVeryLongVeryLongVeryLongVeryLongVeryLongVeryLongVeryLongVeryLongVeryLongVeryLongVeryLongVeryLongVeryLongVeryLongVeryLongVeryLongVeryLongVeryLongVeryLongVeryLongVeryLongVeryLongVeryLongVeryLongVeryLongVeryLongVeryLongVeryLongVeryLongVeryLongVeryLongVeryLongVeryLongVeryLongVeryLongVeryLongVeryLongVeryLongVeryLongVeryLongVeryLongVeryLongVeryLongVeryLongVeryLong";

    [SetUp]
    public void Setup()
    {
        byte[] payload = new byte[Length];
        Body = new(payload, 0, Length); // body repersents some payload with Length

        ValidStrings = new(ValidStringCount);
        for (int i = 0; i < ValidStringCount; i++)
        {
            ValidStrings.Add($"d{i * i}lkfjklfa");
        }

        InvalidStrings = new(ValidStringCount);
        for (int i = 0; i < ValidStringCount; i++)
        {
            InvalidStrings.Add(InvalidString);
        }

        InvalidStringArray = new(InvalidStringArrayLength);
        for (int i = 0; i < InvalidStringArrayLength; i++)
        {
            InvalidStringArray.Add($"dsfasfasfsf{i}");
        }
    }

    [Test]
    public void Serialize_Correct()
    {
        string[] strs = ValidStrings.ToArray();
        var seg = Body.Slice(0, StringArray.GetRequiredLength(strs));
        StringArray sa = new(seg, strs);
        Assert.That(sa.Count, Is.EqualTo(ValidStringCount));
        for (int i = 0; i < ValidStringCount; i++)
        {
            Assert.That(sa[i], Is.EqualTo(ValidStrings[i]));
        }
    }

    [Test]
    public void Deserialize_Correct()
    {
        string[] strs = ValidStrings.ToArray();
        var seg = Body.Slice(0, StringArray.GetRequiredLength(strs));
        StringArray sa = new(seg, strs);
        StringArray sad = new(seg);
        Assert.That(sad.Count, Is.EqualTo(ValidStringCount));
        for (int i = 0; i < ValidStringCount; i++)
        {
            Assert.Multiple(() =>
            {
                Assert.That(ValidStrings[i], Is.EqualTo(sad[i]));
            });
        }

    }

    [Test]
    public void GetHeaderSize_Correct()
    {
        int count = 1 + ValidStringCount;
        foreach (var str in ValidStrings)
        {
            count += str.Length;
        }

        Assert.That(StringArray.GetRequiredLength(ValidStrings.ToArray()), Is.EqualTo(count));
    }

    [Test]
    [TestCase(0)]
    [TestCase(ValidStringCount + 1)]
    [TestCase(ValidStringCount - 1)]
    public void Serialize_Throws_SegmentOutOfRange(int invalidSegmetnCount)
    {
        var seg = Body.Slice(0, invalidSegmetnCount);
        void Test() => new StringArray(seg, ValidStrings.ToArray());
        Assert.Throws<StringArray.SegmentOutOfRange>(Test);
    }

    [Test]
    [TestCase(256)]
    public void Serialze_Throws_StringArrayOutOfRange(int invalidElementCount)
    {
        var seg = Body.Slice(0, invalidElementCount);
        void Test() => new StringArray(seg, InvalidStringArray.ToArray());
        Assert.Throws<StringArray.StringArrayOutOfRange>(Test);
    }

    [Test]
    public void Serialize_Throws_StringOutOfRange()
    {
        var seg = Body.Slice(0, StringArray.GetRequiredLength(InvalidStrings.ToArray()));
        void Throws() => new StringArray(seg, InvalidStrings.ToArray());
        Assert.Throws<StringArray.StringOutOfRange>(Throws);
    }

    [Test]
    public void Deserialize_Throws_BadString()
    {
        string[] strings = { "sfsf", "asfsafas", ""};
        int countForEmptyString = 2; // number of bytes requried to repersent an empty or null string
        int indexOfEmptyString = 3; // the index in the seg for empty stirng length

        var seg = Body.Slice(0, StringArray.GetRequiredLength(strings[0], strings[1]) + countForEmptyString);
        new StringArray(seg, strings[0], strings[1], " ");
        seg[indexOfEmptyString] = 0;

        void Throws() => new StringArray(seg);
        Assert.Throws<StringArray.BadString>(Throws);
    }
}