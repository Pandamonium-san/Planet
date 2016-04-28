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
        List<GameObject> gameObjects;

        AIController ai;

        Player player1;
        Ship go;

        public ObjectManager()
        {
            gameObjects = new List<GameObject>();

            go = new TesekShip(new Vector2(500, 500));
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
            for (int i = 0; i < gameObjects.Count(); i++)
            {
                GameObject go = gameObjects[i];
                go.Update(gt);
                foreach (GameObject go2 in gameObjects)
                {
                    if (go == go2 || go.destroy)
                        continue;
                    if (go.IsColliding(go2))
                    {
                        go.DoCollision(go2);
                        go2.DoCollision(go);
                    }
                }
                if (go.destroy)
                {
                    gameObjects.Remove(go);
                    --i;
                }
            }
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

        public void Draw(SpriteBatch sb)
        {
            sb.Begin();
            foreach (GameObject go in gameObjects)
            {
                go.Draw(sb);
            }
            sb.DrawString(AssetManager.GetFont("font1"), (this.go.rotation * 57.3f).ToString(), new Vector2(10, 10), Color.Red);
            sb.End();
        }
    }
}
