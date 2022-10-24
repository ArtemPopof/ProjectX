using UnityEngine;
using UnityEngine.UI;


public class Property {

    public IPropertyFormater formater { private get; set;}

    private Text _field;
    public Text Field

    {
        get => _field;
        set
        {
            _field = value;
            // update value state
            Value = Value;
        }
    }
    private object _value;
    public object Value    
    {
         get { return _value; }

         set 
         {
             _value = value; 
             if (Field != null) {
                Field.text = formater.Format(value); 
             }
         }
    }

    public Property(object value, Text field = null) {
        _field = field;
        _value = value;
        formater = FormaterFactory.GetFormater(value);
    }
}