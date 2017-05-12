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
  class GameStateMenuMain : MenuGameState
  {
    private GameStateManager gsm;
    private GameSettings settings;
    private SpriteFont future18;
    private MenuCursor cursor1, cursor2;
    private MenuController mc1, mc2;
    private Text titleText;
    private Menu mainMenu;


    public GameStateMenuMain(GameStateManager gameStateManager, GameSettings settings)
    {
      gsm = gameStateManager;
      this.settings = settings;
      future18 = AssetManager.GetFont("future18");

      titleText = new Text(AssetManager.GetFont("future48"), "Planet", new Vector2(Game1.ScreenWidth / 2f, 200), Color.White);
      mainMenu = new Menu(3);

      Button b = new Button(new Vector2(Game1.ScreenWidth / 2f, 300), "Play");
      b.AddText(future18, "Play");
      mainMenu.AddSelection(0, b);

      b = new Button(new Vector2(Game1.ScreenWidth / 2f, 400), "Options");
      b.AddText(future18, "Options");
      mainMenu.AddSelection(1, b);

      b = new Button(new Vector2(Game1.ScreenWidth / 2f, 500), "Credits");
      b.AddText(future18, "Credits");
      mainMenu.AddSelection(2, b);

      cursor1 = new MenuCursor(mainMenu, AssetManager.GetTexture("grey_sliderRight"), false);
      cursor1.color = Color.PaleTurquoise;
      mc1 = new MenuController(PlayerIndex.One, cursor1, this);

      cursor2 = new MenuCursor(mainMenu, AssetManager.GetTexture("grey_sliderRight"), true);
      cursor2.color = Color.CornflowerBlue;
      mc2 = new MenuController(PlayerIndex.Two, cursor2, this);
    }
    public override void Confirm(MenuController mc)
    {
      if (fadeTimer.Counting)
        return;
      switch (mc.GetSelected().Name)
      {
        case "Play":
          FadeTransition(1.0f, ToCharacterSelect, false);
          break;
        case "Options":
          //gsm.Push(new GameStateCharacterSelect(gsm));
          //push options state to gsm
          break;
        case "Credits":
          // ???
          break;
      }
      AudioManager.PlaySound("confirm");
    }
    public override void Cancel(MenuController mc)
    {
      if (fadeTimer.Counting)
        return;
      switch (mc.GetSelected().Name)
      {
        case "Options":
          //gsm.Push(new GameStateCharacterSelect(gsm));
          //push options state to gsm
          break;
        case "Credits":
          // ???
          break;
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
      titleText.Draw(spriteBatch, a);
      mainMenu.Draw(spriteBatch, a);
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
      if (settings.startGame)
        gsm.Pop();
      FadeTransition(1.0f);
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
      gsm.Push(new GameStateCharacterSelect(gsm, settings));
    }
  }
}
