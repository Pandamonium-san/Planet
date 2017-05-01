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
    private Queue<GameObject> goToAdd;
    private Queue<Projectile> pToAdd;
    private SpriteFont font;
    private Effect effect;
    private Matrix transformMatrix;

    public World()
    {
      font = AssetManager.GetFont("font1");
      effect = AssetManager.GetEffect("ColorChanger");
      Particles = new ParticleManager();
      goToAdd = new Queue<GameObject>();
      pToAdd = new Queue<Projectile>();
      gameObjects = new List<GameObject>();
      projectiles = new List<Projectile>();
      transformMatrix = Matrix.CreateScale(1.0f);
    }

    public void Update(GameTime gameTime)
    {
      while (goToAdd.Count > 0)
        gameObjects.Add(goToAdd.Dequeue());
      while (pToAdd.Count > 0)
        projectiles.Add(pToAdd.Dequeue());

      for (int i = 0; i < projectiles.Count(); i++)
        projectiles[i].Update(gameTime);
      for (int i = 0; i < gameObjects.Count(); i++)
        gameObjects[i].Update(gameTime);

      for (int i = 0; i < gameObjects.Count(); i++)
      {
        GameObject go = gameObjects[i];
        if (!go.IsActive || !go.CollisionEnabled)
          continue;

        for (int j = 0; j < gameObjects.Count(); j++)
        {
          GameObject go2 = gameObjects[j];
          if (!go2.IsActive || !go2.CollisionEnabled || go == go2)
            continue;
          if (go.IsColliding(go2))
          {
            go.DoCollision(go2);
            go2.DoCollision(go);
            if (!go.IsActive || !go.CollisionEnabled)
              break;
          }
        }

        for (int j = 0; j < projectiles.Count(); j++)
        {
          Projectile p = projectiles[j];
          if (!p.IsActive || !p.CollisionEnabled)
            continue;
          if (p.IsColliding(go))
          {
            go.DoCollision(p);
            p.DoCollision(go);
            if (!go.IsActive || !go.CollisionEnabled)
              break;
          }
        }
      }

      Particles.Update(gameTime);

      projectiles.RemoveAll(x => x.Disposed);
      gameObjects.RemoveAll(x => x.Disposed == true);
    }
    public List<GameObject> GetPlayers()
    {
      List<GameObject> result = new List<GameObject>();
      foreach (GameObject go in gameObjects)
      {
        if (go.Layer == Layer.PLAYER_SHIP)
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
      goToAdd.Enqueue(go);
      go.layerDepth += 0.00001f * gameObjects.Count;
    }
    public void PostProjectile(Projectile p)
    {
      pToAdd.Enqueue(p);
      p.layerDepth += 0.00001f * projectiles.Count;
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

      //debug
      if (Game1.debugMode)
      {
        spriteBatch.DrawString(font, "Objects: " + (this.gameObjects.Count()).ToString(), new Vector2(0, 20), Color.Red);
        spriteBatch.DrawString(font, "Projectiles: " + (this.projectiles.Count()).ToString(), new Vector2(0, 40), Color.Red);
      }

      spriteBatch.End();
    }

  }
}
