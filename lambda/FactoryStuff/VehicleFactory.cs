using System;
using System.Collections.Generic;
using System.Text;
using AltV.Net;
using AltV.Net.Elements.Entities;
using Vehicle = Lambda.Entity.Vehicle;

namespace Lambda.FactoryStuff
{
    class VehicleFactory : IEntityFactory<IVehicle>
    {
        public IVehicle Create(IntPtr vehiclePointer, ushort id)
        {
            return new Vehicle(vehiclePointer, id);
        }
    }
}
