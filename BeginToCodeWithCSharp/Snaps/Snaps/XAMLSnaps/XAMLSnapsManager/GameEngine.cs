using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

using SnapsLibrary;
using Windows.Foundation;
using Windows.UI.Xaml;

using System.Collections.Concurrent;

using System.Reflection;
using System.Threading;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Media;
using Windows.UI;
using Windows.Storage;
using Windows.Storage.Pickers;

using Windows.UI.Xaml.Media.Animation;

using Windows.Gaming.Input;
using Windows.UI.ViewManagement;

namespace XAMLSnaps
{
    public partial class SnapsManager
    {
        /// <summary>
        /// All the sprites being managed by the game
        /// </summary>
        List<ISnapsSprite> gameSprites = new List<ISnapsSprite>();

        #region Sprite adding and Removal

        object addLock = new object();

        public void AddSpriteToGame(ISnapsSprite sprite)
        {
            sprite.SpriteSetup(graphicsCanvas);
            gameSprites.Add(sprite);
        }

        //private void AddNewSpritesToGame()
        //{
        //    //lock (addLock)
        //    //{
        //    //    foreach(ISnapsSprite sprite in newSprites)
        //    //    {
        //    //        sprite.SpriteSetup(graphicsCanvas);
        //    //        graphicsCanvas.Children.Add(sprite.Element);
        //    //        gameSprites.Add(sprite);
        //    //    }
        //    //    newSprites.Clear();
        //    //}
        //}

        List<ISnapsSprite> removeSprites = new List<ISnapsSprite>();

        object removeLock = new object();

        public void RemoveSpriteFromGame(ISnapsSprite sprite)
        {
            removeSprites.Add(sprite);
        }

        private void RemoveSpritesFromGame()
        {
            lock(removeLock)
            {
                foreach(ISnapsSprite sprite in removeSprites)
                {
                    gameSprites.Remove(sprite);
                    graphicsCanvas.Children.Remove(sprite.Element);
                }
                removeSprites.Clear();
            }
        }

        #endregion

        Storyboard move = null;

        bool engineStarted = false;

        DateTime lastFrameStart;

        TimeSpan frameTimeSpan;

        TranslateTransform viewportTranslate;
        RotateTransform viewportRotate;
        ScaleTransform viewportScale;

        TransformGroup viewportTransformGroup = null;

        public bool StartGameEngine(bool fullScreen, int framesPerSecond)
        {
            frameRate = 1.0 / framesPerSecond;
            frameTimeSpan = TimeSpan.FromSeconds(frameRate);

            AutoResetEvent startComplete = new AutoResetEvent(false);

            bool failed = false;

            var tcs = new TaskCompletionSource<object>();

            WindowSizeChangedEventHandler endedLambda = (s, e) =>
            {
                GameViewportWidthValue = e.Size.Width;
                GameViewportHeightValue = e.Size.Height;
                tcs.TrySetResult(null);
            };

            InvokeOnUIThread(
            async () =>
            {
                if (fullScreen)
                {
                    //Try to enter full screen
                    ApplicationView view = ApplicationView.GetForCurrentView();
                    Window.Current.SizeChanged += endedLambda;
                    if (!view.IsFullScreenMode)
                        view.TryEnterFullScreenMode();
                    await tcs.Task;
                }
                else
                {
                    SetGameViewPortSizeFromWindow();
                }

                if (move == null)
                {
                    move = new Storyboard();
                    graphicsCanvas.Resources.Add("Move", move);
                    move.Completed += Move_Completed;
                }

                if(viewportTransformGroup == null)
                {
                    viewportTransformGroup = new TransformGroup();
                    viewportTranslate = new TranslateTransform();
                    viewportTransformGroup.Children.Add(viewportTranslate);
                    viewportRotate = new RotateTransform();
                    viewportTransformGroup.Children.Add(viewportRotate);
                    viewportScale = new ScaleTransform();
                    viewportTransformGroup.Children.Add(viewportScale);
                    graphicsCanvas.RenderTransform = viewportTransformGroup;
                }

                startComplete.Set();
            });

            startComplete.WaitOne();

            engineStarted = true;

            return !failed;
        }

        DateTime lastDrawEnded;

        bool firstDraw = true;

        int slowcount = 0;

        double frameRate;

        public double GetFrameRate()
        {
            return frameRate;
        }

        AutoResetEvent RenderComplete = new AutoResetEvent(false);

        TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();

        private void Move_Completed(object sender, object e)
        {
            tcs.TrySetResult(null);
        }

        public bool DrawGamePage()
        {
            if (!engineStarted)
                throw new Exception("Need to start the Game Engine before drawing a page");

            AutoResetEvent startComplete = new AutoResetEvent(false);

            tcs = new TaskCompletionSource<object>();

            DateTime newFrameStart = DateTime.Now;

            if (firstDraw)
                lastFrameStart = newFrameStart;

            TimeSpan frameTime = newFrameStart - lastFrameStart;

            lastFrameStart = newFrameStart;

            double timeInSeconds = frameTime.TotalMilliseconds / 1000;

            InvokeOnUIThread(
            async () =>
            {
                //AddNewSpritesToGame();

                RemoveSpritesFromGame();

                foreach (ISnapsSprite sprite in gameSprites)
                    sprite.SpriteUpdate(timeInSeconds);

                move.Begin();

                await tcs.Task;

                startComplete.Set();
            });

            startComplete.WaitOne();

            // Now need to make sure that the interval between the last 
            // draw finishing and this draw finishing is the same
            // This is the framerate as selected

            if (firstDraw)
            {
                lastDrawEnded = DateTime.Now;
                firstDraw = false;
                return true;
            }

            DateTime drawEnded = DateTime.Now;

            TimeSpan timeSinceLastDrawEnded = drawEnded - lastDrawEnded;

            if (timeSinceLastDrawEnded > frameTimeSpan)
            {
                slowcount++;
                return false;
            }

            TimeSpan padDelay = frameTimeSpan - timeSinceLastDrawEnded;

            using (EventWaitHandle tmpEvent = new ManualResetEvent(false))
            {
                tmpEvent.WaitOne(padDelay);
            }

            lastDrawEnded = DateTime.Now;

            return true;
        }

        double GameViewportWidthValue;
        double GameViewportHeightValue;

        public double GameViewportWidth
        {
            get
            {
                return GameViewportWidthValue;
            }
        }

        public double GameViewportHeight
        {
            get
            {
                return GameViewportHeightValue;
            }
        }

        private void SetGameViewPortSizeFromWindow()
        {
            // Set the screen size to the current values
            GameViewportWidthValue = graphicsCanvas.ActualWidth;
            GameViewportHeightValue = graphicsCanvas.ActualHeight;
        }

        private void View_VisibleBoundsChanged(ApplicationView sender, object args)
        {
        }

        public void ClearGameEngine()
        {
            manager.InvokeOnUIThread(
                () =>
                {
                    foreach (ISnapsSprite sprite in gameSprites)
                        graphicsCanvas.Children.Remove(sprite.Element);
                    gameSprites.Clear();
                }
                );
        }

        #region Gamepad

        bool gamePadSetup = false;

        bool gamePadVisible = false;

        void setupGamePad()
        {
            if (gamePadSetup)
                return;

            Gamepad.GamepadAdded += Gamepad_GamepadAdded;

            Gamepad.GamepadRemoved += Gamepad_GamepadRemoved;

        }

        private void Gamepad_GamepadRemoved(object sender, Gamepad e)
        {
            showGamePad();
        }

        private void Gamepad_GamepadAdded(object sender, Gamepad e)
        {
            hideGamePad();
        }

        private void showGamePad()
        {
            manager.InvokeOnUIThread(
                () =>
                {
                    joyPad.Visibility = Visibility.Visible;
                    gamePadVisible = true;
                }
                );
        }

        private void hideGamePad()
        {
            manager.InvokeOnUIThread(
                () =>
                {
                    joyPad.Visibility = Visibility.Collapsed;
                    gamePadVisible = false;
                }
                );
        }

        private void readyGamePad()
        {
            if (Gamepad.Gamepads.Count == 0)
            {
                if (!gamePadVisible)
                    showGamePad();
            }
        }

        public bool GetUpGamepad()
        {
            readyGamePad();

            if (Gamepad.Gamepads.Count > 0)
            {
                GamepadReading reading = Gamepad.Gamepads[0].GetCurrentReading();
                return reading.Buttons == GamepadButtons.DPadUp;
            }

            return manager.joyPad.UpPressed;
        }

        public bool GetDownGamepad()
        {
            readyGamePad();

            if (Gamepad.Gamepads.Count > 0)
            {
                GamepadReading reading = Gamepad.Gamepads[0].GetCurrentReading();
                return reading.Buttons == GamepadButtons.DPadDown;
            }

            return manager.joyPad.DownPressed;
        }

        public bool GetLeftGamepad()
        {
            readyGamePad();

            if (Gamepad.Gamepads.Count > 0)
            {
                GamepadReading reading = Gamepad.Gamepads[0].GetCurrentReading();
                return reading.Buttons == GamepadButtons.DPadLeft;
            }

            return manager.joyPad.LeftPressed;
        }

        public bool GetRightGamepad()
        {
            readyGamePad();
            if (Gamepad.Gamepads.Count > 0)
            {
                GamepadReading reading = Gamepad.Gamepads[0].GetCurrentReading();
                return reading.Buttons == GamepadButtons.DPadRight;
            }

            return manager.joyPad.RightPressed;
        }

        public bool GetFireGamepad()
        {
            readyGamePad();
            if (Gamepad.Gamepads.Count > 0)
            {
                GamepadReading reading = Gamepad.Gamepads[0].GetCurrentReading();
                return reading.Buttons == GamepadButtons.A;
            }

            return manager.joyPad.FirePressed;
        }

        #endregion
    }
}