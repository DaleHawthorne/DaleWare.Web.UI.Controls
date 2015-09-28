using System;
using System.Collections.Generic;
using System.Security.Permissions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace DaleWare.Web.UI.Controls.WebParts
{
    /// <summary>
    /// 
    /// </summary>
    [AspNetHostingPermission(SecurityAction.Demand,
    Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(SecurityAction.InheritanceDemand,
      Level = AspNetHostingPermissionLevel.Minimal)]
    public class SearchBarWebPart: WebPart, ISearchBar
    {
        private List<Button> ButtonList = null;
        private Label _searchBarLabel = null;
        private TextBox _searchText = null;
        private Button _searchButton = null;
        private char[] _alphabet =  { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

       [WebBrowsable(),
        WebDescription("This property sets the display text for the search bar")]
        public string SearchBarLabelText { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public bool ShowWildcardButton { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool ShowNumericSearch { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ValidationExpression { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string SearchString { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [ConnectionProvider("Search Bar", "SearchBar")]
        public ISearchBar ProvideSearchBar()
        {  
            return this;  
        }

        protected void OnButtonClicked(object sender, EventArgs e)
        { 
        
        }

        protected override void CreateChildControls()
        {
            this.Controls.Clear();
            this.ButtonList = new List<Button>();
            
            if (string.IsNullOrEmpty(this.SearchBarLabelText) == false)
            {  this._searchBarLabel = new Label();
                this._searchBarLabel.Text = this.SearchBarLabelText;
                this._searchBarLabel.Width = new Unit(100, UnitType.Percentage);
                this.Controls.Add(this._searchBarLabel); 
            }

            foreach (char c in this._alphabet)
            {
                this.addNewButton(Convert.ToString(c));
            }

            if (this.ShowNumericSearch)
            { 
                for(int i = 1; i < 10; i++)
                {
                    this.addNewButton(i.ToString());
                }
            }
            if (this.ShowWildcardButton)
            {
                this.addNewButton("*");
            }
            this._searchText = new TextBox();
            this._searchText.Width = new Unit(75, UnitType.Percentage);
            this.Controls.Add(_searchText);

            this._searchButton = new Button();
            this._searchButton.Text = "Search";
            this._searchButton.CommandArgument = this._searchButton.Text;
            this._searchButton.Click += new EventHandler(this.OnButtonClicked);
               
            this.Controls.Add(this._searchButton);
        }
        
        private void addNewButton(string buttonText)
        {
                Button button = new Button();
                button.Text = buttonText;
                button.CommandArgument = buttonText;
                button.Click += new EventHandler(this.OnButtonClicked);
                this.ButtonList.Add(button);
                this.Controls.Add(button);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (this._searchBarLabel != null)
            {
                _searchBarLabel.RenderControl(writer);
            }

            foreach (Button b  in this.ButtonList)
            {
                b.RenderControl(writer);
            }
            writer.WriteBreak();
            this._searchText.RenderControl(writer);
            this._searchButton.RenderControl(writer);

        }
    }
}
