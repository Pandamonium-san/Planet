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
    public float Alpha { get; set; }
    public float DriftSpeed
    {
      get { return driftSpeed; }
      set
      {
        driftSpeed = value;
        starGenerator = new Timer(Game1.ScreenHeight / (StarDensity * DriftSpeed), GenerateStar, true, true);
      }
    }
    public int StarDensity
    {
      get { return starDensity; }
      set
      {
        starDensity = value;
        starGenerator = new Timer(Game1.ScreenHeight / (StarDensity * DriftSpeed), GenerateStar, true, true);
      }
    }

    private int starsToSpawn;
    private int starDensity;
    private float driftSpeed;

    private Vector2 pos = new Vector2(-Game1.ScreenWidth, -Game1.ScreenHeight);
    private Texture2D background;
    private Texture2D[] starTex;
    private Timer starGenerator;
    private List<Particle> stars;
    private Sprite planet;

    public Background(float alpha = 1.0f, float driftSpeed = 75, int starDensity = 150)
    {
      Alpha = alpha;
      this.driftSpeed = driftSpeed;
      this.starDensity = starDensity;
      starsToSpawn = 1;

      background = AssetManager.GetTexture("bg_blue");
      planet = new Sprite(new Vector2(Game1.ScreenWidth / 2, Game1.ScreenHeight - 300), AssetManager.GetTexture("bg_planet"));
      planet.color = new Color(100, 100, 100);
      planet.layerDepth = 0.99f;

      stars = new List<Particle>();
      starTex = new Texture2D[3] { AssetManager.GetTexture("star1"), AssetManager.GetTexture("star2"), AssetManager.GetTexture("star3") };
      starGenerator = new Timer(0, GenerateStar, true, true);
      CalibrateStarGenerator();

      for (int i = 0; i < StarDensity; i++)
      {
        Vector2 pos = new Vector2(Utility.RandomFloat(0, Game1.ScreenWidth), Utility.RandomFloat(0, Game1.ScreenHeight));
        Particle p = CreateStar(pos, 60.0f, 0, DriftSpeed + 25, Color.White, 0.1f * Alpha, 0.5f, 0.5f);
        p.Velocity = Vector2.One * p.Velocity.Length();
        p.layerDepth = 1.0f;
        stars.Add(p);
      }
    }
    private void CalibrateStarGenerator()
    {
      starGenerator.Start(0.5f * Game1.ScreenHeight / (StarDensity * DriftSpeed));
      while (starGenerator.seconds < 1 / 60)
      {
        starGenerator.Start(starGenerator.seconds * 2);
        starsToSpawn *= 2;
      }
    }
    private void GenerateStar()
    {
      for (int i = 0; i < starsToSpawn; i++)
      {
        Vector2 pos = new Vector2(Utility.RandomFloat(-Game1.ScreenWidth + Game1.ScreenWidth - Game1.ScreenHeight, Game1.ScreenWidth - Game1.ScreenHeight), Utility.RandomFloat(-Game1.ScreenHeight, 0));
        Particle p = CreateStar(pos, 60.0f, 0, DriftSpeed + 30, Color.White, 0.1f * Alpha, 0.5f, 0.5f);
        p.Velocity = Vector2.One * p.Velocity.Length();
        p.layerDepth = 1.0f;
        stars.Add(p);
      }
    }
    private Particle CreateStar(Vector2 pos, float lifeTime, float minSpeed, float maxSpeed, Color color, float alpha, float scale, float variation)
    {
      int i = Utility.RandomInt(0, 2);
      float r = 1 + Utility.RandomFloat(-variation, variation);

      float xVel = Utility.RandomFloat(minSpeed, maxSpeed);
      float yVel = Utility.RandomFloat(minSpeed, maxSpeed);

      color = color * r;
      color.A = 255;

      Particle p = new Particle(pos, starTex[i], new Vector2(xVel, yVel) * r, lifeTime * r, color, alpha, 0, scale * r);
      return p;
    }
    public void Update(GameTime gameTime)
    {
      foreach (Particle star in stars)
        star.Update(gameTime);
      stars.RemoveAll(x => x.Disposed);
      starGenerator.Update(gameTime);
      this.pos += new Vector2(DriftSpeed, DriftSpeed) * (float)gameTime.ElapsedGameTime.TotalSeconds;
      if (pos.X > 0 || pos.Y > 0)
        pos = new Vector2(-Game1.ScreenWidth, -Game1.ScreenHeight);

      float v = 0.55f + (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * AudioManager.SplashSinCycle) * 0.15f;
      planet.color = new Color(v, v, v);
    }
    public void Draw(SpriteBatch spriteBatch, float a = 1.0f)
    {
      spriteBatch.Draw(background, pos, null, new Rectangle(0, 0, Game1.ScreenWidth * 2, Game1.ScreenHeight * 2), null, 0.0f, null, new Color(100, 100, 100) * a, SpriteEffects.None, 1.0f);
      foreach (Particle star in stars)
        star.Draw(spriteBatch, a);
      planet.Draw(spriteBatch, a);
    }
  }
}
