using Lambda.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lambda.Utils
{
    public class Request
    {
        public Player Player;
        public string Name { get; set; }
        public string Text { get; set; }
        public Player Sender { get; set; }
        public List<Answer> Answers { get; }
        public Func<bool> Condition;
        public Request(Player player, string name, string text, Player sender)
        {
            Player = player;
            Name = name;
            Text = text;
            Sender = sender;
            Answers = new List<Answer>();
        }

        public void AddAnswer(string text, Action action)
        {
            Answer answer = new Answer(this, text, action);
            Answers.Add(answer);

        }
    }
}
