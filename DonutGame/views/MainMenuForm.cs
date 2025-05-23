using System.Reflection;

namespace DonutGame.views;

public partial class MainMenuForm : Form
{
    public MainMenuForm()
    {
        InitializeComponent();
        
        SetupUI();
    }

    private void SetupUI()
    {
        Text = "Donut Catcher - Main Menu";
        ClientSize = new Size(800, 600);
        BackgroundImage = Resources.background_menu;
        BackgroundImageLayout = ImageLayout.Stretch;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        StartPosition = FormStartPosition.CenterScreen;

        var titleText = "DONUT CATCHER";
        var titleFont = new Font("Arial", 36, FontStyle.Bold);
        var textSize = TextRenderer.MeasureText(titleText, titleFont);

        var titleLabel = new Label
        {
            Text = titleText,
            Font = titleFont,
            ForeColor = Color.White,
            Size = textSize, 
            BackColor = Color.Transparent
        };
        titleLabel.Location = new Point(
            (ClientSize.Width - titleLabel.Width) / 2, 
            100); 

        var startButton = new Button
        {
            Text = "START GAME",
            Font = new Font("Arial", 18),
            Size = new Size(200, 50),
            BackColor = Color.LightPink
        };
        startButton.Location = new Point(
            (ClientSize.Width - startButton.Width) / 2,
            250);
        startButton.Click += (s, e) => { Hide(); new GameForm().ShowDialog(); this.Show(); };

        var settingsButton = new Button
        {
            Text = "SETTINGS",
            Font = new Font("Arial", 18),
            Size = new Size(200, 50),
            BackColor = Color.LightPink
        };
        settingsButton.Location = new Point(
            (ClientSize.Width - settingsButton.Width) / 2,
            320);

        var exitButton = new Button
        {
            Text = "EXIT",
            Font = new Font("Arial", 18),
            Size = new Size(200, 50),
            BackColor = Color.LightPink
        };
        exitButton.Location = new Point(
            (ClientSize.Width - exitButton.Width) / 2,
            390);
        exitButton.Click += (s, e) => Application.Exit();

        Controls.Add(titleLabel);
        Controls.Add(startButton);
        Controls.Add(settingsButton);
        Controls.Add(exitButton);
    }
}