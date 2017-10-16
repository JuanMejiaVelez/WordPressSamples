using System;
using System.Windows.Media;

namespace ComboBoxOfColors
{
  /// <summary>
  /// Represents a Color fom the XML data tag.
  /// </summary>
  public class RowColor
  {
    /// <summary>
    /// Constructor for RowColor.
    /// </summary>
    public RowColor()
    {
      FillColor = new SolidColorBrush();
    }
    /// <summary>
    /// Gets/Sets the fill brush color for the row.
    /// </summary>
    public SolidColorBrush FillColor { get; set; }

    /// <summary>
    /// Gets/Sets the display color for the row.
    /// </summary>
    public String ColorName { get; set; }
  }
}
