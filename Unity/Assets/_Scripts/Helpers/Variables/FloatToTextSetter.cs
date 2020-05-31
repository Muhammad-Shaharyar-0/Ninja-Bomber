
using UnityEngine;
using UnityEngine.UI;


public class FloatToTextSetter : MonoBehaviour
{
    public Text Text;

    public FloatVariable Variable;

    public bool AlwaysUpdate;

    private void OnEnable()
    {
        Text.text = Variable.value + "";
    }

    private void Update()
    {
        if (AlwaysUpdate && Variable)
        {
            Text.text = Variable.value + "";
        }
    }
}

