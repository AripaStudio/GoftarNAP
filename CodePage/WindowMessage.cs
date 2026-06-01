using Godot;
using System;

public partial class WindowMessage : Window
{
    private Button Btn_OK;
	public override void _Ready()
    {
        Btn_OK = GetNode<Button>("BTN_Ok");
        Btn_OK.Pressed += Btn_OKOnPressed;
    }

    private void Btn_OKOnPressed()
    {
        this.Visible = false;
    }

    
}
