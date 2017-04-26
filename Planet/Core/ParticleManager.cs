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
    List<Particle> particles;

    public ParticleManager()
    {
      particles = new List<Particle>();
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
    public Particle CreateParticle(Vector2 pos, Texture2D tex, Vector2 velocity, float lifeTime, Color color, float alpha, float rotationSpeed = 0.0f, float scale = 1.0f)
    {
      Particle p = new Particle(pos, tex, velocity, lifeTime, color, alpha, rotationSpeed, scale);
      AddParticle(p);
      return p;
    }
    public Particle CreateExplosion(Vector2 pos, float lifeTime, float alpha, float scale)
    {
      string texPath = "explosion0";
      int i = Utility.RandomInt(0, 9);
      texPath += i.ToString();
      Particle p = new Particle(pos, AssetManager.GetTexture(texPath), Vector2.Zero, lifeTime, Color.White, alpha, 0, scale);
      AddParticle(p);
      return p;
    }
    public Particle CreateHitEffect(Vector2 pos, float lifeTime, float minSpeed, float maxSpeed, Color color, float alpha, float scale, float variation)
    {
      string texPath = "star";
      int i = Utility.RandomInt(1, 4);
      texPath += i.ToString();
      float r = 1 + Utility.RandomFloat(-variation, variation);

      float xVel = Utility.RandomFloat(minSpeed, maxSpeed);
      float yVel = Utility.RandomFloat(minSpeed, maxSpeed);

      color = color * r;
      color.A = 255;

      Particle p = new Particle(pos, AssetManager.GetTexture(texPath), new Vector2(xVel, yVel) * r, lifeTime * r, color, alpha, 0, scale * r);
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
