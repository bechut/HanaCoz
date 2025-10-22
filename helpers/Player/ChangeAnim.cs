using Godot;

namespace HanaCoz.Helpers.Player;

public static partial class Helper
{
    public static void ChangeAnim(AnimatedSprite2D sprite, string newAtlasPath)
    {
        var frames = sprite.SpriteFrames;
        if (frames == null )
            return;
        
        if (string.IsNullOrEmpty(newAtlasPath))
        {
            foreach (var anim in frames.GetAnimationNames())
            {
                var count = frames.GetFrameCount(anim);
                for (var i = 0; i < count; i++)
                {
                    // Preserve the region but remove the texture
                    var frame = frames.GetFrameTexture(anim, i);
                    if (frame is AtlasTexture atlasTex)
                    {
                        var cleared = new AtlasTexture
                        {
                            Atlas = null, // remove texture
                            Region = atlasTex.Region // keep region data
                        };
                        frames.SetFrame(anim, i, cleared);
                    }
                }
            }

            return;
        }
        var newAtlas = GD.Load<CompressedTexture2D>(newAtlasPath);
        if (newAtlas == null)
        {
            return;
        }
        foreach (var anim in frames.GetAnimationNames())
        {
            var count = frames.GetFrameCount(anim);
            for (var i = 0; i < count; i++)
            {
                var frame = frames.GetFrameTexture(anim, i);
                if (frame is not AtlasTexture atlasTex) return;
                var newTex = new AtlasTexture
                {
                    Atlas = newAtlas,
                    Region = atlasTex.Region
                };
                frames.SetFrame(anim, i, newTex);
            }
        } 
    }
}