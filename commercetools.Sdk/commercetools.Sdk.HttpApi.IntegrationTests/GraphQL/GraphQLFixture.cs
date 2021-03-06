﻿using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Categories;
using System;
using System.Collections.Generic;
using commercetools.Sdk.Domain.Categories.UpdateActions;
using Type = commercetools.Sdk.Domain.Type;

namespace commercetools.Sdk.HttpApi.IntegrationTests
{
    public class GraphQLFixture : ClientFixture, IDisposable
    {
        private CategoryFixture categoryFixture;

        public GraphQLFixture() : base()
        {
            this.categoryFixture = new CategoryFixture();
        }

        public Category CreateCategory()
        {
            return this.categoryFixture.CreateCategory();
        }

        public void Dispose()
        {
            this.categoryFixture.Dispose();
        }
    }
}
