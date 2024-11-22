using Autofac;
using Yarnique.Common.Infrastructure.InternalCommands;
using Yarnique.Common.Infrastructure;
using Yarnique.Common.Application.Configuration.Commands;

namespace Yarnique.Modules.Designs.Infrastructure.Configuration.Processing.InternalCommands
{
    internal class InternalCommandsModule : Autofac.Module
    {
        private readonly BiDictionary<string, Type> _internalCommandsMap;

        public InternalCommandsModule(BiDictionary<string, Type> internalCommandsMap)
        {
            _internalCommandsMap = internalCommandsMap;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<InternalCommandsMapper>()
                .As<IInternalCommandsMapper>()
                .FindConstructorsWith(new AllConstructorFinder())
                .WithParameter("internalCommandsMap", _internalCommandsMap)
                .SingleInstance();

            this.CheckMappings();
        }

        private void CheckMappings()
        {
            var internalCommands = Assemblies.Application
                .GetTypes()
                .Where(x => x.BaseType != null &&
                            (
                                (x.BaseType.IsGenericType &&
                                x.BaseType.GetGenericTypeDefinition() == typeof(InternalCommandBase<>)) ||
                                x.BaseType == typeof(InternalCommandBase)))
                .ToList();

            List<Type> notMappedInternalCommands = [];
            foreach (var internalCommand in internalCommands)
            {
                _internalCommandsMap.TryGetBySecond(internalCommand, out var name);

                if (name == null)
                {
                    notMappedInternalCommands.Add(internalCommand);
                }
            }

            if (notMappedInternalCommands.Any())
            {
                throw new ApplicationException($"Internal Commands {notMappedInternalCommands.Select(x => x.FullName).Aggregate((x, y) => x + "," + y)} not mapped");
            }
        }
    }
}
