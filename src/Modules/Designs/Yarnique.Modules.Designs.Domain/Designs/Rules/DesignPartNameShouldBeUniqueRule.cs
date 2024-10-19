using Yarnique.Common.Domain;

namespace Yarnique.Modules.Designs.Domain.Designs.Rules
{
    public class DesignPartNameShouldBeUniqueRule : IBusinessRule
    {
        private readonly List<string> _designPartsNames;
        private readonly string _designPartName;

        public DesignPartNameShouldBeUniqueRule(List<string> designPartsNames, string designPartName)
        {
            _designPartsNames = designPartsNames;
            _designPartName = designPartName;
        }

        public bool IsBroken() => !_designPartsNames.Any(x => x.ToLowerInvariant() == _designPartName.ToLowerInvariant());

        public string Message => "Design Part name should be unique.";
    }
}
