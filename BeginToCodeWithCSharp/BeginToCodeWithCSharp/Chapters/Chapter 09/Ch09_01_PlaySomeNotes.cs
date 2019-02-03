using SnapsLibrary;

class Ch09_01_PlaySomeNotes
{
    public void StartProgram()
    {
        SnapsEngine.SetTitleString("Play Three Notes");
        SnapsEngine.PlayNote(pitch:0,duration:0.4);
        SnapsEngine.PlayNote(pitch:2,duration:0.8);
        SnapsEngine.PlayNote(pitch:4,duration:0.4);
    }
}
