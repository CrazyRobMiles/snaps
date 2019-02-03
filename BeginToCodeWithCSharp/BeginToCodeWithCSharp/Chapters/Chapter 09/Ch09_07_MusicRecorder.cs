using SnapsLibrary;
using System;

class Ch09_07_MusicRecorder
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
    }

    public void PlaySongNote(SongNote noteToPlay)
    {
        SnapsEngine.PlayNote(noteToPlay.NotePitch, noteToPlay.NoteDuration);
    }

    public void StartProgram()
    {
        SnapsEngine.SetTitleString("Song Recorder");

        // Store the notes in an array
        SongNote[] tune = new SongNote[100];

        // Holds the length of the tune
        int tuneLength = 0;

        // repeatedly read notes until the tune is complete
        for (int tunePos = 0; tunePos < tune.Length; tunePos = tunePos + 1)
        {
            string command = SnapsEngine.SelectFrom2Buttons("New Note", "Play Tune");

            // Quit the loop if the user wants to exit
            if (command == "Play Tune")
            {
                // Record the length of the tune for playback
                tuneLength = tunePos;
                break;
            }

            // Get the note details
            int notePitch = SnapsEngine.ReadInteger("Note Pitch");
            float noteDuration = SnapsEngine.ReadFloat("Note Duration");

            // Create a new note
            SongNote newNote = new SongNote(pitch: notePitch, duration: noteDuration);

            // Store the new note in the tune
            tune[tunePos] = newNote;
        }

        // Play the tune 
        for (int tunePos = 0; tunePos < tuneLength; tunePos++)
        {
            SnapsEngine.PlayNote(pitch: tune[tunePos].NotePitch, duration: tune[tunePos].NoteDuration);
        }
    }
}