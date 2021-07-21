using System.Windows;
using System.Windows.Controls;

namespace custom_controls.Controls
{
    public class CardView : Control
    {
        public double UniformCornerRadius
        {
            get { return (double)GetValue(UniformCornerRadiusProperty); }
            set { SetValue(UniformCornerRadiusProperty, value); }
        }
        public static readonly DependencyProperty UniformCornerRadiusProperty
            = DependencyProperty.Register(nameof(UniformCornerRadius), typeof(double), typeof(CardView),
                new FrameworkPropertyMetadata(2.0, FrameworkPropertyMetadataOptions.AffectsMeasure));

        public object CardImage
        {
            get { return (Image)GetValue(CardImageProperty); }
            set { SetValue(CardImageProperty, value); }
        }

        public static readonly DependencyProperty CardImageProperty =
            DependencyProperty.Register("CardImage", typeof(object), typeof(CardView), new PropertyMetadata(null));

        public object CardBody
        {
            get { return (object)GetValue(CardBodyProperty); }
            set { SetValue(CardBodyProperty, value); }
        }

        public static readonly DependencyProperty CardBodyProperty =
            DependencyProperty.Register("CardBody", typeof(object), typeof(CardView), new PropertyMetadata(null));

        public string CardHeader
        {
            get { return (string)GetValue(CardHeaderProperty); }
            set { SetValue(CardHeaderProperty, value); }
        }

        public static readonly DependencyProperty CardHeaderProperty =
            DependencyProperty.Register("CardHeader", typeof(string), typeof(CardView), new PropertyMetadata(null));

        public string CardFooter
        {
            get { return (string)GetValue(CardFooterProperty); }
            set { SetValue(CardFooterProperty, value); }
        }

        public static readonly DependencyProperty CardFooterProperty =
            DependencyProperty.Register("CardFooter", typeof(string), typeof(CardView), new PropertyMetadata(null));


        static CardView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CardView), new FrameworkPropertyMetadata(typeof(CardView)));
        }
    }
}
