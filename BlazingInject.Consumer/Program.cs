
using BlazingInject.Core;
using BlazingInject.Consumer;

var services = new ServiceCollection();

// services.AddTransient<IIdGenerator, IdGenerator>();

services.AddSingleton<IConsoleWriter, SpecialConsoleWriter>();
services.TryAddSingleton<IConsoleWriter, ConsoleWriter>();


services.AddTransient<IIdGenerator, IdGenerator>();

var serviceProvider = services.BuildServiceProvider();

var idGen1 = serviceProvider.GetService<IIdGenerator>();
var idGen2 = serviceProvider.GetService<IIdGenerator>();

idGen1!.PrintGuid();
idGen2!.PrintGuid();
