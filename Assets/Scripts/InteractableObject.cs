using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public enum Type { Totem, Rock };
    public Type type;
    public int value;
}
