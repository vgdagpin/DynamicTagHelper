using System.Text.RegularExpressions;

namespace TestProject2;

public class MyTagKey
{
    public string Key { get; set; }
    public int? Index { get; set; }
    public string? PropertyName { get; set; }
    public string? PropertyValue { get; set; }

    public MyTagKey(string key)
    {
        Key = key;
    }

    public static MyTagKey? Parse(string? key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            return null;
        }

        var myTagKey = new MyTagKey(key);
        var match = Regex.Match(key, @"^(?<key>\w+)(\[(?<index>\d+)\])?(\.(?<property>\w+))?(?<propertyName>\w+)?=(?<propertyValue>\w+)?$");

        if (match.Success)
        {
            myTagKey.Key = match.Groups["key"].Value;

            myTagKey.Index = match.Groups["index"].Success 
                ? int.Parse(match.Groups["index"].Value) 
                : null;

            myTagKey.PropertyName = match.Groups["propertyName"].Success
                ? NullIfEmpty(match.Groups["propertyName"].Value)
                : NullIfEmpty(match.Groups["property"].Value);

            myTagKey.PropertyValue = match.Groups["propertyValue"].Success
                ? NullIfEmpty(match.Groups["propertyValue"].Value)
                : null;
        }

        string? NullIfEmpty(string? data)
        {
            if (string.IsNullOrWhiteSpace(data))
            {
                return null;
            }

            return data;
        }

        return myTagKey;
    }
}

public class TagKeyTests
{
    [Fact]
    public void ParseKey()
    {
        var result = MyTagKey.Parse("Tag");

        Assert.NotNull(result);
        Assert.Equal("Tag", result.Key);
        Assert.Null(result.Index);
        Assert.Null(result.PropertyName);
        Assert.Null(result.PropertyValue);
    }

    [Fact]
    public void ParseKeyNull()
    {
        var result = MyTagKey.Parse(null);

        Assert.Null(result);
    }

    [Fact]
    public void ParseKeyEmpty()
    {
        var result = MyTagKey.Parse("");

        Assert.Null(result);
    }

    [Fact]
    public void ParseKeyIndex()
    {
        var result = MyTagKey.Parse("Tag[2]");

        Assert.NotNull(result);
        Assert.Equal("Tag", result.Key);
        Assert.Equal(2, result.Index);
        Assert.Null(result.PropertyName);
        Assert.Null(result.PropertyValue);
    }

    [Fact]
    public void ParseKeyProperty()
    {
        var result = MyTagKey.Parse("Tag[2].Description");

        Assert.NotNull(result);
        Assert.Equal("Tag", result.Key);
        Assert.Equal(2, result.Index);
        Assert.Equal("Description", result.PropertyName);
        Assert.Null(result.PropertyValue);
    }

    [Fact]
    public void ParseKeyPropertyByValue()
    {
        var result = MyTagKey.Parse("Tag[Name=Test]");

        Assert.NotNull(result);
        Assert.Equal("Tag", result.Key);
        Assert.Null(result.Index);
        Assert.Equal("Name", result.PropertyName);
        Assert.Equal("Test", result.PropertyValue);
    }
}