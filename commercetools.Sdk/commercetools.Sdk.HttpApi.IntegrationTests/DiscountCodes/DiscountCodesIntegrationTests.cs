using System;
using System.Collections.Generic;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.CartDiscounts;
using commercetools.Sdk.Domain.Predicates;
using commercetools.Sdk.Domain.Query;
using commercetools.Sdk.HttpApi.Domain.Exceptions;
using Xunit;
using SetDescriptionUpdateAction = commercetools.Sdk.Domain.CartDiscounts.SetDescriptionUpdateAction;

namespace commercetools.Sdk.HttpApi.IntegrationTests.DiscountCodes
{
    [Collection("Integration Tests")]
    public class DiscountCodeIntegrationTests : IClassFixture<DiscountCodeFixture>
    {
        private readonly DiscountCodeFixture discountCodeFixture;

        public DiscountCodeIntegrationTests(DiscountCodeFixture discountCodeFixture)
        {
            this.discountCodeFixture = discountCodeFixture;
        }

        [Fact]
        public void CreateDiscountCode()
        {
            IClient commerceToolsClient = this.discountCodeFixture.GetService<IClient>();
            DiscountCodeDraft discountCodeDraft = this.discountCodeFixture.GetDiscountCodeDraft();
            DiscountCode discountCode = commerceToolsClient
                .ExecuteAsync(new CreateCommand<DiscountCode>(discountCodeDraft)).Result;
            this.discountCodeFixture.DiscountCodesToDelete.Add(discountCode);
            Assert.Equal(discountCodeDraft.Code, discountCode.Code);
        }

        #region UpdateActions

        #endregion
    }
}
