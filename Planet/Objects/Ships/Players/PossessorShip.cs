﻿using Microsoft.Xna.Framework;
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

      maxHealth = 80;
      currentHealth = maxHealth;
      maxShield = 20;
      currentShield = maxShield;
      LeadShots = true;
      Hitbox.LocalScale = 0.7f;

      AbilityCooldown = new Timer(18, null, false);
      player = pc;

      weapons.Add(WeaponList.Laser(this, world));
      weapons.Add(WeaponList.Grenade(this, world));
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
    public override void Die()
    {
      base.Die();
      if (parasite != null)
      {
        parasite.Unlatch();
        parasite.Die();
      }
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
      possessedShip.Controller.SetShip(null);
      psc = new PlayerShipController(player.Index, possessedShip);

      possessedShip.SetLayer(Layer);
      possessedShip.maxHealth = possessedShip.maxHealth / 4;
      possessedShip.currentHealth = possessedShip.maxHealth;
      possessedShip.maxShield = maxShield;
      possessedShip.currentShield = currentShield;
      possessedShip.damageModifier *= 2.0f;
      possessedShip.speedModifier *= 1.5f;
      possessedShip.rotationModifier *= 1.2f;
      possessedShip.LeadShots = true;
      possessedShip.color = Color.Turquoise;
      possessedShip.Switch();

      if (possessedShip is EnemyBoss)
      {
        possessedShip.GetWeapon(0).Desc.magReloadTime = 1.0f;
        possessedShip.GetWeapon(1).Desc.magReloadTime = 1.0f;
        possessedShip.maxHealth = 500;
        possessedShip.currentHealth = possessedShip.maxHealth;
        possessedShip.damageModifier = 1.0f;
        possessedShip.speedModifier = 1.0f;
        possessedShip.rotationModifier = 1.0f;
        List<Weapon> xlaser = ((CompoundWeapon)(possessedShip.GetWeapon(2))).GetWeapons();
        xlaser.RemoveRange(3, 3);
        xlaser[0].Desc.damage = 30;
        xlaser[1].Desc.damage = 1;
        xlaser[2].Desc.damage = 1;
      }

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
