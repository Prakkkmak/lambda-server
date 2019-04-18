using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using AltV.Net;
using Lambda.Entity;
using Lambda.Administration;

namespace Lambda.Commands
{
    /// <summary>
    /// Custom attribute for specify commands methods
    /// </summary>
    public class CommandAttribute : Attribute
    {
        public Command.CommandType Type;
        public string[] Syntax;
        public CommandAttribute(Command.CommandType type, params string[] syntax)
        {
            this.Type = type;
            this.Syntax = syntax;
        }
    }

    public class PermissionAttribute : Attribute
    {
        public string Permission;
        public PermissionAttribute(string permission)
        {
            Permission = permission;
        }
    }

    /// <summary>
    /// A command is an action triggered by the player with the command in game or with an UI
    /// </summary>
    public class Command
    {
        public delegate CmdReturn CommandFunc(Player player, string[] parameters); // The declaration delegate of the method

        public enum CommandType
        {
            DEFAULT,
            ACCOUNT,
            ADMIN,
            TEST
        }
        public string Name { get; } // The Name of the command
        private CommandFunc func; // The function associed
        private CommandType type;
        private string[] syntax;
        private string permission;
        /// <summary>
        /// Construct a command
        /// </summary>
        /// <param Name="name">The Name of the command</param>
        /// <param Name="func">The function to perform</param>
        public Command(string name, CommandFunc func, CommandType type, string[] syntax, string permission = "")
        {
            this.Name = name;
            this.func = func;
            this.syntax = syntax;
            this.type = type;
            this.permission = permission;
        }
        /// <summary>
        /// Generate the syntax of the command
        /// </summary>
        /// <returns>The cmd return with the syntax</returns>
        public CmdReturn Syntax()
        {
            if (this.Name == null) throw new NullReferenceException();
            string syntaxString = "";
            if (this.syntax != null)
            {
                syntaxString += $"/{Name.Replace("_", " ")}";
                syntaxString = this.syntax.Aggregate(syntaxString, (current, s) => current + $" [{s}]");
            }
            return new CmdReturn(syntaxString, CmdReturn.CmdReturnType.SYNTAX);
        }

        /// <summary>
        /// Execute the f° associated with the command
        /// </summary>
        /// <param name="account">The account who perform the command</param>
        /// <param name="parameters">The parameters list given</param>
        /// <returns></returns>
        public CmdReturn Execute(Player player, string[] parameters)
        {
            if (syntax == null) throw new NullReferenceException();
            if (player.Account == null)
            {
                return new CmdReturn("Vous n etes pas connecté", CmdReturn.CmdReturnType.WARNING);
            }

            if (parameters.Length < syntax.Length + Name.Split("_").Length)  // syntax.length is parameters. The strict is for the Name
            {
                return Syntax();
            }

            if (!string.IsNullOrWhiteSpace(permission) && !player.PermissionExist(permission))
            {
                return new CmdReturn("Vous n'avez pas la permission de faire ceci", CmdReturn.CmdReturnType.WARNING);
            }

            if (func == null) throw new NullReferenceException("Aucune fonction n'est définie");
            CmdReturn cmdReturn = func(player, parameters); // Launch the command
            if (player.Account != null) player.Game.DbPlayer.Save(player);
            return cmdReturn;
        }

    }
}
