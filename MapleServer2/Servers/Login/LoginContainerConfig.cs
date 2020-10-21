﻿using System.Reflection;
using Autofac;
using MapleServer2.Data;
using MapleServer2.Data.Static;
using MapleServer2.Network;
using MapleServer2.PacketHandlers;

namespace MapleServer2.Servers.Login {
    public static class LoginContainerConfig {
        public static IContainer Configure() {
            var builder = new ContainerBuilder();
            builder.RegisterLogger();
            builder.RegisterType<LoginServer>()
                .AsSelf()
                .SingleInstance();
            builder.RegisterType<PacketRouter<LoginSession>>()
                .As<PacketRouter<LoginSession>>()
                .SingleInstance();
            builder.RegisterType<LoginSession>()
                .AsSelf();
            builder.RegisterType<StaticAccountStorage>()
                .As<IAccountStorage>()
                .SingleInstance();

            // Make all packet handlers available to PacketRouter
            builder.RegisterAssemblyTypes(Assembly.Load(nameof(MapleServer2)))
                .Where(t => typeof(IPacketHandler<LoginSession>).IsAssignableFrom(t))
                .As<IPacketHandler<LoginSession>>()
                .SingleInstance();

            return builder.Build();
        }
    }
}