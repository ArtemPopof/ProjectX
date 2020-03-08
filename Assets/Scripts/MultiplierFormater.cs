public class MultiplierFormater : IPropertyFormater {

    public string Format(object value)
    {
        float multiplier = (float) value;
        if (multiplier < 1) {
            multiplier = 1;
        }
        return "x" + multiplier.ToString("0.0");
    }
}