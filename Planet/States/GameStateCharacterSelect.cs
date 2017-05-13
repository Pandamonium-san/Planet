﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Planet
{
  class GameStateCharacterSelect : MenuGameState, IJoinable
  {
    private GameStateManager gsm;
    private MenuCursor cursor1, cursor2;
    private MenuController mc1, mc2;
    private Menu characterMenu;

    public GameStateCharacterSelect(GameStateManager gsm)
      : base()
    {
      this.gsm = gsm;
      characterMenu = Menu.CharSelect();
      if (gsm.P1.Joined)
        Join(gsm.P1);
      if (gsm.P2.Joined)
        Join(gsm.P2);
    }
    public void Join(Player player)
    {
      if (player.Index == PlayerIndex.One)
      {
        cursor1 = new MenuCursor(characterMenu, Color.PaleTurquoise);
        mc1 = new MenuController(player, cursor1, this);
      }
      else
      {
        cursor2 = new MenuCursor(characterMenu, Color.CornflowerBlue);
        mc2 = new MenuController(player, cursor2, this);
      }
    }
    public override void Confirm(MenuController mc)
    {
      if (fadeTimer.Counting || mc.Cursor.Locked)
        return;
      mc.Player.SelectedShip = mc.GetSelected().Name;
      mc.Cursor.Lock();
      if ((cursor1 == null || cursor1.Locked) && (cursor2 == null || cursor2.Locked))
      {
        gsm.Settings.startGame = true;
        FadeTransition(1.0f, gsm.Pop, false);
      }
      AudioManager.PlaySound("boop");
    }
    public override void Cancel(MenuController mc)
    {
      if (fadeTimer.Counting)
        return;
      if (mc.Cursor.Locked)
      {
        mc.Cursor.Unlock();
      }
      else
      {
        FadeTransition(1.0f, gsm.Pop, false);
      }
      AudioManager.PlaySound("boop2");
    }
    public override void Update(GameTime gameTime)
    {
      base.Update(gameTime);
      if (mc1 != null)
        mc1.Update(gameTime);
      if (mc2 != null)
        mc2.Update(gameTime);
    }
    public override void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Begin();
      if (cursor1 != null)
        cursor1.Draw(spriteBatch, a);
      if (cursor2 != null)
        cursor2.Draw(spriteBatch, a);
      characterMenu.Draw(spriteBatch, a);
      spriteBatch.End();
    }
    public void SetPlayerShip(Player player, string shipType)
    {
      player.SelectedShip = shipType;
    }
    public override void Entered()
    {
      FadeTransition(0.5f);
    }

    public override void Leaving()
    {
    }

    public override void Obscuring()
    {
    }

    public override void Revealed()
    {
      FadeTransition(0.5f);
      cursor1.Unlock();
      cursor2.Unlock();
      cursor1.alpha = 1.0f;
      cursor2.alpha = 1.0f;
    }
  }
}
