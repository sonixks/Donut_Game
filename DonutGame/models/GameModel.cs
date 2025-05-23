namespace DonutGame.models;

using System.Collections.Generic;
using System.Drawing;

public class GameModel
{
    public int Score { get; private set; }
    public int Lives { get; private set; }
    public int Level { get; private set; }
    public bool IsGameOver { get; private set; }
    public Rectangle Basket { get; set; }
    public List<GameObject> FallingObjects { get; private set; }
    private int ComboCounter { get; set; }
    public int ScreenWidth { get; }
    private int ScreenHeight { get; }
    public int BaseSpeed { get; set; }

    private const int BaseBasketWidth = 100;
    private const int BaseBasketHeight = 60;
    private const int BaseObjectSize = 80;
    public const int InitialBaseSpeed = 3;
    
    public Dictionary<DonutType, Bitmap> DonutImages { get; set; }
    public Bitmap BasketImage { get; set; }
    public Bitmap BackgroundImage { get; set; }

    public GameModel(int screenWidth, int screenHeight)
    {
        ScreenWidth = screenWidth;
        ScreenHeight = screenHeight;
        Initialize();
    }

    public void Initialize()
    {
        Score = 0;
        Lives = 3;
        Level = 1;
        IsGameOver = false;
        Basket = new Rectangle(
            ScreenWidth / 2 - BaseBasketWidth / 2,
            ScreenHeight - BaseBasketHeight - 40,
            BaseBasketWidth, 
            BaseBasketHeight);
        FallingObjects = [];
        ComboCounter = 0;
        BaseSpeed = InitialBaseSpeed;
    }

    public void SpawnObject()
    {
        var random = new Random();
        var x = random.Next(0, ScreenWidth - BaseObjectSize);
        var typeRand = random.Next(100);
        DonutType type;
        var size = BaseObjectSize;
        
        if (typeRand < 15)
        {
            if (Level > 1 && random.Next(100) < 30)
                type = DonutType.Bomb;
            else
                type = DonutType.Junk;
        }
        else 
        {
            var donutType = random.Next(3);
            type = donutType switch
            {
                0 => DonutType.Chocolate,
                1 => DonutType.Strawberry,
                _ => DonutType.Vanilla
            };
        }
        
        var obj = new GameObject(
            type,
            new Rectangle(x, -BaseObjectSize, BaseObjectSize, BaseObjectSize),
            BaseSpeed,
            DonutImages[type]
        );
        
        FallingObjects.Add(obj);
    }

    public void Update()
    {
        for (var i = FallingObjects.Count - 1; i >= 0; i--)
        {
            var obj = FallingObjects[i];
            obj.Bounds = obj.Bounds with { Y = obj.Bounds.Y + obj.Speed };
            
            if (obj.Bounds.IntersectsWith(Basket))
            {
                HandleCollision(obj);
                FallingObjects.RemoveAt(i);
            }
            else if (obj.Bounds.Y > ScreenHeight) 
            {
                if (obj.Type != DonutType.Junk && obj.Type != DonutType.Bomb)
                {
                    Lives--;
                    if (Lives <= 0) IsGameOver = true;
                    ComboCounter = 0;
                }
                FallingObjects.RemoveAt(i);
            }
        }

        if (Score < Level * 250) return;
        Level++;
        BaseSpeed += 1;
    }

    private void HandleCollision(GameObject obj)
    {
        switch (obj.Type)
        {
            case DonutType.Chocolate:
            case DonutType.Strawberry:
            case DonutType.Vanilla:
                Score += 10;
                ComboCounter++;
                if (ComboCounter >= 5)
                {
                    Score += 50; 
                    ComboCounter = 0;
                }
                break;
            
            case DonutType.Junk:
                Score = Math.Max(0, Score - 10);
                ComboCounter = 0;
                break;
            
            case DonutType.Bomb:
                Lives--;
                ComboCounter = 0;
                if (Lives <= 0) IsGameOver = true;
                break;
        }
    }
}