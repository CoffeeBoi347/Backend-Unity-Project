using UnityEngine;

[CreateAssetMenu(fileName = "BackendConfigOBJ", menuName = "BackendConfig/Config", order = 1)]
public class BackendConfig : ScriptableObject
{
    public string urlPath;
    public string requestMethod;
}