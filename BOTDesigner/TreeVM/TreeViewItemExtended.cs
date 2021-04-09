// <copyright file=TreeViewItemExtended company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Prabodhini Funde</author>
// <date> 03-10-2018 16:02:53</date>
// <summary></summary>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BOTDesigner
{
    public class TreeViewItemExtended : TreeViewItem, INotifyPropertyChanged
    {
        private string projectOrlibraryPath = string.Empty;
        public string ProjectOrLibraryPath
        {
            get
            {
                return this.projectOrlibraryPath;
            }

            set
            {
                if (value != this.projectOrlibraryPath)
                {
                    this.projectOrlibraryPath = value;

                    // NotifyPropertyChanged("LibraryPath");
                }
            }
        }

        public string folderIcon = string.Empty;
        public string FolderIcon
        {
            get
            {
                return this.folderIcon;
            }

            set
            {
                if (value != this.folderIcon)
                {
                    this.folderIcon = value;
                    NotifyPropertyChanged("FolderIcon");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        // This method is called by the Set accessor of each property.
        // The CallerMemberName attribute that is applied to the optional propertyName
        // parameter causes the property name of the caller to be substituted as an argument.
        private void NotifyPropertyChanged(String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}
