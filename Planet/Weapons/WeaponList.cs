using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  class WeaponList
  {
    #region PlayerWeapons
    public static Weapon Burst(Ship ship, World world, string texture = "laserBlue06")
    {
      WpnDesc desc = new WpnDesc(4, 3.0f, 600, 12, 8, 75, 1, 3, 0, 0, 0, 2);
      Weapon wpn = new Weapon(ship, world, desc, texture);
      wpn.SFX = AssetManager.GetSfx("laser9");
      wpn.Volume = 0.40f;
      wpn.Scale = 0.9f;
      wpn.Name = "Burst";
      return wpn;
    }
    public static Weapon Cyclone(Ship ship, World world, string texture = "laserBlue11")
    {
      Weapon wpn = new CycloneGun(ship, world);
      wpn.ProjTex = AssetManager.GetTexture(texture);
      wpn.SetMuzzle(new Vector2(0, -30));
      wpn.Name = "Cyclone";
      return wpn;
    }
    public static Weapon Gatling(Ship ship, World world, string texture = "laserBlue16")
    {
      WpnDesc desc = new WpnDesc(5, 15, 900, 1, 5, 0, 0, 30, 0, 0, 0, 3);
      Weapon wpn = new Weapon(ship, world, desc, texture);
      wpn.SetMuzzle(new Vector2(0, -20));
      wpn.SFX = AssetManager.GetSfx("laser_shoot2");
      wpn.Volume = 0.40f;
      wpn.Name = "Gatling";
      wpn.Scale = .7f;
      return wpn;
    }
    public static Weapon Grenade(Ship ship, World world, string texture = "laserBlue08")
    {
      Weapon wpn = new ExplodeGun(ship, world);
      wpn.ProjTex = AssetManager.GetTexture(texture);
      wpn.SFX = AssetManager.GetSfx("laser3");
      wpn.Volume = 0.80f;
      wpn.Name = "Grenade";
      wpn.Scale = 1.0f;
      return wpn;
    }
    public static Weapon Laser(Ship ship, World world, bool red = false)
    {
      WpnDesc desc = new WpnDesc(1.75f, 30, 1500, 1, 0.1f, 0, 0, 30, 0, 0, 0, 0.15f);
      LaserGun wpn = new LaserGun(ship, world, desc, 20, 700, red);
      wpn.SFX = AssetManager.GetSfx("lasergun");
      wpn.ShotsPerSFX = 5;
      wpn.Volume = 0.40f;
      wpn.SetMuzzle(new Vector2(0, -10));
      wpn.Name = "Laser";
      return wpn;
    }
    public static Weapon Volcano(Ship ship, World world, string texture = "laserBlue04")
    {
      WpnDesc desc = new WpnDesc(30, 1, 800, 1, 0, 0, 2.0f, 1, 0, 0, 0, 4, false, true);
      Weapon wpn = new Weapon(ship, world, desc, texture, "Volcano");
      wpn.SFX = AssetManager.GetSfx("laser2");
      wpn.Volume = 0.60f;
      wpn.LocalScale = 3.5f;
      wpn.LocalPos = new Vector2(-20, -10);
      CompoundWeapon cw = new CompoundWeapon(wpn);
      wpn = new Weapon(ship, world, desc, texture, "Volcano");
      wpn.LocalScale = 3.5f;
      wpn.LocalPos = new Vector2(20, -10);
      cw.AddWeapon(wpn);
      desc = new WpnDesc(4, 1, 400, 10, 5, 300, 2.0f, 1, 0, 0, 0, 2, false, false);
      wpn = new Weapon(ship, world, desc, "laserBlue09", "Trail");
      wpn.ProjRotSpeed = 8.0f;
      wpn.LocalScale = 0.5f;
      cw.AddWeapon(wpn);
      desc = new WpnDesc(4, 1, 400, 10, 5, 300, 2.0f, 1, 0, 0, 0, 2, false, false);
      wpn = new Weapon(ship, world, desc, "laserBlue10", "Trail");
      wpn.ProjRotSpeed = 8.0f;
      wpn.LocalScale = 0.5f;
      cw.AddWeapon(wpn);
      return cw;
    }
    public static Weapon Wing(Ship ship, World world, string texture = "spaceEffects_018")
    {
      WpnDesc desc = new WpnDesc(60, 30, 600, 1, 0, 0, 0, 90, 0, 0, -105, 0.15f, false, true);
      Weapon wpn = new LaserGun(ship, world, desc, 40, 100, false);
      wpn.InvulnOnHit = 0.4f;
      wpn.ProjTex = AssetManager.GetTexture(texture);
      wpn.SFX = AssetManager.GetSfx("lasergun");
      wpn.ShotsPerSFX = 5;
      wpn.Volume = 0.40f;
      wpn.LocalPos = new Vector2(-25, 0);
      wpn.Scale = 0.25f;
      wpn.Name = "Wing";
      CompoundWeapon cw = new CompoundWeapon(wpn);
      desc = new WpnDesc(60, 30, 600, 1, 0, 0, 0, 90, 0, 0, 105, 0.15f, false, true);
      wpn = new LaserGun(ship, world, desc, 40, 100, false);
      wpn.InvulnOnHit = 0.4f;
      wpn.SFX = null;
      wpn.ProjTex = AssetManager.GetTexture(texture);
      wpn.LocalPos = new Vector2(25, 0);
      wpn.Scale = 0.25f;
      wpn.Name = "Wing";
      cw.AddWeapon(wpn);
      cw.DashUsable = true;
      return cw;
    }
    #endregion

    #region EnemyWeapons
    public static Weapon Pew(Ship ship, World world, string texture = "laserRed10")
    {
      WpnDesc desc = new WpnDesc(8, 3f, 300, 1, 10, 10, 3, 9, 0, 0, 0, 5);
      Weapon wpn = new Weapon(ship, world, desc, texture, "Pew");
      wpn.Scale = 0.6f;
      return wpn;
    }
    public static Weapon Torrent(Ship ship, World world, string texture = "laserBlue10")
    {
      WpnDesc desc = new WpnDesc(1.4f, 10, 1000, 4, 1, 10, 1.0f, 10, 3, 0, -4.5f, 3);
      Weapon wpn = new Weapon(ship, world, desc, texture, "Torrent");
      wpn.Scale = 0.6f;
      return wpn;
    }
    public static Weapon Split(Ship ship, World world, string texture = "laserRed07")
    {
      WpnDesc desc = new WpnDesc(12, 3f, 500, 3, 2, 0, 3, 3, 15, 0, -15, 3);
      Weapon wpn = new Weapon(ship, world, desc, texture, "Split");
      wpn.Scale = 1.0f;
      wpn.LocalPos = new Vector2(0, -20);
      return wpn;
    }
    public static Weapon Beam(Ship ship, World world)
    {
      WpnDesc desc = new WpnDesc(30, 0.75f, 1500, 1, 0, 0, 0, 1, 0, 0, 0, 1.25f, false, true);
      WHitScan wpn = new WHitScan(ship, world, desc, 25, 1500);
      wpn.ProjTex = AssetManager.GetTexture("laserBlue15");
      wpn.OriginSticks = false;
      wpn.Name = "Beam";
      wpn.SFX = AssetManager.GetSfx("laser2");
      wpn.Volume = 0.45f;
      wpn.Scale = 1.5f;
      wpn.LocalPos = new Vector2(0, -20);
      return wpn;
    }
    public static Weapon FourWay(Ship ship, World world, string texture = "laserRed08")
    {
      WpnDesc desc = new WpnDesc(8, 4f, 300, 4, 2, 0, 4, 5, 360 / 4, 360 / 4 / 5, 0, 10, true);
      Weapon wpn = new Weapon(ship, world, desc, texture, "4-way");
      wpn.Scale = 0.4f;
      wpn.DashUsable = true;
      return wpn;
    }
    public static Weapon Spinny(Ship ship, World world, string texture = "laserBlue08")
    {
      WpnDesc desc = new WpnDesc(3.0f, 40f, 500, 4, 0, 0, 0, 7, 360 / 4, 360 / 4 / 7, 0, 0.5f, true);
      Weapon wpn = new Weapon(ship, world, desc, texture, "Spinny");
      wpn.Scale = 0.4f;
      wpn.DashUsable = true;
      return wpn;
    }
    public static CompoundWeapon BFG(Ship ship, World world, string texture = "laserRed08")
    {
      WpnDesc desc = new WpnDesc(20, 0.75f, 250, 1, 0, 0, 3, 2, 0, 0, 0, 15, false, true);
      Weapon wpn = new Weapon(ship, world, desc, texture, "BFG");
      wpn.Scale = 2.5f;
      CompoundWeapon cw = new CompoundWeapon(wpn);

      desc = new WpnDesc(8, 0.75f, 250, 4, 0, 0, 3, 2, 120 / 3, 0, -60);
      wpn = new Weapon(ship, world, desc, texture, "BFG");
      wpn.Scale = 0.4f;
      cw.AddWeapon(wpn);

      return cw;
    }
    public static Weapon Scatter(Ship ship, World world, bool red = false)
    {
      WpnDesc desc = new WpnDesc(1.5f, 10, 1500, 3, 5, 0, 0, 1, 0, 0, 0, 0.15f);
      Weapon wpn = new LaserGun(ship, world, desc, 8, 10000, red);
      wpn.SFX = AssetManager.GetSfx("Laser_Shoot2");
      wpn.Volume = 0.40f;
      wpn.Name = "Scatter Laser";
      return wpn;
    }
    public static Weapon Dagger(Ship ship, World world, bool red = false)
    {
      WpnDesc desc = new WpnDesc(15, 30, 1, 1, 0, 0, 0, 1, 0, 0, 0, 0.15f, false, true);
      Weapon wpn = new LaserGun(ship, world, desc, 20, 50, true);
      wpn.LocalPos = new Vector2(0, -10);
      wpn.InvulnOnHit = 0.1f;
      wpn.ProjTex = AssetManager.GetTexture("fire11");
      wpn.SFX = AssetManager.GetSfx("lasergun");
      wpn.ShotsPerSFX = 5;
      wpn.Volume = 0.20f;
      wpn.Scale = 0.5f;
      wpn.DashUsable = true;
      wpn.Name = "Dagger";
      return wpn;
    }
    public static Weapon Sword(Ship ship, World world, bool red = false)
    {
      WpnDesc desc = new WpnDesc(17.5f, 1.2f, 350, 4, 0, 0, 0, 1, 0, 0, 0, 0.7f, false, true);
      Weapon wpn = new Weapon(ship, world, desc, "spaceEffects_018", "Sword", "whoosh3");
      wpn.InvulnOnHit = 0.1f;
      wpn.Volume = 0.15f;
      wpn.Scale = 2.5f;
      wpn.DashUsable = true;
      return wpn;
    }
    public static Weapon Spread(Ship ship, World world)
    {
      WpnDesc desc = new WpnDesc(12, 8f, 500, 8, 0, 50, 2, 8, 90 / 7, 0, -45, 10, false, false);
      Weapon wpn = new Weapon(ship, world, desc, "laserRed08", "Spread");
      wpn.Scale = 0.6f;
      return wpn;
    }
    public static CompoundWeapon Spinny2(Ship ship, World world)
    {
      WpnDesc desc = new WpnDesc(8, 12f, 500, 4, 2, 10, 2.0f, 24, 360 / 4, 360 / 4 / 10, 0, 5f, true);
      Weapon wpn = new Weapon(ship, world, desc, "laserRed08", "Spinny");
      wpn.Scale = 0.4f;
      CompoundWeapon cw = new CompoundWeapon(wpn);

      desc = new WpnDesc(12, 7f, 500, 4, 5, 20, 2.0f, 14, 360 / 4, 360 / 4 / 5, -45, 5f, true);
      wpn = new Weapon(ship, world, desc, "laserRed10", "Spinny");
      wpn.Scale = 0.6f;
      cw.AddWeapon(wpn);
      return cw;
    }
    public static CompoundWeapon XLaser(Ship ship, World world)
    {
      WpnDesc desc = new WpnDesc(20, 2.0f, 400, 3, 5, 50, 4, 12, 45 / 3, 0, -45 / 3, 15, false, true);
      Weapon wpn = new Weapon(ship, world, desc, "laserRed10", "BFG");
      wpn.Scale = 1.5f;
      CompoundWeapon cw = new CompoundWeapon(wpn);

      desc = new WpnDesc(30, 60.0f, 500, 2, 0, 0, 4, 360, 45, 0, -22.5f, 0.1f, false, true);
      wpn = new LaserGun(ship, world, desc, 30, 10000, true);
      wpn.Name = "X Laser front";
      wpn.ShotsPerSFX = 2;
      cw.AddWeapon(wpn);
      desc = new WpnDesc(30, 60.0f, 500, 2, 0, 0, 4, 360, 45, 0, -22.5f + 180, 0.1f, false, true);
      wpn = new LaserGun(ship, world, desc, 30, 10000, true);
      wpn.ShotsPerSFX = 2;
      wpn.Name = "X Laser back";
      cw.AddWeapon(wpn);

      desc = new WpnDesc(8, 3.0f, 400, 1, 5, 10, 4, 18, 0, 0, -10, 5);
      wpn = new Weapon(ship, world, desc, "laserRed08", "Pew");
      wpn.Scale = 0.4f;
      wpn.LocalPos = new Vector2(-15, 0);
      wpn.Name = "Left pew";
      cw.AddWeapon(wpn);

      desc = new WpnDesc(8, 3.0f, 400, 1, 5, 10, 4, 18, 0, 0, 10, 5);
      wpn = new Weapon(ship, world, desc, "laserRed08", "Pew");
      wpn.Scale = 0.4f;
      wpn.LocalPos = new Vector2(15, 0);
      wpn.Name = "Right pew";
      cw.AddWeapon(wpn);

      desc = new WpnDesc(12, 4f, 450, 15, 10, 0, 4, 6 * 4, 315 / 15, 0, 22.5f + 21 / 2, 10, false, false);
      wpn = new Weapon(ship, world, desc, "laserRed08", "DeathZone");
      wpn.Scale = 0.7f;
      cw.AddWeapon(wpn);

      return cw;
    }
    public static CompoundWeapon Nova(Ship ship, World world)
    {
      WpnDesc desc = new WpnDesc(12, 0.5f, 200, 16, 0, 0, 0, 1, 360 / 15, 0, 0, 15f, false);
      Weapon wpn = new Weapon(ship, world, desc, "laserRed10", "Nova");
      wpn.Scale = 0.8f;
      CompoundWeapon cw = new CompoundWeapon(wpn);

      desc = new WpnDesc(5, 0.5f, 225, 12, 8, 175, 0, 1, 0, 0, 0, 10, false, false);
      wpn = new Weapon(ship, world, desc, "laserRed09", "Trail");
      wpn.ProjRotSpeed = 8.0f;
      wpn.LocalScale = 0.6f;
      cw.AddWeapon(wpn);
      cw.DashUsable = true;
      return cw;
    }
    #endregion
  }
}
