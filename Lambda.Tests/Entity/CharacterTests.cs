using System;
using System.Collections.Generic;
using System.Text;
using AltV.Net.Elements.Entities;
using Lambda.Entity;
using Xunit;

namespace Lambda.Tests.Entity
{
    public class CharacterTests
    {
        [Fact]
        public void GetCharacter_ShouldWork()
        {
            Character character = new Character("Player0", 0);
            Character character2 = new Character("Player1", 1);
            Character.Characters.Add(character);
            Character.Characters.Add(character2);
            Character actual = Character.GetCharacter(0);
            Assert.NotNull(actual);
        }
        [Fact]
        public void GetCharacter_ShouldFail()
        {
            Character character = new Character("Player0", 0);
            Character character2 = new Character("Player1", 1);
            Character.Characters.Add(character);
            Character.Characters.Add(character2);
            Character actual = Character.GetCharacter(100);
            Assert.Null(actual);
        }
        [Fact]
        public void GetCharacters_ShouldWork()
        {
            Character character = new Character("Player0", 0);
            Character character2 = new Character("Player1", 1);
            Character.Characters.Add(character);
            Character.Characters.Add(character2);
            Character[] actual = Character.GetCharacters("Pla");
            Assert.True(actual.Length == 2);
        }
        [Fact]
        public void GetCharacters_ShouldFail()
        {
            Character character = new Character("Player0", 0);
            Character character2 = new Character("Player1", 1);
            Character.Characters.Add(character);
            Character.Characters.Add(character2);
            Character[] actual = Character.GetCharacters("NotGoodName");
            Assert.True(actual.Length == 0);
        }
    }
}
