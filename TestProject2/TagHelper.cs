using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TestProject2;

public static class TagHelper
{
    public class TagKey
    {
        public string Key { get; set; }

        public int? Index { get; set; }
        public string? IndexPropertyName { get; set; }
        public string? IndexPropertyValue { get; set; }

        public string? PropertyName { get; set; }

        public TagKey(string key)
        {
            Key = key;
        }

        public static TagKey? Parse(string? key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return null;
            }

            var myTagKey = new TagKey(key);
            var match = Regex.Match(key, @"^(?<key>\w+)(?:\[(?<index>\d+)\](?:\.(?<property>\w+))?|(?:\[(?<indexPropertyName>\w+)=(?<indexPropertyValue>\w+)\](?:\.(?<property>\w+))?))$");

            if (match.Success)
            {
                myTagKey.Key = match.Groups["key"].Value;
                myTagKey.Index = match.Groups["index"].Success ? int.Parse(match.Groups["index"].Value) : null;
                myTagKey.IndexPropertyName = match.Groups["indexPropertyName"].Success ? NullIfEmpty(match.Groups["indexPropertyName"].Value) : null;
                myTagKey.IndexPropertyValue = match.Groups["indexPropertyValue"].Success ? NullIfEmpty(match.Groups["indexPropertyValue"].Value) : null;
                myTagKey.PropertyName = match.Groups["property"].Success ? NullIfEmpty(match.Groups["property"].Value) : null;
            }

            string? NullIfEmpty(string? data) => string.IsNullOrWhiteSpace(data) ? null : data;

            return myTagKey;
        }
    }

    public static string[] GetTags(string input)
    {
        var str = new List<string>();

        foreach (Match match in Regex.Matches(input, @"~([^~]+)~"))
        {
            str.Add(match.Groups[1].Value);
        }

        return str.ToArray();
    }

    public static string[] GetRegisteredTags()
    {
        return data.Keys.Select(a => a).ToArray();
    }

    private static Dictionary<string, object> data = new Dictionary<string, object>();

    public static void Register<T>(string tag, Func<TagKey, T, string> func)
    {
        if (data.ContainsKey(tag))
        {
            return;
        }

        data[tag] = func;
    }

    public static string? RunTag<T>(string tag, T arg)
    {
        var tagKey = TagKey.Parse(tag) ?? throw new ArgumentNullException(tag);

        if (data.ContainsKey(tagKey.Key) && data[tagKey.Key] is Func<TagKey, T, string> func)
        {
            return func(tagKey, arg);
        }

        return null;
    }
}
