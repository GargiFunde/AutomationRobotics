namespace MRULib.Views
{
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Class implements a custom control that displays a check pin -
    /// a needle that is in
    /// IsChecked = true state (needle is pinned vertically)
    /// 
    /// or
    /// IsChecked = false state (needle is note pinned, displayed only on mouseover vie PinnableBoxItem)
    /// 
    /// </summary>
    public class CheckPin : CheckBox
    {
        #region constructor
        /// <summary>
        /// Class constructor
        /// </summary>
        static CheckPin()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CheckPin),
                                                     new FrameworkPropertyMetadata(typeof(CheckPin)));
        }

        public CheckPin()
        {
        }
        #endregion constructor

        #region methods
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }
        #endregion methods
    }
}
