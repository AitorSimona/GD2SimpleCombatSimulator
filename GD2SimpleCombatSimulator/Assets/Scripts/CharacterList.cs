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
        public Character(string given_name, uint hp, uint atk, uint rge, uint sta, int mana, uint spd, Item it, Spell sp,uint lvl = 0, bool mage = false)
        {
            name = given_name;
            HP = (int)hp;
            maxHP = hp;
            attack = atk;
            range = rge;
            stamina = (int)sta;
            maxSTA = sta;
            this.mana = mana;
            maxMana = mana;
            speed = spd;
            level = lvl;
            is_mage = mage;
            is_freezed = false;
            last_action = "none";
            was_dead = false;
            item_used = false;
            spell = sp;
            item = it;
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
            level += value;
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
        Spell coneofcold = new Spell("ConeOfCold", 7, 7);

        Character character1 = new Character("Protagonist",5,3,5,5,0,3,fire_bomb, none);
        Team.Add(character1);

        Character character2 = new Character("Secondary1",10,5,1,3,0,2,potion,none);
        Team.Add(character2);

        Character character3 = new Character("Secondary2",5,7,3,1,5,1,potion,coneofcold,0,true);
        Team.Add(character3);

        Debug.Log("Created Awesome characters: Team A");
    }

    void CreateTeamB(List<CharacterList.Character> Team)
    {
        Item potion = new Item("HP potion", 5);
        Item fire_bomb = new Item("Fire bomb", 5);

        Spell none = new Spell("none", 0, 0);

        Spell bolt = new Spell("Bolt", 7, 7);
        Spell coneofcold = new Spell("ConeOfCold", 7, 7);

        // Fill team here
        Character character1 = new Character("Antagonist",50,10,3,3,10,2, fire_bomb, bolt, 0, true);
        Team.Add(character1);

        Character character2 = new Character("Minion1",8,4,1,3,0,2, potion, none);
        Team.Add(character2);

        Character character3 = new Character("Minion2",3,2,5,5,0,3, potion, none);
        Team.Add(character3);

        Character character4 = new Character("Minion3",3,5,3,1,5,1,potion, coneofcold,0, true);
        Team.Add(character4);

        Debug.Log("Created Awesome characters: Team B");
    }
}
