using Dapper.ColumnMapper;
using Dapper.Contrib.Extensions;
using Passero.Framework;
using PasseroDemo.Models;
using Xunit;

namespace Passero.Framework.Model.Tests;

/// <summary>
/// Characterization tests: capture actual metadata semantics without changing production.
/// A passing test here can document a limitation; it is not an endorsement of that behavior.
/// </summary>
public sealed class ModelMetadataEdgeTests
{
    private sealed class WriteAttributeModel : ModelBase
    {
        [Write(true)] public string IncludedByWriteTrue { get; set; } = "";
        [Write(false)] public string ExcludedByWriteFalse { get; set; } = "";
    }

    private sealed class DapperOnlyKeyModel : ModelBase
    {
        [ExplicitKey] public string KeyCode { get; set; } = "";
    }

    private sealed class MappedModel : ModelBase
    {
        [ColumnMapping("db_column")] public string PropertyName { get; set; } = "";
    }

    [Fact]
    public void CurrentWriteFiltering_TreatsTrueAndFalseIdentically()
    {
        var props = Utilities.GetModelPropertiesInfo(typeof(WriteAttributeModel), ExcludeComputed: true);
        Assert.DoesNotContain(props, x => x.Name == "IncludedByWriteTrue");
        Assert.DoesNotContain(props, x => x.Name == "ExcludedByWriteFalse");
    }

    [Fact]
    public void DapperExplicitKey_IsRecognizedBySharedMetadata()
    {
        Assert.Equal("KeyCode", Assert.Single(
            Utilities.GetModelPrimaryKeysPropertiesInfo(typeof(DapperOnlyKeyModel))).Name);
    }

    [Fact]
    public void ColumnMapping_ResolvesPhysicalColumnName()
    {
        var prop = typeof(MappedModel).GetProperty(nameof(MappedModel.PropertyName))!;
        Assert.Equal("db_column", Utilities.GetMappedColumnName(prop));
    }

    [Fact]
    public void ActualDemoModels_ContainCompositeAndGeneratedKeys()
    {
        var composite = Utilities.GetModelPrimaryKeysPropertiesInfo(typeof(Titleauthor))
            .Select(p => p.Name).ToArray();
        Assert.Equal(new[] { nameof(Titleauthor.au_id), nameof(Titleauthor.title_id) }, composite);

        var generated = Utilities.GetModelPrimaryKeysPropertiesInfo(typeof(Salesmaster));
        Assert.Equal(nameof(Salesmaster.ord_num), Assert.Single(generated).Name);
    }

    [Fact]
    public void ActualDemoModels_DeclareTheirTables()
    {
        Assert.Equal("Authors", Utilities.GetModelTableName(typeof(Author)));
        Assert.Equal("publishers", Utilities.GetModelTableName(typeof(Publisher)));
        Assert.Equal("titles", Utilities.GetModelTableName(typeof(Title)));
    }
}
