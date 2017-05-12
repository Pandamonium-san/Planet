using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  class GameStateTitleScreen : GameState
  {
    private Background bg;
    private GameStateManager gsm;
    private GameSettings settings;

    public GameStateTitleScreen(GameStateManager gameStateManager)
    {
      this.gsm = gameStateManager;
      settings = new GameSettings();
      bg = new Background(1.0f, 20, 100);
      bg.DriftSpeed = 20;
      bg.StarDensity = 100;
    }
    public override void Update(GameTime gameTime)
    {
      bg.Update(gameTime);
    }
    public override void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Begin();
      bg.Draw(spriteBatch);
      spriteBatch.End();
    }
    public override void Entered()
    {
      AudioManager.PlayBgm("PerituneMaterial_splash");
      gsm.Push(new GameStateMenuMain(gsm, settings));
    }
    public override void Leaving()
    {
    }
    public override void Revealed()
    {
      if (settings.startGame)
      {
        gsm.ChangeState(new GameStatePlaying(gsm, settings));
      }
    }
    public override void Obscuring()
    {

    }
  }
}
