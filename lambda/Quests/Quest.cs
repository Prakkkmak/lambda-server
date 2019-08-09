using AltV.Net;
using AltV.Net.Data;
using AltV.Net.Enums;
using Lambda.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lambda.Quests
{
    public class Quest
    {
        public string Name = "quest_default";

        public string Text = "Test quest";

        public Position Position = default;

        public Func<Player,bool> Condition = (player) => { return true; };

        public Action<Player> Complete = (player) => { };
        

        public Quest(string name, Position pos, string text)
        {
            Name = name;
            Position = pos;
            Text = text;
        }
        
        public void Send(Player player)
        {
            Alt.Log("Envoie d'une quete : " + Text);
            player.Emit("startQuest", Position, Text);
        }

        public static void StartQuest(Player player, string questName)
        {
            foreach(Quest quest in Quests)
            {
                if (quest.Name.Equals(questName))
                {
                    quest.Send(player);
                    player.Quest = quest;
                    return;
                }
            }
           
        }

        public static void GenerateQuests()
        {
            Quests.AddRange(FarmingQuests.GenerateFarmingQuests());
        }

        public static List<Quest> Quests = new List<Quest>();
        //public static Quest()

    }
}
