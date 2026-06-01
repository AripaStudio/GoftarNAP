using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

public partial class JsonManager : Node
{
    public string NameJsonFile = "GoftarNAPjson.json";
    public void AddInJsonGoftarNAP(string name, string address)
    {
        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(address))
        {
            return;
        }

        var CurrentList = GetAllJsonGoftarNAPdata();
        if (CurrentList.Any(x => x.name.ToLower() == name.ToLower()))
        {
            return;
        }
        StructureJsonGoftarNAP newJsonGoftarNap = new StructureJsonGoftarNAP() { name = name, address = address };
        CurrentList.Add(newJsonGoftarNap);
        var options = new JsonSerializerOptions { WriteIndented = true };
        using (FileStream fileStream = File.Create(NameJsonFile))
        {
            JsonSerializer.Serialize(fileStream, CurrentList, options);
        }
    }

    public void RemoveFromJsonGoftarNAP(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return;
        }

        var CurrentList = GetAllJsonGoftarNAPdata();
        var itemToRemove = CurrentList.FirstOrDefault(x => x.name.ToLower() == name.ToLower());
        if (itemToRemove != null)
        {
            CurrentList.Remove(itemToRemove);
        }

        var options = new JsonSerializerOptions { WriteIndented = true };
        using (FileStream fileStream = File.Create(NameJsonFile))
        {
            JsonSerializer.Serialize(fileStream, CurrentList, options);
        }

    }

    public List<StructureJsonGoftarNAP> GetAllJsonGoftarNAPdata()
    {
        if (!File.Exists(NameJsonFile))
        {
            return new List<StructureJsonGoftarNAP>();
        }
        using (FileStream fileStream = File.OpenRead(NameJsonFile))
        {
            var list = JsonSerializer.Deserialize<List<StructureJsonGoftarNAP>>(fileStream);
            return list ?? new List<StructureJsonGoftarNAP>();
        }
    }

    public StructureJsonGoftarNAP getJsonGoftarNap(string name)
    {
        var list = GetAllJsonGoftarNAPdata();
        return list.FirstOrDefault(x => x.name.ToLower() == name.ToLower()) ?? new StructureJsonGoftarNAP();
    }

    public bool TryParseAndValidateJson(string jsonContent, out List<StructureJsonDocument> parsedList)
    {
        parsedList = null;

        if (string.IsNullOrWhiteSpace(jsonContent))
        {
            return false;
        }

        try
        {
            using JsonDocument document = JsonDocument.Parse(jsonContent);

            if (document.RootElement.ValueKind != JsonValueKind.Array)
            {
                return false;
            }

            parsedList = JsonSerializer.Deserialize<List<StructureJsonDocument>>(jsonContent);

            return parsedList != null;
        }
        catch
        {
            return false;
        }
    }



}

public class StructureJsonGoftarNAP
{
    [ForeignKey("name")]
    public string name { get; set; }
    public string address { get; set; }
}


public class StructureJsonDocument
{

    [JsonPropertyName("name")]
    public string NameSection { get; set; }
    [JsonPropertyName("category")]
    public string CategorySection { get; set; }
    [JsonPropertyName("description")]
    public string descriptionSection { get; set; }
    [JsonPropertyName("link")]
    public string LinkSection { get; set; }
}