using System.Text.RegularExpressions;

namespace TestProject2;

public class TagKeyTests
{
    [Fact]
    public void ParseKey()
    {
        var result = TagHelper.TagKey.Parse("Tag");

        Assert.NotNull(result);
        Assert.Equal("Tag", result.Key);
        Assert.Null(result.Index);
        Assert.Null(result.PropertyName);
        Assert.Null(result.IndexPropertyName);
        Assert.Null(result.IndexPropertyValue);
    }

    [Fact]
    public void ParseKeyNull()
    {
        var result = TagHelper.TagKey.Parse(null);

        Assert.Null(result);
    }

    [Fact]
    public void ParseKeyEmpty()
    {
        var result = TagHelper.TagKey.Parse("");

        Assert.Null(result);
    }

    [Fact]
    public void ParseKeyIndex()
    {
        var result = TagHelper.TagKey.Parse("Tag[2]");

        Assert.NotNull(result);
        Assert.Equal("Tag", result.Key);
        Assert.Equal(2, result.Index);
        Assert.Null(result.PropertyName);
        Assert.Null(result.IndexPropertyName);
        Assert.Null(result.IndexPropertyValue);
    }

    [Fact]
    public void ParseKeyProperty()
    {
        var result = TagHelper.TagKey.Parse("Tag[2].Description");

        Assert.NotNull(result);
        Assert.Equal("Tag", result.Key);
        Assert.Equal(2, result.Index);
        Assert.Equal("Description", result.PropertyName);
        Assert.Null(result.IndexPropertyName);
        Assert.Null(result.IndexPropertyValue);
    }

    [Fact]
    public void ParseKeyPropertyByValue()
    {
        var result = TagHelper.TagKey.Parse("Tag[Name=Test]");

        Assert.NotNull(result);
        Assert.Equal("Tag", result.Key);
        Assert.Null(result.Index);
        Assert.Equal("Name", result.IndexPropertyName);
        Assert.Equal("Test", result.IndexPropertyValue);
    }

    [Fact]
    public void ParseKeyPropertyByValueProperty()
    {
        var result = TagHelper.TagKey.Parse("Tag[Name=Test].Description");

        Assert.NotNull(result);
        Assert.Equal("Tag", result.Key);
        Assert.Null(result.Index);
        Assert.Equal("Name", result.IndexPropertyName);
        Assert.Equal("Test", result.IndexPropertyValue);
        Assert.Equal("Description", result.PropertyName);
    }
}