namespace MRULib.Views
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    public class PinableListBoxItem : ListBoxItem
    {
        private static readonly DependencyProperty IsMouseOverListBoxItemProperty =
            DependencyProperty.Register("IsMouseOverListBoxItem",
                                        typeof(bool),
                                        typeof(PinableListBoxItem),
                                        new FrameworkPropertyMetadata(IsMouseOverListBoxItemChanged));

        public bool IsMouseOverListBoxItem
        {
            get { return (bool)this.GetValue(IsMouseOverListBoxItemProperty); }

            set { this.SetValue(IsMouseOverListBoxItemProperty, value); }
        }

        protected override void OnMouseEnter(System.Windows.Input.MouseEventArgs e)
        {
            base.OnMouseEnter(e);

            this.IsMouseOverListBoxItem = true;
        }

        protected override void OnMouseLeave(System.Windows.Input.MouseEventArgs e)
        {
            base.OnMouseEnter(e);

            this.IsMouseOverListBoxItem = false;
        }

        private static void IsMouseOverListBoxItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PinableListBoxItem item = d as PinableListBoxItem;

            if (item != null)
            {
                item.IsMouseOverListBoxItem = (bool)e.NewValue;
            }
        }
    }
}
