using System;
using System.Collections.Generic;
using System.Text;
using AltV.Net;
using AltV.Net.Elements.Entities;
using Player = Lambda.Entity.Player;

namespace Lambda.FactoryStuff
{
    class PlayerFactory : IEntityFactory<IPlayer>
    {
        public IPlayer Create(IntPtr playerPointerPtr, ushort id)
        {
            return new Player(playerPointerPtr, id);
        }
    }
}
