using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // --- Settings ---
    public Dropdown PlayerA;
    public Dropdown PlayerB;

    uint iterations = 0;
    uint count = 0;
    uint steps = 0;
    int currentcharA = 0;
    int currentcharB = 0;

    int PlayerAWins = 0;
    int PlayerBWins = 0;

    uint combats = 0;

    public InputField iters;

    // --- Game ---
    public Dropdown ActionA;
    public Dropdown ActionB;
    public Text CurrentCharacterA;
    public Text CurrentCharacterB;
    public CharacterList.Character currentA;
    public CharacterList.Character currentB;
    public Toggle controlA;
    public Toggle controlB;

    public Text outputText;
    public Text statsText;

    public Button Playbutton;
    public Button stepbutton;

    bool playing = false;

    uint maxSPD = 10;
    uint STACostperatk = 3;
    uint STARecoveredperturn = 3;

    // --- Characters ---
    CharacterList character_manager;

    public Dictionary<string, List<CharacterList.Character>> teams;

    // --- Spells ---
    CharacterList.Spell spellA;
    CharacterList.Spell spellB;

    // Start is called before the first frame update
    void Start()
    {
        // --- Initialize variables ---
        character_manager = new CharacterList();
        teams = new Dictionary<string, List<CharacterList.Character>>();
        List<string> options = new List<string>();
        List<string> actions = new List<string>();
        PlayerA.ClearOptions();
        PlayerB.ClearOptions();
        ActionA.ClearOptions();
        ActionB.ClearOptions();


        // --- Create teams ---
        teams.Add("Allies",new List<CharacterList.Character>());
        teams.Add("Enemies", new List<CharacterList.Character>());

        // --- Fill dropdowns ---
        options.Add("Allies");
        PlayerA.AddOptions(options);

        options.Clear();
        options.Add("Enemies");
        PlayerB.AddOptions(options);


        // --- Create actions ---
        actions.Add("Attack");
        actions.Add("Defend");
        actions.Add("Cast Spell");
        actions.Add("Use Item");


        ActionA.AddOptions(actions);
        ActionB.AddOptions(actions);


        // --- Fill teams ---
        character_manager.CreateTeam(teams["Allies"], CharacterList.Teams.A);
        character_manager.CreateTeam(teams["Enemies"], CharacterList.Teams.B);
   

        // --- Set spells ---
        spellA = new CharacterList.Spell();
        spellA.name = "Bolt";
        spellA.damage = 7;
        spellA.mana_cost = 7;

        spellB = new CharacterList.Spell();
        spellB.name = "Cone of Cold";
        spellB.damage = 0;
        spellB.mana_cost = 5;
    }

    // Update is called once per frame
    void Update()
    {
        if(!playing)
        SetIterations(iters.text);

        UpdateUI();
    }

    void UpdateUI()
    {
        if (controlA.isOn == true || controlB.isOn == true)
        {
            stepbutton.interactable = true;
            Playbutton.interactable = false;
        }
        else
        {
            stepbutton.interactable = false;
            Playbutton.interactable = true;
        }

        if (controlA.isOn == true)
        {
            ActionA.interactable = true;
            CurrentCharacterA.enabled = true;
        }
        else
        {
            ActionA.interactable = false;
            CurrentCharacterA.enabled = false;
        }

        if (controlB.isOn == true)
        {
            ActionB.interactable = true;
            CurrentCharacterB.enabled = true;
        }
        else
        {
            ActionB.interactable = false;
            CurrentCharacterB.enabled = false;
        }

        if (currentcharA > teams[PlayerA.options[PlayerA.value].text].Count - 1)
            currentcharA = 0;

        if (currentcharB > teams[PlayerB.options[PlayerB.value].text].Count - 1)
            currentcharB = 0;

        if (teams[PlayerA.options[PlayerA.value].text].Count != 0)
        {
            currentA = teams[PlayerA.options[PlayerA.value].text][currentcharA];
            CurrentCharacterA.text = currentA.GetName();
        }
        if (teams[PlayerB.options[PlayerB.value].text].Count != 0)
        {
            currentB = teams[PlayerB.options[PlayerB.value].text][currentcharB];
            CurrentCharacterB.text = currentB.GetName();
        }

        // --- Update statistics panel ---
        PrintStatistics();
        
    }

    void PrintStatistics()
    {
        statsText.text = "Combat number: " + combats.ToString() + "\n"
            + "PlayerA Wins: " + PlayerAWins.ToString() + "\n" + "PlayerB Wins: " + PlayerBWins.ToString();

    }

    public void SetIterations(string its)
    {
        uint result;
        uint.TryParse(its, out result);

        if (result > 0)
            iterations = result;
    }

    void ResetVars()
    {
        combats = 0;
        statsText.text = "";
        iters.text = "0";
        outputText.text = "Simulation End";
        playing = false;
        steps = 0;
        count = 0;
        PlayerBWins = 0;
        PlayerAWins = 0;

        // --- Check for dead ---
        for (int i = 0; i < teams[PlayerA.options[PlayerA.value].text].Count; ++i)
        {
                teams[PlayerA.options[PlayerA.value].text][i].Reset();
        }

        for (int i = 0; i < teams[PlayerB.options[PlayerB.value].text].Count; ++i)
        {
                teams[PlayerB.options[PlayerB.value].text][i].Reset();
        }

        ResetTeams();

    }

    void ResetTeams()
    {
        teams["Allies"].Clear();
        teams["Enemies"].Clear();
        // --- Fill teams ---
        character_manager.CreateTeam(teams["Allies"], CharacterList.Teams.A);
        character_manager.CreateTeam(teams["Enemies"], CharacterList.Teams.B);
    }


    public void Play()
    {
        playing = true;

        ResetVars();


        if (iterations > 0)
        {
            Debug.Log("Starting simulation");

            for (uint i = 0; i < iterations * (teams[PlayerA.options[PlayerA.value].text].Count + teams[PlayerB.options[PlayerB.value].text].Count); ++ i) 
            {
                count++;

                SimulateCombat();

                UpdateCurrentCharacters();

                if (count > 50)
                {
                    count = 0;
                    outputText.text = "";
                }
            }


            iterations = 0;
        }

    }

    public void Step()
    {
        playing = true;

        if (iterations > 0)
        {
            count++;
            steps++;
            Debug.Log("Stepping simulation");
            Debug.Log("Iteration number: ");
            Debug.Log(iterations);

            SimulateCombat();

            UpdateCurrentCharacters();

            if (count > 50)
            {
                count = 0;
                outputText.text = "";
            }


            if (steps > teams[PlayerA.options[PlayerA.value].text].Count + teams[PlayerB.options[PlayerB.value].text].Count)
            {
                iterations -= 1;
                steps = 0;
            }

            CheckDead();
        }
        else
        {
            ResetVars();
        }
    }

    void UpdateCurrentCharacters()
    {
        CheckDead();

        currentcharA++;
        currentcharB++;

        if (currentcharA > teams[PlayerA.options[PlayerA.value].text].Count - 1)
            currentcharA = 0;

        if (currentcharB > teams[PlayerB.options[PlayerB.value].text].Count - 1)
            currentcharB = 0;

        if (teams[PlayerA.options[PlayerA.value].text].Count != 0)
        {
            currentA = teams[PlayerA.options[PlayerA.value].text][currentcharA];
            CurrentCharacterA.text = currentA.GetName();
        }
        if (teams[PlayerB.options[PlayerB.value].text].Count != 0)
        {
            currentB = teams[PlayerB.options[PlayerB.value].text][currentcharB];
            CurrentCharacterB.text = currentB.GetName();
        }

        // --- Update actions ---
        ActionA.ClearOptions();
        ActionB.ClearOptions();

        List<string> actionsA = new List<string>();
        List<string> actionsB = new List<string>();

        // --- Create actions ---
        actionsA.Add("Attack");
        actionsA.Add("Defend");
        actionsA.Add("Use Item");

        actionsB.Add("Attack");
        actionsB.Add("Defend");
        actionsB.Add("Use Item");

        if (currentA.GetMage())
            actionsA.Add("Cast Spell");

        if (currentB.GetMage())
            actionsB.Add("Cast Spell");

        ActionA.AddOptions(actionsA);
        ActionB.AddOptions(actionsB);
    }

    void SimulateCombat()
    {
        CheckDead();

        // --- Update actions ---
        ActionA.ClearOptions();
        ActionB.ClearOptions();

        List<string> actionsA = new List<string>();
        List<string> actionsB = new List<string>();

        // --- Create actions ---

        // A
        if (currentA.GetStamina() >= STACostperatk)
            actionsA.Add("Attack");

        actionsA.Add("Defend");

        if (currentA.GetItemUsed() == false)
            actionsA.Add("Use Item");

        if (currentA.GetMage())
            actionsA.Add("Cast Spell");
        // B

        if (currentB.GetStamina() >= STACostperatk)
            actionsB.Add("Attack");

        actionsB.Add("Defend");

        if (currentB.GetItemUsed() == false)
            actionsB.Add("Use Item");

        if (currentB.GetMage())
            actionsB.Add("Cast Spell");

        int randA = Random.Range(0, actionsA.Count-1);
        int randB = Random.Range(0, actionsB.Count-1);

        if (controlA.isOn == true)
            randA = ActionA.value;
        if (controlB.isOn == true)
            randB = ActionB.value;

        currentA.SetLastAction("none");

        if (currentA.GetHP() > 0 && !currentA.GetFreezed())
        {
            switch (randA)
            {
                case 0:
                    Attack(currentA);
                    break;

                case 1:
                    currentA.SetLastAction("Defend");
                    outputText.text = outputText.text + "\n" + currentA.GetName() + "Uses Defend";
                    break;

                case 2:
                    currentA.SetItemUsed(true);

                    if (currentA.GetItem().name == "HP potion")
                    {
                        currentA.ModifyHP(currentA.GetItem().damage, true);
                    }
                    else
                    {
                        if (teams[PlayerB.options[PlayerB.value].text].Count != 0)
                        {
                            int randAttack = Random.Range(0, teams[PlayerB.options[PlayerB.value].text].Count - 1);

                            if (teams[PlayerB.options[PlayerB.value].text][randAttack].GetLastAction() == "Defend")
                                teams[PlayerB.options[PlayerB.value].text][randAttack].ModifyHP((int)currentA.GetItem().damage / 2, false);
                            else
                                teams[PlayerB.options[PlayerB.value].text][randAttack].ModifyHP((int)currentA.GetItem().damage, false);
                        }
                    }

                    outputText.text = outputText.text + "\n" + currentA.GetName() + "Uses Item: " + currentA.GetItem().name;

                    break;

                case 3:
                    if (currentA.GetSpell().name != "none")
                    {

                        if (teams[PlayerB.options[PlayerB.value].text].Count != 0 && currentA.GetMana() >= currentA.GetSpell().mana_cost)
                        {
                            int randAttack = Random.Range(0, teams[PlayerB.options[PlayerB.value].text].Count - 1);

                            if (currentA.GetSpell().name != "ConeOfCold")
                            {
                                teams[PlayerB.options[PlayerB.value].text][randAttack].SetFreezed(true);
                            }
                            else
                            {
                                if (teams[PlayerB.options[PlayerB.value].text][randAttack].GetLastAction() == "Defend")
                                    teams[PlayerB.options[PlayerB.value].text][randAttack].ModifyHP((int)currentA.GetSpell().damage / 2, false);
                                else
                                    teams[PlayerB.options[PlayerB.value].text][randAttack].ModifyHP((int)currentA.GetSpell().damage, false);
                            }


                            currentA.ModifyMana((int)currentA.GetSpell().mana_cost);


                        }

                        if (currentA.GetMana() >= currentA.GetSpell().mana_cost)
                            outputText.text = outputText.text + "\n" + currentA.GetName() + "Uses Spell: " + currentA.GetSpell().name;
                        else
                            outputText.text = outputText.text + "\n" + currentA.GetName() + "Cannot use spell: " + currentA.GetSpell().name;

                    }
                    break;
            }

        }
        else if (currentA.GetFreezed())
        {
            outputText.text = outputText.text + "\n" + currentA.GetName() + " Is Freezed!";
            currentA.SetFreezed(false);
        }

        currentB.SetLastAction("none");

        if (currentB.GetHP() > 0 && !currentB.GetFreezed())
        {
            switch (randB)
            {
                case 0:
                    Attack(currentB);
                    break;

                case 1:
                    currentB.SetLastAction("Defend");
                    outputText.text = outputText.text + "\n" + currentB.GetName() + "Uses Defend";
                    break;

                case 2:
                    currentB.SetItemUsed(true);

                    if (currentB.GetItem().name == "HP potion")
                    {
                        currentB.ModifyHP(currentB.GetItem().damage, true);
                    }
                    else
                    {
                        if (teams[PlayerA.options[PlayerA.value].text].Count != 0)
                        {
                            int randAttack = Random.Range(0, teams[PlayerA.options[PlayerA.value].text].Count - 1);

                            if (teams[PlayerA.options[PlayerA.value].text][randAttack].GetLastAction() == "Defend")
                                teams[PlayerA.options[PlayerA.value].text][randAttack].ModifyHP((int)currentB.GetItem().damage / 2, false);
                            else
                                teams[PlayerA.options[PlayerA.value].text][randAttack].ModifyHP((int)currentB.GetItem().damage, false);
                        }
                    }

                    outputText.text = outputText.text + "\n" + currentB.GetName() + "Uses Item: " + currentB.GetItem().name;

                    break;

                case 3:

                    if (currentB.GetSpell().name != "none")
                    {
                        if (teams[PlayerA.options[PlayerA.value].text].Count != 0 && currentB.GetMana() >= currentB.GetSpell().mana_cost)
                        {
                            int randAttack = Random.Range(0, teams[PlayerA.options[PlayerA.value].text].Count - 1);

                            if (currentB.GetSpell().name != "ConeOfCold")
                            {
                                teams[PlayerA.options[PlayerA.value].text][randAttack].SetFreezed(true);
                            }
                            else
                            {
                                if (teams[PlayerA.options[PlayerA.value].text][randAttack].GetLastAction() == "Defend")
                                    teams[PlayerA.options[PlayerA.value].text][randAttack].ModifyHP((int)currentB.GetSpell().damage / 2, false);
                                else
                                    teams[PlayerA.options[PlayerA.value].text][randAttack].ModifyHP((int)currentB.GetSpell().damage, false);
                            }

                            currentB.ModifyMana((int)currentB.GetSpell().mana_cost);
                        }

                        if (currentB.GetMana() >= currentB.GetSpell().mana_cost)
                            outputText.text = outputText.text + "\n" + currentB.GetName() + "Uses Spell: " + currentB.GetSpell().name;
                        else
                            outputText.text = outputText.text + "\n" + currentB.GetName() + "Cannot use spell: " + currentB.GetSpell().name;

                    }
                    break;
            }
        }
        else if (currentB.GetFreezed())
        {
            outputText.text = outputText.text + "\n" + currentB.GetName() + " Is Freezed!";
            currentB.SetFreezed(false);
        }


    }

    void CheckDead()
    {
        bool new_combat = false;

        if (teams[PlayerA.options[PlayerA.value].text].Count == 0)
        {
            new_combat = true;
            PlayerBWins++;
        }
        else if (teams[PlayerB.options[PlayerB.value].text].Count == 0)
        {
            new_combat = true;
            PlayerAWins++;
        }

        if (new_combat)
        {
            combats++;
            outputText.text = outputText.text + "\n" + "COMBAT END" + combats.ToString();

            ResetTeams();
        }

        // --- Check for dead ---
        for (int i = 0; i < teams[PlayerA.options[PlayerA.value].text].Count; ++i)
        {

            if (new_combat)
                teams[PlayerA.options[PlayerA.value].text][i].Reset();

            else if (teams[PlayerA.options[PlayerA.value].text][i].GetHP() <= 0)
            {
                if (teams[PlayerA.options[PlayerA.value].text][i].GetDead() == false)
                {
                    teams[PlayerA.options[PlayerA.value].text][i].SetDead(true);
                    teams[PlayerA.options[PlayerA.value].text].Remove(teams[PlayerA.options[PlayerA.value].text][i]);
                }

            }

        }

        for (int i = 0; i < teams[PlayerB.options[PlayerB.value].text].Count; ++i)
        {

            if (new_combat)
                teams[PlayerB.options[PlayerB.value].text][i].Reset();

            else if (teams[PlayerB.options[PlayerB.value].text][i].GetHP() <= 0)
            {
                if (teams[PlayerB.options[PlayerB.value].text][i].GetDead() == false)
                {
                    teams[PlayerB.options[PlayerB.value].text][i].SetDead(true);
                    teams[PlayerB.options[PlayerB.value].text].Remove(teams[PlayerB.options[PlayerB.value].text][i]);
                }

            }
        }

    }

    void Attack(CharacterList.Character current)
    {
            current.ModifySTA((int)STACostperatk, false);

            if (current.GetName() == currentA.GetName() && teams[PlayerB.options[PlayerB.value].text].Count != 0)
            {
                int randAttack = Random.Range(0, teams[PlayerB.options[PlayerB.value].text].Count - 1);
                int randDodge = Random.Range(0, (int)maxSPD);

                if (randDodge <= teams[PlayerB.options[PlayerB.value].text][randAttack].GetSpeed())
                    outputText.text = outputText.text + "\n" + currentA.GetName() + "Uses Attack on " + teams[PlayerB.options[PlayerB.value].text][randAttack].GetName() + " that dodges it!!";

                else
                {
                    if (teams[PlayerB.options[PlayerB.value].text][randAttack].GetLastAction() == "Defend")
                        teams[PlayerB.options[PlayerB.value].text][randAttack].ModifyHP((int)current.GetAttack() / 2, false);
                    else
                        teams[PlayerB.options[PlayerB.value].text][randAttack].ModifyHP((int)current.GetAttack(), false);

                    outputText.text = outputText.text + "\n" + currentA.GetName() + "Uses Attack on " + teams[PlayerB.options[PlayerB.value].text][randAttack].GetName();

                }

            }
            else if (teams[PlayerA.options[PlayerA.value].text].Count != 0)
            {
                int randAttack = Random.Range(0, teams[PlayerA.options[PlayerA.value].text].Count - 1);
                int randDodge = Random.Range(0, (int)maxSPD);

                if (randDodge <= teams[PlayerA.options[PlayerA.value].text][randAttack].GetSpeed())
                    outputText.text = outputText.text + "\n" + currentB.GetName() + "Uses Attack on " + teams[PlayerA.options[PlayerA.value].text][randAttack].GetName() + " that dodges it!!";

                else
                {
                    if (teams[PlayerA.options[PlayerA.value].text][randAttack].GetLastAction() == "Defend")
                        teams[PlayerA.options[PlayerA.value].text][randAttack].ModifyHP((int)current.GetAttack() / 2, false);
                    else
                        teams[PlayerA.options[PlayerA.value].text][randAttack].ModifyHP((int)current.GetAttack(), false);

                    outputText.text = outputText.text + "\n" + currentB.GetName() + "Uses Attack on " + teams[PlayerA.options[PlayerA.value].text][randAttack].GetName();
                }
            }

    }


}
