using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Planet
{
  class GameStateCharacterSelect : MenuGameState
  {
    private GameStateManager gsm;
    private GameSettings settings;
    private MenuCursor cursor1, cursor2;
    private MenuController mc1, mc2;
    private Menu characterMenu;

    public GameStateCharacterSelect(GameStateManager gsm, GameSettings settings)
      : base()
    {
      this.gsm = gsm;
      this.settings = settings;
      characterMenu = new Menu(3);

      Button b = new Button(new Vector2(Game1.ScreenWidth / 2f, 300), "Rewinder");
      b.AddText(AssetManager.GetFont("future18"), "Rewinder");
      characterMenu.AddSelection(0, b);

      b = new Button(new Vector2(Game1.ScreenWidth / 2f, 400), "Blinker");
      b.AddText(AssetManager.GetFont("future18"), "Blinker");
      characterMenu.AddSelection(1, b);

      b = new Button(new Vector2(Game1.ScreenWidth / 2f, 500), "Possessor");
      b.AddText(AssetManager.GetFont("future18"), "Possessor");
      characterMenu.AddSelection(2, b);

      cursor1 = new MenuCursor(characterMenu, AssetManager.GetTexture("grey_sliderRight"), false);
      cursor1.color = Color.PaleTurquoise;
      mc1 = new MenuController(PlayerIndex.One, cursor1, this);

      cursor2 = new MenuCursor(characterMenu, AssetManager.GetTexture("grey_sliderRight"), true);
      cursor2.color = Color.CornflowerBlue;
      mc2 = new MenuController(PlayerIndex.Two, cursor2, this);
    }
    public override void Confirm(MenuController mc)
    {
      if (fadeTimer.Counting)
        return;
      if (mc.GetCursor().Locked)
        return;
      switch (mc.GetSelected().Name)
      {
        case "Rewinder":
          SetPlayerShip(mc.PIndex, "RewinderShip");
          break;
        case "Blinker":
          SetPlayerShip(mc.PIndex, "BlinkerShip");
          break;
        case "Possessor":
          SetPlayerShip(mc.PIndex, "PossessorShip");
          break;
      }
      mc.GetCursor().alpha = 0.5f;
      mc.GetCursor().Lock();
      if (cursor1.Locked && cursor2.Locked)
      {
        settings.startGame = true;
        FadeTransition(1.0f, gsm.Pop, false);
      }
      AudioManager.PlaySound("confirm");
    }
    public override void Cancel(MenuController mc)
    {
      if (fadeTimer.Counting)
        return;
      if (mc.GetCursor().Locked)
      {
        mc.GetCursor().alpha = 1.0f;
        mc.GetCursor().Unlock();
      }
      else
      {
        FadeTransition(1.0f, gsm.Pop, false);
      }
      AudioManager.PlaySound("cancel");
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
      cursor1.Draw(spriteBatch, a);
      cursor2.Draw(spriteBatch, a);
      characterMenu.Draw(spriteBatch, a);
      spriteBatch.End();
    }
    public void SetPlayerShip(PlayerIndex pi, string shipType)
    {
      switch (pi)
      {
        case PlayerIndex.One:
          settings.p1StarterShip = shipType;
          break;
        case PlayerIndex.Two:
          settings.p2StarterShip = shipType;
          break;
      }
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
      FadeTransition(1.0f);
      cursor1.Unlock();
      cursor2.Unlock();
      cursor1.alpha = 1.0f;
      cursor2.alpha = 1.0f;
    }
  }
}
