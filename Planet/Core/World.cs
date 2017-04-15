using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Planet
{
  /// <summary>
  /// Contains all the objects and handles logic between them.
  /// </summary>
  public class World
  {
    public int Frames { get; private set; }
    public ParticleManager Particles { get; private set; }

    private List<GameObject> gameObjects;
    private List<Projectile> projectiles;
    private SpriteFont font;
    private Effect effect;
    private Matrix transformMatrix;

    public World()
    {
      font = AssetManager.GetFont("font1");
      effect = AssetManager.GetEffect("ColorChanger");
      Particles = new ParticleManager();
      gameObjects = new List<GameObject>();
      projectiles = new List<Projectile>();
      transformMatrix = Matrix.CreateScale(1.0f);
    }

    public void Update(GameTime gameTime)
    {
      for (int i = 0; i < projectiles.Count(); i++)
      {
        projectiles[i].Update(gameTime);
      }

      for (int i = 0; i < gameObjects.Count(); i++)
      {
        GameObject go = gameObjects[i];
        go.Update(gameTime);
        if (!go.isActive)
          continue;

        foreach (Projectile p in projectiles)
        {
          if (!p.isActive)
            continue;
          if (p.IsColliding(go))
          {
            go.DoCollision(p);
            p.DoCollision(go);
          }
          if (!go.isActive)
            break;
        }
      }
      Particles.Update(gameTime);

      projectiles.RemoveAll(x => x.disposed);
      gameObjects.RemoveAll(x => x.disposed == true);
    }
    public List<GameObject> GetPlayers()
    {
      List<GameObject> result = new List<GameObject>();
      foreach (GameObject go in gameObjects)
      {
        if (go.layer == Layer.PLAYER_SHIP)
          result.Add(go);
      }
      return result;
    }
    public List<GameObject> GetGameObjects()
    {
      return gameObjects;
    }
    public void PostGameObj(GameObject go)
    {
      gameObjects.Add(go);
    }

    public void PostProjectile(Projectile p)
    {
      projectiles.Add(p);
    }
    public void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Begin(
        SpriteSortMode.BackToFront,
        BlendState.AlphaBlend,
        SamplerState.LinearClamp,
        DepthStencilState.None,
        RasterizerState.CullCounterClockwise,
        effect,
        transformMatrix
        );
      foreach (GameObject go in gameObjects)
      {
        go.Draw(spriteBatch);
      }
      foreach (Projectile p in projectiles)
      {
        p.Draw(spriteBatch);
      }
      Particles.Draw(spriteBatch);
      spriteBatch.DrawString(font, "Objects: " + (this.gameObjects.Count()).ToString(), new Vector2(0, 20), Color.Red);
      spriteBatch.DrawString(font, "Projectiles: " + (this.projectiles.Count()).ToString(), new Vector2(0, 40), Color.Red);

      spriteBatch.End();
    }

  }
}
