using Example.Web.Services;
namespace Example.ProductCodes.Tests;
public class ProductCodeGeneratorTests
{
    private readonly ProductCodeGenerator generator = new();
    [Fact]
    public void ContainsExactlyThirtyAlphanumericCharacters()
    {
        var actual = generator.Generate().Replace("-", "");
        Assert.Equal(30, actual.Length);
        Assert.Matches("^[A-Z0-9]{30}$", actual);
    }
    [Fact]
    public void HasExpectedFormatAndAllowedCharacters()
    {
        for (var i = 0; i < 100; i++)
            Assert.Matches("^[A-Z0-9]{5}(-[A-Z0-9]{5}){5}$", generator.Generate());
    }
    [Fact]
    public void ContainsFiveSeparatorsAndSixGroupsOfFive()
    {
        var actual = generator.Generate();
        Assert.Equal(5, actual.Count(c => c == '-'));
        var groups = actual.Split('-');
        Assert.Equal(6, groups.Length);
        Assert.All(groups, group => Assert.Equal(5, group.Length));
    }
    [Fact]
    public void MultipleCallsProduceDifferentCodes()
    {
        var codes = Enumerable.Range(0, 1000).Select(_ => generator.Generate()).ToArray();
        Assert.Equal(codes.Length, codes.Distinct().Count());
    }
}

