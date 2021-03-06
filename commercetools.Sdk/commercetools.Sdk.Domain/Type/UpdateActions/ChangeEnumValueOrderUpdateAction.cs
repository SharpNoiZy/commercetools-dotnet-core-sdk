﻿using System.Collections.Generic;

namespace commercetools.Sdk.Domain
{
    public class ChangeEnumValueOrderUpdateAction : UpdateAction<Type>
    {
        public string Action => "changeEnumValueOrder";
        public string FieldNames { get; set; }
        public List<string> Keys { get; set; }
    }
}