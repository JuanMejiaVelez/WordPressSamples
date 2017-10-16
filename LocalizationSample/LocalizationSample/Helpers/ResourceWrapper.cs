using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;

namespace LocalizationSample.Helpers
{
  public sealed class ResourceWrapper : INotifyPropertyChanged
  {
    private static Resources.AppStrings appStringsResource = new Resources.AppStrings();

    public Resources.AppStrings AppStringsResource
    {
      get { return appStringsResource; }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public void UpdateBinding()
    {
      if (this.PropertyChanged != null)
      {
        this.PropertyChanged(this, new PropertyChangedEventArgs("AppStringsResource"));
      }
    }
  }
}
