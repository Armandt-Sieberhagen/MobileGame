using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Quests", menuName = "Quests")]
public class Quests : ScriptableObject
{
    public int CharacterLevel;
    public float XP;
    public int currentAmountofEnemiesKilled;
    public int currentAmountofBossesKilled;
    public int HighestBossLevel;
    public int CurrentAmountOfTilesConquered;
    public string QuestOne;
    public string QuestTwo;
    public string QuestThree;
    public string QuestOneHashInfo;
    public string QuestTwoHashInfo;
    public string QuestThreeHashInfo;

    public void CheckIfQuestIsComplete(string Quest)
    {
        string TypeOfQuest = Quest.Substring(0,1);
        if (TypeOfQuest == "#")
        {
            if (CurrentAmountOfTilesConquered >= int.Parse(Quest.Substring(1, Quest.Length)) || (HighestBossLevel >= int.Parse(Quest.Substring(1, Quest.Length))))
            {

            }
        }
        if (TypeOfQuest == "*")
        {
            if (currentAmountofBossesKilled >= int.Parse(Quest.Substring(1, Quest.Length)))
            {

            }
        }
        if (TypeOfQuest == "%")
        {
            if (currentAmountofEnemiesKilled >= int.Parse(Quest.Substring(1, Quest.Length)))
            {

            }
        }
    }

    void GenerateNewQuestOne()
    {

    }
    void GenerateNewQuestTwo()
    {

    }
    void GenerateNewQuestThree()
    {

    }
    public string GenerateQuest()
    {
        string QuestSentance;
        int QuestChooser = Random.Range(1, 3);
        switch (QuestChooser)
        {
            case 1:
                QuestSentance = GenerateEnemyKillQuest();
                break;
            case 2:
                QuestSentance = GenerateBossKillQuest();
                break;
            case 3:
                QuestSentance = GenerateHexConquestQuest();
                break;
            default:
                QuestSentance = GenerateQuest();
                break;
        }
        return QuestSentance;


    }
    public string GenerateEnemyKillQuest()
    {
        return "todo";
    }
    public string GenerateBossKillQuest()
    {
        return "todo";
    }
    public string GenerateHexConquestQuest()
    {
        return "todo";
    }
    void AddXP(float GenerateXP)
    {
        XP += GenerateXP;
        if (CharacterLevel*1500f>=XP)
        {
            XP = 0;
            CharacterLevel++;
        }
    }
    //types of quests: kill certain number of bosses or normal enemies, or conquer x amount of tiles
    public class Quest
    {

    }

}
