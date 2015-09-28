using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace DaleWare.Web.UI.Controls.Validators
{
    /// <summary>
    /// 
    /// </summary>
    public class StringLengthValidator: RegularExpressionValidator
    {
        #region Public constants
        public const string DEFAULT_ERROR_MESSAGE = "{0} must be at least {1} characters and no more than {2} characters.";        
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public StringLengthValidator()
        {
            this.Display = ValidatorDisplay.Dynamic;
        }
        new public string ErrorMessage
        {
            get { return this.formatMessage(); }
            private set { }
        }
        public override string Text        
        {
            get
            {
                return this.formatMessage();
            }
             set
            {
                base.Text = value;
            }
        }

        public override string ToolTip
        {
            get
            {
                return this.formatMessage();
            }
            set
            {
                base.ToolTip = value;
            }
        }
        //[Themeable(false)]
        //new public string ValidationExpression 
        //{
        //    get { return @"^[\w]{" + this.MinimumLength.ToString() + "," + this.MaximumLength.ToString() + "}$";}
        //    private set { }
        //}

        /// <summary>
        /// This is the minimum length for the string to be checked.
        /// </summary>
        [Browsable(true),
         Category("Behavior"),
         Themeable(false),
         DefaultValue(0),
         Description("This is the minimum length for the string to be checked.")]
        public int MinimumLength
        {
            get
            {
                return ViewState["MinimumLength"]!=null ? (int)ViewState["MinimumLength"] : 0;
            }
            set
            {
                ViewState["MinimumLength"] = value;
            }
        }

        /// <summary>
        /// This is the maximum length for the string to be checked.
        /// </summary>
        [Browsable(true),
         Category("Behavior"),
         Themeable(false),
         DefaultValue(0),
         Description("This is the maximum length for the string to be checked.")]
        public int MaximumLength
        {
            get
            {
                return ViewState["MaximumLength"] != null ? (int)ViewState["MaximumLength"] : 0;
            }
            set
            {
                ViewState["MaximumLength"] = value;
            }
        }

        private string formatMessage()
        {
            return string.Format(StringLengthValidator.DEFAULT_ERROR_MESSAGE, this.ClientID, this.MinimumLength.ToString(), this.MaximumLength.ToString());
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.ValidationExpression = @"^[\w\s]{" + this.MinimumLength.ToString() + "," + this.MaximumLength.ToString() + "}$";
        }
    }
}
