﻿using commercetools.Sdk.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using Type = System.Type;

namespace commercetools.Sdk.Serialization
{
    public class AttributeConverter : JsonConverter
    {
        private readonly IEnumerable<ICustomConverter<Domain.Attribute>> customConverters;

        public AttributeConverter(IEnumerable<ICustomConverter<Domain.Attribute>> customConverters, MoneyConverter moneyConverter)
        {
            this.customConverters = customConverters;
        }

        public override bool CanConvert(Type objectType)
        {
            if (objectType == typeof(Domain.Attribute))
            {
                return true;
            }
            return false;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jsonObject = JObject.Load(reader);
            JToken valueProperty = jsonObject["value"];
            Type attributeType;
            if (IsSetAttribute(valueProperty))
            {
                attributeType = GetSetTypeByValueProperty(valueProperty);
            }
            else
            {
                attributeType = GetTypeByValueProperty(valueProperty);
            }

            if (attributeType == null)
            {
                // TODO Move this message to a localizable resource and add more information to the exception
                throw new JsonSerializationException("Attribute type cannot be determined.");
            }

            return jsonObject.ToObject(attributeType, serializer);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        private System.Type GetSetTypeBySimpleType(System.Type simpleType)
        {
            // TODO Move this to different class and add all types
            Dictionary<Type, Type> setMapping = new Dictionary<Type, Type>();
            setMapping.Add(typeof(TextAttribute), typeof(SetTextAttribute));
            if (setMapping.ContainsKey(simpleType))
            {
                return setMapping[simpleType];
            }
            // TODO Move this message to a localizable resource and add more information to the exception
            throw new JsonSerializationException("There is not set attribute defined for the given structure.");
        }

        private System.Type GetSetTypeByValueProperty(JToken valueProperty)
        {
            // It is assumed that all of the values in the array are of the same attribute type
            System.Type simpleType = GetTypeByValueProperty(valueProperty[0]);
            if (simpleType != null)
            {
                return GetSetTypeBySimpleType(simpleType);
            }
            return null;
        }

        private System.Type GetTypeByValueProperty(JToken valueProperty)
        {
            foreach (var customConvert in this.customConverters.OrderBy(c => c.Priority))
            {
                if (customConvert.CanConvert(valueProperty))
                {
                    return customConvert.Type;
                }
            }
            return null;
        }

        private bool IsSetAttribute(JToken valueProperty)
        {
            if (valueProperty != null)
            {
                return valueProperty.HasValues && valueProperty.Type == JTokenType.Array;
            }
            return false;
        }
    }
}