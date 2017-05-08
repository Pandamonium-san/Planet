using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  class Background
  {
    Vector2 pos = new Vector2(-Game1.ScreenWidth, -Game1.ScreenHeight);
    private Texture2D background;
    private World world;
    private Timer starGenerator;
    private float driftSpeed;
    private int starDensity = 150;
    public Background(World world)
    {
      this.world = world;
      driftSpeed = 75;
      background = AssetManager.GetTexture("bg_blue");

      starGenerator = new Timer(Game1.ScreenHeight / (starDensity * driftSpeed), CreateStar, true, true);
      for (int i = 0; i < starDensity; i++)
      {
        Vector2 pos = new Vector2(Utility.RandomFloat(0, Game1.ScreenWidth), Utility.RandomFloat(0, Game1.ScreenHeight));
        Particle p = world.Particles.CreateStar(pos, 600.0f, 0, driftSpeed + 25, Color.White, 0.1f, 0.5f, 0.5f);
        p.Velocity = Vector2.One * p.Velocity.Length();
        p.layerDepth = 1.0f;
      }
    }
    private void CreateStar()
    {
      Vector2 pos = new Vector2(Utility.RandomFloat(-Game1.ScreenWidth, 0), Utility.RandomFloat(-Game1.ScreenHeight, 0));
      Particle p = world.Particles.CreateStar(pos, 600.0f, 0, driftSpeed + 30, Color.White, 0.1f, 0.5f, 0.5f);
      p.Velocity = Vector2.One * p.Velocity.Length();
      p.layerDepth = 1.0f;
    }
    public void Update(GameTime gameTime)
    {
      starGenerator.Update(gameTime);
      this.pos += new Vector2(driftSpeed, driftSpeed) * (float)gameTime.ElapsedGameTime.TotalSeconds;
      if (pos.X > 0 || pos.Y > 0)
        pos = new Vector2(-Game1.ScreenWidth, -Game1.ScreenHeight);
    }
    public void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(background, pos, null, new Rectangle(0, 0, Game1.ScreenWidth * 2, Game1.ScreenHeight * 2), null, 0.0f, null, new Color(100, 100, 100), SpriteEffects.None, 1.0f);
    }
  }
}
