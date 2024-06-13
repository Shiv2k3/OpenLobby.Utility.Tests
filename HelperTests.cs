using OpenLobby.Utility.Utils;

namespace OpenLobby.Utility.Tests;

[TestFixture]
internal class HelperTests
{
    [Test]
    [TestCase(ushort.MinValue, 0, 1)]
    [TestCase((ushort)5614, 0, 1)]
    [TestCase(ushort.MaxValue, 0, 1)]
    public void SetUshort_Correct(ushort testValue, int index1, int index2)
    {
        byte[] arr = new byte[1024];
        ArraySegment<byte> Body = new(arr);
        Helper.SetUshort(testValue, index1, index2, Body);
        byte b1 = (byte)testValue;
        byte b2 = (byte)(testValue >> 8);
        Assert.That(b1, Is.EqualTo(Body[index1]));
        Assert.That(b2, Is.EqualTo(Body[index2]));
    }

    [Test]
    public void GetUshort_Correct()
    {
        Assert.Fail("Not Implemented");
    }

    [Test]
    public void SumOfByteStrings_Correct()
    {
        string[] s = { "hello", "world", "!" };
        int count = 5 + 5 + 1 + 3 * ByteString.HEADERSIZE;
        Assert.That(count, Is.EqualTo(Helper.SumOfByteStrings(s)));
    }

    [Test]
    public void SumOfByteStrings_Throws_TooLong()
    {
        string[] s = { "hello", "world", "!" };
        for (int i = 0; i <= ushort.MaxValue; i++)
        {
            s[0] += "o";
        }
        void Throws() => Helper.SumOfByteStrings(s);
        Assert.Throws<ArgumentOutOfRangeException>(Throws);
    }


    [Test]
    public void SumOfStrings_Correct()
    {
        string[] s = { "hello", "world", "!" };
        int count = 5 + 5 + 1;
        Assert.That(count, Is.EqualTo(Helper.SumOfStrings(s)));
    }
}
