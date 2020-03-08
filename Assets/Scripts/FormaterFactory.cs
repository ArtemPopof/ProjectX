public class FormaterFactory {

    public static IPropertyFormater GetFormater(object value)
    {
        if (value is float) {
            return new DefaultFloatFormater();
        } else {
            return new DefaultFormater();
        }
    }
}