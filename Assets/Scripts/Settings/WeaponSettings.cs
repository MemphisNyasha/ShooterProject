using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Impulse
{
    public float Weak;
    public float Medium;
    public float Strong;
}

public class WeaponSettings : ScriptableObject
{
    public float FireRate;
    public float BulletSpeed;
    public Impulse ImpactImpulse;
}
