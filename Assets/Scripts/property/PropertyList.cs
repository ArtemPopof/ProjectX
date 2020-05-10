using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Ui properties list
// properties is some values that can be binded to some ui elements
// TODO improved typesafety
public class PropertyList {
    private Dictionary<string, Property> properties = new Dictionary<string, Property>();

    public PropertySettings Add(string name, object value) {
        var property = new Property(value);
        properties.Add(name, property);

        return new PropertySettings(property);
    }

    public void bind(Text field, string propertyName)
    {
        properties[propertyName].Field = field;
    }

    public void setProperty(string name, object value) {
        properties[name].Value = value;
    }

    public class PropertySettings {
        private Property property;

        public PropertySettings(Property prop) {
            property = prop;
        }

        public Property WithCustomFormater(IPropertyFormater formater)
        {
            property.formater = formater;
            return property;
        }
    }

    public object GetProperty(string name)
    {
        return properties[name].Value;
    }

    public float GetFloat(string name)
    {
        return (float) properties[name].Value;
    }

    public int GetInt(string name)
    {
        return (int) properties[name].Value;
    }

    public void AddToIntProperty(string name, int count)
    {
        var current = GetInt(name);

        setProperty(name, current + count);
    }
}