using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Planet
{
  class GameStateCredits : GameState
  {
    private GameStateManager gsm;
    private List<Text> texts;
    private Sprite overlay;

    public GameStateCredits(GameStateManager gsm)
    {
      this.gsm = gsm;
      SpriteFont future48 = AssetManager.GetFont("future48");
      SpriteFont future18 = AssetManager.GetFont("future24");
      texts = new List<Text>();

      texts.Add(new Text(future48, "Created by Henrik Phan", new Vector2(Game1.ScreenWidth / 2, Game1.ScreenHeight / 4 + 100), Color.White, Text.Align.Center, 1.0f));

      texts.Add(new Text(future18, "Art", new Vector2(Game1.ScreenWidth / 3, Game1.ScreenHeight / 4 + 250), Color.White, Text.Align.Center, 1.0f));
      texts.Add(new Text(future18, "Kenney (www.kenney.nl)", new Vector2(Game1.ScreenWidth / 3, Game1.ScreenHeight / 4 + 300), Color.White, Text.Align.Center, 0.7f));
      texts.Add(new Text(future18, "Earth artwork - Heath Rezabek", new Vector2(Game1.ScreenWidth / 3, Game1.ScreenHeight / 4 + 350), Color.White, Text.Align.Center, 0.6f));

      texts.Add(new Text(future18, "Sound", new Vector2(Game1.ScreenWidth / 3 * 2, Game1.ScreenHeight / 4 + 250), Color.White, Text.Align.Center, 1.0f));
      texts.Add(new Text(future18, "www.bfxr.net", new Vector2(Game1.ScreenWidth / 3 * 2, Game1.ScreenHeight / 4 + 300), Color.White, Text.Align.Center, 0.7f));
      texts.Add(new Text(future18, "PerituneMaterial - Splash", new Vector2(Game1.ScreenWidth / 3 * 2, Game1.ScreenHeight / 4 + 350), Color.White, Text.Align.Center, 0.7f));
      texts.Add(new Text(future18, "Kubbi - Firelight", new Vector2(Game1.ScreenWidth / 3 * 2, Game1.ScreenHeight / 4 + 400), Color.White, Text.Align.Center, 0.7f));
      texts.Add(new Text(future18, "Aethernaut - Shine Get", new Vector2(Game1.ScreenWidth / 3 * 2, Game1.ScreenHeight / 4 + 450), Color.White, Text.Align.Center, 0.7f));

      overlay = new Sprite(new Vector2(Game1.ScreenWidth / 2, Game1.ScreenHeight / 2), AssetManager.Pixel);
      overlay.Scale = 2000;
      overlay.color = Color.Black;
      overlay.alpha = 0.2f;
    }
    public override void Update(GameTime gameTime)
    {
      base.Update(gameTime);
      if (fadeTimer.Counting)
        return;

      if (InputHandler.IsButtonDown(PlayerIndex.One, PlayerInput.Start) ||
          InputHandler.IsButtonDown(PlayerIndex.Two, PlayerInput.Start) ||
          InputHandler.IsButtonDown(PlayerIndex.One, PlayerInput.Yellow) ||
          InputHandler.IsButtonDown(PlayerIndex.Two, PlayerInput.Yellow))
      {
        FadeTransition(1.0f, gsm.Pop, false);
        AudioManager.PlaySound("boop");
      }
    }
    public override void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Begin();
      overlay.Draw(spriteBatch, a);
      foreach (Text t in texts)
      {
        t.Draw(spriteBatch, a);
      }
      spriteBatch.End();
    }
    public override void Entered()
    {
      FadeTransition(1.0f);
    }

    public override void Leaving()
    {
    }

    public override void Obscuring()
    {
    }

    public override void Revealed()
    {
    }
  }
}
