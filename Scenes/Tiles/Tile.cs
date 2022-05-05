using Godot;
using System;

public class Tile : Sprite
{
    public Texture[] sprites = new Texture[2];

    public PackedScene populationParticleScene = GD.Load<PackedScene>("res://Scenes/Particles/PopulationParticle.tscn");

    public Area2D collisionArea;
    public Sprite hoverSprite;

    public Sprite stateSprite;

    public bool isBuildBar = false;
    public bool isAnimatingBuildBar = false;
    public Vector2 newBuildBarPosition;

    Random rng = new Random();

    public int population = 0;
    public int maxPopulation = 0;

    public int income = 0;
    public int incomeOccurance = 0;

    public int cost = 0;

    public bool hovered = false;
    public bool canHover = true;

    public bool isSelected = false;

    public enum BuildingTypeEnum
    {
        Production,
        Housing,
        Obstruction,
        Empty
    }

    public virtual BuildingTypeEnum buildingType => BuildingTypeEnum.Empty;

    public bool overridable = true;

    [Signal]
    public delegate void MadeMoney(int income);

    [Signal]
    public delegate void OnClick(Tile tile);

    public override void _Ready()
    {
        // load sprites
        sprites[0] = GD.Load("res://Assets/Sprites/grass_dark.png") as Texture;
        sprites[1] = GD.Load("res://Assets/Sprites/grass_light.png") as Texture;

        collisionArea = GetNode<Area2D>("Area2D");
        hoverSprite = GetNode<Sprite>("HoverSprite");
        stateSprite = GetNode<Sprite>("StateSprite");

        if (isBuildBar) hoverSprite.Visible = true;

        collisionArea.Connect("mouse_entered", this, nameof(OnMouseEnter));
        collisionArea.Connect("mouse_exited", this, nameof(OnMouseExit));
        if (!isBuildBar) SetGrassColor();
    }

    public override void _Process(float delta)
    {
        if (!isBuildBar) return;
        if (isAnimatingBuildBar)
        {
            // move position to new position
            Position = new Vector2(Lerp(Position.x, newBuildBarPosition.x, 0.1f), Lerp(Position.y, newBuildBarPosition.y, 0.1f));

            if (Math.Round(Position.x, 2) == newBuildBarPosition.x && Math.Round(Position.y, 2) == newBuildBarPosition.y)
            {
                isAnimatingBuildBar = false;
            }
        }
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
        // if hovered and left mouse button is pressed print hello
        if (hovered && @event.IsActionPressed("ui_accept"))
        {
            OnMouseClick();
        }
    }

    public void OnMouseClick()
    {
        EmitSignal(nameof(OnClick), this);

        if (isBuildBar) return;
        GD.Print("Tile population: " + population + "/" + maxPopulation);
        GD.Print("Tile income: " + income + " every " + incomeOccurance + " seconds");
        GD.Print("Tile cost: " + cost);
        GD.Print("Tile type: " + this.GetType());
    }

    public void Deselect()
    {
        isSelected = false;
    }

    public void Select()
    {
        isSelected = true;
    }

    public void OnMouseExit()
    {
        hovered = false;
        if (!isBuildBar && !isSelected && canHover) hoverSprite.Visible = false;
    }

    public void OnMouseEnter()
    {
        hovered = true;
        if (!isBuildBar && canHover) hoverSprite.Visible = true;
    }

    public void SetGrassColor()
    {
        // set random sprite
        int spriteIndex = rng.Next(0, 2);
        Texture = sprites[spriteIndex];
    }

    public virtual bool AddCitizen(BuildingTypeEnum buildingType)
    {
        if (population < maxPopulation && buildingType == this.buildingType)
        {
            population++;
            CreatePopulationStatusParticle();
            return true;
        }
        return false;
    }

    public void CreatePopulationStatusParticle()
    {
        if (GetNodeOrNull<Particles>("PopulationParticle") != null)
        {
            GetNode<Particles>("PopulationParticle").QueueFree();
        }

        var populationParticle = (CPUParticles2D)populationParticleScene.Instance();
        populationParticle.Position += new Vector2(0, -6);
        populationParticle.Emitting = true;
        AddChild(populationParticle);
    }

    public float Lerp(float firstFloat, float secondFloat, float by)
    {
        return firstFloat * (1 - by) + secondFloat * by;
    }
}
