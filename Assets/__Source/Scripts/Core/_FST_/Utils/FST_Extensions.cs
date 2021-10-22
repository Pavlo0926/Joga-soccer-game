using UnityEngine;

public static class FST_Extensions
{
    public static Sprite ToSprite(this Texture2D t, Vector2? v = null)
    {
        if (t == null)
            return null;
        var vector = v ?? new Vector2(.5f, .5f);
        return Sprite.Create(t, new Rect(0, 0, t.width, t.height), vector);
    }
}
