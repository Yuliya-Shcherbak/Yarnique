namespace Yarnique.Common.Domain.OrderStatuses
{
    public class ExecutionStatus : ValueObject
    {
        public static ExecutionStatus Pending => new ExecutionStatus(nameof(Pending));

        public static ExecutionStatus InProgress => new ExecutionStatus(nameof(InProgress));

        public static ExecutionStatus OnHold => new ExecutionStatus(nameof(OnHold));

        public static ExecutionStatus Completed => new ExecutionStatus(nameof(Completed));

        public string Value { get; }

        private ExecutionStatus(string value)
        {
            this.Value = value;
        }
    }
}
