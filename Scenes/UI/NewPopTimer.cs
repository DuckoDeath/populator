using Godot;
using System;

public class NewPopTimer : Timer
{

    private StatusBar statusBar;

    public override void _Ready()
    {
        statusBar = GetParent<StatusBar>();
    }

    public override void _Process(float delta)
    {
        if (Mathf.Round(TimeLeft) == Mathf.Ceil(TimeLeft))
        {
            statusBar.NewPopCountdown((int)this.TimeLeft + 1);
        }
    }
}
