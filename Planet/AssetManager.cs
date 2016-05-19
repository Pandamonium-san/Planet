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

            AddFont("font1", "font1");
        }

        public static Texture2D GetTexture(string name)
        {
            return textures[name];
        }
        public static SpriteFont GetFont(string name)
        {
            return fonts[name];
        }

        private static void AddTexture(string name, string path)
        {
            textures.Add(name, AssetManager.Content.Load<Texture2D>(@"Textures/" + path));
        }
        private static void AddFont(string name, string path)
        {
            fonts.Add(name, AssetManager.Content.Load<SpriteFont>(@"Fonts/" + path));
        }

    }
}
