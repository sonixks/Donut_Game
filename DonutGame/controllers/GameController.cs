using DonutGame.models;
using DonutGame.views;

namespace DonutGame.controllers;
public class GameController
{
    private readonly GameModel model;
    private readonly GameForm view;
    private int spawnCounter;
    private float scaleFactor = 2.0f;
    private int frameCount;
    private DateTime lastFpsUpdate = DateTime.Now;
    public int CurrentFPS { get; private set; }
    private DateTime lastUpdateTime = DateTime.Now;
    private const int TargetFPS = 60;
    private const double TargetFrameTime = 1000.0 / TargetFPS;
    
    public GameController(GameModel model, GameForm view)
    {
        this.model = model;
        this.view = view;
        model.BaseSpeed = GameModel.InitialBaseSpeed;
    }

    public void Update()
    {
        var currentTime = DateTime.Now;
        lastUpdateTime = currentTime;
        
        frameCount++;
        if ((currentTime - lastFpsUpdate).TotalSeconds >= 1.0)
        {
            CurrentFPS = frameCount;
            frameCount = 0;
            lastFpsUpdate = currentTime;
        }

        if (!model.IsGameOver)
        {
            spawnCounter++;
            if (spawnCounter >= 60 - (model.Level * 5))
            {
                model.SpawnObject();
                spawnCounter = 0;
            }

            model.Update();
        }
        else
        {
            view.End();
            view.Hide();
            var gameOverForm = new GameOverForm(model.Score);
            var result = gameOverForm.ShowDialog();

            if (result == DialogResult.OK)
            {
                model.Initialize();
                view.Show();
            }
            else
                view.Close();
        }

        view.Invalidate();
    }

    public void HandleKeyDown(object sender, KeyEventArgs e)
    {
        if (model.IsGameOver) return;

        const int moveAmount = 15;
        switch (e.KeyCode)
        {
            case Keys.Left:
                model.Basket = model.Basket with { X = Math.Max(0, model.Basket.X - moveAmount) };
                break;
            case Keys.Right:
                model.Basket = model.Basket with { X = Math.Min(model.ScreenWidth - model.Basket.Width, model.Basket.X + moveAmount) };
                break;
        }
    }

    public void HandleMouseMove(object sender, MouseEventArgs e)
    {
        if (model.IsGameOver) return;
        
        var view = (GameForm)sender;
        var scaledX = e.X / view.ScaleX;
        model.Basket = model.Basket with { X = (int)(scaledX - model.Basket.Width / 2) };
    }
}