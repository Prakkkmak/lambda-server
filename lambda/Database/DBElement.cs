using System;
using System.Collections.Generic;
using System.Text;
using AltV.Net;
using Lambda.Entity;

namespace Lambda.Database
{
    public abstract class DbElement<T> where T : IDBElement, new()
    {
        public string TableName { get; }
        public string Prefix { get; }
        public DBConnect DbConnect { get; }

        public Game Game;

        protected DbElement(Game game, DBConnect dbConnect, string tableName, string prefix)
        {
            this.TableName = tableName;
            this.DbConnect = dbConnect;
            this.Prefix = prefix;

            this.Game = game;
        }

        abstract public Dictionary<string, string> GetData(T entity);
        abstract public void SetData(T entity, Dictionary<string, string> data);

        public void Save(T entity)
        {
            Dictionary<string, string> datas = GetData(entity);
            if (entity.Id == 0)
            {
                entity.Id = (uint)DbConnect.Insert(TableName, datas);
            }
            else
            {
                Dictionary<string, string> where = new Dictionary<string, string>();
                where[Prefix + "_id"] = entity.Id.ToString();
                DbConnect.Update(TableName, datas, where);
            }
        }


        public T Get(uint id)
        {
            Dictionary<string, string> where = new Dictionary<string, string>();
            string index = Prefix + "_id";
            where[index] = id.ToString();
            Dictionary<string, string> result = DbConnect.SelectOne(TableName, where);
            if (result.Count == 0) return default(T);
            T entity = new T();
            SetData(entity, result);
            return entity;
        }
        public T Get(uint id, T entity)
        {
            Dictionary<string, string> where = new Dictionary<string, string>();
            string index = Prefix + "_id";
            where[index] = id.ToString();
            Dictionary<string, string> result = DbConnect.SelectOne(TableName, where);
            SetData(entity, result);
            return entity;
        }

        public void Update(T entity)
        {
            Dictionary<string, string> datas = GetData(entity);
            Dictionary<string, string> where = new Dictionary<string, string>();
            string index = Prefix + "_id";
            where[index] = entity.Id.ToString();
            DbConnect.Update(TableName, datas, where);
        }

        public void Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public void SaveAll(T[] entities)
        {
            throw new NotImplementedException();
        }

        public T[] GetAll()
        {
            List<T> entities = new List<T>();
            List<Dictionary<string, string>> results = DbConnect.Select(TableName, new Dictionary<string, string>());
            foreach (Dictionary<string, string> result in results)
            {
                T entity = new T();
                SetData(entity, result);
                entities.Add(entity);
            }

            return entities.ToArray();
        }

        public void UpdateAll(T[] entity)
        {
            throw new NotImplementedException();
        }

        public string ToString()
        {
            return $"Table : {TableName} / Prefix : {Prefix}";
        }
    }
}
