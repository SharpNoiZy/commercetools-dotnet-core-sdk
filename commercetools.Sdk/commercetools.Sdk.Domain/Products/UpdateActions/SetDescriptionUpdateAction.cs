﻿using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Products.UpdateActions
{
    public class SetDescriptionUpdateAction : UpdateAction<Product>
    {
        public string Action => "setDescription";
        [Required]
        public LocalizedString Description { get; set; }
        public bool Staged { get; set; }
    }
}