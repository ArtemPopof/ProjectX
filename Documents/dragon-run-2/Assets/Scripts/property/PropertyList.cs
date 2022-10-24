using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Ui properties list
// properties is some values that can be binded to some ui elements
// TODO improved typesafety
public class PropertyList {
    private Dictionary<string, Property> properties = new Dictionary<string, Property>();
    private Dictionary<string, List<Action<object>>> listeners = new Dictionary<string, List<Action<object>>>();

    public PropertySettings Add(string name, object value) {
        var property = new Property(value);
        properties.Add(name, property);

        return new PropertySettings(property);
    }

    public void bind(Text field, string propertyName)
    {
        if (field == null) return;

        bind(propertyName, (value) => field.text = value.ToString());
    }

    public void bind(string propertyName, Action<object> callback)
    {
        if (!listeners.ContainsKey(propertyName))
        {
            listeners[propertyName] = new List<Action<object>>();
        }

        listeners[propertyName].Add(callback);
    }

    public void setProperty(string name, object value) {
        properties[name].Value = value;

        if (listeners.ContainsKey(name))
        {
            listeners[name].ForEach((listener) => listener.Invoke(value));
        }
    }

    public void AddToIntProperty(string name, int valueToAdd)
    {
        var oldValue = GetInt(name);
        setProperty(name, oldValue + valueToAdd);
    }

    public void AddToFloatProperty(string name, float valueToAdd)
    {
        var oldValue = GetFloat(name);
        setProperty(name, oldValue + valueToAdd);
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
        var prop = properties[name].Value;
        var type = prop.GetType();
        var conversed = (float)prop;
        return (float) prop;
    }

    public int GetInt(string name)
    {
        return (int) properties[name].Value;
    }
}