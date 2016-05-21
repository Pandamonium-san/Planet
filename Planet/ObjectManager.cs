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
    public class ObjectManager
    {
        // ships and destructible projectiles
        public List<GameObject> gameObjects;

        // do not check collision with each other (there could easily be million checks each frame)
        public List<Projectile> projectiles;

        private SpriteFont font;

        private AIController ai;
        private Player player1;
        private RewinderShip ship;

        public ObjectManager()
        {
            font = AssetManager.GetFont("font1");
            gameObjects = new List<GameObject>();
            projectiles = new List<Projectile>();

            //go = new BlinkerShip(new Vector2(500, 500));
            ship = new RewinderShip(new Vector2(500, 500));
            PostGameObj(ship);

            Ship s = new PumpkinShip(new Vector2(800, 500));
            ai = new AIController(s);
            PostGameObj(s);

            player1 = new Player(PlayerIndex.One);
            player1.SetShip(ship);
        }

        public void Update(GameTime gt)
        {
            player1.Update(gt);
            ai.Update(gt);

            for (int i = 0; i < projectiles.Count(); i++)
            {
                projectiles[i].Update(gt);
            }

            for (int i = 0; i < gameObjects.Count(); i++)
            {
                GameObject go = gameObjects[i];

                go.Update(gt);

                // don't check collision if rewinding or inactive
                if (go.rewind || !go.isActive)
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

        public List<Player> GetPlayers()
        {
            List<Player> result = new List<Player>();
            result.Add(player1);
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
            sb.DrawString(font, "Collision checks: " + (this.collisionChecksPerFrame).ToString(), new Vector2(0, 60), Color.Red);
            collisionChecksPerFrame = 0;
            sb.End();
        }

        public int collisionChecksPerFrame;
    }
}
