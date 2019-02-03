using SnapsLibrary;

class Ch07_01_UnworkableSales
{
    public void StartProgram()
    {
        int sales1, sales2, sales3, sales4, sales5, sales6, sales7,
            sales8, sales9, sales10;

        sales1 = SnapsEngine.ReadInteger("Enter the sales for stand 1");
        sales2 = SnapsEngine.ReadInteger("Enter the sales for stand 2");
        sales3 = SnapsEngine.ReadInteger("Enter the sales for stand 3");
        sales4 = SnapsEngine.ReadInteger("Enter the sales for stand 4");
        sales5 = SnapsEngine.ReadInteger("Enter the sales for stand 5");
        sales6 = SnapsEngine.ReadInteger("Enter the sales for stand 6");
        sales7 = SnapsEngine.ReadInteger("Enter the sales for stand 7");
        sales8 = SnapsEngine.ReadInteger("Enter the sales for stand 8");
        sales9 = SnapsEngine.ReadInteger("Enter the sales for stand 9");
        sales10 = SnapsEngine.ReadInteger("Enter the sales for stand 10");

        if (sales1 > sales2 && sales1 > sales3 && sales1 > sales4 &&
            sales1 > sales5 && sales1 > sales6 && sales1 > sales7 &&
            sales1 > sales8 && sales1 > sales9 && sales1 > sales10)
        {
            SnapsEngine.DisplayString("Stand 1 had the best sales");
        }
    }
}