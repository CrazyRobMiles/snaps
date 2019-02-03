using SnapsLibrary;

class Ch09_15_DrawEnum
{
    enum PenModes
    {
        RoundPen,
        SquarePen,
        ErasePen
    };

    public void StartProgram()
    {
        PenModes penType;

        penType = PenModes.SquarePen;

        // Draw a pallette block for selecting a square pen
        SnapsEngine.SetDrawingColor(SnapsColor.Red);
        // draw the block background
        SnapsEngine.DrawBlock(x: 0, y: 0, width: 50, height: 50);
        SnapsEngine.SetDrawingColor(SnapsColor.White);
        // draw a white block to indicate the square pen
        SnapsEngine.DrawBlock(x: 15, y: 15, width: 20, height: 20);

        // Draw a pallette block for selecting a round pen
        SnapsEngine.SetDrawingColor(SnapsColor.Red);
        // draw the block background
        SnapsEngine.DrawBlock(x: 52, y: 0, width: 50, height: 50);
        SnapsEngine.SetDrawingColor(SnapsColor.White);
        // draw a white dot to indicate the round pen
        SnapsEngine.DrawDot(x: 77, y: 25, width: 20);

        // Draw a pallette block for selecting the "erase" pen
        SnapsEngine.SetDrawingColor(SnapsColor.Red);
        // draw the block background
        SnapsEngine.DrawBlock(x: 104, y: 0, width: 50, height: 50);
        SnapsEngine.SetDrawingColor(SnapsColor.White);
        // draw an X that indicates "erase"
        SnapsEngine.DrawLine(x1: 104, y1: 0, x2: 154, y2: 50);
        SnapsEngine.DrawLine(x1: 104, y1: 50, x2: 154, y2: 0);

        SnapsColor backgroundColor = SnapsColor.White;
        SnapsColor drawColor = SnapsColor.Black;

        while (true)
        {
            SnapsCoordinate drawPos = SnapsEngine.GetDraggedCoordinate();

            // See if the draw position is inside the square pen pallette item
            if (drawPos.XValue > 0 && drawPos.XValue < 50 &&
                drawPos.YValue > 0 && drawPos.YValue < 50)
            {
                // in the square pen area
                penType = PenModes.SquarePen;
                continue; // go round the loop again
            }

            // See if the draw position is inside the round pen pallette item
            if (drawPos.XValue > 52 && drawPos.XValue < 102 &&
                drawPos.YValue > 0 && drawPos.YValue < 50)
            {
                // in the round pen area
                penType = PenModes.RoundPen;
                continue; // go round the loop again
            }

            // See if the draw position is inside the erase pen pallette item
            if (drawPos.XValue > 104 && drawPos.XValue < 154 &&
                drawPos.YValue > 0 && drawPos.YValue < 50)
            {
                // in the erase pen area
                penType = PenModes.ErasePen;
                continue; // go round the loop again
            }

            switch (penType)
            {
                case PenModes.RoundPen:
                    SnapsEngine.SetDrawingColor(drawColor);
                    SnapsEngine.DrawDot(drawPos, 20);
                    break;

                case PenModes.SquarePen:
                    SnapsEngine.SetDrawingColor(drawColor);
                    SnapsEngine.DrawBlock(drawPos.XValue, drawPos.YValue, 20, 20);
                    break;

                case PenModes.ErasePen:
                    SnapsEngine.SetDrawingColor(backgroundColor);
                    SnapsEngine.DrawBlock(drawPos.XValue, drawPos.YValue, 20, 20);
                    break;
            }
        }
    }
}