public class DefaultFloatFormater : IPropertyFormater {

    public string Format(object value)
    {
        float floatValue = (float) value;
        return floatValue.ToString("0.0");
    }
}