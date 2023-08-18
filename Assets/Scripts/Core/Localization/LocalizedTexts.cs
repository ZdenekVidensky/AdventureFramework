//using Sirenix.OdinInspector;
using System.Collections.Generic;
using TVB.Core.Localization;
using UnityEngine;

public class LocalizedTexts : ScriptableObject
{
    public string Name;
    //[Searchable]
    public List<LocalizedTextData> Items = new List<LocalizedTextData>(16)
    {
        new LocalizedTextData()
    };
}
