using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CodeObjectDatabase", menuName = "ScriptableObjects/CodeObjectDatabase", order = 1)]
public class CodeObjectDatabase : ScriptableObject
{
    public List<CodeObjectPair> codeObjects = new List<CodeObjectPair>();
}

[System.Serializable]
public class CodeObjectPair
{
    public string code;
    public string objectName;
}