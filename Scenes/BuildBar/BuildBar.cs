using Godot;
using System;
using System.Collections.Generic;

public class BuildBar : Node
{
    public bool active = false;

    PackedScene HouseTileScene = GD.Load<PackedScene>("res://Scenes/Tiles/House/HouseTile.tscn");
    PackedScene WindmillTileScene = GD.Load<PackedScene>("res://Scenes/Tiles/Windmill/WindmillTile.tscn");
    PackedScene TreeTileScene = GD.Load<PackedScene>("res://Scenes/Tiles/Tree/TreeTile.tscn");

    private List<Tile> tiles = new List<Tile>();

    public int selectedIdx = 0;

    public int spriteSize = 16;
    public int spritePadding = 2;
    public int selectedYOffset = 2;

    public int hideDepth = 32;

    public bool isAnimating = false;

    public override void _Ready()
    {
        InitializeElements();
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
    }

    public bool ToggleActive()
    {
        if (active)
        {
            active = false;
            SetDisabledPositions();
            return false;
        }
        else
        {
            active = true;
            SetEnabledPositions();
            return true;
        }
    }

    void InitializeElements()
    {
        // add tiles
        var houseTile = HouseTileScene.Instance() as Tile;
        CreateNewElement(houseTile);

        var windmillTile = WindmillTileScene.Instance() as Tile;
        CreateNewElement(windmillTile);

        var treeTile = TreeTileScene.Instance() as Tile;
        CreateNewElement(treeTile);

        SetDisabledPositions();
    }

    public void CreateNewElement(Tile tile)
    {
        tile.isBuildBar = true;
        tile.Visible = false;
        tiles.Add(tile);
        tile.Connect("OnClick", this, nameof(OnElementSelected));
        AddChild(tile);
    }

    public void SetNewPositions(int idx)
    {
        Vector2 size = GetViewport().GetVisibleRect().Size;
        // before idx
        for (int i = 0; i < idx; i++)
        {
            tiles[i].newBuildBarPosition = new Vector2(size.x / 2 - (idx - i) * spriteSize - (spritePadding * (idx - i)), 5 * (size.y) / 6 + 2);
            tiles[i].isAnimatingBuildBar = true;
        }

        // idx
        tiles[idx].newBuildBarPosition = new Vector2(size.x / 2, 5 * (size.y) / 6 - selectedYOffset + 2);
        tiles[idx].isAnimatingBuildBar = true;

        // after idx
        for (int i = idx + 1; i < tiles.Count; i++)
        {
            tiles[i].newBuildBarPosition = new Vector2(size.x / 2 + ((i - idx) * spriteSize) + (spritePadding * (i - idx)), 5 * (size.y) / 6 + 2);
            tiles[i].isAnimatingBuildBar = true;
        }

        isAnimating = true;
        selectedIdx = idx;
    }

    public void SetDisabledPositions()
    {
        Vector2 size = GetViewport().GetVisibleRect().Size;

        /*
            just add 200 to the y value and get it off screen
        */

        // before idx
        for (int i = 0; i < selectedIdx; i++)
        {
            tiles[i].newBuildBarPosition = new Vector2(size.x / 2 - (selectedIdx - i) * spriteSize - (spritePadding * (selectedIdx - i)), 5 * (size.y) / 6 + 2 + hideDepth);
            tiles[i].isAnimatingBuildBar = true;
        }

        // idx
        tiles[selectedIdx].newBuildBarPosition = new Vector2(size.x / 2, 5 * (size.y) / 6 - selectedYOffset + 2 + hideDepth);
        tiles[selectedIdx].isAnimatingBuildBar = true;

        // after idx
        for (int i = selectedIdx + 1; i < tiles.Count; i++)
        {
            tiles[i].newBuildBarPosition = new Vector2(size.x / 2 + ((i - selectedIdx) * spriteSize) + (spritePadding * (i - selectedIdx)), 5 * (size.y) / 6 + 2 + hideDepth);
            tiles[i].isAnimatingBuildBar = true;
        }

        isAnimating = true;
    }

    public void SetEnabledPositions()
    {
        Vector2 size = GetViewport().GetVisibleRect().Size;

        /*
            get rid of the extra 200 in the y
        */

        // before idx
        for (int i = 0; i < selectedIdx; i++)
        {
            tiles[i].newBuildBarPosition = new Vector2(size.x / 2 - (selectedIdx - i) * spriteSize - (spritePadding * (selectedIdx - i)), 5 * (size.y) / 6 + 2);
            tiles[i].isAnimatingBuildBar = true;
            tiles[i].Visible = true;
        }

        // idx
        tiles[selectedIdx].newBuildBarPosition = new Vector2(size.x / 2, 5 * (size.y) / 6 - selectedYOffset + 2);
        tiles[selectedIdx].isAnimatingBuildBar = true;
        tiles[selectedIdx].Visible = true;

        // after idx
        for (int i = selectedIdx + 1; i < tiles.Count; i++)
        {
            tiles[i].newBuildBarPosition = new Vector2(size.x / 2 + ((i - selectedIdx) * spriteSize) + (spritePadding * (i - selectedIdx)), 5 * (size.y) / 6 + 2);
            tiles[i].isAnimatingBuildBar = true;
            tiles[i].Visible = true;
        }

        isAnimating = true;
    }

    void OnElementSelected(Tile tile)
    {
        SetNewPositions(tiles.IndexOf(tile));
    }
}
