using Xunit;
using Whalelib.Entity;
using Whalelib.Errors;

namespace Whale.Tests;

public class WhaleTest
{

    Whalelib.Whale whale = new Whalelib.Whale("APIKEY");

    [Fact]
    public void Status_Test()
    {
       Status status =  whale.getStatus();

       Assert.Equal("success",  status.Result);
    }

    [Fact]
    public void Transactions_Test()
    {
        Transactions transactions = whale.GetTransactions();

        Assert.Equal("success", transactions.Result);
    }

    [Fact]
    public void Error_Test()
    {
        var errorWhale = new Whalelib.Whale("");

        Assert.Throws<WhaleExeception>(() => errorWhale.getStatus());
    }

}