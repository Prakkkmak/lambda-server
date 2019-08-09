using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using AltV.Net;
using AltV.Net.Data;
using AltV.Net.Enums;
using Items;
using Lambda.Administration;
using Lambda.Buildings;
using Lambda.Clothing;
using Lambda.Entity;
using Lambda.Items;
using Lambda.Organizations;
using Lambda.Skills;
using Lambda.Telephony;
using Lambda.Utils;

namespace Lambda.Database
{
    public static class DatabaseElement
    {
        public static DBConnect DbConnect = new DBConnect();

        public static Dictionary<Type, string> TableNames = new Dictionary<Type, string>()
        {
            {typeof(Player), "t_character_cha" },
            {typeof(Vehicle), "t_vehicle_veh" },
            {typeof(BaseItem), "t_itemdata_itd" },
            {typeof(Account), "t_account_acc" },
            {typeof(Skin), "t_skin_ski" },
            {typeof(Organization), "t_organization_org" },
            {typeof(Rank), "t_rank_ran" },
            {typeof(Link), "t_link_lin" },
            {typeof(Inventory), "t_inventory_inv" },
            {typeof(Item), "t_item_ite" },
            {typeof(Skill), "t_skill_skl" },
            {typeof(Member), "t_member_mem" },
            {typeof(Phone), "t_phone_pho" },
            {typeof(Message), "t_phonemessage_pme" },
            {typeof(Contact), "t_phonecontact_pco" },
            {typeof(Building), "t_building_bui" },
            {typeof(House), "t_house_hou" },
            {typeof(Whitelist), "t_whitelist_whi" }
        };

        public static string GetPrefix(string str)
        {
            return str.Split("_")[2];
        }

        public static string GetTableName(Type type)
        {
            if (!TableNames.ContainsKey(type)) throw new Exception("Table pas trouvé");
            return TableNames[type];
        }

        public static void Save<T>(T entity) where T : IDBElement
        {
            string tableName = GetTableName(typeof(T));
            Dictionary<string, string> datas = entity.GetData();
            if (entity.Id == 0)
            {
                entity.Id = (uint)DbConnect.Insert(tableName, datas);
            }
            else
            {
                Dictionary<string, string> where = new Dictionary<string, string>();
                where[GetPrefix(tableName) + "_id"] = entity.Id.ToString();
                int rows = DbConnect.Update(tableName, datas, where);
                if (rows != 0) return;
                entity.Id = 0;
                Save(entity);
            }
        }
        public static async Task SaveAsync<T>(T entity) where T : IDBElement
        {
            long t = DateTime.Now.Ticks;
            Alt.Log("Start save " + typeof(T).Name);
            string tableName = GetTableName(typeof(T));
            Dictionary<string, string> datas = entity.GetData();


            try
            {
                if (entity.Id == 0)
                {
                    Alt.Log("Entity " + typeof(T).Name + " have id 0; saving new instance");
                    if (typeof(Building).IsAssignableFrom(typeof(T)))
                    {
                        Dictionary<string, string> buildingData = new Dictionary<string, string>();
                        Dictionary<string, string> otherData = new Dictionary<string, string>();
                        foreach(KeyValuePair<string, string> data in datas)
                        {
                            if (data.Key.StartsWith("bui_"))
                            {
                                buildingData.Add(data.Key, data.Value);
                            }
                            else
                            {
                                otherData.Add(data.Key, data.Value);
                            }
                        }
                        entity.Id = (uint)await DbConnect.InsertAsync("t_building_bui", buildingData);
                        otherData.Add("bui_id", entity.Id.ToString());
                        datas = otherData;
                        await DbConnect.InsertAsync(tableName, datas);
                    }
                    else
                    {
                        entity.Id = (uint)await DbConnect.InsertAsync(tableName, datas);
                        Alt.Log("New instance have id " + entity.Id);
                    }
                    
                    
                }
                else
                {
                    Alt.Log("Entity " + typeof(T).Name + " have id " + entity.Id + "; update instance");
                    Dictionary<string, string> where = new Dictionary<string, string>();
                    where[GetPrefix(tableName) + "_id"] = entity.Id.ToString();
                    if (typeof(Building).IsAssignableFrom(typeof(T)))
                    {
                        where = new Dictionary<string, string>();
                        where["bui_id"] = entity.Id.ToString();
                        Dictionary<string, string> buildingData = new Dictionary<string, string>();
                        Dictionary<string, string> otherData = new Dictionary<string, string>();
                        foreach (KeyValuePair<string, string> data in datas)
                        {
                            if (data.Key.StartsWith("bui_"))
                            {
                                buildingData.Add(data.Key, data.Value);
                            }
                            else
                            {
                                otherData.Add(data.Key, data.Value);
                            }
                        }
                        await DbConnect.UpdateAsync("t_building_bui", buildingData, where);
                        otherData.Add("bui_id", entity.Id.ToString());
                        datas = otherData;
                    }
                    int rows = (int)await DbConnect.UpdateAsync(tableName, datas, where);
                    if (rows == 0)
                    {
                        entity.Id = 0;
                        Save(entity);
                    }
                }
            }
            catch(Exception ex)
            {
                Alt.Log(ex.Message);
            }
            Alt.Log(typeof(T).Name + " Saved en " + (t / TimeSpan.TicksPerMillisecond) + " ms ");
        }
        public static Dictionary<string, string> Get<T>(uint id) where T : IDBElement
        {
            string tableName = GetTableName(typeof(T));
            Dictionary<string, string> where = new Dictionary<string, string>();
            string index = GetPrefix(tableName) + "_id";
            where[index] = id.ToString();
            Dictionary<string, string> result = DbConnect.SelectOne(tableName, where);
            return result;
        }
        public static T Get<T>(T entity, uint id) where T : IDBElement
        {
            string tableName = GetTableName(typeof(T));
            Dictionary<string, string> where = new Dictionary<string, string>();
            string index = GetPrefix(tableName) + "_id";
            where[index] = id.ToString();
            Dictionary<string, string> result = DbConnect.SelectOne(tableName, where);
            if (result.Count < 1) return entity;
            entity.SetData(result);
            return entity;
        }
        public static T Get<T>(T entity, string where1, string param1) where T : IDBElement
        {
            string tableName = GetTableName(typeof(T));
            Dictionary<string, string> where = new Dictionary<string, string>();
            where[where1] = param1;
            Dictionary<string, string> result = DbConnect.SelectOne(tableName, where);
            if (result.Count < 1) return entity;
            entity.SetData(result);
            return entity;
        }

        public static void Update<T>(T entity) where T : IDBElement
        {
            string tableName = GetTableName(typeof(T));
            Dictionary<string, string> datas = entity.GetData();
            Dictionary<string, string> where = new Dictionary<string, string>();
            string index = GetPrefix(tableName) + "_id";
            where[index] = entity.Id.ToString();
            DbConnect.Update(tableName, datas, where);
        }

        public static void Delete<T>(T entity) where T : IDBElement
        {
            string tableName = GetTableName(typeof(T));
            Dictionary<string, string> where = new Dictionary<string, string>();
            string index = GetPrefix(tableName) + "_id";
            where[index] = entity.Id.ToString();
            DbConnect.Delete(tableName, where);
        }
        public static async Task DeleteAsync<T>(T entity) where T : IDBElement
        {
            string tableName = GetTableName(typeof(T));
            Dictionary<string, string> where = new Dictionary<string, string>();
            string index = GetPrefix(tableName) + "_id";
            where[index] = entity.Id.ToString();
            await DbConnect.DeleteAsync(tableName, where);
        }

        public static BaseItem[] GetAllBaseItems()
        {
            string tableName = GetTableName(typeof(BaseItem));
            List<BaseItem> entities = new List<BaseItem>();
            List<Dictionary<string, string>> results = DbConnect.Select(tableName, new Dictionary<string, string>());
            foreach (Dictionary<string, string> result in results)
            {
                BaseItem entity = new BaseItem();
                entity.SetData(result);
                entity.Id = uint.Parse(result[GetPrefix(tableName) + "_id"]);
                entities.Add(entity);
            }

            return entities.ToArray();
        }

        public static string[] GetAllWhitelisted(Whitelist whitelist)
        {
            string tableName = GetTableName(typeof(Whitelist));
            List<string> entities = new List<string>();
            List<Dictionary<string, string>> results = DbConnect.Select(tableName, new Dictionary<string, string>());
            foreach (Dictionary<string, string> result in results)
            {
                entities.Add(result["whi_discordid"]);
            }

            return entities.ToArray();
        }

        public static Vehicle[] GetAllVehicles()
        {
            string tableName = GetTableName(typeof(Vehicle));
            List<Vehicle> entities = new List<Vehicle>();
            List<Dictionary<string, string>> results = DbConnect.Select(tableName, new Dictionary<string, string>());
            foreach (Dictionary<string, string> result in results)
            {
                VehicleModel model = (VehicleModel)Enum.Parse(typeof(VehicleModel), result["veh_model"]);
                Position position = new Position();
                position.X = float.Parse(result["veh_position_x"]);
                position.Y = float.Parse(result["veh_position_y"]);
                position.Z = float.Parse(result["veh_position_z"]);
                Rotation rotation = new Rotation();
                rotation.Roll = float.Parse(result["veh_rotation_r"]);
                rotation.Pitch = float.Parse(result["veh_rotation_p"]);
                rotation.Yaw = float.Parse(result["veh_rotation_y"]);
                Vehicle entity = (Vehicle)Alt.CreateVehicle(model, position, rotation);
                entity.SetData(result);
                entity.Id = uint.Parse(result[GetPrefix(tableName) + "_id"]);
                entities.Add(entity);
            }

            return entities.ToArray();
        }
        public static Phone[] GetAllPhones()
        {
            string tableName = GetTableName(typeof(Phone));
            List<Phone> entities = new List<Phone>();
            List<Dictionary<string, string>> results = DbConnect.Select(tableName, new Dictionary<string, string>());
            foreach (Dictionary<string, string> result in results)
            {
                Phone phone = new Phone();
                phone.SetData(result);
                phone.Id = uint.Parse(result[GetPrefix(tableName) + "_id"]);
                entities.Add(phone);
            }

            return entities.ToArray();
        }

        public static Message[] GetAllMessages(Phone phone)
        {
            string tableName = GetTableName(typeof(Message));
            List<Message> messages = new List<Message>();
            Dictionary<string, string> where = new Dictionary<string, string>();
            string index = "pho_id";
            where[index] = phone.Id.ToString();
            List<Dictionary<string, string>> results = DbConnect.Select(tableName, where);
            foreach (Dictionary<string, string> result in results)
            {
                Message message = new Message(0, phone, "", "", "");
                message.SetData(result);
                message.Id = uint.Parse(result[GetPrefix(tableName) + "_id"]);
                messages.Add(message);
            }

            return messages.ToArray();
        }
        public static Contact[] GetAllContacts(Phone phone)
        {
            string tableName = GetTableName(typeof(Contact));
            List<Contact> contacts = new List<Contact>();
            Dictionary<string, string> where = new Dictionary<string, string>();
            string index = "pho_id";
            where[index] = phone.Id.ToString();
            List<Dictionary<string, string>> results = DbConnect.Select(tableName, where);
            foreach (Dictionary<string, string> result in results)
            {
                Contact contact = new Contact(0, phone, "", "");
                contact.SetData(result);
                contact.Id = uint.Parse(result[GetPrefix(tableName) + "_id"]);
                contacts.Add(contact);
            }

            return contacts.ToArray();
        }

        public static Item[] GetAllItems(Inventory inventory)
        {
            string tableName = GetTableName(typeof(Item));
            List<Item> items = new List<Item>();
            Dictionary<string, string> where = new Dictionary<string, string>();
            string index = "inv_id";
            where[index] = inventory.Id.ToString();
            List<Dictionary<string, string>> results = DbConnect.Select(tableName, where);
            foreach (Dictionary<string, string> result in results)
            {
                Item item = new Item();
                item.SetData(result);
                item.Id = uint.Parse(result[GetPrefix(tableName) + "_id"]);
                items.Add(item);
                item.SetInventory(inventory);
            }

            return items.ToArray();
        }
        public static Member[] GetAllMembers(Rank rank)
        {
            string tableName = GetTableName(typeof(Member));
            List<Member> members = new List<Member>();
            Dictionary<string, string> where = new Dictionary<string, string>();
            string index = "ran_id";
            where[index] = rank.Id.ToString();
            List<Dictionary<string, string>> results = DbConnect.Select(tableName, where);
            foreach (Dictionary<string, string> result in results)
            {
                Member member = new Member();
                member.SetData(result);
                member.Id = uint.Parse(result[GetPrefix(tableName) + "_id"]);
                member.Rank = rank;
                Dictionary<string, string> playerdata = Get<Player>(member.PlayerId);
                if (playerdata.ContainsKey("cha_firstname") && playerdata.ContainsKey("cha_lastname"))
                    member.Name = playerdata["cha_firstname"] + " " + playerdata["cha_lastname"];
                else
                    member.Name = "Inconnu";
                members.Add(member);
            }

            return members.ToArray();
        }
        public static Organization[] GetAllOrganizations()
        {
            string tableName = GetTableName(typeof(Organization));
            List<Organization> organizations = new List<Organization>();
            List<Dictionary<string, string>> results = DbConnect.Select(tableName, new Dictionary<string, string>());
            foreach (Dictionary<string, string> result in results)
            {
                Organization organization = new Organization("babar");
                organization.SetData(result);
                organization.Id = uint.Parse(result[GetPrefix(tableName) + "_id"]);
                organizations.Add(organization);
            }

            return organizations.ToArray();
        }
        public static Rank[] GetAllRanks(Organization organization)
        {
            string tableName = GetTableName(typeof(Rank));
            List<Rank> ranks = new List<Rank>();
            Dictionary<string, string> where = new Dictionary<string, string>();
            where["org_id"] = organization.Id.ToString();
            List<Dictionary<string, string>> results = DbConnect.Select(tableName, where);
            foreach (Dictionary<string, string> result in results)
            {
                Rank rank = new Rank();
                rank.SetData(result);
                rank.Id = uint.Parse(result[GetPrefix(tableName) + "_id"]);
                ranks.Add(rank);
                //rank.Organization = organization;
            }

            return ranks.ToArray();
        }
        public static Skill[] GetAllSkills(Player player)
        {
            string tableName = GetTableName(typeof(Organization));
            List<Skill> items = new List<Skill>();
            Dictionary<string, string> where = new Dictionary<string, string>();
            string index = "cha_id";
            where[index] = player.Id.ToString();
            List<Dictionary<string, string>> results = DbConnect.Select(tableName, where);
            foreach (Dictionary<string, string> result in results)
            {
                Skill item = new Skill();
                item.SetData(result);
                item.Id = uint.Parse(result[GetPrefix(tableName) + "_id"]);
                item.Player = player;
                items.Add(item);
            }

            return items.ToArray();
        }
        public static House[] GetAllHouses()
        {
            string tableName = GetTableName(typeof(House));
            List<House> houses = new List<House>();
            
            List<Dictionary<string, string>> results = DbConnect.Select(tableName, new Dictionary<string, string>());
            
            foreach (Dictionary<string, string> result in results)
            {

                Dictionary<string, string> where = new Dictionary<string, string>();
                where["bui_id"] = result["bui_id"];
                Dictionary<string, string> buildingResult = DbConnect.Select("t_building_bui", where)[0];
                foreach (var r in buildingResult)
                {
                    result[r.Key] = r.Value;
                }
                Position position = new Position();
                position = PositionHelper.PositionFromString(result["bui_position"]);
                House house = new House(position);
                house.SetData(result);
                house.Id = uint.Parse(result["bui_id"]);
                houses.Add(house);
            }

            return houses.ToArray();
        }

        /*
        public void SaveAll(T[] entities)
        {
            throw new NotImplementedException();
        }



        public void UpdateAll(T[] entity)
        {
            throw new NotImplementedException();
        }

        public string ToString()
        {
            return $"Table : {TableName} / Prefix : {Prefix}";
        }

        public static Position GetPosition(Dictionary<string, string> data)
        {
            Position position = new Position(0, 0, 0);
            position.X = float.Parse(data[Prefix + "_position_x"]);
            position.Y = float.Parse(data[Prefix + "_position_y"]);
            position.Z = float.Parse(data[Prefix + "_position_z"]);
            return position;
        }*/

    }
}
