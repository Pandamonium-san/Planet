using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  class GameStatePaused : MenuGameState
  {
    private GameStateManager gsm;
    private SpriteFont future18;
    private MenuCursor cursor1, cursor2;
    private MenuController mc1, mc2;
    private Text titleText;
    private Menu menu;
    private Sprite overlay;

    public GameStatePaused(GameStateManager gameStateManager)
    {
      this.gsm = gameStateManager;
      future18 = AssetManager.GetFont("future18");

      titleText = new Text(AssetManager.GetFont("future48"), "Planet", new Vector2(Game1.ScreenWidth / 2f, 200), Color.White);
      menu = new Menu(2);

      Button b = new Button(new Vector2(Game1.ScreenWidth / 2f, 300), "Resume");
      b.AddText(future18, "Resume");
      menu.AddSelection(0, b);

      b = new Button(new Vector2(Game1.ScreenWidth / 2f, 400), "Main Menu");
      b.AddText(future18, "Main Menu");
      menu.AddSelection(1, b);

      cursor1 = new MenuCursor(menu, AssetManager.GetTexture("grey_sliderRight"), false);
      cursor1.color = Color.PaleTurquoise;
      mc1 = new MenuController(PlayerIndex.One, cursor1, this);

      cursor2 = new MenuCursor(menu, AssetManager.GetTexture("grey_sliderRight"), true);
      cursor2.color = Color.CornflowerBlue;
      mc2 = new MenuController(PlayerIndex.Two, cursor2, this);

      overlay = new Sprite(new Vector2(Game1.ScreenWidth / 2, Game1.ScreenHeight / 2), AssetManager.Pixel);
      overlay.Scale = 2000;
      overlay.color = Color.Black;
      overlay.alpha = 0.5f;
    }
    public override void Confirm(MenuController mc)
    {
      if (fadeTimer.Counting)
        return;
      switch (mc.GetSelected().Name)
      {
        case "Resume":
          FadeTransition(0.5f, gsm.Pop, false);
          break;
        case "Main Menu":
          FadeTransition(0.5f, ToMenu, false);
          break;
      }
      AudioManager.PlaySound("confirm");
    }
    public override void Cancel(MenuController mc)
    {
      if (fadeTimer.Counting)
        return;
      FadeTransition(0.5f, gsm.Pop, false);
      AudioManager.PlaySound("cancel");
    }
    private void ToMenu()
    {
      gsm.Pop();
      gsm.ChangeState(new GameStateTitleScreen(gsm));
    }
    public override void Update(GameTime gameTime)
    {
      base.Update(gameTime);
      mc1.Update(gameTime);
      mc2.Update(gameTime);
    }
    public override void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Begin();
      overlay.Draw(spriteBatch, a);
      cursor1.Draw(spriteBatch, a);
      cursor2.Draw(spriteBatch, a);
      titleText.Draw(spriteBatch, a);
      menu.Draw(spriteBatch, a);
      spriteBatch.End();
    }
    public override void Entered()
    {
      FadeTransition(0.5f);
    }
    public override void Leaving()
    {
    }
    public override void Revealed()
    {
    }
    public override void Obscuring()
    {
      UpdateEnabled = false;
      DrawEnabled = false;
    }
  }
}
