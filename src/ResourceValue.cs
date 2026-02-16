using Microsoft.Xna.Framework;

namespace BrightSpace;

public class ResourceValue
{
    private ValueInfo valueInfo;

    private float currentValue;
    
    public float value
    {
        get { return currentValue; }
        set { currentValue = MathHelper.Clamp(value, 0f, valueInfo.value);; }
    }

    public ResourceValue(ValueInfo valueInfo)
    {
        currentValue = valueInfo.value;
        this.valueInfo = valueInfo;
    }
}