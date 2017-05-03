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

    private Projectile parasite;
    private List<Sprite> parasiteLink;
    private Player player;
    private Ship possessedShip;
    private PlayerShipController psc;
    private ShipController oldController;
    private Layer oldLayer;

    private Ship latchedShip;
    private bool latched;

    public PossessorShip(Vector2 pos, World world, Player pc)
      : base(pos, world, AssetManager.GetTexture(@"ships\blue\spaceShips_009"))
    {
      flashTex = AssetManager.GetTexture(@"ships\flash\spaceShips_009");
      SetLayer(Layer.PLAYER_SHIP);

      maxHealth = 100;
      currentHealth = maxHealth;
      maxShield = 20;
      currentShield = 20;
      LeadShots = true;
      Hitbox.LocalScale = 0.5f;

      AbilityCooldown = new Timer(0.1, null, false);
      player = pc;

      Weapon wpn;
      WpnDesc desc = new WpnDesc(30, 1, 2500, 5, 0, 0, 0, 1, 0, 0, 0, 3);           //sniper
      wpn = new Weapon(this, world, desc, "laserBlue16");
      wpn.SetMuzzle(new Vector2(0, -30));
      wpn.Name = "Sniper";
      wpn.Scale = 1.5f;
      weapons.Add(wpn);
    }
    public override void Update(GameTime gt)
    {
      base.Update(gt);
      AbilityCooldown.Update(gt);
      if (possessedShip != null)
      {
        psc.Update(gt);
        if (possessedShip.Disposed)
        {
          Release();
        }
      }
      if (parasite != null && parasite.Disposed)
      {
        parasite = null;
        if (!latched)
          parasiteLink = null;
      }
      if (parasiteLink != null)
      {
        Vector2 dir;
        if(!latched)
          dir = parasite.Pos - Pos;
        else
          dir = latchedShip.Pos - Pos;
        for (int i = 0; i < parasiteLink.Count(); i++)
        {
          parasiteLink[i].Pos = Pos + dir / parasiteLink.Count() * i;
        }
      }
      if (latched)
      {
        Vector2 pull = Vector2.Normalize(latchedShip.Pos - Pos) * 500;
        Acceleration = pull;
        latchedShip.Acceleration = -pull;
        if (Vector2.Distance(Pos, latchedShip.Pos) < 10)
        {
          TakeOver(latchedShip);
          parasite = null;
          parasiteLink = null;
          latchedShip = null;
          latched = false;
        }
      }
    }
    public override void Draw(SpriteBatch spriteBatch)
    {
      base.Draw(spriteBatch);
      if (parasiteLink != null)
        foreach (Sprite sp in parasiteLink)
          sp.Draw(spriteBatch);
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
      if (AbilityCooldown.Counting || (parasite != null && !parasite.Disposed))
        return;
      parasiteLink = new List<Sprite>();
      for (int i = 0; i < 15; i++)
      {
        parasiteLink.Add(new Sprite(Pos, AssetManager.GetTexture("proj1")));
      }

      parasite = new Projectile(
      world,
      tex,
      Pos,
      Forward,
      500.0f,
      0,
      this,
      0.7f,
      ParasiteBehaviour,
      LatchOn
      );
      parasite.color = Color.Red;
      parasite.Scale *= Scale * 0.4f;
      world.PostProjectile(parasite);
    }
    private void ParasiteBehaviour(Projectile p, GameTime gt)
    {
      p.LocalPos += p.velocity * (float)gt.ElapsedGameTime.TotalSeconds;
      p.LocalRotation = Utility.Vector2ToAngle(p.Pos - Pos);
    }
    private void LatchOn(Projectile p, GameObject other)
    {
      if (other.Layer != Layer.ENEMY_SHIP || possessedShip != null)
        return;
      parasite.Pos = other.Pos;
      CollisionEnabled = false;
      latched = true;
      latchedShip = (Ship)other;
    }
    private void TakeOver(Ship other)
    {
      if (other.Layer != Layer.ENEMY_SHIP || possessedShip != null)
        return;

      possessedShip = other;

      oldLayer = possessedShip.Layer;
      oldController = possessedShip.Controller;
      psc = new PlayerShipController(player.Index, possessedShip);

      oldController.SetShip(null);
      possessedShip.SetLayer(Layer);
      possessedShip.damageModifier *= 5.0f;
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
        CollisionEnabled = true;
        possessedShip.SetDash(false);
        possessedShip.SetLayer(oldLayer);
        possessedShip.damageModifier /= 5.0f;
        possessedShip.speedModifier /= 3.0f;
        possessedShip.rotationModifier /= 1.5f;
        possessedShip.LeadShots = false;
        possessedShip.color = Color.White;
        possessedShip.SetWeapon(0);
        possessedShip.TakeDamage(50);
        possessedShip = null;
        psc = null;
        AbilityCooldown.Start();
      }
    }
  }
}
