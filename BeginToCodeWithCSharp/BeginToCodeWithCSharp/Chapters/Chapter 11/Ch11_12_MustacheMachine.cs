using SnapsLibrary;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;

class Ch11_12_MustacheMachine
{
    class MustachePicture
    {
        public DateTime date;

        private string fileName
        {
            get
            {
                return date.ToFileTime().ToString();
            }
        }

        public void StoreGraphicsNow()
        {
            date = DateTime.Now;
            SnapsEngine.SaveGraphicsImageToLocalStoreAsPNG(fileName);
        }

        public void ShowStoredGraphics()
        {
            SnapsEngine.LoadGraphicsPNGImageFromLocalStore(fileName);
        }
    }

    List<MustachePicture> rogueImages;

    string SAVE_NAME = "MyRogues.json";

    void StoreAllRogues()
    {
        string json = JsonConvert.SerializeObject(rogueImages);

        SnapsEngine.SaveStringToLocalStorage(itemName: SAVE_NAME, itemValue: json);
    }

    void LoadAllRogues()
    {
        string json = SnapsEngine.FetchStringFromLocalStorage(SAVE_NAME);

        if (json == null)
        {
            // If we get here there is no string in local storage
            SnapsEngine.WaitForButton("Created empty rogues gallery, click here to get started");
            rogueImages = new List<MustachePicture>();
        }
        else
        {
            rogueImages = JsonConvert.DeserializeObject<List<MustachePicture>>(json);
        }
    }

    void DisplayRoguesGallery()
    {
        foreach (MustachePicture d in rogueImages)
        {
            d.ShowStoredGraphics();
            SnapsEngine.Delay(.5);
        }
        SnapsEngine.ClearGraphics();
    }

    private void StorePicture()
    {
        MustachePicture record = new MustachePicture();
        record.StoreGraphicsNow();
        rogueImages.Add(record);
        StoreAllRogues();
        SnapsEngine.ClearGraphics();
    }

    void DisplayHelp()
    {
        SnapsEngine.SetTitleString("Mustache Machine");
        SnapsEngine.DisplayString("Touch the top left hand corner to display the menu");

        SnapsEngine.Delay(3);

        SnapsEngine.SetTitleString("");
        SnapsEngine.DisplayString("");

        SnapsEngine.TakePhotograph();
    }

    void ProcessCommand()
    {
        string command = SnapsEngine.SelectFrom3Buttons("Photo", "Save", "Play");

        switch (command)
        {
            case "Photo":
                SnapsEngine.TakePhotograph();
                break;
            case "Save":
                StorePicture();
                break;
            case "Play":
                DisplayRoguesGallery();
                break;
        }
    }

    void DrawDotsUntilDrawInLeftCorner()
    {
        while (true)
        {
            SnapsCoordinate drawPos = SnapsEngine.GetDraggedCoordinate();
            if (drawPos.XValue < 50 && drawPos.YValue < 50)
            {
                break;
            }
            SnapsEngine.DrawDot(pos: drawPos, width: 20);
        }
    }

    public void StartProgram()
    {
        

        LoadAllRogues();

        SnapsEngine.SetDrawingColor(SnapsColor.Blue);

        DisplayHelp();

        while (true)
        {
            DrawDotsUntilDrawInLeftCorner();

            ProcessCommand();
        }
    }
}

