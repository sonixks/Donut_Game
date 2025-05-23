using System.Reflection;
using DonutGame.controllers;
using DonutGame.models;
using Timer = System.Windows.Forms.Timer;

namespace DonutGame.views;

public partial class GameForm : Form
{
    private GameModel model;
    private GameController controller;
    private Timer gameTimer;
    private const int BaseWidth = 800;
    private const int BaseHeight = 600;
    private const int MinWidth = 600;
    private const int MinHeight = 450;
    private float scaleX = 1.0f;
    private float scaleY = 1.0f;
    public float ScaleX => scaleX;
    public float ScaleY => scaleY;
    private Dictionary<DonutType, Bitmap> scaledDonutImages = new();
    private Bitmap scaledBasketImage;
    private Bitmap scaledBackgroundImage;
    private bool needRescaleImages = true;
    
    private static Dictionary<string, Bitmap> _imageCache = new();

    public GameForm()
    {
        InitializeComponent();
        SetupGame();
    }

    private void SetupGame()
    {
        Text = "Donut Catcher";
        ClientSize = new Size(BaseWidth, BaseHeight);
        MinimumSize = new Size(MinWidth, MinHeight);
        DoubleBuffered = true;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = true;
        StartPosition = FormStartPosition.CenterScreen;
        
        var background = Resources.background;
        var donut1 = Resources.donut1;
        var donut2 = Resources.donut2;
        var donut3 = Resources.donut3;
        var garbage = Resources.garbage;
        var bomb = Resources.bomb;
        var basket = Resources.basket;
        model = new GameModel(BaseWidth, BaseHeight)
        {
            DonutImages = new Dictionary<DonutType, Bitmap>
            {
                { DonutType.Chocolate, donut1 },
                { DonutType.Strawberry, donut2 },
                { DonutType.Vanilla, donut3 },
                { DonutType.Junk, garbage },
                { DonutType.Bomb, bomb }
            },
            BasketImage = basket,
            BackgroundImage = background
        };
        
        controller = new GameController(model, this);

        gameTimer = new Timer { Interval = 1 };
        gameTimer.Tick += (s, e) => controller.Update();
        gameTimer.Start();

        KeyDown += controller.HandleKeyDown;
        MouseMove += controller.HandleMouseMove;
        Paint += GameForm_Paint;
        Resize += GameForm_Resize;

        CalculateScaleFactors();
        PreScaleImages();
    }
    
    private void GameForm_Resize(object sender, EventArgs e)
    {
        CalculateScaleFactors();
        needRescaleImages = true;
        Invalidate();
    }

    private void CalculateScaleFactors()
    {
        scaleX = (float)ClientSize.Width / BaseWidth;
        scaleY = (float)ClientSize.Height / BaseHeight;
    }
    
    private void PreScaleImages()
    {
        if (!needRescaleImages) return;
        
        scaledBackgroundImage = new Bitmap(model.BackgroundImage, ClientSize.Width, ClientSize.Height);
        
        foreach (var kvp in model.DonutImages)
        {
            var original = kvp.Value;
            var scaledWidth = (int)(original.Width * scaleX);
            var scaledHeight = (int)(original.Height * scaleY);
            scaledDonutImages[kvp.Key] = new Bitmap(original, scaledWidth, scaledHeight);
        }
        
        var basket = model.BasketImage;
        scaledBasketImage = new Bitmap(basket, (int)(basket.Width * scaleX), (int)(basket.Height * scaleY));
        
        needRescaleImages = false;
    }

    private void GameForm_Paint(object sender, PaintEventArgs e)
    {
        PreScaleImages();
        
        var g = e.Graphics;
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
        g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
        
        DrawBackground(g);
        DrawFallingObjects(g);
        DrawBasket(g);
        DrawUI(g);
        DrawGameOver(g);
    }

    private void DrawBackground(Graphics g)
    {
        g.DrawImage(scaledBackgroundImage, 0, 0);
    }

    private void DrawFallingObjects(Graphics g)
    {
        foreach (var obj in model.FallingObjects)
        {
            var bounds = ScaleRectangle(obj.Bounds);
            if (scaledDonutImages.TryGetValue(obj.Type, out var scaledImage))
            {
                g.DrawImage(scaledImage, bounds);
            }
        }
    }

    private void DrawBasket(Graphics g)
    {
        var basketBounds = ScaleRectangle(model.Basket);
        g.DrawImage(scaledBasketImage, basketBounds);
    }

    private void DrawUI(Graphics g)
    {
        var fontSize = 16 * Math.Min(scaleX, scaleY);
        var font = new Font("Arial", fontSize);

        var textX = 10 * scaleX;
        var startY = 10 * scaleY;
        var lineSpacing = 30 * scaleY;

        string[] labels = {
            $"Score: {model.Score}",
            $"Lives: {model.Lives}",
            $"Level: {model.Level}",
            $"FPS: {controller.CurrentFPS}"
        };

        float maxWidth = 0;
        float totalHeight = 0;

        foreach (var label in labels)
        {
            var size = g.MeasureString(label, font);
            maxWidth = Math.Max(maxWidth, size.Width);
            totalHeight += size.Height;
        }
        totalHeight += (labels.Length - 1) * (lineSpacing - font.Height);

        float padding = 10;
        var backgroundBrush = new SolidBrush(Color.FromArgb(150, 0, 0, 0));
        var backgroundRect = new RectangleF(
            textX - padding / 2,
            startY - padding / 2,
            maxWidth + padding,
            totalHeight + padding
        );

        g.FillRectangle(backgroundBrush, backgroundRect);

        var currentY = startY;
        foreach (var label in labels)
        {
            g.DrawString(label, font, Brushes.White, textX, currentY);
            currentY += lineSpacing;
        }
    }

    private void DrawGameOver(Graphics g)
    {
        if (!model.IsGameOver) return;

        var fontSize = 48 * Math.Min(scaleX, scaleY);
        var font = new Font("Arial", fontSize, FontStyle.Bold);
        const string text = "GAME OVER";
        var size = g.MeasureString(text, font);
        g.DrawString(text, font, Brushes.Red,
            (ClientSize.Width - size.Width) / 2,
            (ClientSize.Height - size.Height) / 2);
    }

    public void End()
    {
        gameTimer.Stop();
    }

    private Rectangle ScaleRectangle(Rectangle rect)
    {
        return new Rectangle(
            (int)(rect.X * scaleX),
            (int)(rect.Y * scaleY),
            (int)(rect.Width * scaleX),
            (int)(rect.Height * scaleY));
    }
}