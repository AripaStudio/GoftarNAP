using Godot;
using System;
using System.IO;
using System.Text.Json;

public partial class FileDialogAp : FileDialog
{
    [Signal]
    public delegate void JsonFileValidatedEventHandler(string path);
    private string GetPathString;
    private bool everythingIsTrue = false;
    public bool JsonSelected = false;

    public struct GetPathFileStruct
    {
        public string path;
        public bool Error;
    }

    public override void _EnterTree()
    {
        
        FileSelected += OnFileSelectedDialog;
        Canceled += OnCanceledDialog;
    }

    private void OnCanceledDialog()
    {
        everythingIsTrue = false;
    }

    private void OnFileSelectedDialog(string path)
    {
        //(: slide
        if (File.Exists(path))
        {
            
            if (Path.GetExtension(path).ToLower() == ".json")
            {
                
                if (IsValidJson(path))
                {
                    
                    GetPathString = path;
                    everythingIsTrue = true;
                    JsonSelected = true;

                    EmitSignal(SignalName.JsonFileValidated, path);
                    return;
                }
            }
        }
        else
        {
            
            everythingIsTrue = false;
        }

        
    }

    private bool IsValidJson(string path)
    {
        try
        {
            string jsonText = File.ReadAllText(path);
            using (JsonDocument doc = JsonDocument.Parse(jsonText))
            {
                return true;
            }
        }
        catch (Exception)
        {
            return false;
        }
    }

    public GetPathFileStruct GetPathFile()
    {
        return new GetPathFileStruct() { path = GetPathString, Error = everythingIsTrue };
    }

    public bool GetJsonSelected()
    {
        return JsonSelected;
    }


}
