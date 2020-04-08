public class DefaultFormater : IPropertyFormater {

    public string Format(object value)
    {
        return value.ToString();
    }
}