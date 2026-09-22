using System;
using System.Collections.Generic;
using UnityEngine;

public class NoiseSystem : MonoBehaviour
{
    public Action<Noise> OnNoiseEmitted;

    public void EmitNoise(Vector3 pos, float strength, NoiseType type, IDetectableEntity instigator)
    {
        Noise noise = new Noise(pos, strength, type, instigator);
        OnNoiseEmitted?.Invoke(noise);
    }
}

public struct Noise
{
    public Noise(Vector3 pos, float strength, NoiseType type, IDetectableEntity instigator)
    {
        Position = pos;
        Strength = strength;
        Type = type;
        Instigator = instigator;
    }
    public Vector3 Position;
    public float Strength;
    public NoiseType Type;
    public IDetectableEntity Instigator;

}
public enum NoiseType
{
    None,
    Footstep,
    Impact,
    Interaction,
    Combat,
    Other,
}