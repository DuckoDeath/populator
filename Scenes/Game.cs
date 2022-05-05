using Godot;
using System;

public class Game : Node
{
    Tile[,] board = new Tile[4, 4];

    int boardOffsetX = 56;
    int boardOffsetY = 30;

    public int population = 0;
    public int maxPopulation = 0;

    public int money = 50;

    public int citizenQueue = 0;

    public Tile selectedTile = null;

    PackedScene tileScene;
    PackedScene houseScene;
    PackedScene windmillScene;
    PackedScene treeScene;

    StatusBar statusBar;

    BuildBar buildBar;

    NewPopTimer newPopTimer;

    Random rng = new Random();

    public override void _Ready()
    {
        tileScene = (PackedScene)ResourceLoader.Load("res://Scenes/Tiles/Tile.tscn");
        houseScene = (PackedScene)ResourceLoader.Load("res://Scenes/Tiles/House/HouseTile.tscn");
        windmillScene = (PackedScene)ResourceLoader.Load("res://Scenes/Tiles/Windmill/WindmillTile.tscn");
        treeScene = (PackedScene)ResourceLoader.Load("res://Scenes/Tiles/Tree/TreeTile.tscn");

        statusBar = GetNode<StatusBar>("StatusBar");
        buildBar = GetNode<BuildBar>("BuildBar");

        newPopTimer = statusBar.GetNode<NewPopTimer>("NewPopTimer");
        newPopTimer.Connect("timeout", this, nameof(AddNewCitizen));
        GetNewCitizen();

        CreateNewBoard();

        ProcessEachTile((Tile tile, int x, int y) =>
        {
            maxPopulation += tile.maxPopulation;
        });

        statusBar.SetPopulation(population, maxPopulation);
    }

    public void CreateNewBoard()
    {
        ProcessEachTile((Tile tile, int x, int y) =>
        {
            var newTile = (Tile)tileScene.Instance();
            newTile.Position = new Vector2(x * 16 + boardOffsetX, y * 16 + boardOffsetY);
            AddChild(newTile);
            board[x, y] = newTile;
        });

        addStartingTrees();
        addStartingHouse();
        addStartingWindmill();
    }

    public void addStartingTrees()
    {
        for (int i = 0; i < 2; i++)
        {
            // add a tree
            int x = rng.Next(0, 4);
            int y = rng.Next(0, 4);

            while (board[x, y] is TreeTile)
            {
                x = rng.Next(0, 4);
                y = rng.Next(0, 4);
            }

            var newTree = (Tile)treeScene.Instance();
            newTree.Position = new Vector2(x * 16 + boardOffsetX, y * 16 + boardOffsetY);
            AddChild(newTree);
            board[x, y].QueueFree();
            board[x, y] = newTree;
        }
    }

    public void addStartingHouse()
    {
        AddStartingTile(houseScene);
    }

    public void addStartingWindmill()
    {
        AddStartingTile(windmillScene);
    }

    public void AddStartingTile(PackedScene scene)
    {
        // add a tile
        int x = rng.Next(0, 4);
        int y = rng.Next(0, 4);

        while ((board[x, y].GetType() != typeof(Tile)))
        {
            x = rng.Next(0, 4);
            y = rng.Next(0, 4);
        }

        var newTile = (Tile)scene.Instance();
        newTile.Position = new Vector2(x * 16 + boardOffsetX, y * 16 + boardOffsetY);
        newTile.Connect(nameof(Tile.MadeMoney), this, nameof(OnMadeMoney));
        newTile.Connect(nameof(Tile.OnClick), this, nameof(OnTileClick));
        AddChild(newTile);
        board[x, y].QueueFree();
        board[x, y] = newTile;
    }

    public void GetNewCitizen()
    {
        newPopTimer.WaitTime = 3;
        newPopTimer.Start();
    }

    public void AddNewCitizen()
    {
        GD.Print("Adding new citizen");
        if (AddCitizenToBuilding(Tile.BuildingTypeEnum.Production))
        {
            GD.Print("Added citizen to production building");
        }
        else if (AddCitizenToBuilding(Tile.BuildingTypeEnum.Housing))
        {
            GD.Print("Added citizen to house");
        }
        else
        {
            GD.Print("No avaliable buildings");
            citizenQueue++;
        }
        CalculateCurrentPopulation();
    }

    public bool AddCitizenToBuilding(Tile.BuildingTypeEnum buildingType)
    {
        bool foundBuilding = false;
        ProcessEachTile((Tile tile, int x, int y) =>
        {
            if (!foundBuilding && tile.AddCitizen(buildingType))
            {
                foundBuilding = true;
            }
        });
        return foundBuilding;
    }

    public void CalculateCurrentPopulation()
    {
        int newPopulation = 0;
        ProcessEachTile((Tile tile, int x, int y) =>
        {
            newPopulation += tile.population;
        });
        population = newPopulation;
        statusBar.SetPopulation(population, maxPopulation);
    }

    public void OnMadeMoney(int income)
    {
        money += income;
        statusBar.SetMoney(money);
    }

    public void OnTileClick(Tile newSelectedTile)
    {
        buildBar.ToggleActive();
        if (selectedTile == null)
        {
            selectedTile = newSelectedTile;
            selectedTile.Select();
        }
        else if (selectedTile == newSelectedTile)
        {
            selectedTile.Deselect();
            selectedTile = null;
        }
        else
        {
            selectedTile.Deselect();
            selectedTile = newSelectedTile;
        }

        ProcessEachTile((Tile tile, int x, int y) =>
        {
            if (tile != selectedTile) tile.canHover = false;
            if (selectedTile == null) tile.canHover = true;
            GD.Print(selectedTile);
        });
    }

    public delegate void EachTileDelegate(Tile tile, int x, int y);
    public void ProcessEachTile(EachTileDelegate del)
    {
        for (int x = 0; x < 4; x++)
        {
            for (int y = 0; y < 4; y++)
            {
                del(board[x, y], x, y);
            }
        }
    }
}