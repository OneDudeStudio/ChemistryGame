using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AggregateState
{
    Solid,
    Liquid,
    Gas,
}
[CreateAssetMenu(fileName = "ElementData", menuName = "Element")]
public class ElementSetting : ScriptableObject
{
    public AggregateState ItemAggregateState;
    public string ElementName;
    public string ElementLat;
    public string ElementReactLat;
}
