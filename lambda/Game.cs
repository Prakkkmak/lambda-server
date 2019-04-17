using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using AltV.Net;
using AltV.Net.Data;
using Items;
using Lambda.Commands;
using Lambda.Database;
using Lambda.Entity;
using Lambda.Items;
using Lambda.Utils;

namespace Lambda
{
    public class Game
    {
        private List<Player> players;
        private List<Vehicle> vehicles;
        private List<Area> areas;
        private List<Spawn> spawns;
        private List<Command> commands;
        private List<ComponentLink> componentLinks;
        private Skin[] skins;
        private BaseItem[] baseitems;
        private DBConnect dbConnect;
        private Events events;

        public Chat Chat;
        public DbVehicle DbVehicle { get; }
        public DbBaseItem DbBaseItem { get; }
        public DbArea DbArea { get; }
        public DbPlayer DbPlayer { get; }
        public DbAccount DbAccount { get; }

        public DbSkin DbSkin { get; }

        public DbComponentLink DbComponentLink { get; }

        public Game()
        {
            players = new List<Player>();
            vehicles = new List<Vehicle>();
            areas = new List<Area>();
            spawns = new List<Spawn>();
            commands = new List<Command>();
            componentLinks = new List<ComponentLink>();
            baseitems = new BaseItem[0];
            dbConnect = new DBConnect();
            events = new Events(this);
            skins = new Skin[0];

            Chat = new Chat();
            DbVehicle = new DbVehicle(this, dbConnect, "t_vehicle_veh", "veh");
            DbBaseItem = new DbBaseItem(this, dbConnect, "t_itemdata_itd", "itd");
            DbArea = new DbArea(this, dbConnect, "t_area_are", "are");
            DbPlayer = new DbPlayer(this, dbConnect, "t_character_cha", "cha");
            DbAccount = new DbAccount(this, dbConnect, "t_account_acc", "acc");
            DbComponentLink = new DbComponentLink(this, dbConnect, "t_link_lin", "lin");
            DbSkin = new DbSkin(this, dbConnect, "t_skin_ski", "ski");
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
            AddSpawn(new Spawn(new Position(-1023.89f, -487.7407f, 36.96338f)));
            Alt.Log("=== Spawns added... ===");
            Alt.Log("=== Register Commands... ===");
            AddAllCommands();
            Alt.Log("=== Commands are registered ===");
            Alt.Log("=== Load in database... ===");
            Alt.Log(DbVehicle.ToString());
            AddAllVehicles();
            Alt.Log(">Vehicles spawned");
            AddAllBaseItems();
            Alt.Log(">Base items created");
            AddAllComponentLinks();
            Alt.Log(">Components links loaded");
            AddAllAreas();
            Alt.Log(">All areas loaded");
        }

        public void AddPlayer(Player player)
        {
            players.Add(player);
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
                else if (player.Name.StartsWith(nameOrId)) players.Add(player);
            }
            return players.ToArray();
        }

        public void RemovePlayer(Player player)
        {
            players.Remove(player);
        }

        public void AddVehicle(Vehicle vehicle)
        {
            vehicles.Add(vehicle);
            vehicle.Game = this;
            vehicle.Spawn();
        }

        public void AddAllVehicles()
        {
            vehicles = DbVehicle.GetAll().ToList();
            vehicles.ForEach((veh) =>
            {
                veh.Spawn();
            });
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

        public void AddArea(Area area)
        {
            areas.Add(area);
        }

        public void AddAllAreas()
        {
            areas = DbArea.GetAll().ToList();
            areas.ForEach((area) =>
            {
                //area.Spawn();
            });
        }

        public void RemoveArea(Area area)
        {
            areas.Remove(area);
            DbArea.Delete(area);
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

        public void AddCommand(Command command)
        {
            commands.Add(command);
            Alt.Log($"[COMMAND]{command.Name} registered");
        }

        public void AddAllCommands(string namespaceName = "", Assembly assembly = null)
        {

            if (namespaceName.Length == 0) namespaceName = typeof(Command).Namespace; //If no namespace is specified
            if (assembly == null) assembly = Assembly.GetExecutingAssembly();
            Type[] types = GetTypesInNamespace(assembly, namespaceName); // Get all classes in this namespace
            Alt.Log($"Nombre de types dans le namespace {namespaceName}: " + types.Length);
            foreach (Type type in types)
            {
                foreach (MethodInfo method in type.GetMethods())
                {
                    if (method.GetCustomAttributes(typeof(CommandAttribute), false).Length <= 0) continue;
                    if (method.GetParameters()[0].ParameterType == typeof(Player) && method.GetParameters()[1].ParameterType == typeof(string[]))
                    {
                        Command.CommandFunc function = (Command.CommandFunc)Delegate.CreateDelegate(typeof(Command.CommandFunc), method, false); // Create the delegate of my méthod
                        if (function == null) Alt.Log("FUNCTION NULL" + method.Name);
                        CommandAttribute attribute = (CommandAttribute)method.GetCustomAttributes(typeof(CommandAttribute), false)[0];
                        PermissionAttribute[] permissionAttributes = (PermissionAttribute[])method.GetCustomAttributes(typeof(PermissionAttribute), false);
                        string permission = "";
                        if (permissionAttributes.Length > 0) permission = permissionAttributes[0].Permission;
                        Command command = new Command(method.Name.ToLower(), function, attribute.Type, attribute.Syntax, permission);
                        AddCommand(command); // Register the command
                    }
                }
            }
        }

        public static Type[] GetTypesInNamespace(Assembly assembly, string nameSpace)
        {
            return assembly.GetTypes().Where(t => string.Equals(t.Namespace, nameSpace, StringComparison.Ordinal)).ToArray();
        }

        public Command[] GetCommands()
        {
            return commands.ToArray();
        }

        public Command[] GetCommands(string[] parameters)
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

                if (sameLength && i >= cmds[0].Name.Split("_").Length) break;
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

        public void AddComponentLink(ComponentLink componentLink)
        {
            componentLinks.Add(componentLink);
        }

        public void AddComponentLinks(ComponentLink[] links)
        {
            foreach (ComponentLink componentLink in links)
            {
                ComponentLink actual = GetComponentLink(componentLink);
                if (actual == null)
                {
                    componentLinks.Add(componentLink);
                }
                else
                {
                    actual.Validity = componentLink.Validity;
                }
            }
        }

        public void AddAllComponentLinks()
        {
            componentLinks = DbComponentLink.GetAll().ToList();
        }

        public ComponentLink GetComponentLink(ComponentLink componentLink)
        {
            foreach (ComponentLink comp in componentLinks)
            {
                if (ComponentLink.Equals(comp, componentLink)) return comp;
            }
            return null;
        }

        public ComponentLink[] GetComponentLinks(ComponentLink[] componentLinks)
        {
            List<ComponentLink> similarities = new List<ComponentLink>();
            foreach (ComponentLink componentLink in componentLinks)
            {
                ComponentLink comp = GetComponentLink(componentLink);
                if (comp != null) similarities.Add(comp);
            }
            return similarities.ToArray();
        }

        public void AddAllSkins()
        {
            ComponentLink[] links = componentLinks.ToArray();
            List<ComponentLink> TopToLeg = new List<ComponentLink>();
            List<ComponentLink> FeetToLeg = new List<ComponentLink>();
            List<ComponentLink> HairToMask = new List<ComponentLink>();
            List<ComponentLink> TorsoToUndershirt = new List<ComponentLink>();
            List<ComponentLink> TorsoToTop = new List<ComponentLink>();
            List<ComponentLink> UndershirtToTop = new List<ComponentLink>();
            List<ComponentLink> UndershirtToLeg = new List<ComponentLink>();
            List<ComponentLink> MaskToTop = new List<ComponentLink>();
            List<Skin> goodskins = new List<Skin>();
            foreach (ComponentLink link in links)
            {
                if (Link.Equals(link.Link, Link.TopToLeg))
                {
                    TopToLeg.Add(link);
                }
                else if (Link.Equals(link.Link, Link.FeetToLeg))
                {
                    FeetToLeg.Add(link);
                }
                else if (Link.Equals(link.Link, Link.HairToMask))
                {
                    HairToMask.Add(link);
                }
                else if (Link.Equals(link.Link, Link.TorsoToUndershirt))
                {
                    TorsoToUndershirt.Add(link);
                }
                else if (Link.Equals(link.Link, Link.TorsoToTop))
                {
                    TorsoToTop.Add(link);
                }
                else if (Link.Equals(link.Link, Link.UndershirtToTop))
                {
                    UndershirtToTop.Add(link);
                }
                else if (Link.Equals(link.Link, Link.MaskToTop))
                {
                    MaskToTop.Add(link);
                }
                else if (Link.Equals(link.Link, Link.UndershirtToLeg))
                {
                    UndershirtToLeg.Add(link);
                }
            }

            foreach (ComponentLink feetToLeg in FeetToLeg)
            {
                bool isGoodSkin = true;
                if (feetToLeg.Validity != ComponentLink.Valid.TRUE) continue;
                //if (feetToLeg.Validity == ComponentLink.Valid.FALSE) isGoodSkin = false;
                foreach (ComponentLink topToLeg in TopToLeg)
                {
                    if (topToLeg.Validity != ComponentLink.Valid.TRUE ||
                        topToLeg.DrawableB != feetToLeg.DrawableB) continue;
                    //if (topToLeg.Validity == ComponentLink.Valid.FALSE) isGoodSkin = false;
                    foreach (ComponentLink torsoToTop in TorsoToTop)
                    {
                        if (torsoToTop.Validity != ComponentLink.Valid.TRUE ||
                            torsoToTop.DrawableB != topToLeg.DrawableA) continue;
                        //if (torsoToTop.Validity == ComponentLink.Valid.FALSE) isGoodSkin = false;
                        foreach (ComponentLink torsoToUndershirt in TorsoToUndershirt)
                        {
                            if (torsoToUndershirt.Validity != ComponentLink.Valid.TRUE ||
                                torsoToUndershirt.DrawableA != torsoToTop.DrawableA) continue;
                            //if (torsoToUndershirt.Validity == ComponentLink.Valid.FALSE) isGoodSkin = false;
                            foreach (ComponentLink undershortToTop in UndershirtToTop)
                            {
                                //if (torsoToUndershirt.Link.To != undershortToTop.Link.From) continue;
                                if (undershortToTop.Validity != ComponentLink.Valid.TRUE ||
                                    undershortToTop.DrawableB != torsoToTop.DrawableB) continue;
                                // if (undershortToTop.Validity == ComponentLink.Valid.FALSE) isGoodSkin = false;
                                foreach (ComponentLink hairToMask in HairToMask)
                                {
                                    if (hairToMask.Validity != ComponentLink.Valid.TRUE) continue;
                                    // if (hairToMask.Validity == ComponentLink.Valid.FALSE) isGoodSkin = false;
                                    foreach (ComponentLink maskToTop in MaskToTop)
                                    {
                                        if (maskToTop.Validity != ComponentLink.Valid.TRUE ||
                                            maskToTop.DrawableB != topToLeg.DrawableA) continue;
                                        // if (maskToTop.Validity == ComponentLink.Valid.FALSE) isGoodSkin = false;
                                        foreach (ComponentLink undershirtToLeg in UndershirtToLeg)
                                        {
                                            if (undershirtToLeg.Validity != ComponentLink.Valid.TRUE ||
                                                undershirtToLeg.DrawableA != undershortToTop.DrawableA) continue;
                                            //if (undershirtToLeg.Validity == ComponentLink.Valid.FALSE) isGoodSkin = false;

                                            Skin skin = new Skin();
                                            skin.Feet = new Component(feetToLeg.DrawableA);
                                            skin.Leg = new Component(feetToLeg.DrawableB);
                                            skin.Top = new Component(topToLeg.DrawableA);
                                            skin.Torso = new Component(torsoToTop.DrawableA);
                                            skin.Undershirt = new Component(undershortToTop.DrawableA);
                                            skin.Hair = new Component(hairToMask.DrawableA);
                                            skin.Mask = new Component(hairToMask.DrawableB);
                                            if (isGoodSkin) goodskins.Add(skin);
                                            //else BadSkins.Add(skin);

                                        }

                                    }


                                }
                            }
                        }
                    }
                }



            }
            skins = goodskins.ToArray();
            //return GoodSkins.Length;
        }

        public Skin[] GetSkins()
        {
            return skins.ToArray();
        }

        public Skin GetSkinToDiscorver()
        {
            foreach (Skin goodSkin in skins)
            {
                Skin skinToDiscover = goodSkin.Copy();
                for (int nbrDiff = 1; nbrDiff <= 11; nbrDiff++)
                {
                    uint i;

                    for (i = 0; i < Component.HairMaxValue; i++)
                    {
                        skinToDiscover.SetComponent((uint)Skin.ClothNumber.HAIR, i);
                        if (skinToDiscover.GetLinksByType(ComponentLink.Valid.FALSE).Length > 0) continue;
                        ComponentLink[] links = skinToDiscover.GetLinksByType(ComponentLink.Valid.UNKNOW);
                        if (links.Length == nbrDiff) return skinToDiscover;
                    }
                    for (i = 0; i < Component.MaskMaxValue; i++)
                    {
                        skinToDiscover.SetComponent((uint)Skin.ClothNumber.MASK, i);
                        if (skinToDiscover.GetLinksByType(ComponentLink.Valid.FALSE).Length > 0) continue;
                        if (skinToDiscover.GetLinksByType(ComponentLink.Valid.UNKNOW).Length == nbrDiff) return skinToDiscover;
                    }
                    for (i = 0; i < Component.TopMaxValue; i++)
                    {
                        skinToDiscover.SetComponent((uint)Skin.ClothNumber.TOP, i);
                        if (skinToDiscover.GetLinksByType(ComponentLink.Valid.FALSE).Length > 0) continue;
                        if (skinToDiscover.GetLinksByType(ComponentLink.Valid.UNKNOW).Length == nbrDiff) return skinToDiscover;
                    }
                    for (i = 0; i < Component.UndershirtMaxValue; i++)
                    {
                        skinToDiscover.SetComponent((uint)Skin.ClothNumber.UNDERSHIRT, i);
                        if (skinToDiscover.GetLinksByType(ComponentLink.Valid.FALSE).Length > 0) continue;
                        if (skinToDiscover.GetLinksByType(ComponentLink.Valid.UNKNOW).Length == nbrDiff) return skinToDiscover;
                    }
                    for (i = 0; i < Component.TorsoMaxValue; i++)
                    {
                        skinToDiscover.SetComponent((uint)Skin.ClothNumber.TORSO, i);
                        if (skinToDiscover.GetLinksByType(ComponentLink.Valid.FALSE).Length > 0) continue;
                        if (skinToDiscover.GetLinksByType(ComponentLink.Valid.UNKNOW).Length == nbrDiff) return skinToDiscover;
                    }
                    for (i = 0; i < Component.FeetMaxValue; i++)
                    {
                        skinToDiscover.SetComponent((uint)Skin.ClothNumber.FEET, i);
                        if (skinToDiscover.GetLinksByType(ComponentLink.Valid.FALSE).Length > 0) continue;
                        if (skinToDiscover.GetLinksByType(ComponentLink.Valid.UNKNOW).Length == nbrDiff) return skinToDiscover;
                    }
                    for (i = 0; i < Component.LegMaxValue; i++)
                    {
                        skinToDiscover.SetComponent((uint)Skin.ClothNumber.LEG, i);
                        if (skinToDiscover.GetLinksByType(ComponentLink.Valid.FALSE).Length > 0) continue;
                        if (skinToDiscover.GetLinksByType(ComponentLink.Valid.UNKNOW).Length == nbrDiff) return skinToDiscover;
                    }
                }

            }

            return null;
        }
    }
}
