using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;
using XAMLSnaps;

namespace SnapsLibrary
{
    public delegate void UpdateMethod(double seconds);

    public interface ISnapsSprite
    {
        UIElement Element { get; }

        double X { get; set; }
        double Y { get; set; }
        double Left { get; set; }
        double Right { get; set; }
        double Top { get; set; }
        double Bottom { get; set; }
        double CenterX { get; set; }
        double CenterY { get; set; }
        void SpriteSetup(Canvas canvas);
        void SetPosition(SnapsCoordinate pos);
        double Width { get; set; }
        double Height { get; set; }
        double RotationAngle { get; set; }
        double Opacity { get; set; }
        double DistanceFrom(ISnapsSprite sprite);
        bool IntersectsWith(ISnapsSprite sprite);
        void ScaleSpriteWidth(double width);
        void ScaleSpriteHeight(double height);
        void SpriteUpdate(double milliseconds);
        void Hide();
        void Show();
    }

    public abstract class SnapsSprite : ISnapsSprite
    {
        /// <summary>
        /// Rotation angle of the sprite in degrees. 
        /// Positive is clockwise.
        /// </summary>
        public double RotationAngle { get; set; }

        private RotateTransform rotationTransform;

        public virtual void SpriteUpdate(double seconds)
        {
            // change the visibilty state if required

            if (isVisible && elementValue.Visibility == Visibility.Collapsed)
                elementValue.Visibility = Visibility.Visible;

            if (!isVisible && elementValue.Visibility == Visibility.Visible)
                elementValue.Visibility = Visibility.Collapsed;

            elementValue.Opacity = opacityValue;

            Canvas.SetLeft(elementValue, X );
            Canvas.SetTop(elementValue, Y);

            // Only do the rotation thing if we have to
            if(RotationAngle != 0)
            {
                if(rotationTransform == null)
                    rotationTransform = new RotateTransform();
                rotationTransform.Angle = RotationAngle;
                rotationTransform.CenterX = Width / 2.0;
                rotationTransform.CenterY = Height / 2.0;
                elementValue.RenderTransform = rotationTransform;
            }
        }

        protected UIElement elementValue;

        /// <summary>
        /// WPF display element being managed as a sprite
        /// </summary>
        public UIElement Element
        {
            get
            {
                return elementValue;
            }
        }

        private double opacityValue = 1;

        /// <summary>
        /// Opacity of the sprite in the range 0-1, where 0 is completely
        /// transparent. Default value is 1
        /// </summary>
        public double Opacity
        {
            get
            {
                return opacityValue;
            }

            set
            {
                opacityValue = value;
            }
        }

        /// <summary>
        /// X position of the top left hand corner of the sprite.
        /// Increases as the sprite moves to the right.
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Y position of the top left hand corner of the sprite.
        /// Increases as the sprite moves down the screen.
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// Set the position of the top left hand corner of the sprite.
        /// </summary>
        /// <param name="pos">new position of the sprite</param>
        public void SetPosition(SnapsCoordinate pos)
        {
            X = pos.XValue;
            Y = pos.YValue;
        }

        /// <summary>
        /// Width of the sprite in pixels
        /// </summary>
        public abstract double Width { get; set; }

        /// <summary>
        /// Height of the sprite in pixels
        /// </summary>
        public abstract double Height { get; set; }

        /// <summary>
        /// Scale the sprite so that it has the specified width.
        /// The height of the sprite is adjusted to maintain 
        /// the sprite aspect ratio
        /// </summary>
        /// <param name="width">required width of sprite</param>
        public abstract void ScaleSpriteWidth(double width);

        /// <summary>
        /// Scale the sprite so that it has the specified height.
        /// The width of the sprite is adjusted to maintain 
        /// the sprite aspect ratio.
        /// </summary>
        /// <param name="height">required height of the sprite</param>
        public abstract void ScaleSpriteHeight(double height);

        /// <summary>
        /// X position of the left hand edge of the sprite
        /// </summary>
        public double Left
        {
            get
            {
                return X;
            }
            set
            {
                X = value;
            }
        }

        /// <summary>
        /// X position of the right hand edge of the sprite
        /// </summary>
        public double Right
        {
            get
            {
                return X + Width;
            }

            set
            {
                X = value - Width;
            }
        }

        /// <summary>
        /// Y position of the top of the sprite
        /// </summary>
        public double Top
        {
            get
            {
                return Y;
            }
            set
            {
                Y = value ;
            }
        }

        /// <summary>
        /// Y position of the bottom of the sprite
        /// </summary>
        public double Bottom
        {
            get
            {
                return Y + Height;
            }
            set
            {
                Y = value - Height;
            }
        }

        /// <summary>
        /// X position of the center of the sprite
        /// </summary>
        public double CenterX
        {
            get
            {
                return X + (Width / 2);
            }
            set
            {
                X = value - (Width / 2);
            }
        }

        /// <summary>
        /// Y position of the center of the sprite
        /// </summary>
        public double CenterY
        {
            get
            {
                return Y + (Height / 2);
            }
            set
            {
                Y = value - (Height / 2);
            }
        }

        /// <summary>
        /// Text to see if a bounding box around this sprite
        /// intersects with a bounding box around another sprite
        /// </summary>
        /// <param name="sprite">sprite to test against</param>
        /// <returns>true if the bounding boxes intersect</returns>
        public bool IntersectsWith(ISnapsSprite sprite)
        {
            if (this.X > (sprite.X + sprite.Width))
                return false;

            if (this.Y > (sprite.Y + sprite.Height))
                return false;

            if (this.X + this.Width < sprite.X)
                return false;

            if (this.Y + this.Height < sprite.Y)
                return false;

            return true;
        }

        private bool isVisible = true;

        /// <summary>
        /// Hides the sprite so that it will not be drawn
        /// </summary>
        public virtual void Hide()
        {
            isVisible = false;
        }

        /// <summary>
        /// Makes the sprite visible
        /// </summary>
        public virtual void Show()
        {
            isVisible = true;
        }

        /// <summary>
        /// Gives the distance in pixels between the center of 
        /// this sprite and the center of the target sprite
        /// </summary>
        /// <param name="sprite">target sprite</param>
        /// <returns>distance in pixels</returns>
        public double DistanceFrom(ISnapsSprite sprite)
        {
            double dx = Math.Abs(this.CenterX - sprite.CenterX);
            double dy = Math.Abs(this.CenterY - sprite.CenterY);

            return Math.Sqrt((dx * dx) + (dy * dy));
        }

        /// <summary>
        /// Sets the sprite up and assigns it to the given canvas
        /// Runs in the context of the display
        /// </summary>
        /// <param name="canvas">canvas to assign the sprite to</param>
        public abstract void SpriteSetup(Canvas canvas);

        protected Task runAsync(Action a)
        {
            if (SnapsManager.UICoreDispatcher == null)
                throw new Exception("Snaps Manager dispatcher not set up");

            return SnapsManager.UICoreDispatcher.RunAsync(CoreDispatcherPriority.Normal, () => a()).AsTask();
        }

        /// <summary>
        /// Create a sprite and set the position and rotation
        /// </summary>
        /// <param name="x">x position of sprite top left hand corner</param>
        /// <param name="y">y position of sprite top right hand corner </param>
        /// <param name="rotationAngle">rotation angle in degrees</param>
        public SnapsSprite(double x, double y, double rotationAngle)
        {
            this.X = x;
            this.Y = y;
            this.RotationAngle = rotationAngle;
        }

        /// <summary>
        /// Create a sprite and set the position. Rotation set to 0.
        /// </summary>
        /// <param name="x">x position of sprite top left hand corner</param>
        /// <param name="y">y position of sprite top right hand corner </param>
        public SnapsSprite(double x, double y) : 
            this(x: x, y: y, rotationAngle: 0)
        {
        }

        /// <summary>
        /// Create a sprite at the top left hand corner with a rotation of 0
        /// </summary>
        public SnapsSprite() :
            this(x:0,y:0)
        {
        }
    }

    public class DotSprite : SnapsSprite
    {
        Ellipse ellipse;

        private SnapsColor color;
        
        private double dotDiameter;

        public override double Width
        {
            get
            {
                return dotDiameter;
            }

            set
            {
                dotDiameter = value;
            }
        }

        public override double Height
        {
            get
            {
                return dotDiameter;
            }

            set
            {
                dotDiameter = Height;
            }
        }

        public override void ScaleSpriteHeight(double height)
        {
            dotDiameter = height;
        }

        public override void ScaleSpriteWidth(double width)
        {
            dotDiameter = width;
        }

        public override void SpriteSetup(Canvas canvas)
        {
            AutoResetEvent loadCompletedEvent = new AutoResetEvent(false);

            Task doit = runAsync(
                 () =>
                {
                    ellipse = new Ellipse();
                    SolidColorBrush brush = new SolidColorBrush(Color.FromArgb(255, color.RedValue, color.GreenValue, color.BlueValue));
                    ellipse.Stroke = brush;
                    ellipse.Fill = brush;
                    ellipse.Width = Width;
                    ellipse.Height = Height;
                    elementValue = ellipse;
                    canvas.Children.Add(elementValue);
                    loadCompletedEvent.Set();
                });

            loadCompletedEvent.WaitOne();
        }

        public override void SpriteUpdate(double milliseconds)
        {
            base.SpriteUpdate(milliseconds);
            ellipse.Width = Width;
            ellipse.Height = Height;
        }

        public DotSprite(double diameter, SnapsColor color, double x, double y):base(x,y)
        {
            this.color = color;
            this.Width = diameter;
            this.Height = diameter;
        }
    }

    public class ImageSprite : SnapsSprite
    {
        private Image spriteImage;

        private bool heightSet = false;

        private double heightValue;

        public override double Height
        {
            get
            {
                if (double.IsNaN(heightValue))
                    return Width / aspectRatio;
                else
                    return heightValue;
            }
            set
            {
                heightSet = true;
                // If we are setting the height after
                // we have locked the aspect ratio we have
                // to fix the width at this point
                if (double.IsNaN(widthValue))
                {
                    widthValue = Width;
                }
                heightValue = value;
            }
        }

        private bool widthSet = false;

        private double widthValue;

        public override double Width {
            get
            {
                if (widthValue == float.NaN)
                    return Height * aspectRatio;
                else
                    return widthValue;
            }
            set
            {
                widthSet = true;
                // If we are setting the width after
                // we have locked the aspect ratio we have
                // to fix the height at this point
                if (double.IsNaN(heightValue))
                {
                    heightValue = Height;
                }
                widthValue = value;
            }
        }

        private double aspectRatio = .5;

        public override void ScaleSpriteHeight(double height)
        {
            Height = height;
            Width = double.NaN;
        }

        public override void ScaleSpriteWidth(double width)
        {
            Width = width;
            Height = double.NaN;
        }

        public override void SpriteSetup(Canvas canvas)
        {
            bool failed = false;

            var tcs = new TaskCompletionSource<object>();

            AutoResetEvent loadCompletedEvent = new AutoResetEvent(false);

            RoutedEventHandler endedLambda = (s, e) =>
            {
                if (!widthSet)
                    this.widthValue = spriteBitmapImage.PixelWidth;
                if (!heightSet)
                    this.heightValue = spriteBitmapImage.PixelHeight;
                aspectRatio = (double)spriteBitmapImage.PixelWidth / (double)spriteBitmapImage.PixelHeight;
                spriteImage.Visibility = Visibility.Collapsed;
                tcs.TrySetResult(null);
            };

            ExceptionRoutedEventHandler endedFailed = (s, e) =>
            {
                failed = true;
                tcs.TrySetResult(null);
            };

            Task doit = runAsync(
                async () =>
                {
                    try {
                        spriteImage = new Image();
                        spriteImage.Stretch = Stretch.Fill;
                        elementValue = spriteImage;
                        canvas.Children.Add(elementValue);
                        Uri imageURI = new Uri(imageURL, UriKind.RelativeOrAbsolute);
                        spriteBitmapImage = new BitmapImage();
                        spriteBitmapImage.ImageFailed += endedFailed;
                        spriteBitmapImage.ImageOpened += endedLambda;
                        spriteBitmapImage.UriSource = imageURI;
                        spriteImage.Source = spriteBitmapImage;

                        await tcs.Task;

                        if (failed)
                        {
                            imageURI = new Uri("ms-appx:///Images/ImageNotFound.png");
                            spriteBitmapImage.UriSource = imageURI;
                            spriteImage.Source = spriteBitmapImage;
                            await tcs.Task;
                        }

                        loadCompletedEvent.Set();
                    }
                    finally
                    {
                        spriteBitmapImage.ImageFailed -= endedFailed;
                        spriteBitmapImage.ImageOpened -= endedLambda;
                    }
                    loadCompletedEvent.Set();
                });

            loadCompletedEvent.WaitOne();
        }

        private void Result_ImageOpened(object sender, RoutedEventArgs e)
        {
        }

        private void Result_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public override void SpriteUpdate(double milliseconds)
        {
            base.SpriteUpdate(milliseconds);
            spriteImage.Width = Width;
            spriteImage.Height = Height;
        }

        string imageURL;
        BitmapImage spriteBitmapImage = null;

        public ImageSprite(string imageURL, double x, double y, double xSpeed, double ySpeed, double rotationAngle, double rotationSpeed) :
            base(x: x, y: y, rotationAngle: rotationAngle)
        {
        }

        public ImageSprite(string imageURL, double x, double y, double xSpeed, double ySpeed) :
            this(imageURL: imageURL, x: x, y: y, xSpeed: xSpeed, ySpeed: ySpeed, rotationAngle: 0, rotationSpeed: 0)
        {
        }

        public ImageSprite(string imageURL, double x, double y ) :
            this(imageURL: imageURL, x: x, y: y, xSpeed: 0, ySpeed: 0, rotationAngle: 0, rotationSpeed: 0)
        {
        }

        public ImageSprite(string imageURL) :
            this(imageURL: imageURL, x: 0, y: 0, xSpeed: 0, ySpeed: 0, rotationAngle: 0, rotationSpeed: 0)
        {
            this.imageURL = imageURL;
        }
    }

    public class BlockSprite : SnapsSprite
    {
        private Rectangle block;
        private SnapsColor color;

        public override double Height { get; set; }

        public override double Width { get; set; }

        private double aspectRatio = 1;

        public override void ScaleSpriteHeight(double height)
        {
            Height = height;
            Width = height * aspectRatio;
        }

        public override void ScaleSpriteWidth(double width)
        {
            Width = width;
            Height = width / aspectRatio;
        }

        public override void SpriteUpdate(double milliseconds)
        {
            base.SpriteUpdate(milliseconds);
            block.Width = Width;
            block.Height = Height;
        }

        #pragma warning disable 1998, 4014

        public override void SpriteSetup(Canvas canvas)
        {
            var tcs = new TaskCompletionSource<object>();

            runAsync(
                async () =>
                {
                    block = new Rectangle();
                    SolidColorBrush brush = new SolidColorBrush(Color.FromArgb(100, color.RedValue, color.GreenValue, color.BlueValue));
                    block.Stroke = brush;
                    block.Fill = brush;
                    block.Width = Width;
                    block.Height = Height;
                    elementValue = block;
                    canvas.Children.Add(elementValue);
                });
        }

        public BlockSprite(double width, double height, SnapsColor color, double x, double y ,double rotation):
            base(x,y,rotation)
        {
            this.color = color;
            this.Width = width;
            this.Height = height;
            this.aspectRatio = width / height;
            this.RotationAngle = rotation;
        }
    }

    public class TextBlockSprite : SnapsSprite
    {
        private TextBlock textBlock;
        private SnapsColor color;


        private double heightValue;
        public override double Height
        {
            get
            {
                return heightValue;
            }

            set
            {
                throw new Exception("Can't change height of TextBlock");
            }
        }

        private double widthValue;

        public override double Width
        {
            get
            {
                return widthValue;
            }

            set
            {
                throw new Exception("Can't change width of TextBlock");
            }
        }

        string textValue;

        public string Text
        {
            get
            {
                return textValue;
            }

            set
            {
                textValue = value;
            }
        }

        private double fontSizeValue;

        public double FontSize
        {
            get
            {
                return fontSizeValue;
            }

            set
            {
                fontSizeValue = value;
            }
        }

        private string fontFamilyNameValue;

        public string FontFamilyName
        {
            get
            {
                return fontFamilyNameValue;
            }
            set
            {
                fontFamilyNameValue = value;
            }
        }

        private double aspectRatio = 1;

        public override void ScaleSpriteHeight(double height)
        {
            Height = height;
            Width = height * aspectRatio;
        }

        public override void ScaleSpriteWidth(double width)
        {
            Width = width;
            Height = width / aspectRatio;
        }

        public override void SpriteUpdate(double milliseconds)
        {
            base.SpriteUpdate(milliseconds);

            if (textBlock.Text != textValue)
                textBlock.Text = textValue;

            if (textBlock.FontSize != FontSize)
                textBlock.FontSize = FontSize;

            heightValue = textBlock.RenderSize.Height;
        }

        public override void SpriteSetup(Canvas canvas)
        {

            var tcs = new TaskCompletionSource<object>();

            AutoResetEvent loadCompletedEvent = new AutoResetEvent(false);

            SizeChangedEventHandler textLoaded = (s, e) =>
            {
                    TextBlock_SizeChanged(s, e);
                    tcs.TrySetResult(null);
            };

            Task doit = runAsync(
                async () =>
                {
                    try {
                        textBlock = new TextBlock();
                        SolidColorBrush brush = new SolidColorBrush(Color.FromArgb(255, color.RedValue, color.GreenValue, color.BlueValue));
                        textBlock.Foreground = brush;

                        FontFamily f = new FontFamily(FontFamilyName);
                        textBlock.FontFamily = f;
                        textBlock.FontSize = FontSize;
                        elementValue = textBlock;
                        canvas.Children.Add(elementValue);
                        heightValue = textBlock.RenderSize.Height;
                        textBlock.SizeChanged +=  textLoaded;
          
                        await tcs.Task;

                        loadCompletedEvent.Set();
                    }
                    finally
                    {
                        textBlock.SizeChanged -= textLoaded;
                        textBlock.SizeChanged += TextBlock_SizeChanged;
                    }

                });

            loadCompletedEvent.WaitOne();
        }

        private void TextBlock_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            heightValue = e.NewSize.Height;
            widthValue = e.NewSize.Width;
        }

        public TextBlockSprite(string text, float fontSize, string fontFamily, SnapsColor color, double x, double y, double rotation) :
            base(x, y, rotation)
        {
            this.textValue = text;
            this.FontSize = fontSize;
            this.FontFamilyName = fontFamily;
            this.color = color;
            this.RotationAngle = rotation;
        }

        public TextBlockSprite(string text, float fontSize, SnapsColor color, double x, double y) : this(text,fontSize, "Segoe UI", color,x,y,0)
        {
        }

        public TextBlockSprite(string text, float fontSize, SnapsColor color) : this(text, fontSize, "Segoe UI", color, 0, 0, 0)
        {
        }

        public TextBlockSprite(string text, float fontSize, string fontFamily, SnapsColor color) : this(text, fontSize, fontFamily, color, 0, 0, 0)
        {
        }
    }
}
