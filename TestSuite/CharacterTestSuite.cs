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
        public void TestCharacterInitialization()
        {
            Character character = new Character();

            Assert.AreEqual(1000, character.getHealth());
            Assert.AreEqual(1, character.getLevel());
            Assert.IsTrue(character.isAlive());
        }

        [TestMethod]
        public void TestDamagingNonLethaly()
        {
            Character attacker = new Character();
            Character target = new Character();

            attacker.attack(target, 100);            

            Assert.AreEqual(900, target.getHealth());           
        }

        [TestMethod]
        public void TestDamagingLethaly()
        {
            Character attacker = new Character();
            Character target = new Character();

            attacker.attack(target, 1200);            

            Assert.AreEqual(target.getHealth(), 0);
            Assert.AreEqual(target.isAlive(), false);
        }

        //Damage dealt drops target health exactly to 0.
        [TestMethod]
        public void TestDamagingLethalyExactlyToZero()
        {
            Character attacker = new Character();
            Character target = new Character();

            attacker.attack(target, 1000);
            
            Assert.AreEqual(0, target.getHealth());
            Assert.IsFalse(target.isAlive());
        }

        [TestMethod]
        public void TestHealingBelowMaxHealth()
        {
            Character attacker = new Character();
            Character target = new Character();

            attacker.attack(target, 700);            
            target.heal(100);

            Assert.AreEqual(400, target.getHealth());
        }

        [TestMethod]
        public void TestHealingExceedingMaxHealth()
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
        public void TestHealingToExactMaxHealth()
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
        public void TestDamagingSelf()
        {
            // A character cannot deal damage to itself.
            Character target = new Character();

            target.attack(target, 100);            

            Assert.AreEqual(1000, target.getHealth());
        }
      
        [TestMethod]
        public void TestDamagingWhenAttackerIsMoreThanFiveLevelsAbove()
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
        public void TestDamagingWhenTargetIsMoreThanFiveLevelsAbove()
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
        public void TestDamagingMeleeOutOfRange()
        {
            Character melee = new Character(new Melee());
            Character ranged = new Character(new Ranged());

            melee.setPosition(-1.2, -0.3);
            ranged.setPosition(0, 3);

            melee.attack(ranged, 100);            

            Assert.AreEqual(1000, ranged.getHealth());
        }

        [TestMethod]
        public void TestDamagingMeleeInRange()
        {
            Character melee = new Character(new Melee());
            Character ranged = new Character(new Ranged());

            ranged.setPosition(1, 0.5);
            ranged.setPosition(0.2, 0.5);
            
            melee.attack(ranged, 100);

            Assert.AreEqual(900, ranged.getHealth());
        }

        [TestMethod]
        public void TestDamagingMeleeInRangeLimit()
        {
            Character melee = new Character(new Melee());
            Character ranged = new Character(new Ranged());

            ranged.setPosition(0, 2);            
            melee.attack(ranged, 100);

            Assert.AreEqual(900, ranged.getHealth());
        }

        [TestMethod]
        public void TestDamagingRangedOutOfRange()
        {
            Character melee = new Character(new Melee());
            Character ranged = new Character(new Ranged());

            ranged.setPosition(15.4, 22.3);
            melee.setPosition(-4.3, 1.1);
            
            ranged.attack(melee, 100);

            Assert.AreEqual(1000, melee.getHealth());
        }

        [TestMethod]
        public void TestDamagingRangedInRange()
        {
            Character melee = new Character(new Melee());
            Character ranged = new Character(new Ranged());

            ranged.setPosition(10.4, 12.3);
            melee.setPosition(-1.3, 1.1);

            ranged.attack(melee, 100);

            Assert.AreEqual(900, melee.getHealth());
        }

        [TestMethod]
        public void TestDamagingRangedInRangeLimit()
        {
            Character melee = new Character(new Melee());
            Character ranged = new Character(new Ranged());

            ranged.setPosition(20, 0);
            ranged.attack(melee, 100);

            Assert.AreEqual(900, melee.getHealth());
        }
        
        /************
          Iteration 4
        ************/
        [TestMethod]
        public void TestDamagingAlly()
        {
            Character attacker = new Character();
            Character ally = new Character();

            attacker.joinFaction("Test");
            ally.joinFaction("Test");

            attacker.attack(ally, 100);

            Assert.AreEqual(1000, ally.getHealth());
        }

        [TestMethod]
        public void TestDamagingEnemy()
        {
            Character attacker = new Character();
            Character ally = new Character();

            attacker.joinFaction("Test");
            ally.joinFaction("Test");

            attacker.attack(ally, 100);

            Assert.AreEqual(1000, ally.getHealth());
        }

        [TestMethod]
        public void TestHealingAlly()
        {
            Character healer = new Character();
            Character ally = new Character();

            healer.attack(ally, 200);

            healer.joinFaction("Test");
            ally.joinFaction("Test");

            healer.heal(ally, 100);

            Assert.AreEqual(900, ally.getHealth());
        }

        [TestMethod]
        public void TestHealingEnemy()
        {
            Character healer = new Character();
            Character enemy = new Character();

            healer.attack(enemy, 200);

            healer.joinFaction("Test");
            enemy.joinFaction("Enemy");
            
            healer.heal(enemy, 100);

            Assert.AreEqual(800, enemy.getHealth());
        }

        [TestMethod]
        public void TestHealingNoFaction()
        {
            Character healer = new Character();
            Character enemy = new Character();

            healer.attack(enemy, 200);          
            healer.heal(enemy, 100);

            Assert.AreEqual(800, enemy.getHealth());
        }

        [TestMethod]
        public void TestHealingFactionAndNoFaction()
        {
            Character healer = new Character();
            Character enemy = new Character();

            healer.attack(enemy, 200);

            healer.joinFaction("Test");

            healer.heal(enemy, 100);

            Assert.AreEqual(800, enemy.getHealth());
        }

        [TestMethod]
        public void TestHealingNoFactionAndFaction()
        {
            Character healer = new Character();
            Character enemy = new Character();

            healer.attack(enemy, 200);

            enemy.joinFaction("Enemy");

            healer.heal(enemy, 100);

            Assert.AreEqual(800, enemy.getHealth());
        }

        [TestMethod]
        public void TestHealingAllyMultipleAndSingleFaction()
        {
            Character healer = new Character();
            Character enemy = new Character();

            healer.attack(enemy, 200);
            
            healer.joinFaction("Enemy");
            healer.joinFaction("Test");

            enemy.joinFaction("Enemy");

            healer.heal(enemy, 100);

            Assert.AreEqual(900, enemy.getHealth());
        }

        [TestMethod]
        public void TestHealingAllySingleAndMultipleFaction()
        {
            Character healer = new Character();
            Character enemy = new Character();

            healer.attack(enemy, 200);

            healer.joinFaction("Test");            

            enemy.joinFaction("Enemy");
            enemy.joinFaction("Test");

            healer.heal(enemy, 100);

            Assert.AreEqual(900, enemy.getHealth());
        }

        [TestMethod]
        public void TestHealingHealerLeftFaction()
        {
            Character healer = new Character();
            Character enemy = new Character();

            healer.attack(enemy, 200);

            healer.joinFaction("Test");
            healer.joinFaction("Enemy");           
            healer.leaveFaction("Enemy");

            enemy.joinFaction("Enemy");
            
            healer.heal(enemy, 100);

            Assert.AreEqual(800, enemy.getHealth());
        }

        [TestMethod]
        public void TestHealingTargetLeftFaction()
        {
            Character healer = new Character();
            Character enemy = new Character();

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
        public void TestDamagingNonLetalyProp()
        {
            Prop tree = new Prop(2000);
            Character attacker = new Character();

            attacker.attack(tree, 200);

            Assert.AreEqual(1800, tree.getHealth());
            Assert.IsTrue(tree.isAlive());
        }

        [TestMethod]
        public void TestDamagingLetalyProp()
        {
            Prop tree = new Prop(2000);
            Character attacker = new Character();

            attacker.attack(tree, 2200);

            Assert.AreEqual(0, tree.getHealth());
            Assert.IsFalse(tree.isAlive());
        }

        [TestMethod]
        public void TestDamagingLethalyExactlyToZeroProp()
        {
            Prop tree = new Prop(2000);
            Character attacker = new Character();

            attacker.attack(tree, 2000);

            Assert.AreEqual(0, tree.getHealth());
            Assert.IsFalse(tree.isAlive());
        }
    }
}
