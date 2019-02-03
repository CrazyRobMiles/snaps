using SnapsLibrary;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;

class Ch11_11_DrawingDiary
{
    class Drawing
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

    List<Drawing> drawings;

    string SAVE_NAME = "MyDoodles.json";

    void StoreAllDoodles()
    {
        string json = JsonConvert.SerializeObject(drawings);

        SnapsEngine.SaveStringToLocalStorage(itemName: SAVE_NAME, itemValue: json);
    }

    void LoadAllDrawings()
    {
        string json = SnapsEngine.FetchStringFromLocalStorage(SAVE_NAME);

        if (json == null)
        {
            // If we get here there is no string in local storage
            SnapsEngine.WaitForButton("Created empty drawing store. Click here to start.");
            drawings = new List<Drawing>();
        }
        else
        {
            drawings = JsonConvert.DeserializeObject<List<Drawing>>(json);
        }
    }

    void DisplayDrawings()
    {
        foreach (Drawing d in drawings)
        {
            d.ShowStoredGraphics();
            SnapsEngine.Delay(.5);
        }
        SnapsEngine.ClearGraphics();
    }

    private void StoreDrawing()
    {
        Drawing record = new Drawing();
        record.StoreGraphicsNow();
        drawings.Add(record);
        StoreAllDoodles();
        SnapsEngine.ClearGraphics();
    }

    void DisplayHelp()
    {
        SnapsEngine.SetTitleString("Drawing Diary");
        SnapsEngine.DisplayString("Touch the top left hand corner to display the menu");

        SnapsEngine.Delay(3);

        SnapsEngine.SetTitleString("");
        SnapsEngine.DisplayString("");
    }

    void ProcessCommand()
    {
        string command = SnapsEngine.SelectFrom3Buttons("Clear", "Save", "Play");

        switch (command)
        {
            case "Clear":
                SnapsEngine.ClearGraphics();
                break;
            case "Save":
                StoreDrawing();
                break;
            case "Play":
                DisplayDrawings();
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
        LoadAllDrawings();

        SnapsEngine.SetDrawingColor(SnapsColor.Blue);

        DisplayHelp();

        while (true)
        {
            DrawDotsUntilDrawInLeftCorner();

            ProcessCommand();
        }
    }
}

