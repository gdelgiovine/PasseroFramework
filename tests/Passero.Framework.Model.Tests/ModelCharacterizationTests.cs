using Dapper.Contrib.Extensions;
using Passero.Framework;
using Xunit;

namespace Passero.Framework.Model.Tests;

// Tests against the real framework assembly; no database is required.
public sealed class ModelCharacterizationTests
{
    private sealed class NotifyingModel : ModelBase
    {
        private string _name = "";
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }
    }

    [Table("TestAuthors")]
    private sealed class AuthorModel : ModelBase
    {
        [ExplicitKey]
        public string Id { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";

        [Computed]
        public string FullName => $"{FirstName} {LastName}".Trim();
    }

    private sealed class CompositeKeyModel : ModelBase
    {
        [ExplicitKey] public string PartA { get; set; } = "";
        [ExplicitKey] public int PartB { get; set; }
    }

    [Fact]
    public void SetProperty_RaisesChangingBeforeChanged_AndSkipsEqualValues()
    {
        var model = new NotifyingModel();
        var events = new List<string>();
        model.PropertyChanging += (_, e) => events.Add($"changing:{e.PropertyName}");
        model.PropertyChanged += (_, e) => events.Add($"changed:{e.PropertyName}");

        model.Name = "Mario";
        model.Name = "Mario";
        model.Name = "Luigi";

        Assert.Equal(new[]
        {
            "changing:Name", "changed:Name",
            "changing:Name", "changed:Name"
        }, events);
    }

    [Fact]
    public void AutoProperties_DoNotRaiseModelBaseEvents()
    {
        var model = new AuthorModel();
        int notifications = 0;
        model.PropertyChanged += (_, _) => notifications++;

        model.FirstName = "Ada";

        Assert.Equal(0, notifications);
    }

    [Fact]
    public void ComputedProperty_ReflectsCurrentValues()
    {
        var model = new AuthorModel { FirstName = "Ada", LastName = "Lovelace" };
        Assert.Equal("Ada Lovelace", model.FullName);
        model.LastName = "Byron";
        Assert.Equal("Ada Byron", model.FullName);
    }

    [Fact]
    public void Clone_ProducesIndependentCopy()
    {
        var original = new NotifyingModel { Name = "Original" };
        var clone = Assert.IsType<NotifyingModel>(original.Clone());

        Assert.NotSame(original, clone);
        Assert.Equal("Original", clone.Name);
        clone.Name = "Changed";
        Assert.Equal("Original", original.Name);
    }

    [Fact]
    public void Metadata_RecognizesExplicitAndCompositeKeys()
    {
        var single = Utilities.GetModelPrimaryKeysPropertiesInfo(typeof(AuthorModel));
        var composite = Utilities.GetModelPrimaryKeysPropertiesInfo(typeof(CompositeKeyModel));

        Assert.Equal("Id", Assert.Single(single).Name);
        Assert.Equal(new[] { "PartA", "PartB" }, composite.Select(p => p.Name).ToArray());
    }

    [Fact]
    public void Metadata_ExcludesComputedPropertiesWhenRequested()
    {
        var included = Utilities.GetModelPropertiesInfo(typeof(AuthorModel));
        var excluded = Utilities.GetModelPropertiesInfo(typeof(AuthorModel), ExcludeComputed: true);

        Assert.Contains(included, p => p.Name == nameof(AuthorModel.FullName));
        Assert.DoesNotContain(excluded, p => p.Name == nameof(AuthorModel.FullName));
    }

    [Fact]
    public void Metadata_UsesDeclaredTableName()
    {
        Assert.Equal("TestAuthors", Utilities.GetModelTableName(typeof(AuthorModel)));
    }
}
