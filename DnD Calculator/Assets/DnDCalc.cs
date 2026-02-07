using UnityEngine;
using System.Collections.Generic;

public class SolutionOne : MonoBehaviour
{
    // ============================
    // INSPECTOR INPUTS
    // ============================
    public string characterName;
    public int level;
    public int conScore;
    public string race;

    public bool hasToughFeat;
    public bool hasStoutFeat;

    public enum HpMode { Averaged, Rolled }
    public HpMode hpMode;
    public string summaryText;
    // ============================
    // OUTPUT
    // ============================
    public int calculatedHP;

    // ============================
    // DATA STRUCTURES
    // ============================
    Dictionary<string, int> raceBonusPerLevel;
    List<string> validRaces;

    void Awake()
    {
        // Race → HP bonus per level
        raceBonusPerLevel = new Dictionary<string, int>()
        {
            { "Dwarf", 2 },
            { "Orc", 1 },
            { "Goliath", 1 }
        };

        // Valid races list (data management)
        validRaces = new List<string>()
        {
            "Aasimar",
            "Dragonborn",
            "Dwarf",
            "Elf",
            "Gnome",
            "Goliath",
            "Halfling",
            "Human",
            "Orc",
            "Tiefling"
        };
    }

    void Start()
    {
        CalculateHP();
        Debug.Log(summaryText);
    }

    void CalculateHP()
    {
        // Clamp level to D&D limits
        int safeLevel = Mathf.Clamp(level, 1, 20);

        // Constitution modifier
        int conModifier = Mathf.FloorToInt((conScore - 10) / 2f);

        // Base hit die (generic for SolutionOne)
        int hitDieSides = 10; // arbitrary but consistent

        // Race bonus per level
        int raceBonus = 0;
        if (raceBonusPerLevel.ContainsKey(race))
        {
            raceBonus = raceBonusPerLevel[race];
        }

        // Feat bonus per level
        int featBonus = 0;
        if (hasToughFeat) featBonus += 2;
        if (hasStoutFeat) featBonus += 1;

        // HP calculation
        int totalHP = 0;

        // Level 1 HP
        totalHP += hitDieSides + conModifier;

        // Levels 2+
        for (int i = 2; i <= safeLevel; i++)
        {
            int dieRoll;

            if (hpMode == HpMode.Averaged)
            {
                dieRoll = Mathf.RoundToInt((hitDieSides + 1) / 2f);
            }
            else
            {
                dieRoll = Random.Range(1, hitDieSides + 1);
            }

            totalHP += dieRoll + conModifier;
        }

        // Add race + feat bonuses
        totalHP += (raceBonus + featBonus) * safeLevel;

        calculatedHP = totalHP;

        summaryText =
            "My character " + characterName + " is a level " + safeLevel +
            " with a CON score of " + conScore + " and is a " + race + " and " +
            (hasToughFeat && hasStoutFeat ? "has Tough and Stout feat." :
            hasToughFeat ? "has Tough feat." : hasStoutFeat ? "has Stout feat." :
            "has no Tough or Stout feat.")  + " I want the HP " +
            (hpMode == HpMode.Averaged ? "averaged." : "rolled") + " with a total of " + calculatedHP + " HP";
    }
}
