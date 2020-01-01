using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    // --- Settings ---
    public Dropdown PlayerA;
    public Dropdown PlayerB;

    // --- Game ---
    public Dropdown ActionA;
    public Dropdown ActionB;

    // --- Characters ---
    CharacterList character_manager;

    Dictionary<string, List<CharacterList.Character>> teams;

    // Start is called before the first frame update
    void Start()
    {
        // --- Initialize variables ---
        character_manager = new CharacterList();
        teams = new Dictionary<string, List<CharacterList.Character>>();
        List<string> options = new List<string>();
        PlayerA.ClearOptions();
        PlayerB.ClearOptions();
        ActionA.ClearOptions();
        ActionB.ClearOptions();


        // --- Create teams ---
        teams.Add("Allies",null);
        teams.Add("Enemies",null);

        options.Add("Allies");
        options.Add("Enemies");

        // --- Fill teams ---
        character_manager.CreateTeam(teams["Allies"], CharacterList.Teams.A);
        character_manager.CreateTeam(teams["Enemies"], CharacterList.Teams.B);
   
        // --- Fill dropdowns ---
        PlayerA.AddOptions(options);
        PlayerB.AddOptions(options);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play()
    {
        Debug.Log("Starting simulation");
    }

    public void Step()
    {
        Debug.Log("Stepping simulation");
    }
}
