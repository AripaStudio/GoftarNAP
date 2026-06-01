using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
using goftarNAP.CodePage;

public partial class MainPage : Control
{
    private JsonManager jsonManager;
    private ItemList DocumentsItemList;
    private Button Btn_openDoc;
    private Button Btn_otherInfo;
    private Button Btn_addNewDoc;
    private Button Btn_deleteDoc;
    private FileDialogAp openJsonFileDialog;
    private long SelectedItemID;
    private WindowMessage windowMessage;

    public override void _Ready()
    {
        jsonManager = new JsonManager();
        DocumentsItemList = GetNode<ItemList>("ItemList");
        Btn_addNewDoc = GetNode<Button>("HBoxContainer/BTN_AddNewDoc");
        Btn_openDoc = GetNode<Button>("HBoxContainer/BTN_OpenDoc");
        Btn_otherInfo = GetNode<Button>("HBoxContainer/Btn_OtherInfo");
        Btn_deleteDoc = GetNode<Button>("HBoxContainer/BTN_DeleteDoc");
        openJsonFileDialog = GetNode<FileDialogAp>("FileDialogAP");
        windowMessage = GetNode<WindowMessage>("WindowMessage");

        if (Btn_addNewDoc != null && Btn_openDoc != null && Btn_otherInfo != null && Btn_deleteDoc != null && openJsonFileDialog != null)
        {
            Btn_otherInfo.Pressed += Btn_otherInfoOnPressed;
            Btn_openDoc.Pressed += Btn_openDocOnPressed;
            Btn_addNewDoc.Pressed += Btn_addNewDocOnPressed;
            Btn_deleteDoc.Pressed += Btn_deleteDocOnPressed;

        }

        DocumentsItemList.ItemSelected += DocumentsItemListOnItemSelected;
        openJsonFileDialog.JsonFileValidated += OpenJsonFileDialogOnFileSelected;
        var allItemsOfList = jsonManager.GetAllJsonGoftarNAPdata();
        if (allItemsOfList != null)
        {
            foreach (StructureJsonGoftarNAP structureJsonGoftarNap in allItemsOfList)
            {
                DocumentsItemList.AddItem(structureJsonGoftarNap.name);
            }
        }

    }

    private void OpenJsonFileDialogOnFileSelected(string path)
    {

        var getNameFile = Path.GetFileNameWithoutExtension(path);
        var a = DocumentsItemList.AddItem(getNameFile);
        jsonManager.AddInJsonGoftarNAP(getNameFile, path);

    }


    private void DocumentsItemListOnItemSelected(long index)
    {
        SelectedItemID = index;
    }

    private void Btn_deleteDocOnPressed()
    {
        var getnameSelectedForRemoveID = DocumentsItemList.GetSelectedItems();
        string getnameSelectedForRemoveName = DocumentsItemList.GetItemText(getnameSelectedForRemoveID[0]);
        jsonManager.RemoveFromJsonGoftarNAP(getnameSelectedForRemoveName);
    }

    private void Btn_addNewDocOnPressed()
    {
        openJsonFileDialog.PopupCenteredClamped();

    }

    private void Btn_openDocOnPressed()
    {
        var getItemID = DocumentsItemList.GetSelectedItems();
        if (getItemID.Length == 0) return;
        string getName = DocumentsItemList.GetItemText(getItemID[0]);
        var getJsonGoftarNap = jsonManager.getJsonGoftarNap(getName);
        if (!File.Exists(getJsonGoftarNap.address)) return;
        var jsonContent = File.ReadAllText(getJsonGoftarNap.address);
        List<StructureJsonDocument> docList;
        if (jsonManager.TryParseAndValidateJson(jsonContent, out docList))
        {
            SaveCurrentDocumentData.CurrentDocumentData = docList;
            GetTree().ChangeSceneToFile("res://UIpage/document_page.tscn");
            
        }
    }

    private void Btn_otherInfoOnPressed()
    {
        GetTree().ChangeSceneToFile("res://UIpage/about_me_page.tscn");
        
    }


    public override void _Process(double delta)
    {

    }
}
