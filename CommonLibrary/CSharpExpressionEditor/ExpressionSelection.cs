// <copyright file=ExpressionSelection company=E2E Robotics>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Saurabh Mundhe</author>
// <date> 03-10-2018 16:02:54</date>
// <summary></summary>

using System;
using System.Activities.Presentation;
using System.Activities.Presentation.Model;

namespace CommonLibrary.CSharpExpressionEditor
{
    public class ExpressionSelection : ContextItem
    {
        ModelItem modelItem;

        public ExpressionSelection()
        {
        }

        public ExpressionSelection(ModelItem modelItem)
        {
            this.modelItem = modelItem;
        }

        public ModelItem ModelItem
        {
            get { return this.modelItem; }
        }

        public override Type ItemType
        {
            get
            {
                return typeof(ExpressionSelection);
            }
        }
    }
}
