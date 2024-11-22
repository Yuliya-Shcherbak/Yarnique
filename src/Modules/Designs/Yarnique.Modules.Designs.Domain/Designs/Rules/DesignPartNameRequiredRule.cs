using Yarnique.Common.Domain;

namespace Yarnique.Modules.Designs.Domain.Designs.Rules
{
    public class DesignPartNameRequiredRule : IBusinessRule
    {
        private readonly string _designPartName;

        public DesignPartNameRequiredRule(string designPartName)
        {
            _designPartName = designPartName;
        }

        public bool IsBroken() => string.IsNullOrEmpty(_designPartName);

        public string Message => "Design Part name required.";
    }
}
