using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using AltV.Net;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using Items;
using Lambda.Commands;
using Lambda.Database;
using Lambda.Entity;
using Lambda.Items;
using Lambda.Organizations;
using Lambda.Utils;
using MoreLinq;
using Player = Lambda.Entity.Player;
using Vehicle = Lambda.Entity.Vehicle;

namespace Lambda
{
    public class Game
    {
        private List<Player> players;
        private List<Vehicle> vehicles;
        private List<Area> areas;
        private List<Spawn> spawns;
        private Command[] commands;
        //private List<ComponentLink> componentLinks;
        private List<Organization> organizations;
        public List<Link> links;

        private Interior[] interiors;
        private Skin[] skins;
        private BaseItem[] baseitems;
        private DBConnect dbConnect;
        private Events events;
        public Chat Chat;
        public IVoiceChannel VoiceChannel;
        //public Links Links { get; set; }



        public DbVehicle DbVehicle { get; }
        public DbBaseItem DbBaseItem { get; }
        public DbArea DbArea { get; }
        public DbPlayer DbPlayer { get; }
        public DbAccount DbAccount { get; }
        public DbSkin DbSkin { get; }
        public DbOrganization DbOrganization { get; }
        public DbRank DbRank { get; }
        public DbLink DbLink { get; }
        //public DbLink DbComponentLink { get; }
        public DbInterior DbInterior { get; }
        public DbInventory DbInventory { get; }
        public DbItem DbItem { get; }
        public DbSkill DbSkill { get; }


        public Game()
        {
            players = new List<Player>();
            vehicles = new List<Vehicle>();
            areas = new List<Area>();
            spawns = new List<Spawn>();
            commands = new Command[0];
            //componentLinks = new List<ComponentLink>();
            organizations = new List<Organization>();
            links = new List<Link>();
            baseitems = new BaseItem[0];
            dbConnect = new DBConnect();
            events = new Events(this);
            skins = new Skin[0];
            interiors = new Interior[0];
            VoiceChannel = Alt.CreateVoiceChannel(true, 10);

            Chat = new Chat();

            DbVehicle = new DbVehicle(this, dbConnect, "t_vehicle_veh", "veh");
            DbBaseItem = new DbBaseItem(this, dbConnect, "t_itemdata_itd", "itd");
            DbArea = new DbArea(this, dbConnect, "t_area_are", "are");
            DbPlayer = new DbPlayer(this, dbConnect, "t_character_cha", "cha");
            DbAccount = new DbAccount(this, dbConnect, "t_account_acc", "acc");
            //DbComponentLink = new DbLink(this, dbConnect, "t_link_lin", "lin");
            DbSkin = new DbSkin(this, dbConnect, "t_skin_ski", "ski");
            DbInterior = new DbInterior(this, dbConnect, "t_interior_int", "int");
            DbOrganization = new DbOrganization(this, dbConnect, "t_organization_org", "org");
            DbRank = new DbRank(this, dbConnect, "t_rank_ran", "ran");
            DbLink = new DbLink(this, dbConnect, "t_link_lin", "lin");
            DbInventory = new DbInventory(this, dbConnect, "t_inventory_inv", "inv");
            DbItem = new DbItem(this, dbConnect, "t_item_ite", "ite");
            DbSkill = new DbSkill(this, dbConnect, "t_skill_skl", "skl");
        }

        public void Init()
        {

            Alt.Log("=== Register events... ===");
            events.RegisterEvents();
            Alt.Log(">Server events registered");
            Chat.RegisterEvents();
            Alt.Log(">Chat registered");
            Alt.Log("=== Events are registered ===");
            Alt.Log("=== Register Spawns... ===");
            AddSpawn(new Spawn(new Position(-263.2484f, 2195.248f, 130.3956f)));
            Alt.Log("=== Spawns added... ===");
            Alt.Log("=== Register Commands... ===");
            AddAllCommands();
            Alt.Log("=== Commands are registered ===");
            Alt.Log("=== Load in database... ===");
            AddAllInteriors();
            Alt.Log(">All interiors loaded");
            AddAllBaseItems();
            Alt.Log(">Base items created");
            AddAllVehicles();
            Alt.Log(">Vehicles spawned");
            AddAllAreas();
            Alt.Log(">All areas loaded");
            AddAllOrganizations();
            Alt.Log(">All organizations loaded");
            AddAllLinks();

        }

        public void AddPlayer(Player player)
        {
            players.Add(player);
            VoiceChannel.AddPlayer(player.AltPlayer);
            Alt.EmitAllClients("chatmessage", null, $"{player.AltPlayer.Name} c'est connecté!");
        }

        public Player GetPlayerByDbId(uint id)
        {
            foreach (Player player in players)
            {
                if (player.Id == id) return player;
            }

            return null;
        }

        public Player[] GetPlayers()
        {
            return players.ToArray();
        }

        public Player[] GetPlayers(string nameOrId)
        {
            List<Player> players = new List<Player>();
            foreach (Player player in this.players)
            {
                if (player.ServerId.ToString().Equals(nameOrId)) players.Add(player);
                else if (player.Name.ToLower().StartsWith(nameOrId.ToLower())) players.Add(player);
            }
            return players.ToArray();
        }

        public void RemovePlayer(Player player)
        {
            players.Remove(player);
            VoiceChannel.RemovePlayer(player.AltPlayer);
            Alt.EmitAllClients("chatmessage", $"{player.AltPlayer.Name} c'est déconnecté!");
        }

        public void AddVehicle(Vehicle vehicle)
        {
            vehicles.Add(vehicle);
            vehicle.Game = this;
            //vehicle.Spawn();
        }

        public void AddAllVehicles()
        {
            vehicles = DbVehicle.GetAll().ToList();
            vehicles.ForEach((veh) => { veh.Game = this; });
        }

        public Vehicle[] GetVehicles()
        {
            return vehicles.ToArray();
        }

        public void RemoveVehicle(Vehicle vehicle)
        {
            vehicles.Remove(vehicle);
            if (vehicle.Id != 0) DbVehicle.Delete(vehicle);
            vehicle.AltVehicle.Remove();
            //
        }

        public void AddAllBaseItems()
        {
            baseitems = DbBaseItem.GetAll();
        }

        public BaseItem GetBaseItem(uint id)
        {
            return baseitems.FirstOrDefault(baseitem => baseitem.Id == id);
        }

        public BaseItem[] GetBaseItems()
        {
            return baseitems.ToArray();
        }

        public Area GetArea(Position position)
        {
            foreach (Area area in areas)
            {
                if (area.Position.Distance(position) < area.Radius)
                {
                    return area;
                }
            }

            return null;
        }
        public Location GetDestination(Position position, short dimension)
        {
            foreach (Area area in areas)
            {
                if (area.InteriorLocation.Position.Distance(position) < area.Radius && area.Id == dimension)
                {
                    return area.ExteriorLocation;
                }
                if (area.ExteriorLocation.Position.Distance(position) < area.Radius && area.Dimension == dimension)
                {
                    return area.InteriorLocation;
                }
            }

            return default;
        }
        public Area GetArea(Position position, Area.AreaType type)
        {
            foreach (Area area in areas)
            {
                if (area.Type != type) continue;
                if (area.Position.Distance(position) < area.Radius)
                {
                    return area;
                }
            }

            return null;
        }
        public Area GetArea(Player player, Area.AreaType type)
        {
            return GetArea(player.FeetPosition, type);
        }

        public void AddArea(Area area)
        {
            areas.Add(area);
        }

        public void AddAllAreas()
        {
            areas = DbArea.GetAll().ToList();
            areas.ForEach((area) =>
            {
                area.Game = this;
                //area.Spawn(); //TODO
            });
        }

        public void RemoveArea(Area area)
        {
            areas.Remove(area);
            DbArea.Delete(area);
            area.AltCheckpoint.Remove();
        }

        public void AddSpawn(Spawn spawn)
        {
            spawns.Add(spawn);
        }

        public Spawn GetSpawn(int id)
        {
            if (id < spawns.Count)
            {
                return spawns[id];
            }
            else
            {
                return null;
            }
        }

        public void RemoveSpawn(Spawn spawn)
        {
            spawns.Remove(spawn);
        }




        public void AddAllCommands()
        {
            commands = Command.GetAllCommands();
        }


        public Command[] GetCommands()
        {
            return commands.ToArray();
        }

        public Command[] GetCommands(Command.CommandType commandType)
        {
            return commands.Where(cmd => cmd.Type == commandType).ToArray();
        }

        public Command[] GetCommands(string[] parameters)
        {
            List<Command> cmdsValid = new List<Command>();
            foreach (Command command in commands)
            {
                bool isValid = CommandFitToParameter(command, parameters);
                if (isValid)
                {
                    if (parameters.Length >= command.Name.Split("_").Length)
                    {
                        for (int i = 0; i < cmdsValid.Count; i++)
                        {
                            Command command1 = cmdsValid[i];
                            string[] command1Text = command1.Name.Split("_");
                            string[] commandText = command.Name.Split("_");
                            if (command1Text.Length < commandText.Length)
                            {
                                cmdsValid.Remove(command1);
                                i--;
                            }
                        }
                    }

                    cmdsValid.Add(command);
                }
            }

            foreach (Command command in cmdsValid)
            {
                string[] commandText = command.Name.Split("_");
                if (parameters.Length >= commandText.Length)
                {
                    if (commandText[commandText.Length - 1].Equals(parameters[commandText.Length - 1]))
                    {
                        return new[] { command };
                    }
                }
            }

            return cmdsValid.ToArray();
        }

        public bool CommandFitToParameter(Command command, string[] parameters)
        {
            string[] commandText = command.Name.Split("_");
            for (int i = 0; i < parameters.Length; i++)
            {
                if (commandText.Length <= i) return true;
                if (!commandText[i].StartsWith(parameters[i])) return false;
            }
            return true;
        }

        public Command[] GetCommands3(string[] parameters)
        {
            List<Command> cmds = new List<Command>(commands);
            int i = 0;
            while (cmds.Count > 1 && i < parameters.Length) // While good commands is too high
            {
                for (int j = 0; j < cmds.Count; j++)
                {
                    Command command = cmds[j];
                    string[] commandText = command.Name.Split("_");// vehicule_park => vehicule park
                    if (commandText.Length > i) // If command have good size for test
                    {


                        if (!commandText[i].StartsWith(parameters[i]))
                        {
                            j--;
                            cmds.Remove(command);
                        }
                    }
                }

                if (cmds.Count <= 1) break;
                bool sameLength = true;
                for (int j = 1; j < cmds.Count; j++)
                {
                    Command command = cmds[j];
                    Command command2 = cmds[j - 1];
                    string[] commandText = command.Name.Split("_");// vehicule_park => vehicule park
                    string[] commandText2 = command2.Name.Split("_");// vehicule_park => vehicule park
                    if (commandText2.Length != commandText.Length)
                    {
                        sameLength = false;
                        break;
                    }
                }

                if (sameLength && i >= cmds[0].Name.Split("_").Length)
                {

                    break;
                }
                for (int j = 0; j < cmds.Count; j++)
                {
                    Command command = cmds[j];
                    string[] commandText = command.Name.Split("_");// vehicule_park => vehicule park
                    if (commandText.Length <= i)
                    {
                        j--;
                        cmds.Remove(command);
                    }
                }
                i++;
            }

            return cmds.ToArray();
        }




        public Skin[] GetSkins()
        {
            return skins.ToArray();
        }


        public void AddAllInteriors()
        {
            interiors = DbInterior.GetAll();
        }
        public Interior GetInterior(uint id)
        {
            foreach (Interior interior in interiors)
            {
                if (interior.Id == id) return interior;
            }
            return null;
        }
        public Interior[] GetInteriors(string name)
        {
            List<Interior> inters = new List<Interior>();
            foreach (Interior interior in interiors)
            {
                if (interior.Id.ToString().Equals(name)) inters.Add(interior);
                if (interior.Name.Replace(" ", "_").StartsWith(name)) inters.Add(interior);
            }
            return inters.ToArray();
        }
        public Interior[] GetInteriors()
        {
            return interiors;
        }

        public void AddOrganization(Organization org)
        {
            organizations.Add(org);
        }

        public void AddAllOrganizations()
        {
            organizations = DbOrganization.GetAll().ToList();
            foreach (Organization organization in organizations)
            {
                Rank[] ranks = DbRank.GetAll(organization);
                foreach (Rank rank in ranks)
                {
                    organization.AddRank(rank);
                }
            }
        }
        public Organization GetOrganization(uint id)
        {
            foreach (Organization organization in organizations)
            {
                if (organization.Id == id) return organization;
            }

            return null;
        }
        public Organization[] GetOrganizations(string name)
        {
            List<Organization> orgs = new List<Organization>();
            foreach (Organization organization in this.organizations)
            {
                if (organization.Id.ToString().Equals(name) || organization.Name.Replace(" ", "_").Equals(name)) orgs.Add(organization);
            }
            return orgs.ToArray();
        }
        public Organization[] GetOrganizations()
        {
            return organizations.ToArray();
        }
        public void RemoveOrganization(Organization organization)
        {
            organizations.Remove(organization);
            DbOrganization.Delete(organization);
            //if (organization.Id != 0) DbVehicle.Delete(organization);

        }


        public Link AddLink(Link link)
        {
            Link sLink = GetLink(link);
            if (sLink == null)
            {
                links.Add(link);
                return link;
            }
            else
            {
                sLink.Type = link.Type;
                return sLink;
            }
        }
        public void AddAllLinks()
        {
            links = DbLink.GetAll().ToList();
        }

        public Link GetLink(byte from, byte to, ushort drawableFrom, ushort drawableTo)
        {
            foreach (Link link in links)
            {
                if (link.Component1.Item1 == from &&
                    link.Component1.Item2 == drawableFrom &&
                    link.Component2.Item1 == to &&
                    link.Component2.Item2 == drawableTo)
                {
                    return link;
                }
            }

            return null;

        }
        public Link GetLink(Link link)
        {
            return GetLink(link.Component1.Item1, link.Component2.Item1, link.Component1.Item2, link.Component2.Item2);
        }
        public Link[] GetLinks()
        {
            return links.ToArray();
        }

        public static Game BaseGame = new Game();

    }
}
