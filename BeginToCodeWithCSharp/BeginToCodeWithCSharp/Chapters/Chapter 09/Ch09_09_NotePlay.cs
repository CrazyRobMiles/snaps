using SnapsLibrary;
using System;

class Ch09_09_NotePlay
{
    public struct SongNote
    {
        public int NotePitch;
        public double NoteDuration;

        public SongNote(int pitch, double duration)
        {
            if (pitch < 0 || pitch > 12)
                throw new Exception("Invalid note value");

            if (duration < 0.1 || duration > 1)
                throw new Exception("Invalid duration value");

            NotePitch = pitch;
            NoteDuration = duration;
        }

        public void Play()
        {
            SnapsEngine.PlayNote(pitch: NotePitch, duration: NoteDuration);
        }
    }

    public void StartProgram()
    {
        SnapsEngine.SetTitleString("Twinkle Twinkle");

        SongNote[] twinkleTwinkle = new SongNote[] {
            new SongNote(pitch:0, duration:0.4), new SongNote(pitch:0, duration:0.4),
            new SongNote(pitch:7, duration:0.4), new SongNote(pitch:7, duration:0.4),
            new SongNote(pitch:9, duration:0.4), new SongNote(pitch:9, duration:0.4),
            new SongNote(pitch:7, duration:0.8), new SongNote(pitch:5, duration:0.4),
            new SongNote(pitch:5, duration:0.4), new SongNote(pitch:4, duration:0.4),
            new SongNote(pitch:4, duration:0.4), new SongNote(pitch:2, duration:0.4),
            new SongNote(pitch:2, duration:0.4), new SongNote(pitch:0, duration:0.8)
        };

        foreach (SongNote note in twinkleTwinkle)
        {
            note.Play();
        }
    }
}
