using SnapsLibrary;

class Ch09_05_KeyboardCatAlarm
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
        SnapsEngine.PlayNote(pitch:noteToPlay.Pitch,duration:noteToPlay.Duration);
    }

    public void StartProgram()
    {
        SnapsEngine.SetTitleString("Keyboard Cat Alarm");

        SnapsEngine.DisplayString("Tap the screen to stop the alarm");

        SnapsEngine.ClearScreenTappedFlag();

        while (true)
        {
            SongNote note = RandomSongNote();
            PlaySongNote(note);
            if (SnapsEngine.ScreenHasBeenTapped())
                break;
        }

        SnapsEngine.DisplayString("Alarm cleared");
    }
}
