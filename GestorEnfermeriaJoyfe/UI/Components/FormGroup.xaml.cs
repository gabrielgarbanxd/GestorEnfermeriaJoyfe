using System.Windows;
using System.Windows.Controls;

namespace GestorEnfermeriaJoyfe.UI.Components
{
    /// <summary>
    /// Lógica de interacción para FormGroup.xaml
    /// </summary>
    public partial class FormGroup : UserControl
    {
        // //===>> Fields <<====//
        public static readonly DependencyProperty LabelTextProperty = DependencyProperty.Register(
            "LabelText",
            typeof(string),
            typeof(FormGroup),
            new PropertyMetadata(default(string)));

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text", 
            typeof(string), 
            typeof(FormGroup), 
            new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        // //===>> Propertys <<====//
        public string LabelText
        {
            get { return (string)GetValue(LabelTextProperty); }
            set { SetValue(LabelTextProperty, value); }
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // //===>> Constructor <<====//
        public FormGroup()
        {
            InitializeComponent();
        }
    }
}
