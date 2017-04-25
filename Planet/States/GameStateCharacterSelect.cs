using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Planet
{
  class GameStateCharacterSelect : GameState, IMenuGameState
  {
    private GameStateManager gsm;
    private GameSettings gameSettings;
    private MenuCursor cursor1, cursor2;
    private MenuController mc1, mc2;
    private Menu characterMenu;

    public GameStateCharacterSelect(GameStateManager gsm)
      : base()
    {
      this.gsm = gsm;
      characterMenu = new Menu(3);
      gameSettings = new GameSettings();

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
    public void Confirm(MenuController mc)
    {
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
      mc.GetCursor().color = Color.White;
      mc.GetCursor().Lock();
      if (cursor1.Locked && cursor2.Locked)
        gsm.Push(new GameStatePlaying(gsm, gameSettings));
    }
    public void SetPlayerShip(PlayerIndex pi, string shipType)
    {
      switch (pi)
      {
        case PlayerIndex.One:
          gameSettings.p1StarterShip = shipType;
          break;
        case PlayerIndex.Two:
          gameSettings.p2StarterShip = shipType;
          break;
      }
    }
    public void Cancel(MenuController mc)
    {
      gsm.Pop();
    }
    public override void Update(GameTime gt)
    {
      mc1.Update(gt);
      mc2.Update(gt);
    }
    public override void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Begin();
      cursor1.Draw(spriteBatch);
      cursor2.Draw(spriteBatch);
      characterMenu.Draw(spriteBatch);
      spriteBatch.End();
    }

    public override void Entered()
    {
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
