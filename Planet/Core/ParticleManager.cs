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
    public void CreateParticle(Vector2 pos, Texture2D tex, float xVelMin, float xVelMax, float yVelMin, float yVelMax, float lifeTime, Color color, float alpha, float rotationSpeed = 0.0f, float scale = 1.0f)
    {
      float xVel = Utility.RandomFloat(xVelMin, xVelMax);
      float yVel = Utility.RandomFloat(yVelMin, yVelMax);
      Particle p = new Particle(pos, tex, new Vector2(xVel, yVel), lifeTime, color, alpha, rotationSpeed, scale);
      AddParticle(p);
    }
    public void CreateParticle(Vector2 pos, Texture2D tex, Vector2 velocity, float lifeTime, Color color, float alpha, float rotationSpeed = 0.0f, float scale = 1.0f)
    {
      Particle p = new Particle(pos, tex, velocity, lifeTime, color, alpha, rotationSpeed, scale);
      AddParticle(p);
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
