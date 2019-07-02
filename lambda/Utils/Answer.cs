using Lambda.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lambda.Utils
{
    public class Answer
    {
        public Request Request { get; }
        public string Text { get; set; }
        public Action Action;

        public Answer(Request request, string text, Action action)
        {
            Text = text;
            Action = action;
            Request = request;
        }
    }
}
