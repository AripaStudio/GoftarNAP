using Godot;
using System;
using System.Diagnostics;

public partial class AboutMePage : Control
{

    private Button btn_BackToMainPage;
	private Button btn_OpenTelegram;
	private Button btn_OpenGithub;
	public override void _Ready()
    {
        btn_BackToMainPage = GetNode<Button>("Btn_Back");
        btn_OpenGithub = GetNode<Button>("StackPanel_Main/DockPanel_Buttons/BTN_openGithubLink");
        btn_OpenTelegram = GetNode<Button>("StackPanel_Main/DockPanel_Buttons/BTN_openTelegramLink");

        btn_BackToMainPage.Pressed += Btn_BackToMainPageOnPressed;
        btn_OpenGithub.Pressed += Btn_OpenGithubOnPressed;
        btn_OpenTelegram.Pressed += Btn_OpenTelegramOnPressed;
    }

    private void Btn_OpenTelegramOnPressed()
    {
        Process.Start(new ProcessStartInfo()
        {
            FileName = "https://t.me/AripaStudio",
            UseShellExecute = true
        });
    }

    private void Btn_OpenGithubOnPressed()
    {
        Process.Start(new ProcessStartInfo()
        {
            FileName = "https://github.com/AripaStudio",
            UseShellExecute = true
        });
    }

    private void Btn_BackToMainPageOnPressed()
    {
        
        GetTree().ChangeSceneToFile("res://UIpage/main_page.tscn");
    }

    
}
