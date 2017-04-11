using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  public static class AssetManager
  {
    private static ContentManager Content;
    private static Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();
    private static Dictionary<string, SpriteFont> fonts = new Dictionary<string, SpriteFont>();
    private static Dictionary<string, Effect> effects = new Dictionary<string, Effect>();

    public static void LoadContent(ContentManager content)
    {
      AssetManager.Content = content;

      LoadFont("font1", "font1");
      LoadFont("future48", "future48");
      LoadFont("future18", "future18");

      LoadEffect("ColorChanger", "ColorChanger");
    }
    public static Texture2D GetTexture(string name)
    {
      if (!textures.ContainsKey(name))
      {
        LoadTexture(name, name);
      }
      return textures[name];
    }
    public static SpriteFont GetFont(string name)
    {
      return fonts[name];
    }
    public static Effect GetEffect(string name)
    {
      return effects[name];
    }
    private static void LoadTexture(string name, string path)
    {
      textures.Add(name, AssetManager.Content.Load<Texture2D>(@"Textures\" + path));
    }
    private static void LoadFont(string name, string path)
    {
      fonts.Add(name, AssetManager.Content.Load<SpriteFont>(@"Fonts\" + path));
    }
    private static void LoadEffect(string name, string path)
    {
      effects.Add(name, AssetManager.Content.Load<Effect>(@"Effects\" + path));
    }
  }
}
