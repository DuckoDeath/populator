using Godot;
using System;

public class TreeTile : Tile
{

    public override BuildingTypeEnum buildingType => BuildingTypeEnum.Obstruction;

    public override void _Ready()
    {
        base._Ready();
        population = 0;
        maxPopulation = 0;

        income = 0;
        incomeOccurance = 0;

        cost = 0;
        overridable = false;
    }
}
