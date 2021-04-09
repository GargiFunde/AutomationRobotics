
using System;
using System.IO;
using System.Windows.Data;
using BOTDesigner.Properties;

namespace BOTDesigner.Views.InvokeChildWorkflow
{
     class StringToFileNameConvertors : IValueConverter
    {
        /// <summary>
        /// Converts a string representing a file path to just the filename component of that string. Any environment variables within the string
        /// are first resolved.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted string. If the method returns null, the valid null value is used.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string fullPath = value as string;
            if (!string.IsNullOrEmpty(fullPath))
            {
                try
                {
                    return Path.GetFileName(DynamicActivityStore.ReplaceEnvironmentVariables(fullPath));
                }
                catch
                {
                    return fullPath;
                }
            }

            return Resources.UndefinedPathErrorText;
        }

        /// <summary>
        /// Not implemented.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
    }
}
