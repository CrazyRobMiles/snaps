using System.Collections.Generic;
using System.Threading;
using Windows.Devices.Gpio;
using Windows.Foundation;

namespace SnapsLibrary
{
    class GPIOSnap
    {

        GpioController gpio = null;

        Dictionary<int, GpioPin> activePins = new Dictionary<int, GpioPin>();

        public GPIOSnap()
        {
            gpio = GpioController.GetDefault();
        }

        public void WriteToPin(int pinNumber, bool value)
        {
            GpioPin pin = null;

            if (!activePins.ContainsKey(pinNumber))
            {
                pin = gpio.OpenPin(pinNumber);
                activePins.Add(pinNumber, pin);
            }
            else
            {
                pin = activePins[pinNumber];
            }

            if(pin.GetDriveMode() != GpioPinDriveMode.Output)
                pin.SetDriveMode(GpioPinDriveMode.Output);

            if (value)
                pin.Write(GpioPinValue.High);
            else
                pin.Write(GpioPinValue.Low);
        }

        public bool ReadFromPin(int pinNumber)
        {
            GpioPin pin = null;

            if (!activePins.ContainsKey(pinNumber))
            {
                pin = gpio.OpenPin(pinNumber);
                activePins.Add(pinNumber, pin);
            }
            else
            {
                pin = activePins[pinNumber];
            }

            if (pin.GetDriveMode() != GpioPinDriveMode.InputPullUp)
                pin.SetDriveMode(GpioPinDriveMode.InputPullUp);

            return pin.Read() == GpioPinValue.High;
        }

        void waitForPinChange(GpioPin SwitchPin)
        {
            AutoResetEvent gotTextEvent = new AutoResetEvent(false);

            TypedEventHandler<GpioPin, GpioPinValueChangedEventArgs> startWaitLambda = (s, e) =>
            {
                gotTextEvent.Set();
            };

            try
            {
                SwitchPin.ValueChanged += startWaitLambda;
            }
            finally
            {
                SwitchPin.ValueChanged -= startWaitLambda;
            }

            gotTextEvent.WaitOne();
        }

        public void WaitForPinHigh(int pinNumber)
        {
            GpioPin pin = null;

            if (!activePins.ContainsKey(pinNumber))
            {
                pin = gpio.OpenPin(pinNumber);
                activePins.Add(pinNumber, pin);
            }

            if (pin.GetDriveMode() != GpioPinDriveMode.InputPullUp)
                pin.SetDriveMode(GpioPinDriveMode.InputPullUp);

            if (pin.Read() == GpioPinValue.High)
                return;

            waitForPinChange(pin);

            return ;
        }

#if wally

        const int P3 = 22;
        const int P4 = 23;
        const int P5 = 24;
        const int P6 = 25;


        private const int RED_LED_PIN = P3;
        private const int RED_SWITCH_PIN = P4;
        private const int YELLOW_LED_PIN = P5;
        private const int YELLOW_SWITCH_PIN = P6;

        private GpioPin redLedPin;
        private GpioPinValue redLedPinValue;

        private GpioPin redSwitchPin;
        private GpioPinValue redSwitchPinValue;

        private GpioPin yellowLedPin;
        private GpioPinValue yellowLedPinValue;

        private GpioPin yellowSwitchPin;
        private GpioPinValue yellowSwitchPinValue;

        private DispatcherTimer timer;
        private SolidColorBrush redBrush = new SolidColorBrush(Windows.UI.Colors.Red);
        private SolidColorBrush yellowBrush = new SolidColorBrush(Windows.UI.Colors.Yellow);
        private SolidColorBrush grayBrush = new SolidColorBrush(Windows.UI.Colors.LightGray);

        public MainPage()
        {
            InitializeComponent();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += Timer_Tick;
            InitGPIO();
            if (redLedPin != null)
            {
                timer.Start();
            }
        }

        Player redPlayer, yellowPlayer;

        private void InitGPIO()
        {
            var gpio = GpioController.GetDefault();

            // Show an error if there is no GPIO controller
            if (gpio == null)
            {
                redLedPin = null;
                GameStatus.Text = "There is no GPIO controller on this device.";
                return;
            }

            redLedPin = gpio.OpenPin(RED_LED_PIN);
            redLedPinValue = GpioPinValue.High;
            redLedPin.Write(redLedPinValue);
            redLedPin.SetDriveMode(GpioPinDriveMode.Output);

            redSwitchPin = gpio.OpenPin(RED_SWITCH_PIN);
            redSwitchPin.SetDriveMode(GpioPinDriveMode.Input);

            redPlayer = new Player(redSwitchPin, redLedPin, redBrush, RedMessageBlock, RedLed, RedScoreBlock, GpioPinValue.Low, GpioPinValue.High, GpioPinValue.Low);

            yellowLedPin = gpio.OpenPin(YELLOW_LED_PIN);
            yellowLedPinValue = GpioPinValue.High;
            yellowLedPin.Write(yellowLedPinValue);
            yellowLedPin.SetDriveMode(GpioPinDriveMode.Output);

            yellowSwitchPin = gpio.OpenPin(YELLOW_SWITCH_PIN);
            yellowSwitchPin.SetDriveMode(GpioPinDriveMode.Input);
            yellowPlayer = new Player(yellowSwitchPin, yellowLedPin, yellowBrush, YellowMessageBlock, YellowLed, YellowScoreBlock, GpioPinValue.Low, GpioPinValue.High, GpioPinValue.Low);
        }

        class Player
        {

            static Random playerRand = new Random();

            GpioPin SwitchPin { get; set; }

            GpioPin LightPin { get; set; }

            SolidColorBrush LitBrush { get; set; }

            TextBlock ScoreBlock;
            TextBlock Timeblock;

            DateTime LedLitTime { get; set; }

            GpioPinValue ButtonPressedState;
            GpioPinValue LedLitState;
            GpioPinValue LedOffState;

            DateTime TargetTime { get; set; }

            Ellipse LedEllipse;
            SolidColorBrush grayBrush = new SolidColorBrush(Windows.UI.Colors.LightGray);

            public Player(GpioPin switchPin, GpioPin lightPin, SolidColorBrush litBrush, TextBlock scoreBlock, Ellipse ledEllipse, TextBlock timeblock, GpioPinValue buttonPressedState, GpioPinValue ledLitState, GpioPinValue ledOffState)
            {
                this.SwitchPin = switchPin;
                this.LightPin = lightPin;
                this.LitBrush = litBrush;
                this.ScoreBlock = scoreBlock;
                this.Timeblock = timeblock;
                this.ButtonPressedState = buttonPressedState;
                this.LedLitState = ledLitState;
                this.LedOffState = ledOffState;
                this.LedEllipse = ledEllipse;
            }

            public bool WaitingForStart = false;

            async Task waitForStart()
            {
                WaitingForStart = true;

                await App.Current.InvokeOnUIThread(
                    () =>
                    {
                        Timeblock.Text = "Press the button to start";
                    });

                DateTime waitStartTime = DateTime.Now;

                var startTcs = new TaskCompletionSource<object>();

                TypedEventHandler<GpioPin, GpioPinValueChangedEventArgs> startWaitLambda = (s, e) =>
                {
                    if (s.Read() == this.ButtonPressedState)
                    {
                        TimeSpan timeToStart = DateTime.Now - waitStartTime;
                        if (timeToStart.Seconds > 2)
                            score += 2000;
                        startTcs.TrySetResult(null);
                    }
                };

                try
                {
                    SwitchPin.ValueChanged += startWaitLambda;

                    await startTcs.Task;
                }
                finally
                {
                    SwitchPin.ValueChanged -= startWaitLambda;
                }

                WaitingForStart = false;
            }

            bool waitComplete = false;

            DateTime timerStarts = DateTime.Now;
            DateTime timerEnds = DateTime.Now;

            void performDelay(float delayInMilliseconds)
            {
                timerStarts = DateTime.Now;

                waitComplete = false;
                using (EventWaitHandle tmpEvent = new ManualResetEvent(false))
                {
                    tmpEvent.WaitOne(TimeSpan.FromMilliseconds(delayInMilliseconds));
                }

                timerEnds = DateTime.Now;

                LightPin.Write(LedOffState);

                App.Current.InvokeOnUIThread(
                    () =>
                    {
                        Timeblock.Text = "Press the button to start";
                        LedEllipse.Fill = grayBrush;
                    });

                waitComplete = true;
            }

            void launchDelay(float delayInMiliseconds)
            {
                IAsyncAction WorkItem =
                Windows.System.Threading.ThreadPool.RunAsync(delegate
                {
                    performDelay(delayInMiliseconds);
                });
            }

            int score = 0;

            public int GetScore()
            {
                return score;
            }

            async Task RandomWait()
            {

                await App.Current.InvokeOnUIThread(
                    () =>
                    {
                        Timeblock.Text = "Waiting";
                    });

                LightPin.Write(LedLitState);

                await App.Current.InvokeOnUIThread(
                    () =>
                    {
                        LedEllipse.Fill = LitBrush;
                    });

                string message = "";

                DateTime buttonReleased;

                var tcs = new TaskCompletionSource<object>();

                TypedEventHandler<GpioPin, GpioPinValueChangedEventArgs> waitingLambda = (s, e) =>
                {
                    if (s.Read() != ButtonPressedState)
                    {
                        if (!waitComplete)
                        {
                            message = "Fail";
                            score = score + 10000;
                        }
                        else
                        {
                            buttonReleased = DateTime.Now;

                            TimeSpan reactionTime = buttonReleased - timerEnds;
                            score = score + reactionTime.Milliseconds;
                            message = "Score:" + score.ToString();
                        }
                        tcs.TrySetResult(null);
                    }
                };

                try
                {
                    SwitchPin.ValueChanged += waitingLambda;

                    int delayTime = playerRand.Next(1000, 3000);

                    launchDelay(delayTime);

                    await tcs.Task;
                }
                finally
                {
                    SwitchPin.ValueChanged -= waitingLambda;
                }

                await App.Current.InvokeOnUIThread(
                    () =>
                    {
                        Timeblock.Text = message;
                    });

            }

            async public void PlayRound(TimeSpan roundTimeSpan)
            {
                DateTime endTime = DateTime.Now + roundTimeSpan;
                score = 0;
                await App.Current.InvokeOnUIThread(
                    () =>
                    {
                        ScoreBlock.Text = "0";
                    });

                while (DateTime.Now < endTime)
                {
                    LightPin.Write(LedOffState);

                    await App.Current.InvokeOnUIThread(
                        () =>
                        {
                            LedEllipse.Fill = grayBrush;
                        });

                    if (SwitchPin.Read() != ButtonPressedState)
                    {
                        await waitForStart();
                    }
                    await RandomWait();

                    await App.Current.InvokeOnUIThread(
                        () =>
                        {
                            ScoreBlock.Text = score.ToString();
                        });

                }
            }
        }

        enum GameState
        {
            gameOver,
            playingGame
        }

        GameState state;

        void RunGame(Player p, TimeSpan t)
        {
            IAsyncAction WorkItem =
            Windows.System.Threading.ThreadPool.RunAsync(delegate
            {
                p.PlayRound(t);
            });
        }

        TimeSpan gameLength = new TimeSpan(0, 0, 30);
        DateTime gameStartTime;
        DateTime gameEndtime;

        private void startButtonClick(object sender, RoutedEventArgs e)
        {
            if (state == GameState.playingGame)
                return;

            state = GameState.playingGame;
            gameStartTime = DateTime.Now;
            gameEndtime = gameStartTime + gameLength;

            RunGame(redPlayer, gameLength);
            RunGame(yellowPlayer, gameLength);
        }

        Random displayRand = new Random();

        private void Timer_Tick(object sender, object e)
        {
            switch (state)
            {
                case GameState.gameOver:

                    string message = "";

                    if (redPlayer.GetScore() == 0 && yellowPlayer.GetScore() == 0)
                    {
                        message = "Click Start Game to Play";
                    }
                    else
                    {
                        if (redPlayer.WaitingForStart && yellowPlayer.GetScore() != 0)
                        {
                            message = "Yellow player wins by default";
                        }
                        else
                        {
                            if (yellowPlayer.WaitingForStart && redPlayer.GetScore() != 0)
                            {
                                message = "Red player wins by default";
                            }
                            else
                            {
                                if (redPlayer.GetScore() < yellowPlayer.GetScore())
                                {
                                    message = "Game Over: Red Wins";
                                }
                                else
                                {
                                    message = "Game Over: Yellow Wins";
                                }
                            }
                        }
                    }

                    GameStatus.Text = message;

                    if (displayRand.Next(0, 100) > 80)
                    {
                        if (YellowLed.Fill == grayBrush)
                        {
                            YellowLed.Fill = yellowBrush;
                        }
                        else
                        {
                            YellowLed.Fill = grayBrush;
                        }
                    }
                    if (displayRand.Next(0, 100) > 80)
                    {
                        if (RedLed.Fill == grayBrush)
                        {
                            RedLed.Fill = redBrush;
                        }
                        else
                        {
                            RedLed.Fill = grayBrush;
                        }
                    }
                    break;

                case GameState.playingGame:
                    TimeSpan timeLeft = gameEndtime - DateTime.Now;
                    GameStatus.Text = timeLeft.Seconds.ToString();
                    if (timeLeft.Milliseconds < 0)
                        state = GameState.gameOver;
                    break;
            }
        }
#endif

    }
}
