﻿using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.CartDiscounts
{
    public class SetCustomFieldUpdateAction : UpdateAction<CartDiscount>
    {
        public string Action => "setCustomField";
        [Required]
        public string Name { get; set; }
        public object Value { get; set; }
    }
}