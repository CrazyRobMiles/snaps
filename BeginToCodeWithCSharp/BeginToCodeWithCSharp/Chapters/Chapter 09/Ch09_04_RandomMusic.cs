using SnapsLibrary;

class Ch09_04_RandomMusic
{
    public struct SongNote
    {
        public int Pitch;
        public double Duration;
    }

    public SongNote RandomSongNote()
    {
        SongNote result;
        result.Pitch = SnapsEngine.ThrowDice();
        result.Duration = SnapsEngine.ThrowDice() / 10.0;
        return result;
    }

    public void PlaySongNote(SongNote noteToPlay)
    {
        SnapsEngine.PlayNote(pitch:noteToPlay.Pitch, duration:noteToPlay.Duration);
    }

    public void StartProgram()
    {
        

        for (int i = 0; i < 20; i = i + 1)
        {
            SongNote note = RandomSongNote();
            PlaySongNote(note);
        }
    }
}