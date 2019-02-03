using SnapsLibrary;

class Ch09_03_PlayNotesFromStructureArray
{
    public struct SongNote
    {
        public int NotePitch;
        public double NoteDuration;
    }

    public void StartProgram()
    {
        SongNote [] notes = new SongNote[3];
        notes[0].NotePitch = 0; notes[0].NoteDuration = 0.4;
        notes[1].NotePitch = 2; notes[1].NoteDuration = 0.8;
        notes[2].NotePitch = 4; notes[2].NoteDuration = 0.4;

        for (int i = 0; i < 3; i = i + 1)
        {
            SnapsEngine.PlayNote(pitch:notes[i].NotePitch,duration:notes[i].NoteDuration);
        }
    }
}
