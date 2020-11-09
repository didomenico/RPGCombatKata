using DomainLogic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection.PortableExecutable;

namespace TestSuite
{
    [TestClass]
    public class CharacterTestSuite
    {
        /************
          Iteration 1
        ************/
        [TestMethod]
        public void testCharacterInitialization()
        {
            Character character = new Character();

            Assert.AreEqual(1000, character.getHealth());
            Assert.AreEqual(1, character.getLevel());
            Assert.IsTrue(character.isAlive());
        }

        [TestMethod]
        public void testDamagingNonLethaly()
        {
            Character attacker = new Character();
            Character target = new Character();

            attacker.attack(target, 100);            

            Assert.AreEqual(900, target.getHealth());           
        }

        [TestMethod]
        public void testDamagingLethaly()
        {
            Character attacker = new Character();
            Character target = new Character();

            attacker.attack(target, 1200);            

            Assert.AreEqual(target.getHealth(), 0);
            Assert.AreEqual(target.isAlive(), false);
        }

        //Damage dealt drops target health exactly to 0.
        [TestMethod]
        public void testDamagingLethalyExactlyToZero()
        {
            Character attacker = new Character();
            Character target = new Character();

            attacker.attack(target, 1000);
            
            Assert.AreEqual(0, target.getHealth());
            Assert.IsFalse(target.isAlive());
        }

        [TestMethod]
        public void testHealingBelowMaxHealth()
        {
            Character attacker = new Character();
            Character target = new Character();

            attacker.attack(target, 700);            
            target.heal(100);

            Assert.AreEqual(400, target.getHealth());
        }

        [TestMethod]
        public void testHealingExceedingMaxHealth()
        {
            Character attacker = new Character();
            Character target = new Character();

            attacker.attack(target, 600); 
            target.heal(700);

            Assert.AreEqual(1000, target.getHealth());
        }

        //Test case where received healing will make the targets reach 
        //their exact maximum health, instead of more.
        [TestMethod]
        public void testHealingToExactMaxHealth()
        {
            Character attacker = new Character();
            Character target = new Character();

            attacker.attack(target, 100);            
            target.heal(100);

            Assert.AreEqual(1000, target.getHealth());
        }

        /************
          Iteration 2
        ************/
        [TestMethod]
        public void testDamagingSelf()
        {
            // A character cannot deal damage to itself.
            Character target = new Character();

            target.attack(target, 100);            

            Assert.AreEqual(1000, target.getHealth());
        }
      
        [TestMethod]
        public void testDamagingWhenAttackerIsMoreThanFiveLevelsAbove()
        {
            Character attacker = new Character();
            Character target = new Character();

            //If the target is 5 or more Levels above the attacker, damage is reduced by 50 %
            for (int i = 0; i < 5; i++)
            {
                attacker.levelUp();
            }

            attacker.attack(target, 100);            

            Assert.AreEqual(850, target.getHealth());
        }

        [TestMethod]
        public void testDamagingWhenTargetIsMoreThanFiveLevelsAbove()
        {
            Character attacker = new Character();
            Character target = new Character();

            //If the target is 5 or more Levels above the attacker, damage is reduced by 50 %
            for (int i = 0; i < 5; i++)
            {
                target.levelUp();
            }
            attacker.attack(target, 100);            

            Assert.AreEqual(950, target.getHealth());
        }
     
        /************
          Iteration 3
        ************/
        [TestMethod]
        public void testDamagingMeleeOutOfRange()
        {
            Character melee = new Character(Weapon.Melee);
            Character ranged = new Character(Weapon.Ranged);

            melee.setPosition(-1.2, -0.3);
            ranged.setPosition(0, 3);

            melee.attack(ranged, 100);            

            Assert.AreEqual(1000, ranged.getHealth());
        }

        [TestMethod]
        public void testDamagingMeleeInRange()
        {
            Character melee = new Character(Weapon.Melee);
            Character ranged = new Character(Weapon.Ranged);

            ranged.setPosition(1, 0.5);
            ranged.setPosition(0.2, 0.5);
            
            melee.attack(ranged, 100);

            Assert.AreEqual(900, ranged.getHealth());
        }

        [TestMethod]
        public void testDamagingMeleeInRangeLimit()
        {
            Character melee = new Character(Weapon.Melee);
            Character ranged = new Character(Weapon.Ranged);

            ranged.setPosition(0, 2);            
            melee.attack(ranged, 100);

            Assert.AreEqual(900, ranged.getHealth());
        }

        [TestMethod]
        public void testDamagingRangedOutOfRange()
        {
            Character melee = new Character(Weapon.Melee);
            Character ranged = new Character(Weapon.Ranged);

            ranged.setPosition(15.4, 22.3);
            melee.setPosition(-4.3, 1.1);
            
            ranged.attack(melee, 100);

            Assert.AreEqual(1000, melee.getHealth());
        }

        [TestMethod]
        public void testDamagingRangedInRange()
        {
            Character melee = new Character(Weapon.Melee);
            Character ranged = new Character(Weapon.Ranged);

            ranged.setPosition(10.4, 12.3);
            melee.setPosition(-1.3, 1.1);

            ranged.attack(melee, 100);

            Assert.AreEqual(900, melee.getHealth());
        }

        [TestMethod]
        public void testDamagingRangedInRangeLimit()
        {
            Character melee = new Character(Weapon.Melee);
            Character ranged = new Character(Weapon.Ranged);

            ranged.setPosition(20, 0);
            ranged.attack(melee, 100);

            Assert.AreEqual(900, melee.getHealth());
        }
        
        /************
          Iteration 4
        ************/
        [TestMethod]
        public void testDamagingAlly()
        {
            Character attacker = new Character(Weapon.Melee);
            Character ally = new Character(Weapon.Ranged);

            attacker.joinFaction("Test");
            ally.joinFaction("Test");

            attacker.attack(ally, 100);

            Assert.AreEqual(1000, ally.getHealth());
        }

        [TestMethod]
        public void testDamagingEnemy()
        {
            Character attacker = new Character(Weapon.Melee);
            Character ally = new Character(Weapon.Ranged);

            attacker.joinFaction("Test");
            ally.joinFaction("Test");

            attacker.attack(ally, 100);

            Assert.AreEqual(1000, ally.getHealth());
        }

        [TestMethod]
        public void testHealingAlly()
        {
            Character healer = new Character(Weapon.Melee);
            Character ally = new Character(Weapon.Ranged);

            healer.attack(ally, 200);

            healer.joinFaction("Test");
            ally.joinFaction("Test");

            healer.heal(ally, 100);

            Assert.AreEqual(900, ally.getHealth());
        }

        [TestMethod]
        public void testHealingEnemy()
        {
            Character healer = new Character(Weapon.Melee);
            Character enemy = new Character(Weapon.Ranged);

            healer.attack(enemy, 200);

            healer.joinFaction("Test");
            enemy.joinFaction("Enemy");
            
            healer.heal(enemy, 100);

            Assert.AreEqual(800, enemy.getHealth());
        }

        [TestMethod]
        public void testHealingNoFaction()
        {
            Character healer = new Character(Weapon.Melee);
            Character enemy = new Character(Weapon.Ranged);

            healer.attack(enemy, 200);          
            healer.heal(enemy, 100);

            Assert.AreEqual(800, enemy.getHealth());
        }

        [TestMethod]
        public void testHealingFactionAndNoFaction()
        {
            Character healer = new Character(Weapon.Melee);
            Character enemy = new Character(Weapon.Ranged);

            healer.attack(enemy, 200);

            healer.joinFaction("Test");

            healer.heal(enemy, 100);

            Assert.AreEqual(800, enemy.getHealth());
        }

        [TestMethod]
        public void testHealingNoFactionAndFaction()
        {
            Character healer = new Character(Weapon.Melee);
            Character enemy = new Character(Weapon.Ranged);

            healer.attack(enemy, 200);

            enemy.joinFaction("Enemy");

            healer.heal(enemy, 100);

            Assert.AreEqual(800, enemy.getHealth());
        }

        [TestMethod]
        public void testHealingAllyMultipleAndSingleFaction()
        {
            Character healer = new Character(Weapon.Melee);
            Character enemy = new Character(Weapon.Ranged);

            healer.attack(enemy, 200);
            
            healer.joinFaction("Enemy");
            healer.joinFaction("Test");

            enemy.joinFaction("Enemy");

            healer.heal(enemy, 100);

            Assert.AreEqual(900, enemy.getHealth());
        }

        [TestMethod]
        public void testHealingAllySingleAndMultipleFaction()
        {
            Character healer = new Character(Weapon.Melee);
            Character enemy = new Character(Weapon.Ranged);

            healer.attack(enemy, 200);

            healer.joinFaction("Test");            

            enemy.joinFaction("Enemy");
            enemy.joinFaction("Test");

            healer.heal(enemy, 100);

            Assert.AreEqual(900, enemy.getHealth());
        }

        [TestMethod]
        public void testHealingHealerLeftFaction()
        {
            Character healer = new Character(Weapon.Melee);
            Character enemy = new Character(Weapon.Ranged);

            healer.attack(enemy, 200);

            healer.joinFaction("Test");
            healer.joinFaction("Enemy");           
            healer.leaveFaction("Enemy");

            enemy.joinFaction("Enemy");
            
            healer.heal(enemy, 100);

            Assert.AreEqual(800, enemy.getHealth());
        }

        [TestMethod]
        public void testHealingTargetLeftFaction()
        {
            Character healer = new Character(Weapon.Melee);
            Character enemy = new Character(Weapon.Ranged);

            healer.attack(enemy, 200);

            healer.joinFaction("Test");

            enemy.joinFaction("Enemy");
            enemy.joinFaction("Test");
            enemy.leaveFaction("Test");

            healer.heal(enemy, 100);

            Assert.AreEqual(800, enemy.getHealth());
        }

        /************
          Iteration 5
        ************/
        [TestMethod]
        public void testDamagingNonLetalyProp()
        {
            Prop tree = new Prop(2000);
            Character attacker = new Character();

            attacker.attack(tree, 200);

            Assert.AreEqual(1800, tree.getHealth());
            Assert.IsTrue(tree.isAlive());
        }

        [TestMethod]
        public void testDamagingLetalyProp()
        {
            Prop tree = new Prop(2000);
            Character attacker = new Character();

            attacker.attack(tree, 2200);

            Assert.AreEqual(0, tree.getHealth());
            Assert.IsFalse(tree.isAlive());
        }

        [TestMethod]
        public void testDamagingLethalyExactlyToZeroProp()
        {
            Prop tree = new Prop(2000);
            Character attacker = new Character();

            attacker.attack(tree, 2000);

            Assert.AreEqual(0, tree.getHealth());
            Assert.IsFalse(tree.isAlive());
        }

    }
}
