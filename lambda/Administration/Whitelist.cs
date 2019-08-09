using Lambda.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lambda.Administration
{
    public class Whitelist
    {
        public List<string> Whitelisted = new List<string>();
        public async void Add(string id)
        {
            Whitelisted.Add(id);
            Dictionary<string, string> data = new Dictionary<string, string>();
            data["whi_discordid"] = id;
            await DatabaseElement.DbConnect.InsertAsync("t_whitelist_whi", data);
            Console.WriteLine(id + " a été ajouté à la whitelist");
        }
        public async void Remove(string id)
        {
            Whitelisted.Remove(id);
            Dictionary<string, string> data = new Dictionary<string, string>();
            data["whi_discordid"] = id;
            Dictionary<string, string> wheres = data;
            await DatabaseElement.DbConnect.DeleteAsync("t_whitelist_whi", wheres);
            Console.WriteLine(id + " a été supprimé de la whitelist");
        }
        public bool Contains(string id)
        {
            return Whitelisted.Contains(id);
        }

        public void GetWhitelist()
        {
            Whitelisted.AddRange(DatabaseElement.GetAllWhitelisted(this));
        }

        public static Whitelist MainWhitelist = new Whitelist();
    }
}
