using System.Globalization;

namespace SmhiApi.Model
{
    public static class JsonParsers
    {
        public static double ToDouble(this string value, double defaultValue)
        {
            double result = defaultValue;

            //Try parsing the double in en-US first, if it fails then try in Invariant
            if (!double.TryParse(value, NumberStyles.Float, CultureInfo.GetCultureInfo("en-US"), out result))
                double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out result);

            return result;
        }
    }
}
