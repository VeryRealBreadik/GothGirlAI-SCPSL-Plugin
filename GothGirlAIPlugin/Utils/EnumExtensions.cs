namespace GothGirlAIPlugin.Utils
{
    public static class EnumExtensions
    {
        public static TEnum Next<TEnum>(this TEnum value) where TEnum : struct, Enum
        {
            var values = (TEnum[]) Enum.GetValues(typeof(TEnum));
            int idx = Array.IndexOf(values, value);
            return values[(idx + 1) % values.Length];
        }
    }
}
