namespace DonutGame.models;

public enum DonutType
{
    Chocolate,
    Strawberry,
    Vanilla,
    Junk,
    Bomb
}

public class GameObject(DonutType type, Rectangle bounds, int speed, Bitmap image)
{
    public DonutType Type { get; } = type;
    public Rectangle Bounds { get; set; } = bounds;
    public int Speed { get; } = speed;
    public Bitmap Image { get; } = image;
}