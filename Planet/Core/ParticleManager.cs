using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  public class ParticleManager
  {
    Texture2D[] starTex;
    Texture2D[] explosionTex;
    List<Particle> particles;

    public ParticleManager()
    {
      particles = new List<Particle>();
      starTex = new Texture2D[3] { AssetManager.GetTexture("star1"), AssetManager.GetTexture("star2"), AssetManager.GetTexture("star3") };
      explosionTex = new Texture2D[9] {
        AssetManager.GetTexture("explosion00"),
        AssetManager.GetTexture("explosion01"),
        AssetManager.GetTexture("explosion02"),
        AssetManager.GetTexture("explosion03"),
        AssetManager.GetTexture("explosion04"),
        AssetManager.GetTexture("explosion05"),
        AssetManager.GetTexture("explosion06"),
        AssetManager.GetTexture("explosion07"),
        AssetManager.GetTexture("explosion08") };
    }
    public void AddParticle(Particle p)
    {
      particles.Add(p);
    }
    public Particle CreateParticle(Vector2 pos, Texture2D tex, float xVelMin, float xVelMax, float yVelMin, float yVelMax, float lifeTime, Color color, float alpha, float rotationSpeed = 0.0f, float scale = 1.0f)
    {
      float xVel = Utility.RandomFloat(xVelMin, xVelMax);
      float yVel = Utility.RandomFloat(yVelMin, yVelMax);
      float rotation = Utility.RandomFloat(0, (float)Math.PI * 2);
      Particle p = new Particle(pos, tex, new Vector2(xVel, yVel), lifeTime, color, alpha, rotationSpeed, scale);
      p.Rotation = rotation;
      AddParticle(p);
      return p;
    }
    public Particle CreateParticle(Vector2 pos, Texture2D tex, Vector2 velocity, float lifeTime, Color color, float alpha, float rotationSpeed = 0.0f, float scale = 1.0f, float rotation = 0.0f)
    {
      Particle p = new Particle(pos, tex, velocity, lifeTime, color, alpha, rotationSpeed, scale);
      p.Rotation = rotation;
      AddParticle(p);
      return p;
    }
    public Particle CreateExplosion(Vector2 pos, float lifeTime, float alpha, float scale)
    {
      int i = Utility.RandomInt(0, 9);
      Particle p = new Particle(pos, explosionTex[i], Vector2.Zero, lifeTime, Color.White, alpha, 0, scale);
      AddParticle(p);
      return p;
    }
    public Particle CreateStar(Vector2 pos, float lifeTime, float minSpeed, float maxSpeed, Color color, float alpha, float scale, float variation)
    {
      int i = Utility.RandomInt(0, 2);
      float r = 1 + Utility.RandomFloat(-variation, variation);

      float xVel = Utility.RandomFloat(minSpeed, maxSpeed);
      float yVel = Utility.RandomFloat(minSpeed, maxSpeed);

      color = color * r;
      color.A = 255;

      Particle p = new Particle(pos, starTex[i], new Vector2(xVel, yVel) * r, lifeTime * r, color, alpha, 0, scale * r);
      AddParticle(p);
      return p;
    }

    public void Update(GameTime gameTime)
    {
      foreach (Particle p in particles)
      {
        p.Update(gameTime);
      }
      particles.RemoveAll(x => x.Disposed);
    }
    public void Draw(SpriteBatch spriteBatch)
    {
      foreach (Particle p in particles)
      {
        p.Draw(spriteBatch);
      }
    }
  }
}
