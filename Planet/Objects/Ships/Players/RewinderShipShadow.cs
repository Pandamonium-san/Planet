using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  class RewinderShipShadow : Ship
  {
    Stack<RewinderShip.State> stateStack;
    RewinderShip.State currentState;
    public RewinderShipShadow(World world, RewinderShip rs, List<Weapon> rsWeapons, Stack<RewinderShip.State> stateStack)
      : base(rs.Pos, world, rs.tex)
    {
      SetLayer(rs.Layer);
      LeadShots = rs.LeadShots;
      Rotation = rs.Rotation;
      Scale = rs.Scale;
      Hitbox.LocalScale = rs.Hitbox.LocalScale;
      origin = rs.origin;
      layerDepth = rs.layerDepth + 0.1f;
      maxHealth = rs.maxHealth;
      currentHealth = maxHealth;
      baseSpeed = rs.baseSpeed;
      rotationSpeed = rs.rotationSpeed;
      Target = rs.Target;


      this.weaponIndex = stateStack.Peek().weaponIndex;
      foreach (Weapon wpn in rsWeapons)
      {
        Type wType = wpn.GetType();
        Weapon wp = (Weapon)Activator.CreateInstance(wType, wpn);
        wp.SetShip(this);
        weapons.Add(wp);
      }

      color = Color.Gray * 0.8f;
      this.stateStack = new Stack<RewinderShip.State>(stateStack.Reverse());
    }
    protected override void DoUpdate(GameTime gt)
    {
      if (stateStack.Count > 0)
        currentState = stateStack.Pop();
      else
        Die();

      movementDirection = currentState.movementDirection;
      Dashing = currentState.dashing;
      rotationModifier = currentState.rotationModifier;
      speedModifier = currentState.speedModifier;
      if (currentState.firing)
        Fire1();
      if (currentState.switching)
        Switch();
      if (currentState.switchingTarget)
        SwitchTarget();

      base.DoUpdate(gt);
    }
  }
}
