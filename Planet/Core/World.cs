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
    // ships and destructible projectiles
    public List<GameObject> gameObjects;
    // do not check collision with each other (there could easily be a million checks each frame)
    public List<Projectile> projectiles;

    private SpriteFont font;

    public World()
    {
      font = AssetManager.GetFont("font1");
      gameObjects = new List<GameObject>();
      projectiles = new List<Projectile>();


    }

    public void Update(GameTime gt)
    {
      for (int i = 0; i < projectiles.Count(); i++)
      {
        projectiles[i].Update(gt);
      }

      for (int i = 0; i < gameObjects.Count(); i++)
      {
        GameObject go = gameObjects[i];

        go.Update(gt);

        // don't check collision if rewinding or inactive
        if (go.isRewinding || !go.isActive)
          continue;

        // collision check
        foreach (GameObject go2 in gameObjects)
        {
          if (go == go2 || go.disposed)
            continue;
          if (go.IsColliding(go2))
          {
            go.DoCollision(go2);
            go2.DoCollision(go);
          }
        }

        // projectile collision check
        foreach (Projectile p in projectiles)
        {
          if (p.isDead || !p.isActive)
            continue;

          if (p.IsColliding(go))
          {
            go.DoCollision(p);
            p.DoCollision(go);
          }
        }
      }

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

    public void PostGameObj(GameObject go)
    {
      gameObjects.Add(go);
    }

    public void PostProjectile(Projectile p)
    {
      projectiles.Add(p);
    }

    public void Draw(SpriteBatch sb)
    {
      sb.Begin();
      foreach (GameObject go in gameObjects)
      {
        go.Draw(sb);
      }
      foreach (Projectile p in projectiles)
      {
        p.Draw(sb);
      }
      sb.DrawString(font, "Objects: " + (this.gameObjects.Count()).ToString(), new Vector2(0, 20), Color.Red);
      sb.DrawString(font, "Projectiles: " + (this.projectiles.Count()).ToString(), new Vector2(0, 40), Color.Red);

      sb.End();
    }

  }
}
