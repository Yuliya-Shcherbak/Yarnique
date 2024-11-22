namespace Yarnique.Common.Application.Configuration.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CacheableEntityAttribute : Attribute
    {
        private string[] EntitiesNames;

        public CacheableEntityAttribute(params string[] entitiesNames)
        {
            EntitiesNames = entitiesNames;
        }

        public string[] GetEntities() => EntitiesNames;
    }
}
