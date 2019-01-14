using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planet
{
  public enum PlayerInput
  {
    A,        //Special ability
    B,        
    Yellow,   //Fire/confirm
    Red,      //Switch weapon
    Green,    //Dash/cancel
    Blue,     //Change target
    Side,     
    Start,     
    Up,       
    Down,     
    Left,     
    Right     
  }
  //
  // Summary:
  //     InputHandler is a static class, containing static methods for getting info on
  //     button and joystick states.
  public static class InputHandler
  {
    static KeyboardState keyboardState;
    static KeyboardState oldKeyboardState;
    static GamePadState[] gamePadStates;
    static Dictionary<PlayerIndex, Dictionary<PlayerInput, Func<int, bool>>> p;

    public static void Update()
    {
      oldKeyboardState = keyboardState;
      keyboardState = Keyboard.GetState();
      for (int i = 0; i < 4; i++)
      {
        gamePadStates[i + 4] = gamePadStates[i];
        gamePadStates[i] = GamePad.GetState(i);
      }
    }
    public static void InitializeBindings()
    {
      p = new Dictionary<PlayerIndex, Dictionary<PlayerInput, Func<int, bool>>>();
      p[PlayerIndex.One] = new Dictionary<PlayerInput, Func<int, bool>>();
      p[PlayerIndex.Two] = new Dictionary<PlayerInput, Func<int, bool>>();
      // old states are kept in indices 4-7
      gamePadStates = new GamePadState[8];

      if (GamePad.GetState(0).IsConnected)
      {
        p[PlayerIndex.One][PlayerInput.A] = (x) => gamePadStates[0 + x].Triggers.Left > 0.1;
        p[PlayerIndex.One][PlayerInput.Yellow] = (x) => gamePadStates[0 + x].Triggers.Right > 0.1;
        p[PlayerIndex.One][PlayerInput.Red] = (x) => gamePadStates[0 + x].Buttons.B == ButtonState.Pressed;
        p[PlayerIndex.One][PlayerInput.Side] = (x) => gamePadStates[0 + x].Buttons.Back == ButtonState.Pressed;
        p[PlayerIndex.One][PlayerInput.Green] = (x) => gamePadStates[0 + x].Buttons.A == ButtonState.Pressed;
        p[PlayerIndex.One][PlayerInput.Blue] = (x) => gamePadStates[0 + x].Buttons.X == ButtonState.Pressed;
        p[PlayerIndex.One][PlayerInput.Start] = (x) => gamePadStates[0 + x].Buttons.Start == ButtonState.Pressed;

        p[PlayerIndex.One][PlayerInput.Up] = (x) => gamePadStates[0 + x].ThumbSticks.Left.Y > 0.1;
        p[PlayerIndex.One][PlayerInput.Down] = (x) => gamePadStates[0 + x].ThumbSticks.Left.Y < -0.1;
        p[PlayerIndex.One][PlayerInput.Left] = (x) => gamePadStates[0 + x].ThumbSticks.Left.X < -0.1;
        p[PlayerIndex.One][PlayerInput.Right] = (x) => gamePadStates[0 + x].ThumbSticks.Left.X > 0.1;
      }
      else
      {
        p[PlayerIndex.One][PlayerInput.A] = (x) => x == 0 ? keyboardState.IsKeyDown(Keys.M) : oldKeyboardState.IsKeyDown(Keys.M);
        p[PlayerIndex.One][PlayerInput.Yellow] = (x) => x == 0 ? keyboardState.IsKeyDown(Keys.B) : oldKeyboardState.IsKeyDown(Keys.B);
        p[PlayerIndex.One][PlayerInput.Red] = (x) => x == 0 ? keyboardState.IsKeyDown(Keys.G) : oldKeyboardState.IsKeyDown(Keys.G);
        p[PlayerIndex.One][PlayerInput.Side] = (x) => x == 0 ? keyboardState.IsKeyDown(Keys.V) : oldKeyboardState.IsKeyDown(Keys.V);
        p[PlayerIndex.One][PlayerInput.Green] = (x) => x == 0 ? keyboardState.IsKeyDown(Keys.N) : oldKeyboardState.IsKeyDown(Keys.N);
        p[PlayerIndex.One][PlayerInput.Blue] = (x) => x == 0 ? keyboardState.IsKeyDown(Keys.H) : oldKeyboardState.IsKeyDown(Keys.H);
        p[PlayerIndex.One][PlayerInput.Start] = (x) => x == 0 ? keyboardState.IsKeyDown(Keys.Space) : oldKeyboardState.IsKeyDown(Keys.Space);

        p[PlayerIndex.One][PlayerInput.Up] = (x) => x == 0 ? keyboardState.IsKeyDown(Keys.W) : oldKeyboardState.IsKeyDown(Keys.W);
        p[PlayerIndex.One][PlayerInput.Down] = (x) => x == 0 ? keyboardState.IsKeyDown(Keys.S) : oldKeyboardState.IsKeyDown(Keys.S);
        p[PlayerIndex.One][PlayerInput.Left] = (x) => x == 0 ? keyboardState.IsKeyDown(Keys.A) : oldKeyboardState.IsKeyDown(Keys.A);
        p[PlayerIndex.One][PlayerInput.Right] = (x) => x == 0 ? keyboardState.IsKeyDown(Keys.D) : oldKeyboardState.IsKeyDown(Keys.D);
      }

      if (GamePad.GetState(1).IsConnected)
      {
        p[PlayerIndex.Two][PlayerInput.A] = (x) => gamePadStates[3 + x].Triggers.Left > 0.1;
        p[PlayerIndex.Two][PlayerInput.Yellow] = (x) => gamePadStates[3 + x].Triggers.Right > 0.1;
        p[PlayerIndex.Two][PlayerInput.Red] = (x) => gamePadStates[3 + x].Buttons.B == ButtonState.Pressed;
        p[PlayerIndex.Two][PlayerInput.Side] = (x) => gamePadStates[3 + x].Buttons.Back == ButtonState.Pressed;
        p[PlayerIndex.Two][PlayerInput.Green] = (x) => gamePadStates[3 + x].Buttons.A == ButtonState.Pressed;
        p[PlayerIndex.Two][PlayerInput.Blue] = (x) => gamePadStates[3 + x].Buttons.X == ButtonState.Pressed;
        p[PlayerIndex.Two][PlayerInput.Start] = (x) => gamePadStates[3 + x].Buttons.Start == ButtonState.Pressed;

        p[PlayerIndex.Two][PlayerInput.Up] = (x) => gamePadStates[3 + x].ThumbSticks.Left.Y > 0.1;
        p[PlayerIndex.Two][PlayerInput.Down] = (x) => gamePadStates[3 + x].ThumbSticks.Left.Y < -0.1;
        p[PlayerIndex.Two][PlayerInput.Left] = (x) => gamePadStates[3 + x].ThumbSticks.Left.X < -0.1;
        p[PlayerIndex.Two][PlayerInput.Right] = (x) => gamePadStates[3 + x].ThumbSticks.Left.X > 0.1;
      }
      else
      {
        p[PlayerIndex.Two][PlayerInput.A] = (x) => x == 0 ? keyboardState.IsKeyDown(Keys.NumPad3) : oldKeyboardState.IsKeyDown(Keys.NumPad3);
        p[PlayerIndex.Two][PlayerInput.Yellow] = (x) => x == 0 ? keyboardState.IsKeyDown(Keys.NumPad1) : oldKeyboardState.IsKeyDown(Keys.NumPad1);
        p[PlayerIndex.Two][PlayerInput.Red] = (x) => x == 0 ? keyboardState.IsKeyDown(Keys.NumPad4) : oldKeyboardState.IsKeyDown(Keys.NumPad4);
        p[PlayerIndex.Two][PlayerInput.Side] = (x) => x == 0 ? keyboardState.IsKeyDown(Keys.NumPad7) : oldKeyboardState.IsKeyDown(Keys.NumPad7);
        p[PlayerIndex.Two][PlayerInput.Green] = (x) => x == 0 ? keyboardState.IsKeyDown(Keys.NumPad2) : oldKeyboardState.IsKeyDown(Keys.NumPad2);
        p[PlayerIndex.Two][PlayerInput.Blue] = (x) => x == 0 ? keyboardState.IsKeyDown(Keys.NumPad5) : oldKeyboardState.IsKeyDown(Keys.NumPad5);
        p[PlayerIndex.Two][PlayerInput.Start] = (x) => x == 0 ? keyboardState.IsKeyDown(Keys.NumPad0) : oldKeyboardState.IsKeyDown(Keys.NumPad0);

        p[PlayerIndex.Two][PlayerInput.Up] = (x) => x == 0 ? keyboardState.IsKeyDown(Keys.Up) : oldKeyboardState.IsKeyDown(Keys.Up);
        p[PlayerIndex.Two][PlayerInput.Down] = (x) => x == 0 ? keyboardState.IsKeyDown(Keys.Down) : oldKeyboardState.IsKeyDown(Keys.Down);
        p[PlayerIndex.Two][PlayerInput.Left] = (x) => x == 0 ? keyboardState.IsKeyDown(Keys.Left) : oldKeyboardState.IsKeyDown(Keys.Left);
        p[PlayerIndex.Two][PlayerInput.Right] = (x) => x == 0 ? keyboardState.IsKeyDown(Keys.Right) : oldKeyboardState.IsKeyDown(Keys.Right);
      }
    }
    //
    // Summary:
    //     InuptIsIdle is true if no input have been registered last frame.
    public static bool InputIsIdle { get; }
    //
    // Summary:
    //     If bInputSuppressed is true, no input is registered.
    public static bool InputSuppressed { get; }
    //
    // Summary:
    //     InputIdleTime is the total time in seconds that the in input has been idle.
    public static double InputSuppressedTime { get; }
    //
    // Summary:
    //     InputIdleTime is the total time in seconds that the in input has been idle.
    public static double InuptIdleTime { get; }

    //
    // Summary:
    //     Returns whether a specified key is currently being pressed.
    //
    // Parameters:
    //   playerIndex:
    //     The index number identifying the player controller.
    //
    //   button:
    //     Enumerated value that specifies the PlayerInput to query.
    //
    //   checkLastInputState:
    //     If true check the current state, else check the last state
    //
    // Returns:
    //     True if the button is down, else false.
    public static bool IsButtonDown(PlayerIndex playerIndex, PlayerInput button, bool checkLastInputState = false)
    {
      return p[playerIndex][button].Invoke(checkLastInputState ? 4 : 0);
    }
    //
    // Summary:
    //     Returns whether a specified PlayerInput is currently not pressed.
    //
    // Parameters:
    //   playerIndex:
    //     The index number identifying the player controller.
    //
    //   button:
    //     Enumerated value that specifies the PlayerInput to query.
    //
    //   checkLastInputState:
    //     If true check the current state, else check the last state
    //
    // Returns:
    //     True if the button is up, else false.
    public static bool IsButtonUp(PlayerIndex playerIndex, PlayerInput button, bool checkLastInputState = false)
    {
      return !p[playerIndex][button].Invoke(checkLastInputState ? 4 : 0);
    }
  }
}
