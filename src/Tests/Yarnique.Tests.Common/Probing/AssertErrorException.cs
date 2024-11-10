namespace Yarnique.Test.Common.Probing
{
    public class AssertErrorException : Exception
    {
        public AssertErrorException(string message)
            : base(message)
        {
        }
    }
}
