using System;
using System.Collections.Generic;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Categories;

namespace commercetools.Sdk.HttpApi.IntegrationTests.TaxCategories
{
    public class TaxCategoryFixture : ClientFixture, IDisposable
    {
        public List<TaxCategory> TaxCategoriesToDelete { get; private set; }

        public TaxCategoryFixture() : base()
        {
            this.TaxCategoriesToDelete = new List<TaxCategory>();
        }

        public void Dispose()
        {
            IClient commerceToolsClient = this.GetService<IClient>();
            this.TaxCategoriesToDelete.Reverse();
            foreach (TaxCategory category in this.TaxCategoriesToDelete)
            {
                TaxCategory deletedCategory = commerceToolsClient.ExecuteAsync(new DeleteByIdCommand<TaxCategory>(new Guid(category.Id), category.Version)).Result;
            }
        }

        public TaxCategoryDraft GetTaxCategoryDraft(string country = null)
        {
            TaxCategoryDraft taxCategoryDraft = new TaxCategoryDraft()
            {
                Name = this.RandomString(5),
                Key = this.RandomString(4),
                Rates = new List<TaxRate>(){ this.GetTaxRate(country)}
            };
            return taxCategoryDraft;
        }

        public TaxCategory CreateTaxCategory(string country = null)
        {
            return this.CreateTaxCategory(this.GetTaxCategoryDraft(country));
        }

        public TaxCategory CreateTaxCategory(TaxCategoryDraft taxCategoryDraft)
        {
            IClient commerceToolsClient = this.GetService<IClient>();
            TaxCategory taxCategory = commerceToolsClient.ExecuteAsync(new CreateCommand<TaxCategory>(taxCategoryDraft)).Result;
            return taxCategory;
        }

        /// <summary>
        /// Get Tax Rate
        /// </summary>
        /// <returns></returns>
        private TaxRate GetTaxRate(string country = null)
        {
            TaxRate taxRate = new TaxRate()
            {
                Name = this.RandomString(4),
                Country = country?? "DE",
                Amount = this.RandomDouble()
            };
            return taxRate;
        }
    }
}
