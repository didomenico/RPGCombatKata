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
            setPosition(x, y);
        }

        public int getLevel()
        {
            return level;
        }

        protected uint recalculateDamageAccordingToLevel(uint damage, Character target)
        {
            double extraDamage = damage * DAMAGE_MODIFIER_PERCENTAGE;

            if (MINIMUM_LEVEL_DIFFERENCE <= level - target.getLevel())
            {                
                return Convert.ToUInt32(Math.Round(damage + extraDamage));
            }         
            
            if (MINIMUM_LEVEL_DIFFERENCE <= target.getLevel() - level)
            {                
                return Convert.ToUInt32(Math.Round(damage - extraDamage));
            }

            return damage;
        }

        protected bool isInRange(Prop target)
        {
            double targetDistance = distance(target);
            
            return weapon.isInRange(targetDistance);         
        }

        public Weapon getWeapon()
        {
            return weapon;
        }
        
        public void healSelf(uint amount)
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

        public void levelUp()
        {
            level++;
        }

        public void attack(Character target, uint damage)
        {
            if (belongsToSameFaction(target)) { return; }

            if (target == this) { return; }

            damage = recalculateDamageAccordingToLevel(damage, target);

            // Calls the attack part that is common to target and props to avoid code replication.
            attack((Prop)target, damage);
        }

        public void attack(Prop target, uint damage)
        {
            if (isInRange(target) == false) { return; }

            target.sufferDamage(damage);
        }

        public void joinFaction(string faction)
        {
            factions.Add(faction);
        }

        public void leaveFaction(string faction)
        {
            factions.Remove(faction);
        }

        public void heal(Character target, uint amount)
        {
            if (belongsToSameFaction(target) == false) { return; }

            target.healSelf(amount);
        }

        public HashSet<string> getFactions()
        {
            return factions;
        }

        public bool belongsToSameFaction(Character other)
        {
            HashSet<string> otherFactions = other.getFactions();
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
        internal protected void sufferDamage(uint damage)
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

        public (double x, double y) getPosition()
        {
            return position;
        }

        public uint getHealth()
        {
            return health;
        }

        public void setPosition(double x, double y)
        {
            position = (x, y);
        }

        public bool isAlive()
        {
            return alive;
        }

        protected double distance(Prop other)
        {
            // Distance formula = √[(x₂ - x₁)² + (y₂ - y₁)²]
            double distance = Math.Sqrt(Math.Pow(position.x - other.getPosition().x, 2) +
                                        Math.Pow(position.y + other.getPosition().y, 2));

            return distance;
        }
    }

    public abstract class Weapon
    { 
        public abstract double range { get; }

        public bool isInRange(double distance)
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