
using UnityEngine;

[CreateAssetMenu]
public class ReadmeScriptableObject : ScriptableObject {

#if UNITY_EDITOR
    [Multiline]
    public string DeveloperDescription = "";
#endif

}
