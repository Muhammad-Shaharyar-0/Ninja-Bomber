
using UnityEngine;

[CreateAssetMenu]
public class BoolVariable : ScriptableObject {

    [SerializeField]
    private bool _value = true;

    public bool value
    {
        get { return _value; }
        set { _value = value; }
    }
}
