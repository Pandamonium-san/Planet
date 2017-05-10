using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Planet
{
  class CompoundWeapon : Weapon
  {
    List<Weapon> weapons;
    public CompoundWeapon(Weapon wpn)
      : base(wpn)
    {
      weapons = new List<Weapon>();
      weapons.Add(wpn);
    }
    public CompoundWeapon(CompoundWeapon wpn)
      : base(wpn)
    {
      weapons = new List<Weapon>();
      foreach (Weapon w in wpn.weapons)
      {
        weapons.Add(new Weapon(w));
      }
    }
    public override void Update(GameTime gt)
    {
      foreach (Weapon wpn in weapons)
        wpn.Update(gt);
    }
    public override void Fire()
    {
      foreach (Weapon wpn in weapons)
        wpn.Fire();
    }
    public override void ResetShootTimer()
    {
      foreach (Weapon wpn in weapons)
        wpn.ResetShootTimer();
    }
    public override void FinishShootTimer()
    {
      foreach (Weapon wpn in weapons)
        wpn.FinishShootTimer();
    }
    public override void Reload()
    {
      foreach (Weapon wpn in weapons)
        wpn.Reload();
    }
    public override void SetShip(Ship ship)
    {
      foreach (Weapon wpn in weapons)
        wpn.SetShip(ship);
    }
    public void AddWeapon(Weapon wpn)
    {
      weapons.Add(wpn);
    }
    public List<Weapon> GetWeapons()
    {
      return weapons;
    }
  }
}
