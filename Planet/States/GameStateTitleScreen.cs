using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  class GameStateTitleScreen : GameState, IMenuGameState
  {
    private GameStateManager gsm;

    private MenuCursor cursor1, cursor2;
    private MenuController mc1, mc2;
    private Text titleText;
    private Menu mainMenu;

    public GameStateTitleScreen(GameStateManager gameStateManager)
    {
      gsm = gameStateManager;

      titleText = new Text(AssetManager.GetFont("future48"), "Planet", new Vector2(Game1.ScreenWidth / 2f, 200), Color.White);
      mainMenu = new Menu(3);

      Button b = new Button(new Vector2(Game1.ScreenWidth / 2f, 300), "Play");
      b.AddText(AssetManager.GetFont("future18"), "Play");
      mainMenu.AddSelection(0, b);

      b = new Button(new Vector2(Game1.ScreenWidth / 2f, 400), "Options");
      b.AddText(AssetManager.GetFont("future18"), "Options");
      mainMenu.AddSelection(1, b);

      b = new Button(new Vector2(Game1.ScreenWidth / 2f, 500), "Exit");
      b.AddText(AssetManager.GetFont("future18"), "Exit");
      mainMenu.AddSelection(2, b);

      cursor1 = new MenuCursor(mainMenu, AssetManager.GetTexture("grey_sliderRight"), false);
      cursor1.color = Color.PaleTurquoise;
      mc1 = new MenuController(PlayerIndex.One, cursor1, this);

      cursor2 = new MenuCursor(mainMenu, AssetManager.GetTexture("grey_sliderRight"), true);
      cursor2.color = Color.CornflowerBlue;
      mc2 = new MenuController(PlayerIndex.Two, cursor2, this);
    }
    public void Confirm(MenuController mc)
    {
      switch (mc.GetSelected().Name)
      {
        case "Play":
          gsm.Push(new GameStatePlaying(gsm));
          break;
        case "Options":
          //push options state to gsm
          break;
        case "Exit":
          // ???
          break;
      }
    }
    public void Cancel(MenuController mc)
    {
    }
    public override void Update(GameTime gameTime)
    {
      mc1.Update(gameTime);
      mc2.Update(gameTime);
    }
    public override void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Begin();
      cursor1.Draw(spriteBatch);
      cursor2.Draw(spriteBatch);
      titleText.Draw(spriteBatch);
      mainMenu.Draw(spriteBatch);
      spriteBatch.End();
    }
    public override void Entered()
    {

    }
    public override void Leaving()
    {

    }
    public override void Revealed()
    {

    }
    public override void Obscuring()
    {

    }
  }
}
