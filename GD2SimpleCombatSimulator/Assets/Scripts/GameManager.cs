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
    uint max_charactersA = 3;
    uint max_charactersB = 4;
    int currentcharA = 0;
    int currentcharB = 0;

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

    //public UnityEngine.UIElements.ScrollView output;
    public Text outputText;

    public Button Playbutton;
    public Button stepbutton;

    bool playing = false;

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

        options.Add("Allies");
        options.Add("Enemies");

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
   
        // --- Fill dropdowns ---
        PlayerA.AddOptions(options);
        PlayerB.AddOptions(options);

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

        currentA = teams[PlayerA.options[PlayerA.value].text][currentcharA];
        currentB = teams[PlayerB.options[PlayerB.value].text][currentcharB];
        CurrentCharacterA.text = currentA.name;
        CurrentCharacterB.text = currentB.name;

    }

    public void SetIterations(string its)
    {
        uint result;
        uint.TryParse(its, out result);

        if (result > 0)
            iterations = result;
    }

    public void Play()
    {
        playing = true;

        if (iterations > 0)
        {
            Debug.Log("Starting simulation");

            for (uint i = 0; i < iterations; ++i) 
            {
                Debug.Log("Iteration number: ");
                Debug.Log(iterations);

                SimulateCombat();

                UpdateCurrentCharacters();
            }

            iterations = 0;
        }

        playing = false;
        outputText.text = "Simulation End";
        iters.text = "0";
    }

    public void Step()
    {
        playing = true;

        if (iterations > 0)
        {
            Debug.Log("Stepping simulation");
            Debug.Log("Iteration number: ");
            Debug.Log(iterations);

            SimulateCombat();
            UpdateCurrentCharacters();

            iterations -= 1;
        }
        else
        {
            playing = false;
            outputText.text = "Simulation End";
            iters.text = "0";
        }
    }

    void UpdateCurrentCharacters()
    {
        currentcharA++;
        currentcharB++;

        if (currentcharA > max_charactersA - 1)
            currentcharA = 0;

        if (currentcharB > max_charactersB - 1)
            currentcharB = 0;

        currentA = teams[PlayerA.options[PlayerA.value].text][currentcharA];
        currentB = teams[PlayerB.options[PlayerB.value].text][currentcharB];

        CurrentCharacterA.text = currentA.name;
        CurrentCharacterB.text = currentB.name;

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

        if (currentA.is_mage)
            actionsA.Add("Cast Spell");

        if (currentB.is_mage)
            actionsB.Add("Cast Spell");

        ActionA.AddOptions(actionsA);
        ActionB.AddOptions(actionsB);
    }

    void SimulateCombat()
    {
        int randA = Random.Range(0, ActionA.options.Count-1);
        int randB = Random.Range(0, ActionB.options.Count-1);

        currentA.last_action = "none";
        currentB.last_action = "none";


        if (currentA.HP > 0 && !currentA.is_freezed)
        {
            switch (randA)
            {
                case 0:
                    Attack(currentA);
                    break;

                case 1:
                    currentA.last_action = "Defend";
                    outputText.text = outputText.text + "\n" + currentA.name + "Uses Defend";
                    break;

                case 2:
                    outputText.text = outputText.text + "\n" + currentA.name + "Uses Item";
                    break;

                case 3:
                    break;
            }

        }

        if (currentB.HP > 0 && !currentB.is_freezed)
        {
            switch (randB)
            {
                case 0:
                    Attack(currentB);
                    break;

                case 1:
                    currentA.last_action = "Defend";
                    outputText.text = outputText.text + "\n" + currentB.name + "Uses Defend";
                    break;

                case 2:

                    break;

                case 3:

                    break;
            }
        }

    }

    void Attack(CharacterList.Character current)
    {
        if (current.name == currentA.name)
        {
            int randAttack = Random.Range(0, PlayerB.options.Count - 1);

            if (teams[PlayerB.options[PlayerB.value].text][randAttack].last_action == "Defend")
                teams[PlayerB.options[PlayerB.value].text][randAttack].ModifyHP(current.attack / 2, false);
            else
                teams[PlayerB.options[PlayerB.value].text][randAttack].ModifyHP(current.attack, false);

            outputText.text = outputText.text + "\n" + currentA.name + "Uses Attack" + teams[PlayerB.options[PlayerB.value].text][randAttack].name;

        }
        else
        {
            int randAttack = Random.Range(0, PlayerA.options.Count - 1);

            if (teams[PlayerA.options[PlayerA.value].text][randAttack].last_action == "Defend")
                teams[PlayerA.options[PlayerA.value].text][randAttack].ModifyHP(current.attack / 2, false);
            else
                teams[PlayerA.options[PlayerA.value].text][randAttack].ModifyHP(current.attack, false);

            outputText.text = outputText.text + "\n" + currentB.name + "Uses Attack" + teams[PlayerA.options[PlayerA.value].text][randAttack].name;

        }

    }

    void PrintStatistics()
    {

    }
}
