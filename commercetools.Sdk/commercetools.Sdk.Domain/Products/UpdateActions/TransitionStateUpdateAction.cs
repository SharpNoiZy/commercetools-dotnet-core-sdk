﻿using commercetools.Sdk.Domain.States;

namespace commercetools.Sdk.Domain.Products.UpdateActions
{
    public class TransitionStateUpdateAction : UpdateAction<Product>
    {
        public string Action => "transitionState";
        public Reference<State> State { get; set; }
        public bool Force { get; set; }
    }
}