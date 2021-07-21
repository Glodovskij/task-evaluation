using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace custom_controls.Controls
{
    public class CardView : Control
    {
        public double UniformCornerRadius
        {
            get => (double)GetValue(UniformCornerRadiusProperty);
            set => SetValue(UniformCornerRadiusProperty, value);
        }
        public static readonly DependencyProperty UniformCornerRadiusProperty
            = DependencyProperty.Register(nameof(UniformCornerRadius), typeof(double), typeof(CardView),
                new FrameworkPropertyMetadata(2.0, FrameworkPropertyMetadataOptions.AffectsMeasure));

        public object CardImage
        {
            get { return (Image)GetValue(CardImageProperty); }
            set { SetValue(CardImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CardImage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CardImageProperty =
            DependencyProperty.Register("CardImage", typeof(object), typeof(CardView), new PropertyMetadata(null));

        public object CardBody
        {
            get { return (object)GetValue(CardBodyProperty); }
            set { SetValue(CardBodyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CardBody.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CardBodyProperty =
            DependencyProperty.Register("CardBody", typeof(object), typeof(CardView), new PropertyMetadata(null));

        public string CardHeader
        {
            get { return (string)GetValue(CardHeaderProperty); }
            set { SetValue(CardHeaderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CardHeader.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CardHeaderProperty =
            DependencyProperty.Register("CardHeader", typeof(string), typeof(CardView), new PropertyMetadata(null));

        public string CardFooter
        {
            get { return (string)GetValue(CardFooterProperty); }
            set { SetValue(CardFooterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CardFooter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CardFooterProperty =
            DependencyProperty.Register("CardFooter", typeof(string), typeof(CardView), new PropertyMetadata(null));




        static CardView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CardView), new FrameworkPropertyMetadata(typeof(CardView)));
        }
    }
}
