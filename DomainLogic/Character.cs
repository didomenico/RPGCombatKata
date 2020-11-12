using System;
using System.Collections.Generic;

namespace DomainLogic
{ 
    public class Character : Prop
    {
        protected int level = 1;

        // If undefined, a character is created without weapons. 
        // He will fight bare-handed and thus, melee. 
        protected Weapon weapon = new Melee();

        protected HashSet<string> factions = new HashSet<string>();

        const int MINIMUM_LEVEL_DIFFERENCE = 5;
        const double DAMAGE_MODIFIER_PERCENTAGE = 0.5;

        public Character() : base(1000) { }

        public Character(Weapon weapon) : this()
        {                    
            this.weapon = weapon;
        }

        public Character(double x, double y) : this()
        {
            SetPosition(x, y);
        }

        public int GetLevel()
        {
            return level;
        }

        protected uint RecalculateDamageAccordingToLevel(uint damage, Character target)
        {
            double extraDamage = damage * DAMAGE_MODIFIER_PERCENTAGE;

            if (MINIMUM_LEVEL_DIFFERENCE <= level - target.GetLevel())
            {                
                return Convert.ToUInt32(Math.Round(damage + extraDamage));
            }         
            
            if (MINIMUM_LEVEL_DIFFERENCE <= target.GetLevel() - level)
            {                
                return Convert.ToUInt32(Math.Round(damage - extraDamage));
            }

            return damage;
        }

        protected bool IsInRange(Prop target)
        {
            double targetDistance = Distance(target);
            
            return weapon.IsInRange(targetDistance);         
        }

        public Weapon GetWeapon()
        {
            return weapon;
        }
        
        // Here I decided to not rename to HealSelf() because using
        // the same name with polymorfism makes for a better interface
        // because it is more uniform/isonomic and easier to guess,
        // since the user only have to remember the name of one method.
        public void Heal(uint amount)
        {
            if (health + amount <= 1000)
            {
                health += amount;
            }
            else
            {
                health = 1000;
            }
        }

        public void LevelUp()
        {
            level++;
        }

        public void Attack(Character target, uint damage)
        {
            if (BelongsToSameFaction(target)) { return; }

            if (target == this) { return; }

            damage = RecalculateDamageAccordingToLevel(damage, target);

            // Calls the attack part that is common to target and props to avoid code replication.
            Attack((Prop)target, damage);
        }

        public void Attack(Prop target, uint damage)
        {
            if (IsInRange(target) == false) { return; }

            target.SufferDamage(damage);
        }

        public void JoinFaction(string faction)
        {
            factions.Add(faction);
        }

        public void LeaveFaction(string faction)
        {
            factions.Remove(faction);
        }

        public void Heal(Character target, uint amount)
        {
            if (BelongsToSameFaction(target) == false) { return; }

            target.Heal(amount);
        }

        public HashSet<string> GetFactions()
        {
            return factions;
        }

        public bool BelongsToSameFaction(Character other)
        {
            HashSet<string> otherFactions = other.GetFactions();
            otherFactions.IntersectWith(factions);

            //If number of shared factions is 1 or more, returns true
            if (otherFactions.Count > 0) { return true; }

            //If number of shared factions is 0, returns false
            return false;
        }
    }

    public class Prop
    {
        protected uint health = 1000;

        protected bool alive = true;

        protected (double x, double y) position = (0.0, 0.0);

        public Prop(uint health)
        {
            this.health = health;
        }

        public Prop(uint health, double x, double y) : this(health)
        {
            position = (x, y);
        }

        // Must make it internal to enable Character to attack a Prop
        internal protected void SufferDamage(uint damage)
        {
            if (damage < health)
            {
                health -= damage;                
            }
            else
            { 
                health = 0; 
                alive = false;
            }
        }

        public (double x, double y) GetPosition()
        {
            return position;
        }

        public uint GetHealth()
        {
            return health;
        }

        public void SetPosition(double x, double y)
        {
            position = (x, y);
        }

        public bool IsAlive()
        {
            return alive;
        }

        protected double Distance(Prop other)
        {
            // Distance formula = √[(x₂ - x₁)² + (y₂ - y₁)²]
            double distance = Math.Sqrt(Math.Pow(position.x - other.GetPosition().x, 2) +
                                        Math.Pow(position.y + other.GetPosition().y, 2));

            return distance;
        }
    }

    public abstract class Weapon
    { 
        public abstract double range { get; }

        public bool IsInRange(double distance)
        {
            return distance <= range;
        }
    }

    public class Melee : Weapon
    {
        public override double range => 2;         
    }
    public class Ranged : Weapon
    {
        public override double range => 20;        
    }
}