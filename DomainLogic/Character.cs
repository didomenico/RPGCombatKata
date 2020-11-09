using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace DomainLogic
{    
    public enum Weapon { Melee, Ranged}
    public class Character : Prop
    {                
        protected int level = 1;

        // If undefined, a character is created without weapons. 
        // He will fight bare-handed and thus, melee.
        protected Weapon weapon = Weapon.Melee;

        protected HashSet<string> factions = new HashSet<string>();

        public Character() : base(1000) {}

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

        protected int recalculateDamageAccordingToLevel(int damage, Character target)
        {
            // If the target is 5 or more levels below the attacker, damage is increased by 50%.
            if (target.getLevel() + 5 <= level)
            {
                double increasedAmount = damage * 1.5;

                return Convert.ToInt32(Math.Round(increasedAmount));
            }

            // If the target is 5 or more levels above the attacker, damage is reduced by 50%.
            if (target.getLevel() - 5 >= level)
            {
                double debuffedAmount = damage * 0.5;

                return Convert.ToInt32(Math.Round(debuffedAmount));
            }

            // If they are within 4 levels of difference, deals the original damage
            return damage;
        }

        protected bool isInRange(Prop target)
        {
            if (getWeapon() == Weapon.Melee &&
                distance(target) > 2)
            {
                return false;
            }

            if(getWeapon() == Weapon.Ranged &&
               distance(target) > 20)
            {
                return false;
            }

            return true;
        }

        public Weapon getWeapon()
        {
            return weapon;    
        }

        // Heal himself. No need to check, because method is called from the object itself.
        public void heal(int amount)
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
        public void attack(Character target, int damage)
        {
            if (belongsToSameFaction(target)) { return; }

            if (target == this) { return; }

            damage = recalculateDamageAccordingToLevel(damage, target);

            // Calls the attack part that is common to target and props to avoid code replication.
            attack((Prop)target, damage);
        }

        public void attack(Prop target, int damage)
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

        public void heal(Character target, int amount)
        {            
            if(belongsToSameFaction(target) == false) { return; }            

            target.heal(amount); 
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
        protected int health = 1000;

        protected bool alive = true;

        protected (double x, double y) position = (0.0, 0.0);

        public Prop(int health)
        {
            this.health = health;
        }

        public Prop(int health, double x, double y) : this(health)
        {
            position = (x, y);
        }

        // Must make it internal to enable Character to attack a Prop
        internal protected void sufferDamage(int damage)
        {
            health -= damage;

            if(health < 0) { health = 0; }

            // Not sure what it meant to be destroyed when health drops to 0,
            // but since C# doesn't have explicit memory management, I considered 
            // it's a flag instead of freeing the object from memory.
            if (health == 0) { alive = false; }            
        }

        public (double x, double y) getPosition()
        {
            return position;
        }

        public int getHealth()
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
}