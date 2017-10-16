using System.Windows;

namespace ComboBoxOfColors
{
  /// <summary>
  /// MainWindow code-behind class.
  /// </summary>
  public partial class MainWindow : Window
  {
    /// <summary>
    /// Constructor for the Main Window class.
    /// </summary>
    public MainWindow()
    {
      InitializeComponent();
      cboProjectColor.ItemsSource = ColorUtils.LoadColors();
    }

    /// <summary>
    /// Click event handler for Save button.
    /// </summary>
    private void cmdSave_Click(object sender, RoutedEventArgs e)
    {
      cboProjectColor.Text = "Yellow Green";
    }

    /// <summary>
    /// CLick event handler for Cancel button.
    /// </summary>
    private void cmdCancel_Click(object sender, RoutedEventArgs e)
    {
      cboProjectColor.Text = "Chocolate";
    }

  }
}
