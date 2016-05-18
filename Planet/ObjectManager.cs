using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
    public class ObjectManager
    {
        //ships and destructible projectiles
        private List<GameObject> gameObjects;

        //for projectiles that only collide with ships
        private List<Projectile> projectiles;

        private AIController ai;

        private Player player1;
        private Ship go;

        public ObjectManager()
        {
            gameObjects = new List<GameObject>();
            projectiles = new List<Projectile>();

            go = new BlinkerShip(new Vector2(500, 500));
            PostGameObj(go);

            Ship s = new PumpkinShip(new Vector2(800, 500));
            ai = new AIController();
            ai.SetActor(s);
            PostGameObj(s);

            player1 = new Player(PlayerIndex.One);
            player1.SetShip(go);
        }

        public void Update(GameTime gt)
        {
            player1.Update(gt);
            ai.Update(gt);

            //foreach (Projectile p in projectiles)
            //{
            //    p.Update(gt);
            //}
            for (int i = 0; i < projectiles.Count(); i++)
            {
                projectiles[i].Update(gt);
            }
            for (int i = 0; i < gameObjects.Count(); i++)
            {
                GameObject go = gameObjects[i];
                go.Update(gt);

                //collision check
                foreach (GameObject go2 in gameObjects)
                {
                    if (go == go2 || go.destroyed)
                        continue;
                    if (go.IsColliding(go2))
                    {
                        go.DoCollision(go2);
                        go2.DoCollision(go);
                    }
                }

                //projectile collision check
                foreach(Projectile p in projectiles)
                {
                    if(p.IsColliding(go))
                    {
                        go.DoCollision(p);
                        p.DoCollision(go);
                    }
                }
            }

            projectiles.RemoveAll(x => x.destroyed);
            gameObjects.RemoveAll(x => x.destroyed == true);
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
            sb.DrawString(AssetManager.GetFont("font1"), "Objects: " + (this.gameObjects.Count()).ToString(), new Vector2(0, 20), Color.Red);
            sb.DrawString(AssetManager.GetFont("font1"), "Projectiles: " + (this.projectiles.Count()).ToString(), new Vector2(0, 40), Color.Red);
            sb.DrawString(AssetManager.GetFont("font1"), "Collision checks: " + (this.collisionChecksPerFrame).ToString(), new Vector2(0, 60), Color.Red);
            collisionChecksPerFrame = 0;
            sb.End();
        }

        public int collisionChecksPerFrame;
    }
}
