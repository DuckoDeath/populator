using Godot;
using System;

public class StatusBar : CanvasLayer
{
    private RichTextLabel moneyLabel;
    private RichTextLabel populationLabel;
    private RichTextLabel newPopLabel;

    public override void _Ready()
    {
        moneyLabel = GetNode<RichTextLabel>("MoneyLabel");
        populationLabel = GetNode<RichTextLabel>("PopLabel");
        newPopLabel = GetNode<RichTextLabel>("NewPopLabel");
    }

    public void SetMoney(int money)
    {
        moneyLabel.Text = "Coins: " + money.ToString();
    }

    public void SetPopulation(int population, int maxPopulation)
    {
        populationLabel.Text = "Pop: " + population.ToString() + "/" + maxPopulation.ToString();
    }

    public void NewPopCountdown(int countdown)
    {
        newPopLabel.BbcodeText = "[right]New citizen:[/right]\n[right]" + countdown + " seconds[/right]";
    }
}
