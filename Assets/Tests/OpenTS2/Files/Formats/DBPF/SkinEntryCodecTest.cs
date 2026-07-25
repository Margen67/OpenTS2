using System;
using NUnit.Framework;
using OpenTS2.Common;
using OpenTS2.Content;
using OpenTS2.Content.DBPF;
using OpenTS2.Files.Formats.DBPF;

public class SkinEntryCodecTest
{
    [SetUp]
    public void SetUp()
    {
        TestCore.Initialize();
        ContentManager.Instance.AddPackage("TestAssets/Codecs/SimData.package");
    }

    [Test]
    public void TestParsesProperties()
    {
        var skinEntryAsset = ContentManager.Instance
            .GetAsset<SkinEntryAsset>(new ResourceKey(0xF55A9384, 0x2C17B74A, TypeIDs.SKIN_ENTRY));
        Assert.That(skinEntryAsset.Type, Is.EqualTo("skin"));
        Assert.That(skinEntryAsset.Name, Is.EqualTo("afhairhatwitch_blond_good"));
        Assert.That(skinEntryAsset.Age, Is.EqualTo(0x48));
        Assert.That(skinEntryAsset.Gender, Is.EqualTo(1));
        Assert.That(skinEntryAsset.Species, Is.EqualTo(1));

        Assert.IsNotNull(skinEntryAsset.Category);
        var category = (OutfitCategory)skinEntryAsset.Category;
        Assert.That(category, Is.EqualTo(
            OutfitCategory.Hair | OutfitCategory.Face | OutfitCategory.Top | OutfitCategory.Accessory | OutfitCategory.TailShort));
    }

    [Test]
    public void TestParsesShapeKey()
    {
        var skinEntryAsset = ContentManager.Instance
            .GetAsset<SkinEntryAsset>(new ResourceKey(0xF55A9384, 0x2C17B74A, TypeIDs.SKIN_ENTRY));
        Assert.IsNotNull(skinEntryAsset.ShapeResourceKey);
        Assert.IsInstanceOf<ResourceKeyIndexProp>(skinEntryAsset.ShapeResourceKey);
    }

    [Test]
    public void TestParsesOverrides()
    {
        var skinEntryAsset = ContentManager.Instance
            .GetAsset<SkinEntryAsset>(new ResourceKey(0xF55A9384, 0x2C17B74A, TypeIDs.SKIN_ENTRY));
        Assert.That(skinEntryAsset.MaterialOverrides.Count, Is.EqualTo(4));
        Assert.That(skinEntryAsset.MaterialOverrides[0].SubsetName, Is.EqualTo("hair_alpha5"));
        Assert.That(skinEntryAsset.MaterialOverrides[1].SubsetName, Is.EqualTo("hair_alpha3"));
        Assert.That(skinEntryAsset.MaterialOverrides[2].SubsetName, Is.EqualTo("hat"));
        Assert.That(skinEntryAsset.MaterialOverrides[3].SubsetName, Is.EqualTo("hair"));
    }
}
