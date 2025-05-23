namespace DonutGame.views;

public partial class GameOverForm : Form
{
    public GameOverForm(int score)
    {
        InitializeComponent();
        SetupUI(score);
    }

    private void SetupUI(int score)
    {
        Text = "Game Over";
        ClientSize = new Size(400, 300);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        BackColor = Color.AntiqueWhite;
        StartPosition = FormStartPosition.CenterScreen;

        var titleLabel = new Label
        {
            Text = "GAME OVER",
            Font = new Font("Arial", 24, FontStyle.Bold),
            AutoSize = true
        };

        var scoreLabel = new Label
        {
            Text = $"Your score: {score}",
            Font = new Font("Arial", 18),
            AutoSize = true
        };

        var restartButton = new Button
        {
            Text = "PLAY AGAIN",
            Font = new Font("Arial", 14),
            Size = new Size(150, 40),
            BackColor = Color.LightPink
        };

        var menuButton = new Button
        {
            Text = "MAIN MENU",
            Font = new Font("Arial", 14),
            Size = new Size(150, 40),
            BackColor = Color.LightPink
        };

        // Центрирование после добавления всех элементов
        Controls.Add(titleLabel);
        Controls.Add(scoreLabel);
        Controls.Add(restartButton);
        Controls.Add(menuButton);

        // Центрировать по горизонтали вручную
        titleLabel.Location = new Point((ClientSize.Width - titleLabel.PreferredWidth) / 2, 40);
        scoreLabel.Location = new Point((ClientSize.Width - scoreLabel.PreferredWidth) / 2, 100);
        restartButton.Location = new Point((ClientSize.Width - restartButton.Width) / 2, 160);
        menuButton.Location = new Point((ClientSize.Width - menuButton.Width) / 2, 210);

        // Подключение кнопок
        restartButton.Click += (s, e) =>
        {
            DialogResult = DialogResult.OK;
            Close();
        };

        menuButton.Click += (s, e) =>
        {
            DialogResult = DialogResult.Cancel;
            Close();
        };
    }

}
    