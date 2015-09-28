using System;
using System.ComponentModel;
using System.Security.Permissions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using System.ComponentModel.Design;

namespace DaleWare.Web.UI.Controls
{
    /// <summary>
    /// Summary description for ListPickerControl
    /// </summary>
    [
       AspNetHostingPermission(SecurityAction.Demand,
           Level = AspNetHostingPermissionLevel.Minimal),
       AspNetHostingPermission(SecurityAction.InheritanceDemand,
           Level = AspNetHostingPermissionLevel.Minimal),
       ToolboxData("<{0}:ListPicker runat=\"server\"> </{0}:ListPicker>"),
     SupportsEventValidation(),
    Designer(typeof(ListPickerControlDesigner), typeof(IDesigner))]
    public class ListPickerControl : CompositeControl
    {
        private ListBox _availableChoicesListBox = null;
        private ListBox _selectedChoicesListBox = null;
        private ListItemCollection _availableChoices = null;
        private ListItemCollection _selectedChoices = null;

        private Style _buttonStyle = null;
        private Style _listBoxStyle = null;
        
        private bool _sort;

        private Button btnMoveAllLeftToRight = null;
        private Button btnMoveSelectedLeftToRight = null;
        private Button btnMoveSelectedRightToLeft = null;
        private Button btnMoveAllRightToLeft = null;

        private int _numberOfRows = 10;

        private Label _availableChoicesLabel = null;
        private Label _selectedChoicesLabel = null;

        private Style _labelStyle = null;

        [Category("Behavior"),
       DefaultValue("This property gets or sets whether to use client script to move the items between the list boxes."),
       Description("This property gets or sets whether to use client script to move the items between the list boxes."),
       Localizable(false)]
        public bool UseClientScript
        {
            get;
            set;
        }

        [Category("Behavior"),
        DefaultValue("This button moves all values from the left list box to the right list box."),
        Description("Tool tip for the button that moves all values from the left list box to the right list box."),
        Localizable(true)]
        public string MoveAllLeftToRightButtonToolTip
        {
            get;set;
        }
        
        [Category("Behavior"),
        DefaultValue("This button moves all values from the right list box to the left list box."),
        Description("Tool tip for the button that moves all values from the right list box to the left list box."),
        Localizable(true)]
        public string MoveAllRightToLeftButtonToolTip
        {
            get;
            set;
        }

        [Category("Behavior"),
        DefaultValue("This button moves all selected values from the left list box to the right list box."),
        Description("Tool tip for the button that moves all selected values from the left list box to the right list box."),
        Localizable(true)]
        public string MoveSelectedLeftToRightButtonToolTip
        {
            get; set;
        }
        
        [Category("Behavior"),
        DefaultValue("This button moves all selected values from the right list box to the left list box."),
        Description("Tool tip for the button that moves all selected values from the right list box to the left list box."),
        Localizable(true)]
        public string MoveSelectedRightToLeftButtonToolTip
        {
            get;set;
        }
        
        [Category("Behavior"),
        DefaultValue("This button moves all values from the right list box to the left list box."),
        Description("Tool tip for the button that moves all values from the right list box to the left list box."),
        Localizable(true)]
        public string MoveAllRightToLeft
        {
            get;set;
        }

        [Bindable(true),
        Category("Behavior"),
        DefaultValue(true),
        Description("Whether to sort the listboxes.")]
        public bool Sort
        {
            get { return _sort; }
            set { _sort = value; }
        }
        [Bindable(true),
        Category("Behavior"),
        DefaultValue(SortDirection.Ascending),
        Description("Sort direction for sorting the listboxes.")]
        public SortDirection SortDirection
        { get; set; }

        [Bindable(true),
        Category("Layout"),
        DefaultValue(10),
        Description("The default number of rows for the listboxes.")]
        public int NumberOfListBoxRows
        {
            set { _numberOfRows = value; }
            get { return _numberOfRows; }
        }

        [Category("Behavior"),
         DefaultValue("This is the label text for the left list box."),
         Description("This is the label text for the left list box."),
         Localizable(true)]
        public string AvailableChoicesLabelText
        {
            get;
            set; 
        }

        [Category("Behavior"),
         DefaultValue("This is the label text for the right list box."),
         Description("This is the label text for the right list box."),
         Localizable(true)]
        public string SelectedChoicesLabelText
        {
            get;
            set;
        }

     
        [Bindable(true),
         DefaultValue((string)null),
         PersistenceMode(PersistenceMode.InnerProperty),
         Editor("System.Web.UI.Design.WebControls.ListItemsCollectionEditor,System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a",
         typeof(System.Drawing.Design.UITypeEditor)),
         MergableProperty(false),
        Description("The list of available items intended for the left list box.")]
        public ListItemCollection AvailableChoices
        {
            get
            {
                if (this._availableChoices == null)
                {
                    this._availableChoices = new ListItemCollection();
                    ((IStateManager)this._availableChoices).TrackViewState();
                }
                return this._availableChoices;
            }
        }
        [Bindable(true),
         DefaultValue((string)null),
         PersistenceMode(PersistenceMode.InnerProperty),
         Editor("System.Web.UI.Design.WebControls.ListItemsCollectionEditor,System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a",
         typeof(System.Drawing.Design.UITypeEditor)),
         MergableProperty(false),
         Category("Data"),
         Description("The list of selected items intended for the right list box.")]
        public ListItemCollection SelectedItems
        {
            get
            {
                if (this._selectedChoices == null)
                {
                    this._selectedChoices = new ListItemCollection();
                    if (this._selectedChoicesListBox != null && this._selectedChoicesListBox.Items != null && this._selectedChoicesListBox.Items.Count > 0)
                    {
                        foreach (ListItem li in this._selectedChoicesListBox.Items)
                        {
                            this._selectedChoices.Add(li);
                        }
                    }
                    ((IStateManager)this._selectedChoices).TrackViewState();
                }
                return this._selectedChoices;
            }
        }

        #region Typed Style properties
        [
        Category("Styles"),
        DefaultValue(null),
        DesignerSerializationVisibility(
            DesignerSerializationVisibility.Content),
        PersistenceMode(PersistenceMode.InnerProperty),
        Description(
            "The strongly typed style for the Button child controls.")
        ]
        public virtual Style ButtonStyle
        {
            get
            {
                if (_buttonStyle == null)
                {
                    _buttonStyle = new Style();
                    if (IsTrackingViewState)
                    {
                        ((IStateManager)_buttonStyle).TrackViewState();
                    }
                }
                return _buttonStyle;
            }
        }

        [
        Category("Styles"),
        DefaultValue(null),
        DesignerSerializationVisibility(
            DesignerSerializationVisibility.Content),
        PersistenceMode(PersistenceMode.InnerProperty),
        Description(
            "The strongly typed style for the ListBox child controls.")
        ]
        public virtual Style ListBoxStyle
        {
            get
            {
                if (_listBoxStyle == null)
                {
                    _listBoxStyle = new Style();
                    if (IsTrackingViewState)
                    {
                        ((IStateManager)_listBoxStyle).TrackViewState();
                    }
                }
                return _listBoxStyle;
            }
        }

        [Category("Styles"),
         DefaultValue(null),
         DesignerSerializationVisibility(
           DesignerSerializationVisibility.Content),
         PersistenceMode(PersistenceMode.InnerProperty),
            Description(
                    "The strongly typed style for the listbox labels.")
        ]
        public virtual Style LabelStyle
        {
            get
            {
                if (this._labelStyle == null)
                {
                    _labelStyle = new Style();                    
                    if (IsTrackingViewState)
                    {
                        ((IStateManager)_labelStyle).TrackViewState();
                    }
                }
                return _labelStyle;
            }
        }
        #endregion

        #region Protected Properties
        protected bool RenderClientScript
        {
            get { return (this.UseClientScript && (this.Page.Request.Browser.EcmaScriptVersion.Major >= 1)); }
        }
        #endregion

        #region Protected Methods
        protected override void RecreateChildControls()
        {
            EnsureChildControls();
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.TrackViewState(); // check to see whether this is necessary or not.
        }

        protected override void CreateChildControls()
        {
            Controls.Clear();
            this._availableChoicesListBox = new ListBox();
            this._availableChoicesListBox.Rows = this.NumberOfListBoxRows;
            this._availableChoicesListBox.SelectionMode = ListSelectionMode.Multiple;

            this._selectedChoicesListBox = new ListBox();
            this._selectedChoicesListBox.Rows = this.NumberOfListBoxRows;
            this._selectedChoicesListBox.SelectionMode = ListSelectionMode.Multiple;

            this.btnMoveAllLeftToRight = new Button();
            this.btnMoveAllLeftToRight.Text = ">>";
            this.btnMoveAllLeftToRight.ToolTip = this.MoveAllLeftToRightButtonToolTip;
            this.btnMoveAllLeftToRight.CausesValidation = false;

            this.btnMoveSelectedLeftToRight = new Button();
            this.btnMoveSelectedLeftToRight.Text = " > ";
            this.btnMoveSelectedLeftToRight.ToolTip = this.MoveSelectedLeftToRightButtonToolTip;
            
            this.btnMoveSelectedLeftToRight.CausesValidation = false;

            this.btnMoveSelectedRightToLeft = new Button();
            this.btnMoveSelectedRightToLeft.Text = " < ";
            this.btnMoveSelectedRightToLeft.ToolTip = this.MoveSelectedRightToLeftButtonToolTip;
            this.btnMoveSelectedRightToLeft.CausesValidation = false;

            this.btnMoveAllRightToLeft = new Button();
            this.btnMoveAllRightToLeft.Text = "<<";
            this.btnMoveAllRightToLeft.ToolTip = this.MoveAllRightToLeftButtonToolTip;
            this.btnMoveAllRightToLeft.CausesValidation = false;
                       
            if (this.RenderClientScript)
            {
                this.RegisterClientScriptResource(typeof(ListPickerControl), "DaleWare.Web.UI.Controls.ListPickerClientSide.js");
            }
            else
            {
                this.btnMoveAllLeftToRight.Click += new EventHandler(btnMoveAllLeftToRight_Click);
                this.btnMoveSelectedLeftToRight.Click += new EventHandler(btnMoveSelectedLeftToRight_Click);
                this.btnMoveSelectedRightToLeft.Click += new EventHandler(btnMoveSelectedRightToLeft_Click);
                this.btnMoveAllRightToLeft.Click += new EventHandler(btnMoveAllRightToLeft_Click);
            }

            if (string.IsNullOrEmpty(this.AvailableChoicesLabelText) == false)
            {
                this._availableChoicesLabel = new Label();
                this._availableChoicesLabel.ApplyStyle(this.LabelStyle);
                this._availableChoicesLabel.Text = this.AvailableChoicesLabelText;
                this.Controls.Add(this._availableChoicesLabel);
            }
            if (string.IsNullOrEmpty(this.SelectedChoicesLabelText) == false)
            {
                this._selectedChoicesLabel = new Label();
                this._selectedChoicesLabel.ApplyStyle(this.LabelStyle);
                this._selectedChoicesLabel.Text = this.SelectedChoicesLabelText;
                this.Controls.Add(this._selectedChoicesLabel);
            }

            this.Controls.Add(_availableChoicesListBox);
            this.Controls.Add(_selectedChoicesListBox);
            this.Controls.Add(btnMoveAllLeftToRight);
            this.Controls.Add(btnMoveAllRightToLeft);
            this.Controls.Add(btnMoveSelectedLeftToRight);
            this.Controls.Add(btnMoveSelectedRightToLeft);

            if (this._availableChoices != null && this._availableChoices.Count > 0)
            {
                if (this.Sort) this._availableChoices.SortListItems(this.SortDirection == SortDirection.Descending);
                
                foreach (ListItem li in this._availableChoices)
                {
                    this._availableChoicesListBox.Items.Add(li);
                }
            }
            if (this._selectedChoices != null && this._selectedChoices.Count > 0)
            {
                if (this.Sort) this._selectedChoices.SortListItems(this.SortDirection == SortDirection.Descending);
                foreach (ListItem li in this._selectedChoices)
                {
                    this._selectedChoicesListBox.Items.Add(li);
                }
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            AddAttributesToRender(writer);
            writer.AddAttribute(
               HtmlTextWriterAttribute.Width, this.Width.ToString(), false);
            writer.RenderBeginTag(HtmlTextWriterTag.Table);

            if (this._listBoxStyle != null)
            {
                this._availableChoicesListBox.ApplyStyle(this._listBoxStyle);
                this._selectedChoicesListBox.ApplyStyle(this._listBoxStyle);
            }

            if (this._buttonStyle != null)
            {
                this.btnMoveAllLeftToRight.ApplyStyle(_buttonStyle);
                this.btnMoveAllRightToLeft.ApplyStyle(_buttonStyle);
                this.btnMoveSelectedLeftToRight.ApplyStyle(_buttonStyle);
                this.btnMoveSelectedRightToLeft.ApplyStyle(_buttonStyle);
            }

            if (this.RenderClientScript)
            {
                this.btnMoveAllLeftToRight.Attributes.Add("onclick", string.Format("javascript:return moveListBoxItems(\'{0}\',\'{1}\',false);",
                    this._availableChoicesListBox.ClientID,
                    this._selectedChoicesListBox.ClientID));
                this.btnMoveSelectedLeftToRight.Attributes.Add("onclick", string.Format("javascript:return moveListBoxItems(\'{0}\',\'{1}\',true);",
                    this._availableChoicesListBox.ClientID,
                    this._selectedChoicesListBox.ClientID));
                this.btnMoveSelectedRightToLeft.Attributes.Add("onclick", string.Format("javascript:return moveListBoxItems(\'{0}\',\'{1}\',true);",
                    this._selectedChoicesListBox.ClientID,
                    this._availableChoicesListBox.ClientID));
                this.btnMoveAllRightToLeft.Attributes.Add("onclick", string.Format("javascript:return moveListBoxItems(\'{0}\',\'{1}\',false);",
                    this._selectedChoicesListBox.ClientID,
                    this._availableChoicesListBox.ClientID));

                if(this.IsInsideUpdatePanel() == false)
                {    
                    foreach (ListItem li in this.SelectedItems)
                    {
                        this.Page.ClientScript.RegisterForEventValidation(this._availableChoicesListBox.UniqueID, li.Value);
                    }
                    foreach (ListItem li in this.AvailableChoices)
                    {
                        this.Page.ClientScript.RegisterForEventValidation(this._selectedChoicesListBox.UniqueID, li.Value);
                    }
                }
            }
            
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            // first list box cell   
            writer.AddStyleAttribute(HtmlTextWriterStyle.TextAlign, "left");
            writer.AddStyleAttribute(HtmlTextWriterStyle.Width, "40%");
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            // first list box
            if (this._availableChoicesLabel != null)
            {
                this._availableChoicesLabel.RenderControl(writer);
                writer.WriteBreak();
            }
            this._availableChoicesListBox.RenderControl(writer);
            writer.RenderEndTag();
            // button cell
            writer.AddStyleAttribute(HtmlTextWriterStyle.TextAlign, "center");
            writer.AddStyleAttribute(HtmlTextWriterStyle.Width, "20%");
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            this.btnMoveAllLeftToRight.RenderControl(writer);
            writer.WriteBreak();
            this.btnMoveSelectedLeftToRight.RenderControl(writer);
            writer.WriteBreak();
            this.btnMoveSelectedRightToLeft.RenderControl(writer);
            writer.WriteBreak();
            this.btnMoveAllRightToLeft.RenderControl(writer);
            writer.RenderEndTag();
            // second list box cell
            writer.AddStyleAttribute(HtmlTextWriterStyle.Width, "40%");
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            if (this._selectedChoicesLabel != null)
            {
                this._selectedChoicesLabel.RenderControl(writer);
                writer.WriteBreak();
            }
            this._selectedChoicesListBox.RenderControl(writer);
            writer.RenderEndTag(); // td tag
            writer.RenderEndTag(); // tr tag
            writer.RenderEndTag(); // table tag

        }

        #region Custom state management
        protected override void LoadViewState(object savedState)
        {
            if (savedState == null)
            {
                base.LoadViewState(null);
                return;
            }
            else
            {
                Triplet t = savedState as Triplet;

                if (t != null)
                {
                    // Always invoke LoadViewState on the base class even if 
                    // the saved state is null.
                    base.LoadViewState(t.First);

                    if ((t.Second) != null)
                    {
                        ((IStateManager)ButtonStyle).LoadViewState(t.Second);
                    }

                    if ((t.Third) != null)
                    {
                        ((IStateManager)ListBoxStyle).LoadViewState(t.Third);
                    }
                }
                else
                {
                    throw new ArgumentException("Invalid view state .");
                }
            }
        }

        protected override object SaveViewState()
        {
            object baseState = base.SaveViewState();
            object buttonStyleState = null;
            object textBoxStyleState = null;

            if (_buttonStyle != null)
            {
                buttonStyleState =
                    ((IStateManager)_buttonStyle).SaveViewState();
            }

            if (_listBoxStyle != null)
            {
                textBoxStyleState =
                    ((IStateManager)_listBoxStyle).SaveViewState();
            }

            return new Triplet(baseState,
                buttonStyleState, textBoxStyleState);

        }

        protected override void TrackViewState()
        {
            base.TrackViewState();
            if (_buttonStyle != null)
            {
                ((IStateManager)_buttonStyle).TrackViewState();
            }
            if (_listBoxStyle != null)
            {
                ((IStateManager)_listBoxStyle).TrackViewState();
            }
        }


        #endregion

        #region Event Handlers

        void btnMoveAllRightToLeft_Click(object sender, EventArgs e)
        {
            foreach (ListItem li in this._selectedChoicesListBox.Items)
            {
                if (this._availableChoicesListBox.Items.Contains(li) == false)
                {
                    this._availableChoicesListBox.Items.Add(li);
                }
            }
            this._selectedChoicesListBox.Items.Clear();
        }

        void btnMoveSelectedRightToLeft_Click(object sender, EventArgs e)
        {
            ListItem li = null;

            for (int i = 0; i < this._selectedChoicesListBox.Items.Count; i++)
            {
                li = this._selectedChoicesListBox.Items[i];
                if (li.Selected && this._availableChoicesListBox.Items.Contains(li) == false)
                {
                    this._availableChoicesListBox.Items.Add(li);
                }
            }
            for (int j = this._selectedChoicesListBox.Items.Count - 1; j >= 0; j--)
            {
                li = this._selectedChoicesListBox.Items[j];
                if (li.Selected) this._selectedChoicesListBox.Items.Remove(li);
            }
        }

        void btnMoveSelectedLeftToRight_Click(object sender, EventArgs e)
        {
            ListItem li = null;
            for (int i = 0; i < this._availableChoicesListBox.Items.Count; i++)
            {
                li = this._availableChoicesListBox.Items[i];
                if (li.Selected && this._selectedChoicesListBox.Items.Contains(li) == false)
                {
                    this._selectedChoicesListBox.Items.Add(li);
                }
            }
            for (int j = this._availableChoicesListBox.Items.Count - 1; j >= 0; j--)
            {
                li = this._availableChoicesListBox.Items[j];
                if (li.Selected) this._availableChoicesListBox.Items.Remove(li);
            }
        }

        void btnMoveAllLeftToRight_Click(object sender, EventArgs e)
        {
            foreach (ListItem li in this._availableChoicesListBox.Items)
            {
                if (this._selectedChoicesListBox.Items.Contains(li) == false)
                {
                    this._selectedChoicesListBox.Items.Add(li);
                }
            }
            this._availableChoicesListBox.Items.Clear();
        }
        #endregion

       
        #endregion
    }
}