using Xunit;
using Whalelib.Entity;


namespace Whale.Tests;

public class WhaleTest
{
    Whalelib.Whale whale = new Whalelib.Whale("uODhGCOu5ycbPZf9XY8jFU2HFz9TwHpC");


    [Fact]
    public void Status_Test()
    {
       Status status =  whale.getStatus();

       Assert.NotEqual("success",  status.Result);
    }

    [Fact]
    public void Transactions_Test()
    {
        Transactions transactions = whale.GetTransactions();

        Assert.Equal("success", transactions.Result);
    }

}