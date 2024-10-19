using Yarnique.Common.Domain;

namespace Yarnique.Modules.Designs.Domain.Designs.Rules
{
    public class DesignNameRequiredRule : IBusinessRule
    {
        private readonly string _designName;

        public DesignNameRequiredRule(string designName)
        {
            _designName = designName;
        }

        public bool IsBroken() => string.IsNullOrEmpty(_designName);

        public string Message => "Design name required.";
    }
}
