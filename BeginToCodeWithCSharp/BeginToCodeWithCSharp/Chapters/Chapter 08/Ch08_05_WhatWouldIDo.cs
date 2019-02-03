using SnapsLibrary;

class Ch08_05_WhatWouldIDo
{
    void WhatWouldIDo(int input)
    {
        input = 99;
    }

    public void StartProgram()
    {
        int test = 0;
        WhatWouldIDo(test);
        SnapsEngine.DisplayString("Test is: " + test);
    }
}
