namespace BrightSpace;

public class ValueModifier
{
    public float value;
    public ModifierType type;

    public ValueModifier(float value, ModifierType type)
    {
        this.value = value;
        this.type = type;
    }
}