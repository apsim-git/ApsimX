﻿using System;
using System.Text;
using Models.Core;
using System.Xml.Serialization;
using System.Xml;

namespace Models
{
    /// <summary>This is a memo/text component that stores user entered text information.</summary>
    [Serializable]
    [ViewName("UserInterface.Views.HTMLView")]
    [PresenterName("UserInterface.Presenters.MemoPresenter")]
    public class Memo : Model
    {
        /// <summary>Initializes a new instance of the <see cref="Memo"/> class.</summary>
        public Memo()
        {

        }

        /// <summary>Gets or sets the memo text.</summary>
        /// <value>The memo text.</value>
        [XmlIgnore]
        [Description("Text of the memo")]
        public string MemoText { get; set; }

        /// <summary>Gets or sets the code c data.</summary>
        /// <value>The code c data.</value>
        [XmlElement("MemoText")]
        public XmlNode CodeCData
        {
            get
            {
                XmlDocument dummy = new XmlDocument();
                return dummy.CreateCDataSection(MemoText);
            }
            set
            {
                if (value != null)
                {
                    MemoText = value.InnerText;
                }
            }
        }
    }
}
