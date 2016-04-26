using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
    public class SpriteManager
    {
        ContentManager Content;
        public Texture2D FillTexture;
        Dictionary<string, Texture2D> dict;

        public SpriteManager(ContentManager content)
        {
            this.Content = content;
            dict = new Dictionary<string, Texture2D>();
        }

        public void LoadContent(ContentManager cm)
        {
            AddToDict("Fill", "Filltexture");
            AddToDict("Ship1", "Image1");
            AddToDict("Proj1", "Image2");
        }

        public Texture2D GetTexture(string name)
        {
            return dict[name];
        }

        private void AddToDict(string name, string path)
        {
            dict.Add(name, LoadTexture(path));
        }

        private Texture2D LoadTexture(string path)
        {
            return Content.Load<Texture2D>(path);
        }
    }
}
