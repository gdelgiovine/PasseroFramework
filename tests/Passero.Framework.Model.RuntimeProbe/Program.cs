using Dapper.Contrib.Extensions;
using Passero.Framework;

int passed = 0;
int failed = 0;
void Check(string name, Action test)
{
    try
    {
        test();
        Console.WriteLine($"PASS {name}");
        passed++;
    }
    catch (Exception error)
    {
        Console.Error.WriteLine($"FAIL {name}: {error}");
        failed++;
    }
}

static void Require(bool condition, string message)
{
    if (!condition) throw new InvalidOperationException(message);
}

Check("notifications order and unchanged-value suppression", () =>
{
    var model = new NotifyingModel();
    var events = new List<string>();
    model.PropertyChanging += (_, e) => events.Add("before:" + e.PropertyName);
    model.PropertyChanged += (_, e) => events.Add("after:" + e.PropertyName);
    model.Name = "A";
    model.Name = "A";
    model.Name = "B";
    Require(events.SequenceEqual(new[] { "before:Name", "after:Name", "before:Name", "after:Name" }), string.Join(",", events));
});
Check("automatic properties do not implicitly notify", () =>
{
    var model = new AuthorModel();
    int calls = 0;
    model.PropertyChanged += (_, _) => calls++;
    model.First = "Ada";
    Require(calls == 0, $"observed {calls} notifications");
});
Check("computed property recalculates value", () =>
{
    var model = new AuthorModel { First = "Ada", Last = "Lovelace" };
    Require(model.FullName == "Ada Lovelace", "initial value mismatch");
    model.Last = "Byron";
    Require(model.FullName == "Ada Byron", "updated value mismatch");
});
Check("clone is independent", () =>
{
    var original = new NotifyingModel { Name = "Original" };
    var clone = (NotifyingModel)original.Clone();
    Require(!ReferenceEquals(original, clone) && clone.Name == "Original", "clone mismatch");
    clone.Name = "Changed";
    Require(original.Name == "Original", "clone modified source");
});
Check("explicit key metadata", () =>
{
    var keys = Utilities.GetModelPrimaryKeysPropertiesInfo(typeof(AuthorModel));
    Require(keys.Count == 1 && keys[0].Name == "Id", "key metadata mismatch");
});
Check("composite key metadata", () =>
{
    var keys = Utilities.GetModelPrimaryKeysPropertiesInfo(typeof(CompositeKeyModel));
    Require(keys.Select(k => k.Name).SequenceEqual(new[] { "PartA", "PartB" }), "composite key metadata mismatch");
});
Check("computed property excluded from writable metadata", () =>
{
    Require(Utilities.GetModelPropertiesInfo(typeof(AuthorModel)).Any(p => p.Name == "FullName"), "missing computed");
    Require(!Utilities.GetModelPropertiesInfo(typeof(AuthorModel), ExcludeComputed: true).Any(p => p.Name == "FullName"), "computed not excluded");
});
Check("table name metadata", () =>
{
    Require(Utilities.GetModelTableName(typeof(AuthorModel)) == "TestAuthors", "table name mismatch");
});

Console.WriteLine($"RUNTIME PROBE: Passed={passed}; Failed={failed}; Total={passed + failed}");
if (passed + failed != 8 || failed != 0) Environment.ExitCode = 1;

internal sealed class NotifyingModel : ModelBase
{
    private string _name = "";
    public string Name { get => _name; set => SetProperty(ref _name, value); }
}
[Table("TestAuthors")]
internal sealed class AuthorModel : ModelBase
{
    [ExplicitKey] public string Id { get; set; } = "";
    public string First { get; set; } = "";
    public string Last { get; set; } = "";
    [Computed] public string FullName => $"{First} {Last}".Trim();
}
internal sealed class CompositeKeyModel : ModelBase
{
    [ExplicitKey] public string PartA { get; set; } = "";
    [ExplicitKey] public int PartB { get; set; }
}
