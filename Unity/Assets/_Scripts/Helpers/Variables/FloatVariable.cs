
using UnityEngine;

[CreateAssetMenu]
public class FloatVariable : ScriptableObject {

    public float value;

    public void SetValue(float _value)
    {
        value = _value;
    }

    public void SetValue(FloatVariable _value)
    {
        value = _value.value;
    }

    public void ApplyChange(float amount)
    {
        value += amount;
    }

    public void ApplyChange(FloatVariable amount)
    {
        value += amount.value;
    }

}
