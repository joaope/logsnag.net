namespace LogSnag.Tests.Unit;

public class UnitTest1
{
    [Fact]
    public async Task Test1()
    {
        var logSnag = new LogSnag("13435816e0533cb2005b834feec0ac0b");

        await logSnag.Publish(new LogSnagEvent("logsnag-net", "test-channel", "AnEvent")
        {
            Tags =
            {
                new LogSnagTag("key-one", 1),
                new LogSnagTag("key-two", "val2"),
                new LogSnagTag("bool", true)
            },
            Notify = true,
            Parser = LogSnagParser.Text
        });
    }
}