using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  class PossessorShip : Ship
  {
    private Player player;
    private Ship possessedShip;
    private PlayerShipController psc;
    private ShipController oldController;
    private Layer oldLayer;

    public PossessorShip(Vector2 pos, World world, Player pc)
        : base(pos, world, AssetManager.GetTexture(@"ships\blue\spaceShips_009"))
    {
      flashTex = AssetManager.GetTexture(@"ships\flash\spaceShips_009");
      SetLayer(Layer.PLAYER_SHIP);

      player = pc;
      Weapon wpn;
      WpnDesc desc = new WpnDesc(30, 1, 2500, 5, 0, 0, 0, 1, 0, 0, 0, 3);           //sniper
      wpn = new Weapon(this, world, desc, "laserBlue16");
      wpn.SetMuzzle(new Vector2(0, -30));
      wpn.Name = "Sniper";
      wpn.Scale = 1.5f;
      weapons.Add(wpn);

      maxHealth = 1000;
      currentHealth = maxHealth;
    }
    public override void Update(GameTime gt)
    {
      base.Update(gt);
      if (possessedShip != null)
      {
        psc.Update(gt);
        if (possessedShip.Disposed)
        {
          Release();
        }
      }
    }
    public override void Fire1()
    {
      if (IsActive)
        base.Fire1();
    }
    public override void Fire2()
    {
      if (possessedShip != null)
      {
        Release();
        return;
      }
      Projectile p = new Projectile(
      world,
      tex,
      Pos,
      Forward,
      500.0f,
      0,
      this,
      1.0f,
      null,
      TakeOver
      );
      p.color = Color.Purple;
      p.Scale *= Scale * 0.2f;
      world.PostProjectile(p);
    }
    private void TakeOver(Projectile p, GameObject other)
    {
      if (other.Layer != Layer.ENEMY_SHIP || possessedShip != null)
        return;

      possessedShip = (Ship)other;


      oldLayer = possessedShip.Layer;
      oldController = possessedShip.Controller;
      psc = new PlayerShipController(player.Index, possessedShip);

      oldController.SetShip(null);
      possessedShip.SetLayer(Layer);
      possessedShip.speedModifier *= 3.0f;
      possessedShip.rotationModifier *= 1.5f;
      possessedShip.LeadShots = true;
      possessedShip.color = Color.Turquoise;
      possessedShip.Switch();

      IsActive = false;
      Visible = false;
    }
    private void Release()
    {
      if (possessedShip != null)
      {
        oldController.SetShip(possessedShip);
        Pos = possessedShip.Pos;
        Rotation = possessedShip.Rotation;
        IsActive = true;
        Visible = true;
        possessedShip.SetDash(false);
        possessedShip.SetLayer(oldLayer);
        possessedShip.speedModifier /= 3.0f;
        possessedShip.rotationModifier /= 1.5f;
        possessedShip.LeadShots = false;
        possessedShip.color = Color.White;
        possessedShip.SetWeapon(0);
        possessedShip.TakeDamage(50);
        possessedShip = null;
        psc = null;
      }
    }
  }
}
