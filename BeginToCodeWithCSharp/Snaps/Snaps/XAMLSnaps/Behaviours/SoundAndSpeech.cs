using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Media.SpeechSynthesis;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

using Windows.Media.SpeechRecognition;
using Windows.Foundation;
using SnapsLibrary;
using System.Diagnostics;
using System.Text;

namespace XAMLSnaps
{
    public class SoundAndSpeech
    {
        protected SnapsManager manager;

        public SoundAndSpeech(SnapsManager manager)
        {
            this.manager = manager;
            soundOutputElement = new MediaElement();
        }

        MediaElement soundOutputElement;

        public MediaElement SoundOutputElement
        {
            get
            {
                return soundOutputElement;
            }
        }

        public async Task PlayCompleteSoundAsync()
        {
            AutoResetEvent soundPlaybackCompletedEvent = new AutoResetEvent(false);
            string failedMessage = null;

            var tcs = new TaskCompletionSource<object>();

            RoutedEventHandler endedLambda = (s, e) =>
                tcs.TrySetResult(null);

            ExceptionRoutedEventHandler endedFailed = (s, e) =>
            {
                failedMessage = e.ErrorMessage;
                tcs.TrySetResult(null);
            };

            try
            {
                soundOutputElement.MediaEnded += endedLambda;
                soundOutputElement.MediaFailed += endedFailed;
                soundOutputElement.Play();
                await tcs.Task;
            }
            finally
            {
                soundOutputElement.MediaEnded -= endedLambda;
                soundOutputElement.MediaFailed -= endedFailed;
            }
            if (failedMessage != null)
                throw new Exception("Sound playback error: " + failedMessage);
        }

        TimeSpan start = new TimeSpan(0);

        public async Task PlaySound(string path)
        {
            var package = Windows.ApplicationModel.Package.Current;
            var installedLocation = package.InstalledLocation;

            var storageFile = await installedLocation.GetFileAsync(path);

            if (storageFile != null)
            {
                var stream = await storageFile.OpenAsync(Windows.Storage.FileAccessMode.Read);
                soundOutputElement.SetSource(stream, storageFile.ContentType);
                soundOutputElement.Play();
            }
            else
                throw new Exception("Sound file not found");
        }

        public async Task StartPlayingSoundAsync()
        {
            AutoResetEvent soundPlaybackCompletedEvent = new AutoResetEvent(false);
            bool failed = false;

            var tcs = new TaskCompletionSource<object>();

            RoutedEventHandler openedLambda = (s, e) =>
                tcs.TrySetResult(null);

            ExceptionRoutedEventHandler openFailed = (s, e) =>
            {
                failed = true;
                tcs.TrySetResult(null);
            };

            try
            {
                soundOutputElement.MediaOpened += openedLambda;
                soundOutputElement.MediaFailed += openFailed;
                soundOutputElement.Play();
                await tcs.Task;
            }
            finally
            {
                soundOutputElement.MediaEnded -= openedLambda;
                soundOutputElement.MediaFailed -= openFailed;
            }
            if (failed)
                throw new Exception("Sound playback failed");
        }


        public void SpeakString(string message)
        {
            AutoResetEvent speakCompletedEvent = new AutoResetEvent(false);

            manager.InvokeOnUIThread(
                async () =>
                {
                    if (soundOutputElement != null)
                    {
                        SpeechSynthesizer synth = new Windows.Media.SpeechSynthesis.SpeechSynthesizer();

                        // Generate the audio stream from plain text.
                        SpeechSynthesisStream stream = await synth.SynthesizeTextToStreamAsync(message);

                        // Send the stream to the media object.
                        soundOutputElement.SetSource(stream, stream.ContentType);
                        await PlayCompleteSoundAsync();
                        speakCompletedEvent.Set();
                    }
                }
            );
            speakCompletedEvent.WaitOne();
        }

        string[] resourceNames = new string[] { "beep", "ding", "gameOver", "lose" };

        public void PlayCompleteSoundEffectatLocation(string soundLocation)
        {
            bool failed = false;

            AutoResetEvent PlaySoundEffectCompleteEvent = new AutoResetEvent(false);

            manager.InvokeOnUIThread(
                async () =>
                {
                    try
                    {
                        soundOutputElement.Source = new Uri(soundLocation);
                        await PlayCompleteSoundAsync();
                    }
                    catch
                    {
                        failed = true;
                    }
                    finally
                    {
                        PlaySoundEffectCompleteEvent.Set();
                    }
                });
            PlaySoundEffectCompleteEvent.WaitOne();
            if (failed)
            {
                if (SnapsEngine.ThrowExceptions)
                    throw new Exception("Sound effect playback failed");
            }
            return;
        }

        public void StartPlayingSoundEffectatLocation(string soundLocation)
        {
            bool failed = false;

            AutoResetEvent PlaySoundEffectCompleteEvent = new AutoResetEvent(false);

            manager.InvokeOnUIThread(
                async () =>
                {
                    try
                    {
                        await PlaySound(soundLocation);
                        PlaySoundEffectCompleteEvent.Set();
                    }
                    catch
                    {
                        failed = true;
                    }
                });

            PlaySoundEffectCompleteEvent.WaitOne();

            if (failed)
            {
                throw new Exception("Sound effect playback failed");
            }
            return;
        }


        public void PlaySoundEffect(string soundName)
        {
            PlayCompleteSoundEffectatLocation(@"ms-appx:///Sounds/SoundEffects/" + soundName + ".wav");
        }

        string[] noteNames = new string[] { "c1", "csharp", "d", "dsharp", "e", "f", "fsharp", "g", "gsharp", "a", "asharp", "b", "c2" };

        public void PlayNote(int pitch, double duration)
        {
            if (pitch < 0 || pitch >= noteNames.Length)
                return;

            StartPlayingSoundEffectatLocation(@"Sounds\MusicNotes\" + noteNames[pitch] + ".wav");
            SnapsEngine.Delay(duration);
        }

        public void PlaySoundEffectNoWait(string soundName)
        {
            StartPlayingSoundEffectatLocation(@"Sounds\SoundEffects\" + soundName + ".wav");
        }

        #region Speech Recognition

        SpeechRecognizer recognizer = null;

        async Task<string> SelectSpokenWordAsync(string[] words, string prompt)
        {
            string resultString = "";

            if (recognizer == null)
                recognizer = new SpeechRecognizer();

            recognizer.Constraints.Clear();
            recognizer.Constraints.Add(new SpeechRecognitionListConstraint(words));

            await recognizer.CompileConstraintsAsync();

            SpeechRecognitionResult results;

            recognizer.UIOptions.AudiblePrompt = prompt;

            StringBuilder promptString = new StringBuilder();

            foreach(string word in words)
            {
                promptString.AppendLine(word);
            }

            recognizer.UIOptions.ExampleText = promptString.ToString();
            recognizer.UIOptions.IsReadBackEnabled = false;
            recognizer.UIOptions.ShowConfirmation = false;

            results = await recognizer.RecognizeWithUIAsync();

            if (results.Status == SpeechRecognitionResultStatus.Success)
            {
                resultString = results.Text;
            }
            return resultString;
        }

        private void Recognizer_StateChanged(SpeechRecognizer sender, SpeechRecognizerStateChangedEventArgs args)
        {
            Debug.WriteLine(args.State);
        }

        public string SelectSpokenWord(string prompt, string[] words)
        {
            string result = "";

            AutoResetEvent gotTextEvent = new AutoResetEvent(false);

            manager.InvokeOnUIThread(
                async () =>
                {
                    result = await SelectSpokenWordAsync(words, prompt);
                    gotTextEvent.Set();
                }
            );

            gotTextEvent.WaitOne();

            return result;
        }

        #endregion
    }
}