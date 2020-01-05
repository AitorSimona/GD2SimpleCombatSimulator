using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterList
{
    public struct Spell // cone of cold, bolt
    {

        public Spell(string name, int damage, uint mana_cost)
        {
            this.damage = damage;
            this.name = name;
            this.mana_cost = mana_cost;
        }

        public string name;
        public int damage;
        public uint mana_cost;
    }

    public struct Item
    {

        public Item(string name, int damage )
        {
            this.damage = damage;
            this.name = name;
        }

        public string name; // Fire bomb, hp potion
        public int damage;
    }

    public class Character
    {
        public Character(bool ally,string given_name, uint hp, uint atk, uint rge, uint sta, int mana, uint spd, int xp, Item it, Spell sp,uint lvl = 1, bool mage = false)
        {
            // 
            bool TESTS = false;
            int desired_level = 1;
            //

            name = given_name;
            HP = (int)hp;
            maxHP = maxHPref = hp;
            attack = atk;
            maxATKref = (int)attack;
            range = rge;
            stamina = (int)sta;
            maxSTA = maxSTAref = sta;
            this.mana = mana;
            maxMana = maxManaref = mana;
            speed = spd;
            maxSPDref = (int)speed;
            level = lvl;
            is_mage = mage;
            is_freezed = false;
            last_action = "none";
            was_dead = false;
            item_used = false;
            spell = sp;
            item = it;
            XP = xp;

            ModifyLevel(0);
            // Uncomment to force level (remember to set force_level var in game manager to true)

            //for (uint i = 0; i < desired_level - 1; ++i)
            //{
            //    ModifyLevel(1);
            //}

            // Uncomment stuff to force allies attributes
            if (ally && TESTS)
            {
                int num = 15;

                //HP = num;
                //maxHP = (uint)num;
                //maxHPref = (uint)num;

                //attack = (uint)num;
                //maxATKref = num;

                //mana = num;
                //maxMana = num;
                //maxManaref = num;

                //stamina = num;
                //maxSTA = (uint)num;
                //maxSTAref = (uint)num;

                speed = (uint)num;
                maxSPDref = num;
            }
        }

        public void ModifyHP(int value, bool add)
        {
            if(add)
                HP += value;

            else
                HP -= value;

            if (HP > maxHP)
                HP = (int)maxHP;
        }

        public void ModifySTA(int value, bool add)
        {
            if (add)
                stamina += value;

            else
                stamina -= value;

            if (stamina > maxSTA)
                stamina = (int)maxSTA;
        }

        public void ModifyMana(int value)
        {
                mana -= value;
        }

        public void ModifyLevel(uint value)
        {
            // value will always be 1 
            // all commented stuff was part of a previous iteration

            if(name == "Protagonist")
            {
                // HP
                if (level < 5 || level > 15)
                {
                    HP += 1;
                    maxHP += 1;
                }

                // attack
                if (level == 3 || level == 6 /*|| level == 9*/ || level == 12
                    || level == 15 /*|| level == 18*/ || level == 20)
                    attack++;

                // Stamina
                if (level == 1 || level == 3 || level == 5 /*|| level == 7*/ || level == 10
                 /*|| level == 9 || level == 11*/ || level == 13 /*|| level == 15 || level == 17*/ || level == 16
                 || level == 19)
                {
                    stamina++;
                    maxSTA++;
                }

                // Speed
                if (level == 5 || level == 10 /*|| level == 15*/ || level == 20)
                    speed++;
            }
            else if(name == "Secondary1")
            {
                if (level < 5 || level > 15)
                {
                    HP += 1;
                    maxHP += 1;
                }
                // attack
                if (level == 4 || level == 5 /*|| level == 9*/ || level == 10
                    /*|| level == 14*/ || level == 15)
                    attack++;

                if (level == 19)
                    attack += 2;

                // Stamina
                if (level == 1 /*|| level == 2*/ || level == 3 || level == 6 || level == 9
                 /*|| level == 7 || level == 8 || level == 11*/ || level == 12 /*|| level == 13*/ || level == 15
                 /*|| level == 16 || level == 17*/ || level == 18)
                {
                    stamina++;
                    maxSTA++;
                }

                // Speed
                if (level == 5 /*|| level == 10*/ || level == 15)
                    speed++;
            }
            else if (name == "Secondary2")
            {
                mana++;
                maxMana++;

                if (level < 5 || level > 15)
                {
                    HP += 1;
                    maxHP += 1;
                }

                // attack
                if (level == 1 /*|| level == 3*/ || level == 5 /*|| level == 7*/ || level == 10
                    /*|| level == 9 || level == 11*/ || level == 15 || level == 19)
                    attack++;
            }

            level += value;
        }

        public void ResetLevelAndXP()
        {
            level = 1;
            XP = 0;
            HP = (int)maxHPref;
            mana = maxManaref;
            stamina = (int)maxSTAref;
            speed = (uint)maxSPDref;
            attack = (uint)maxATKref;
        }

        public void ModifyXP(int value)
        {
            XP += value;
        }

        public uint GetLevel()
        {
            return level;
        }

        public int GetHP()
        {
            return HP;
        }

        public bool GetMage()
        {
            return is_mage;
        }

        public bool GetFreezed()
        {
            return is_freezed;
        }

        public bool GetDead()
        {
            return was_dead;
        }

        public bool GetItemUsed()
        {
            return item_used;
        }

        public void SetFreezed(bool value)
        {
           is_freezed = value;
        }

        public void SetDead(bool value)
        {
            was_dead = value;
        }

        public void SetItemUsed(bool value)
        {
            item_used = value;
        }

        public uint GetAttack()
        {
            return attack;
        }

        public int GetMana()
        {
            return mana;
        }

        public uint GetSpeed()
        {
            return speed;
        }

        public int GetStamina()
        {
            return stamina;
        }

        public int GetXP()
        {
            return XP;
        }

        public uint GetRange()
        {
            return range;
        }

        public string GetName()
        {
            return name;
        }

        public Spell GetSpell()
        {
            return spell;
        }

        public Item GetItem()
        {
            return item;
        }


        public string GetLastAction()
        {
            return last_action;
        }

        public void SetLastAction(string value)
        {
            last_action = value;
        }

        public void Reset()
        {
            HP = (int)maxHP;
            is_freezed = false;
            last_action = "none";
            was_dead = false;
            mana = maxMana;
            item_used = false;
            stamina = (int)maxSTA;
        }

        string name;
        int HP;
        uint attack;
        uint range;
        int stamina;
        int mana;
        uint speed;
        uint level;
        string last_action;

        uint maxHP;
        int maxMana;
        uint maxSTA;
        uint maxHPref;
        int maxManaref;
        uint maxSTAref;
        int maxSPDref;
        int maxATKref;
        int XP;

        Item item;
        Spell spell;

        bool is_mage;
        bool is_freezed;
        bool was_dead;
        bool item_used;
    }

    public enum Teams
    {
        A,
        B
    }


    public void CreateTeam(List<CharacterList.Character> Team, Teams type)
    {
        switch(type)
        {
            case Teams.A:
                CreateTeamA(Team);
                break;

            case Teams.B:
                CreateTeamB(Team);
                break;
        }

    }

    void CreateTeamA(List<CharacterList.Character> Team)
    {
        // Fill team here

        Item potion = new Item("HP potion", 5);
        Item fire_bomb = new Item("Fire bomb", 5);

        Spell none = new Spell("none", 0, 0);

        Spell bolt = new Spell("Bolt", 7, 7);
        Spell coneofcold = new Spell("ConeOfCold", 0, 5);

        Character character1 = new Character(true,"Protagonist",5,3,5,5,0,3,0,fire_bomb, none);
        Team.Add(character1);

        Character character2 = new Character(true,"Secondary1",10,5,1,3,0,2,0,potion,none);
        Team.Add(character2);

        Character character3 = new Character(true,"Secondary2",5,7,3,2,5,1,0,potion,coneofcold,1,true);
        Team.Add(character3);

        Debug.Log("Created Awesome characters: Team A");
    }

    void CreateTeamB(List<CharacterList.Character> Team)
    {
        Item potion = new Item("HP potion", 5);
        Item fire_bomb = new Item("Fire bomb", 5);

        Spell none = new Spell("none", 0, 0);

        Spell bolt = new Spell("Bolt", 7, 7);
        Spell coneofcold = new Spell("ConeOfCold", 0, 5);

        // Fill team here
        Character character1 = new Character(false,"Antagonist",50,10,3,3,10,2,3000,fire_bomb, bolt, 1, true);
        Team.Add(character1);

        Character character2 = new Character(false,"Minion1",12,4,1,3,0,2,200, potion, none);
        Team.Add(character2);

        Character character3 = new Character(false,"Minion2",8,2,5,5,0,3,300, potion, none);
        Team.Add(character3);

        Character character4 = new Character(false,"Minion3",8,5,3,2,5,1,500,potion, coneofcold,1, true);
        Team.Add(character4);

        Debug.Log("Created Awesome characters: Team B");
    }
}
