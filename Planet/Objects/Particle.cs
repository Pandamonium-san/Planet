using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  public class Particle : Sprite
  {
    public bool Disposed { get; protected set; }
    float initialAlpha;
    Vector2 velocity;
    float rotationSpeed;
    Timer lifeTimer;
    public Particle(Vector2 pos, Texture2D tex, Vector2 velocity, float lifeTime, Color color, float alpha, float rotationSpeed = 0.0f, float scale = 1.0f) 
      : base(pos, tex)
    {
      this.initialAlpha = alpha;
      this.color = color;
      this.Scale = scale;
      this.velocity = velocity;
      this.rotationSpeed = rotationSpeed;
      lifeTimer = new Timer(lifeTime, () => Die(), true);
    }
    public void Die()
    {
      Disposed = true;
    }
    public void Update(GameTime gameTime)
    {
      lifeTimer.Update(gameTime);
      alpha = initialAlpha - initialAlpha * (float)lifeTimer.Fraction;
      this.Pos += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
      this.Rotation += rotationSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
    }
  }
}
