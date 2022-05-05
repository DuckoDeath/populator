using Godot;
using System;

public class WindmillTile : Tile
{
    Sprite windmillBlades;
    Timer windmillTimer;

    public override BuildingTypeEnum buildingType => BuildingTypeEnum.Production;

    public override void _Ready()
    {
        base._Ready();
        windmillBlades = GetChild<Sprite>(1);
        windmillTimer = GetChild<Timer>(2);

        population = 0;
        maxPopulation = 1;

        income = 1;
        incomeOccurance = 5;

        cost = 20;
        overridable = true;

        if (isBuildBar) return;
        windmillTimer.Connect("timeout", this, nameof(OnTimer));
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        if (population > 0)
        {
            windmillBlades.Rotation += 3f * delta;
        }
    }

    public void StartTimer()
    {
        windmillTimer.WaitTime = incomeOccurance;
        windmillTimer.Start();
    }

    public override bool AddCitizen(BuildingTypeEnum buildingType)
    {
        var res = base.AddCitizen(buildingType);
        if (res) StartTimer();
        return res;
    }

    public void OnTimer()
    {
        EmitSignal(nameof(MadeMoney), income);
    }
}
