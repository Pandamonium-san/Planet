using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  class GameStateMenuMain : MenuGameState, IJoinable
  {
    private GameStateManager gsm;
    private SpriteFont future18;
    private MenuCursor cursor1, cursor2;
    private MenuController mc1, mc2;
    private Menu menuMain;

    public GameStateMenuMain(GameStateManager gameStateManager)
    {
      gsm = gameStateManager;
      future18 = AssetManager.GetFont("future18");
      menuMain = Menu.Main();
    }
    public void Join(Player player)
    {
      if (player.Index == PlayerIndex.One)
      {
        cursor1 = new MenuCursor(menuMain, player.Color);
        mc1 = new MenuController(player, cursor1, this);
      }
      else
      {
        cursor2 = new MenuCursor(menuMain, player.Color);
        mc2 = new MenuController(player, cursor2, this);
      }
    }
    public override void Confirm(MenuController mc)
    {
      if (fadeTimer.Counting)
        return;
      switch (mc.GetSelected().Name)
      {
        case "Play":
          FadeTransition(0.5f, ToCharacterSelect, false);
          AudioManager.PlaySound("boop");
          break;
        case "Options":
          //gsm.Push(new GameStateCharacterSelect(gsm));
          //push options state to gsm
          break;
        case "Credits":
          // ???
          break;
      }
    }
    public override void Cancel(MenuController mc)
    {
    }
    public override void Update(GameTime gameTime)
    {
      base.Update(gameTime);
      foreach (SelectionBox sb in menuMain.GetBoxes())
        sb.alpha = 0.6f;
      if (mc1 != null)
      {
        mc1.Update(gameTime);
        mc1.GetSelected().alpha = 0.9f;
      }
      if (mc2 != null)
      {
        mc2.Update(gameTime);
        mc2.GetSelected().alpha = 0.9f;
      }
    }
    public override void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Begin();
      if (cursor1 != null)
        cursor1.Draw(spriteBatch, a);
      if (cursor2 != null)
        cursor2.Draw(spriteBatch, a);
      menuMain.Draw(spriteBatch, a);
      spriteBatch.End();
    }
    public override void Entered()
    {
      FadeTransition(1.0f);
    }
    public override void Leaving()
    {
    }
    public override void Revealed()
    {
      if (gsm.Settings.startGame)
        gsm.Pop();
      FadeTransition(0.5f);
      UpdateEnabled = true;
      DrawEnabled = true;
    }
    public override void Obscuring()
    {
      UpdateEnabled = false;
      DrawEnabled = false;
    }
    private void ToCharacterSelect()
    {
      gsm.Push(new GameStateCharacterSelect(gsm));
    }
  }
}
