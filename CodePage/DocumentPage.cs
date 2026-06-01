using Godot;
using System;
using System.Diagnostics;
using System.Linq;
using goftarNAP.CodePage;
using Environment = Godot.Environment;

public partial class DocumentPage : Control
{
    
    private ItemList ItemListHeadlines;
    private Label lbl_name;
    private RichTextLabel rlbl_description;
    private Button btn_OpenLink;
    private Button btn_SearchOk;
    private TextEdit txt_search;
    private Button btn_openOfList;
    private Button btn_backMainPage;

    private string SaveCurrentLink = "";
    public override void _Ready()
    {
        ItemListHeadlines = GetNode<ItemList>("ItemList_Headlines");
        lbl_name = GetNode<Label>("HBoxContainer/LBL_name");
        rlbl_description = GetNode<RichTextLabel>("ScrollContainer/RLBL_Description");
        btn_OpenLink = GetNode<Button>("HBoxContainer/BTN_LINK");
        txt_search = GetNode<TextEdit>("TXT_Search");
        btn_SearchOk = GetNode<Button>("BTN_SearchOK");
        btn_openOfList = GetNode<Button>("Btn_OpenOfList");
        btn_backMainPage = GetNode<Button>("Btn_backMainpage");

        btn_backMainPage.Pressed += Btn_backMainPageOnPressed;
        btn_openOfList.Pressed += Btn_openOfListOnPressed;
        btn_SearchOk.Pressed += Btn_SearchOkOnPressed;
        btn_OpenLink.Pressed += Btn_OpenLinkOnPressed;
        if (SaveCurrentDocumentData.CurrentDocumentData != null && SaveCurrentDocumentData.CurrentDocumentData.Count > 0)
        {
            var firstDoc = SaveCurrentDocumentData.CurrentDocumentData[0];
            FillingItems(firstDoc.NameSection, firstDoc.CategorySection, firstDoc.descriptionSection, firstDoc.LinkSection);

            foreach (StructureJsonDocument document in SaveCurrentDocumentData.CurrentDocumentData)
            {
                ItemListHeadlines.AddItem(document.NameSection);
            }
        }
        else
        {
            GD.Print("No document data available.");
        }
        SetupRichText();

        
    }
    private void SetupRichText()
    {
        rlbl_description.BbcodeEnabled = true;
        rlbl_description.AutowrapMode = TextServer.AutowrapMode.WordSmart;

        rlbl_description.FitContent = false;                   
        rlbl_description.ScrollActive = true;

        rlbl_description.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
        rlbl_description.SizeFlagsVertical = Control.SizeFlags.ExpandFill | Control.SizeFlags.Fill;

        rlbl_description.CustomMinimumSize = new Vector2(420, 280);  

        rlbl_description.SizeFlagsStretchRatio = 3;
    }
    private void Btn_SearchOkOnPressed()
    {
        string GetInputText = txt_search.Text;
        if (string.IsNullOrEmpty(GetInputText)) return;
        var foundDoc = SaveCurrentDocumentData.CurrentDocumentData
            .FirstOrDefault(d => d.NameSection.Equals(GetInputText, StringComparison.OrdinalIgnoreCase));

        if (foundDoc != null)
        {
            FillingItems(foundDoc.NameSection, foundDoc.CategorySection, foundDoc.descriptionSection, foundDoc.LinkSection);
        }
    }

    private void Btn_OpenLinkOnPressed()
    {
        if (string.IsNullOrEmpty(SaveCurrentLink)) return;
        Process.Start(new ProcessStartInfo()
        {
            FileName = SaveCurrentLink,
            UseShellExecute = true
        });
    }

    private void Btn_openOfListOnPressed()
    {

        var getid = ItemListHeadlines.GetSelectedItems();
        if (getid.Length == 0) return;
        var getname = ItemListHeadlines.GetItemText(getid[0]);
        var foundDoc = SaveCurrentDocumentData.CurrentDocumentData.FirstOrDefault(d =>
            d.NameSection.Equals(getname, StringComparison.OrdinalIgnoreCase));
        if (foundDoc != null)
        {
            FillingItems(foundDoc.NameSection, foundDoc.CategorySection, foundDoc.descriptionSection, foundDoc.LinkSection);
        }


    }

    private void Btn_backMainPageOnPressed()
    {
        GetTree().ChangeSceneToFile("res://UIpage/main_page.tscn");
    }


    private void FillingItems(string name, string category, string description, string link)
    {
        lbl_name.Text =
            $"Name : {name} || Category : {category}";
       rlbl_description.Text = $"Description : \n {description}";
        SaveCurrentLink = link;
    }
   
}
