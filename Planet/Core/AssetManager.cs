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
      //if (content != null)
      //   throw new InvalidOperationException("Content is already loaded.");
      AssetManager.Content = content;

      // (key, path)
      AddTexture("Fill", "Filltexture");
      AddTexture("Ship1", "ship_temp");
      AddTexture("Proj1", "Image2");
      AddTexture("pumpkin", "pumpkin");
      AddTexture("Parasite", "Image3");
      AddTexture("Laser", "laser");
      AddTexture("Sprites", "Planet_sprites");
      AddTexture("Circle", "Circle");
      AddTexture("enemyBlue2", "enemyBlue2");
      AddTexture("laserBlue07", "laserBlue07");
      AddTexture("blue_button04", "blue_button04");

      AddFont("font1", "font1");
      AddFont("titleFont", "titleFont");
      AddFont("uiFont", "uiFont");

      AddEffect("ColorChanger", "ColorChanger");
    }
    public static Texture2D GetTexture(string name)
    {
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

    private static void AddTexture(string name, string path)
    {
      textures.Add(name, AssetManager.Content.Load<Texture2D>(@"Textures\" + path));
    }
    private static void AddFont(string name, string path)
    {
      fonts.Add(name, AssetManager.Content.Load<SpriteFont>(@"Fonts\" + path));
    }
    private static void AddEffect(string name, string path)
    {
      effects.Add(name, AssetManager.Content.Load<Effect>(@"Effects\" + path));
    }
  }
}
