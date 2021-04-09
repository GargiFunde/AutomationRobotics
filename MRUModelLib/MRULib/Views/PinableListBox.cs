namespace MRULib.Views
{
    using MRULib.Views;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    public class PinableListBox : ListBox
    {
        /// <summary>
        /// Static Standard Constructor
        /// 
        /// Getting CustomControl style from Themes/Generic.xaml does not work ???
        /// </summary>
        static PinableListBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PinableListBox),
                      new FrameworkPropertyMetadata(typeof(PinableListBox)));
        }

        /// <summary>
        /// Standard method is executed when control template is applied to lookless control.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new PinableListBoxItem();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is PinableListBoxItem;
        }
    }

    public class PinableItemsControl : ItemsControl
    {
        /// <summary>
        /// Static Standard Constructor
        /// 
        /// Getting CustomControl style from Themes/Generic.xaml does not work ???
        /// </summary>
        static PinableItemsControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PinableItemsControl),
                      new FrameworkPropertyMetadata(typeof(PinableItemsControl)));
        }

        /// <summary>
        /// Standard method is executed when control template is applied to lookless control.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new PinableListBoxItem();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is PinableListBoxItem;
        }
    }

}
