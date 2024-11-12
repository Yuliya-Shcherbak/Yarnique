﻿using Yarnique.Common.Application;

namespace Yarnique.Test.Common
{
    public class ExecutionContextMock : IExecutionContextAccessor
    {
        public ExecutionContextMock(Guid userId)
        {
            UserId = userId;
        }

        public Guid UserId { get; }

        public Guid CorrelationId { get; }

        public bool IsAvailable { get; }
    }
}