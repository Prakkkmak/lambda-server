using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Lambda.Entity;

namespace Lambda.Database
{
    public interface IDBElement
    {
        uint Id { get; set; }
        Dictionary<string, string> GetData();
        void SetData(Dictionary<string, string> data);

        void Save();
        void Delete();
        Task SaveAsync();
        Task DeleteAsync();
        //void Load(Dictionary<string, string> data);

    }
}
