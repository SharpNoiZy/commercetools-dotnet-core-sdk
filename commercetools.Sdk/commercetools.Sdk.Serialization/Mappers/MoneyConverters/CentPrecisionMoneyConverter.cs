﻿using commercetools.Sdk.Domain;
using Newtonsoft.Json.Linq;
using Type = System.Type;

namespace commercetools.Sdk.Serialization
{
    public class CentPrecisionMoneyConverter : ICustomJsonMapper<Money>
    {
        public int Priority => 4;

        public Type Type => typeof(CentPrecisionMoney);

        public bool CanConvert(JToken property)
        {
            if (property != null && property.HasValues)
            {
                var currencyCodeProperty = property["currencyCode"];
                if (currencyCodeProperty != null)
                {
                    return true;
                }
            }
            return false;
        }
    }
}