using AltV.Net.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lambda.Quests
{
    public class FarmingQuests
    {
        public static Quest FarmingTomato1()
        {
            Position pos = new Position(1996.226f, 4926.224f, 41.5f);
            Quest quest = new Quest("farming_tomato_01",pos, "Allez à la ~y~ferme de tomattes~w~.");
            quest.Complete = (player) =>
            {
                Quest.StartQuest(player, "farming_tomato_02");
            };
            return quest;
        }
        public static Quest FarmingTomato2()
        {
            Position pos = new Position(0, 0, 0);
            Quest quest = new Quest("farming_tomato_02", default, "Ramassez ~g~10 tomattes~w~");
            quest.Condition = (player) =>
            {
                return player.Inventory.GetItem(Enums.Items.Tomato).Amount >= 10;
            };
            quest.Complete = (player) =>
            {
                Quest.StartQuest(player, "farming_tomato_03");
            };
            return quest;
        }
        public static Quest FarmingTomato3()
        {
            Position pos = new Position(1930, 4635, 38);
            Quest quest = new Quest("farming_tomato_03", pos, "Retournez les tomates au ~y~ranch~w~.");
            
            quest.Complete = (player) =>
            {
                if (player.Inventory.GetItem(Enums.Items.Tomato).Amount < 10)
                {
                    player.SendMessage("Vous n'avez pas assez de tomates");
                    Quest.StartQuest(player, "farming_tomato_02");
                }
                    
                player.AddExperience(Skills.Skill.SkillType.FARMING, 100);
                player.Inventory.AddMoney(50);
                player.Inventory.RemoveItem(Enums.Items.Tomato, 10);
            };
            return quest;
        }
        public static List<Quest> GenerateFarmingQuests()
        {
            List<Quest> quests = new List<Quest>();
            quests.Add(FarmingTomato1());
            quests.Add(FarmingTomato2());
            quests.Add(FarmingTomato3());
            return quests;
        }


    }
}
