﻿namespace commercetools.Sdk.HttpApi.DelegatingHandlers
{
    public interface ICorrelationIdProvider
    {
        string CorrelationId { get; }
    }
}