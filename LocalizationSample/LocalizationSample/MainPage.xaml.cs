using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;

namespace LocalizationSample
{
  public partial class MainPage : UserControl
  {
    public MainPage()
    {
      InitializeComponent();
      this.localizationProxy.DataContext = new Helpers.ResourceWrapper();
    }

    // After the Frame navigates, ensure the HyperlinkButton representing the current page is selected
    private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
    {
      foreach (UIElement child in LinksStackPanel.Children)
      {
        HyperlinkButton hb = child as HyperlinkButton;
        if (hb != null && hb.NavigateUri != null)
        {
          if (ContentFrame.UriMapper.MapUri(e.Uri).ToString().Equals(ContentFrame.UriMapper.MapUri(hb.NavigateUri).ToString()))
          {
            VisualStateManager.GoToState(hb, "ActiveLink", true);
          }
          else
          {
            VisualStateManager.GoToState(hb, "InactiveLink", true);
          }
        }
      }
    }

    // If an error occurs during navigation, show an error window
    private void ContentFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
    {
      e.Handled = true;
      ChildWindow errorWin = new ErrorWindow(e.Uri);
      errorWin.Show();
    }

    private void lnkEnglish_Click(object sender, RoutedEventArgs e)
    {
      Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
      Helpers.ResourceWrapper res = this.localizationProxy.DataContext as Helpers.ResourceWrapper;
      res.UpdateBinding();
      ContentFrame.Refresh();
    }

    private void lnkSpanish_Click(object sender, RoutedEventArgs e)
    {
      Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("es");
      Helpers.ResourceWrapper res = this.localizationProxy.DataContext as Helpers.ResourceWrapper;
      res.UpdateBinding();
      ContentFrame.Refresh();
    }

  }
}