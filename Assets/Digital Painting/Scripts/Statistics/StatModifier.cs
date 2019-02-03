namespace wizardscode.statistics
{
    public enum StatModifierType
    {
        Flat = 100,
        PercentAdditive = 200,
        PercentMultiplicative = 300,
    }

    public class StatModifier
    {

        public readonly float Value;
        public readonly StatModifierType Type;
        public readonly int Order;
        public readonly object Source;

        public StatModifier(float value, StatModifierType type, object source, int order)
        {
            Value = value;
            Type = type;
            Order = order;
            Source = source;
        }

        public StatModifier(float value, StatModifierType type, object source) : this(value, type, source, (int)type) { }

    }
}
