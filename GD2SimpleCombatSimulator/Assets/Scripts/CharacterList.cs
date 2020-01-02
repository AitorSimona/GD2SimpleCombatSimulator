using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterList
{
    public struct Spell
    {
        public string name;
        public uint damage;
        public uint mana_cost;
    }

    public struct Character
    {
        public Character(string given_name, uint hp, uint atk, uint rge, uint sta, uint mana, uint spd, uint lvl = 0, bool mage = false)
        {
            name = given_name;
            HP = hp;
            attack = atk;
            range = rge;
            stamina = sta;
            this.mana = mana;
            speed = spd;
            level = lvl;
            is_mage = mage;
            is_freezed = false;
            last_action = "none";
        }

        public void ModifyHP(uint value, bool add)
        {
            if(add)
                HP += value;

            else
                HP -= value;
        }

        public string name;
        public uint HP;
        public uint attack;
        public uint range;
        public uint stamina;
        public uint mana;
        public uint speed;
        public uint level;
        public bool is_mage;
        public string last_action;



        public bool is_freezed;
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
        Character character1 = new Character("Protagonist",5,3,5,5,0,3, 0, true);
        Team.Add(character1);

        Character character2 = new Character("Secondary1",10,5,1,3,0,2);
        Team.Add(character2);

        Character character3 = new Character("Secondary2",5,7,3,1,5,1);
        Team.Add(character3);

        Debug.Log("Created Awesome characters: Team A");
    }

    void CreateTeamB(List<CharacterList.Character> Team)
    {
        // Fill team here
        Character character1 = new Character("Antagonist",50,10,3,3,10,2,0, true);
        Team.Add(character1);

        Character character2 = new Character("Minion1",8,4,1,3,0,2);
        Team.Add(character2);

        Character character3 = new Character("Minion2",3,2,5,5,0,3);
        Team.Add(character3);

        Character character4 = new Character("Minion3",3,5,3,1,5,1,0,true);
        Team.Add(character4);

        Debug.Log("Created Awesome characters: Team B");
    }
}
