using NUnit.Framework;
using OpenTS2.Rendering.Materials;
using UnityEngine;

public class TextureCompositingTest
{
    private static Texture2D MakeSolidTexture(int width, int height, Color32 pixel)
    {
        var texture = new Texture2D(width, height, TextureFormat.RGBA32, mipChain: false);
        var pixels = new Color32[width * height];
        for (var i = 0; i < pixels.Length; i++)
        {
            pixels[i] = pixel;
        }
        texture.SetPixels32(pixels);
        texture.Apply();
        return texture;
    }

    [Test]
    public void TestZeroAlphaOverlayShowsBasePixel()
    {
        var basePixel = new Color32(10, 20, 30, 255);
        var overlayPixel = new Color32(200, 210, 220, 0);

        var result = StandardMaterial.AlphaComposite(MakeSolidTexture(1, 1, basePixel), MakeSolidTexture(1, 1, overlayPixel));

        Assert.That(result.GetPixels32()[0], Is.EqualTo(basePixel));
    }

    [Test]
    public void TestFullAlphaOverlayShowsOverlayPixel()
    {
        var basePixel = new Color32(10, 20, 30, 255);
        var overlayPixel = new Color32(200, 210, 220, 255);

        var result = StandardMaterial.AlphaComposite(MakeSolidTexture(1, 1, basePixel), MakeSolidTexture(1, 1, overlayPixel));

        Assert.That(result.GetPixels32()[0], Is.EqualTo(overlayPixel));
    }

    [Test]
    public void TestHalfAlphaOverlayBlendsHalfway()
    {
        var basePixel = new Color32(0, 0, 0, 255);
        var overlayPixel = new Color32(200, 100, 50, 128);

        var result = StandardMaterial.AlphaComposite(MakeSolidTexture(1, 1, basePixel), MakeSolidTexture(1, 1, overlayPixel));
        var resultPixel = result.GetPixels32()[0];

        // Color32.Lerp interpolates at byte precision, so allow a small rounding tolerance
        // rather than asserting an exact halfway value.
        Assert.That(resultPixel.r, Is.EqualTo(100).Within(2));
        Assert.That(resultPixel.g, Is.EqualTo(50).Within(2));
        Assert.That(resultPixel.b, Is.EqualTo(25).Within(2));
    }

    [Test]
    public void TestResultDimensionsMatchBaseTexture()
    {
        var baseTexture = MakeSolidTexture(4, 2, new Color32(0, 0, 0, 255));
        var overlayTexture = MakeSolidTexture(4, 2, new Color32(255, 255, 255, 128));

        var result = StandardMaterial.AlphaComposite(baseTexture, overlayTexture);

        Assert.That(result.width, Is.EqualTo(4));
        Assert.That(result.height, Is.EqualTo(2));
    }
}
