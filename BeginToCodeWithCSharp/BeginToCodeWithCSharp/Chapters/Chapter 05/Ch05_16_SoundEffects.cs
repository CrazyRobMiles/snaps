using SnapsLibrary;

class Ch05_16_SoundEffects
{
    public void StartProgram()
    {
        string effectName = SnapsEngine.SelectFrom4Buttons("beep", "ding", "gameOver", "lose");
        SnapsEngine.PlaySoundEffect(effectName);
    }
}

