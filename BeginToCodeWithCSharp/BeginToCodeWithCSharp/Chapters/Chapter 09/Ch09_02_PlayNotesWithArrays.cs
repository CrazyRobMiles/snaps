using SnapsLibrary;

class Ch09_02_PlayNotesWithArrays
{
    public void StartProgram()
    {
        SnapsEngine.SetTitleString("Array Play Three Notes");

        int[] notePitches = new int[3];
        double[] noteDurations = new double[3];

        notePitches[0] = 0; noteDurations[0] = 0.4;
        notePitches[1] = 1; noteDurations[1] = 0.8;
        notePitches[2] = 2; noteDurations[2] = 0.4;

        for (int i = 0; i < 3; i = i + 1)
        {
            SnapsEngine.PlayNote(notePitches[i], noteDurations[i]);
        }
    }
}
