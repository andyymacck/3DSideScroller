using UnityEngine;
using System;

[Serializable]
public class CollectableModel
{
    [SerializeField] private int count = 1;
    [SerializeField] private CollectabeType type;

    public CollectabeType CollectabeType => type;
    public int Count => count;

    public CollectableModel(CollectabeType type, int count)
    {
        this.type = type;
        this.count = count;
    }

    public void IncrementCount(int amout = 1)
    {
        this.count++; 
    }

    public void DecrementCount(int amout = 1)
    {
        this.count--;
    }

    public virtual void ApplyEffect(GameObject player)
    {
        Debug.Log("ApplyEffect" + Count);
    }
}


public class GemModel : CollectableModel
{
    public string color;

    public GemModel(CollectabeType type, int count, string color) : base(type, count)
    {
        this.color = color;
    }

    public override void ApplyEffect(GameObject player)
    {
        // For example, increase player's score
        Debug.Log("Increasing score by: " + Count);
        //base.ApplyEffect(player);
    }
}

[System.Serializable]
public class CoinModel : CollectableModel
{
    public CoinModel(CollectabeType type, int count) : base(type, count) { }

    public override void ApplyEffect(GameObject player)
    {
        // Increment player's coin count
        Debug.Log("Adding coins: " + Count);
    }
}

public class PotionModel : CollectableModel
{
    public string effect;

    public PotionModel(CollectabeType type, int count, string effect) : base(type, count)
    {
        this.effect = effect;
    }

    public override void ApplyEffect(GameObject player)
    {
        // Apply a potion effect
        Debug.Log("Applying potion effect: " + effect);
    }
}

public class HealthModel : CollectableModel
{
    public int healthPoints;

    public HealthModel(CollectabeType type, int healthPoints) : base(type, healthPoints)
    {
        this.healthPoints = healthPoints;
    }

    public override void ApplyEffect(GameObject player)
    {
        // Increase player's health
        Debug.Log("Increasing health by: " + healthPoints);
    }
}