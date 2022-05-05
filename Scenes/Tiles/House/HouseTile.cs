using Godot;
using System;

public class HouseTile : Tile
{

    PackedScene smokeParticleScene;

    public override BuildingTypeEnum buildingType => BuildingTypeEnum.Housing;

    public override void _Ready()
    {
        base._Ready();
        smokeParticleScene = (PackedScene)ResourceLoader.Load("res://Scenes/Particles/SmokeParticles.tscn");

        population = 0;
        maxPopulation = 3;

        income = 0;
        incomeOccurance = 0;

        cost = 15;
        overridable = true;
    }

    public override bool AddCitizen(BuildingTypeEnum buildingType)
    {
        var res = base.AddCitizen(buildingType);
        if (res && population == 1) CreateParticles();
        return res;
    }

    public void CreateParticles()
    {
        var smokeParticle = (CPUParticles2D)smokeParticleScene.Instance();
        smokeParticle.Emitting = true;
        smokeParticle.Position = new Vector2(-2.5f, -5.5f);
        smokeParticle.ZIndex = 1;
        AddChild(smokeParticle);

        smokeParticle = (CPUParticles2D)smokeParticleScene.Instance();
        smokeParticle.Emitting = true;
        smokeParticle.Position = new Vector2(-1.5f, -6.5f);
        smokeParticle.ZIndex = 1;
        AddChild(smokeParticle);
    }

    public void DestroyParticles()
    {
        foreach (CPUParticles2D particle in GetChildren())
        {
            particle.QueueFree();
        }
    }
}
