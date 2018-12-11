﻿using commercetools.Sdk.Domain;

namespace commercetools.Sdk.Client
{
    public abstract class SearchCommand<T> : Command<PagedQueryResult<T>>
    {
        public ISearchParameters<T> SearchParameters { get; set; }

        public override System.Type ResourceType => typeof(T);
    }
}