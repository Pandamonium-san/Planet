using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Planet
{
  class PossessorShip : Ship, IPlayerShip
  {
    public Timer AbilityCooldown { get; set; }
    public Ship PossessedShip { get { return possessedShip; } }

    private Parasite parasite;
    private Player player;
    private Ship possessedShip;
    private PlayerShipController psc;

    public PossessorShip(Vector2 pos, World world, Player pc)
      : base(pos, world, AssetManager.GetTexture(@"ships\blue\spaceShips_009"))
    {
      flashTex = AssetManager.GetTexture(@"ships\flash\spaceShips_009");
      SetLayer(Layer.PLAYER_SHIP);

      maxHealth = 70;
      currentHealth = maxHealth;
      maxShield = 30;
      currentShield = maxShield;
      LeadShots = true;
      Hitbox.LocalScale = 0.5f;

      AbilityCooldown = new Timer(5, null, false);
      player = pc;

      WpnDesc desc = new WpnDesc(1, 60, 1500, 1, 0.1f, 0, 0, 30, 0, 0, 0, 0.1f);           // laser
      LaserGun laser = new LaserGun(this, world, desc, 20, false);
      laser.SetMuzzle(new Vector2(0, -20));
      laser.Name = "Laser";
      weapons.Add(laser);

      desc = new WpnDesc(20, 1.5f, 0, 1, 0, 0, 1, 3, 0, 0, 0, 10.0f);           // laser
      ExplodeGun wpn = new ExplodeGun(this, world);
      wpn.SetDesc(desc);
      wpn.Scale = 2.0f;
      wpn.Name = "Mines";
      weapons.Add(wpn);
    }
    public override void Update(GameTime gt)
    {
      base.Update(gt);
      AbilityCooldown.Update(gt);
      if (possessedShip != null)
      {
        psc.Update(gt);
        currentShield = possessedShip.currentShield;
        if (possessedShip.Disposed)
        {
          Release();
        }
      }
    }
    public override void Draw(SpriteBatch spriteBatch)
    {
      base.Draw(spriteBatch);
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
      if (parasite != null && parasite.Latched)
      {
        parasite.Unlatch();
        return;
      }
      if (AbilityCooldown.Counting || (parasite != null && !parasite.Disposed))
        return;

      parasite = new Parasite(
      this,
      world,
      tex,
      Pos,
      Forward,
      750.0f,
      0.7f,
      750.0f,
      500.0f,
      35.0f,
      10.0f
      );
      parasite.color = Color.Red;
      parasite.Scale *= Scale * 0.4f;
      world.PostProjectile(parasite);
    }
    public override void SetDash(bool dash)
    {
      if (dash && parasite != null && parasite.Latched)
        parasite.Unlatch();
      base.SetDash(dash);
    }
    public void TakeOver(Ship other)
    {
      parasite = null;

      possessedShip = other;

      psc = new PlayerShipController(player.Index, possessedShip);

      possessedShip.SetLayer(Layer);
      possessedShip.maxHealth = possessedShip.maxHealth / 4;
      possessedShip.currentHealth = possessedShip.maxHealth;
      possessedShip.maxShield = maxShield;
      possessedShip.currentShield = currentShield;
      possessedShip.damageModifier *= 5.0f;
      possessedShip.speedModifier *= 3.0f;
      possessedShip.rotationModifier *= 1.2f;
      possessedShip.GetWeapon(0).Desc.magReloadTime *= 0.2f;
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
        Pos = possessedShip.Pos;
        Rotation = possessedShip.Rotation;
        currentShield = possessedShip.currentShield;
        IsActive = true;
        Visible = true;
        CollisionEnabled = true;
        possessedShip.Die();
        possessedShip = null;
        psc = null;
        AbilityCooldown.Start();
      }
    }
  }
}
