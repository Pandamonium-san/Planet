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

    private MenuController mc;
    private Text titleText;
    private SelectionList mainMenu;

    public GameStateTitleScreen(GameStateManager gameStateManager)
    {
      gsm = gameStateManager;
      mc = new MenuController(PlayerIndex.One, this);

      titleText = new Text(AssetManager.GetFont("future48"), "Planet", new Vector2(Game1.ScreenWidth / 2f, 200), Color.White);
      mainMenu = new SelectionList(3);

      Button b = new Button(new Vector2(Game1.ScreenWidth / 2f, 300), "Play");
      b.AddText(AssetManager.GetFont("future18"), "Play");
      mainMenu.AddSelection(0, b);

      b = new Button(new Vector2(Game1.ScreenWidth / 2f, 400), "Options");
      b.AddText(AssetManager.GetFont("future18"), "Options");
      mainMenu.AddSelection(1, b);

      b = new Button(new Vector2(Game1.ScreenWidth / 2f, 500), "Exit");
      b.AddText(AssetManager.GetFont("future18"), "Exit");
      mainMenu.AddSelection(2, b);
    }
    public void Next()
    {
      mainMenu.Next();
    }
    public void Previous()
    {
      mainMenu.Previous();
    }
    public void Confirm()
    {
      switch (mainMenu.Selected.Name)
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
    public void Cancel()
    {
    }
    public override void Update(GameTime gameTime)
    {
      mc.Update(gameTime);
    }
    public override void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Begin();
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
