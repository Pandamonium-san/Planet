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
    public float InitialAlpha { get; set; }
    public Vector2 Velocity { get; set; }
    public float RotationSpeed { get; set; }
    Timer lifeTimer;
    public Particle(Vector2 pos, Texture2D tex, Vector2 velocity, float lifeTime, Color color, float alpha, float rotationSpeed = 0.0f, float scale = 1.0f) 
      : base(pos, tex)
    {
      this.InitialAlpha = alpha;
      this.color = color;
      this.Scale = scale;
      this.Velocity = velocity;
      this.RotationSpeed = rotationSpeed;
      lifeTimer = new Timer(lifeTime, () => Die(), true);
    }
    public void Die()
    {
      Disposed = true;
    }
    public void Update(GameTime gameTime)
    {
      lifeTimer.Update(gameTime);
      alpha = InitialAlpha - InitialAlpha * (float)lifeTimer.Fraction;
      this.localPos += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
      this.localRotation += RotationSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
    }
  }
}
