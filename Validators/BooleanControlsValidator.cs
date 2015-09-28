using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Permissions;
using DaleWare.Web.UI.Controls;

namespace DaleWare.Web.UI.Controls.Validators
{
    /// <summary>
    /// This class takes care of validation where there are multiple separate controls (checkboxes or radiobuttons)
    /// which must have one value selected. For validation of a RadioButtonList, use the ASP.NET out of the box RequiredFieldValidator.
    /// </summary>
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal),
     AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal),
     ToolboxData(@"<{0}:BooleanControlsValidator runat=""server""></{0}:BooleanControlsValidator>")]
    public class BooleanControlsValidator : BaseValidator
    {
        #region Overriden Methods

        protected override bool ControlPropertiesValid()
        {
            if (this.ControlsToValidate.Trim().Length == 0)
            {
                throw new HttpException(string.Format("The ControlsToValidate property of {0} cannot be blank.", this.ID));
            }

            string[] controlToValidateIDs = this.GetControlsToValidateIDs();
            if (controlToValidateIDs.Length <= 1)
            {
                throw new HttpException(string.Format("The ControlsToValidate property of {0} must have as least two IDs.", this.ID));
            }

            foreach (string controlToValidateID in controlToValidateIDs)
            {
                if ((((this.Parent.FindControl(controlToValidateID) is CheckBox) 
                 || ((this.Parent.FindControl(controlToValidateID) is  RadioButton)))) == false)
                {
                    throw new HttpException(string.Format("The control {0} in the ControlsToValidate property of {1} is not a checkbox or a radio button.", controlToValidateID, this.ID));
                }
            }

            return true;
        }

        protected override bool EvaluateIsValid()
        {

            string[] controlToValidateIDs = this.GetControlsToValidateIDs();
            bool returnValue = false;
            foreach (string controlToValidateID in controlToValidateIDs)
            {
                CheckBox checkbox = this.Parent.FindControl(controlToValidateID) as CheckBox;
                if (checkbox == null)
                {
                    RadioButton radio = this.Parent.FindControl(controlToValidateID) as RadioButton;
                    if (radio != null && radio.Checked)
                    {
                        returnValue = true;
                        break;
                    }

                }
                else if (checkbox != null && checkbox.Checked)
                {
                    returnValue = true;
                    break;
                }
            }
            return returnValue;
        }
        #endregion

        #region Private Methods
        private string[] GetControlsToValidateIDs()
        {
            string controlsToValidate = this.ControlsToValidate.Replace(" ", "");
            string[] controlToValidateIDs;
            try
            {
                controlToValidateIDs = controlsToValidate.Split(',');
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new FormatException(string.Format("The ControlsToValidate property of {0} is not well-formatted.", this.ID), ex);
            }
            return controlToValidateIDs;
        }

        private string GenerateClientSideControlsToValidate()
        {
            string[] controlToValidateIDs = this.GetControlsToValidateIDs();
            string controlToValidateIDTrimmed;
            string controlRenderIDs = string.Empty;
            foreach (string controlToValidateID in controlToValidateIDs)
            {
                controlToValidateIDTrimmed = controlToValidateID.Trim();
                if (controlToValidateIDTrimmed == string.Empty)
                {
                    throw new FormatException(string.Format("The ControlsToValidate property of {0} is not well-formatted.", this.ID));
                }
                controlRenderIDs += "," + base.GetControlRenderID(controlToValidateIDTrimmed);
            }
            controlRenderIDs = controlRenderIDs.Remove(0, 1);
            return controlRenderIDs;
        }
        
        private string getClientStartupScript()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            
            foreach (string s in this.GetControlsToValidateIDs())
            {
                sb.Append("ValidatorHookupControl(document.getElementById('");
                sb.Append(s);
                sb.Append("'),document.getElementById('");
                sb.Append(this.ClientID);
                sb.Append("'));\r\n");

            }
            return sb.ToString();
        }

        private string getClientScript()
        {
            string script = "function BooleanControlsEvaluateIsValid(val) {\r\n" +
                            "var validatorValue = false;\r\n" +
                            "var returnValue = false;\r\n" +
                            "controltovalidateIDs = val.controlstovalidate.split(',');\r\n" +
                            "    for (var controltovalidateIDIndex in controltovalidateIDs) {\r\n" +
                            "        var control = document.getElementById(controltovalidateIDs[controltovalidateIDIndex]);\r\n" +
                            "        if (control.checked) {\r\n" +
                            "        returnValue = true;\r\n" +
                            "        break;\r\n" +
                            "     }\r\n" +
                            "   }\r\n" +
                            "   return returnValue;\r\n" +
                            "}\r\n;";
            return script;
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (base.RenderUplevel)
            {
                this.RegisterClientScriptBlock(this.GetType(), "BooleanControlsValidator", getClientScript(), true);
                this.RegisterStartupScript(this.GetType(), "BooleanControlsValidator" + this.UniqueID, this.getClientStartupScript(), true);
                this.RegisterExpandoAttribute(this.ClientID, "evaluationfunction", "BooleanControlsEvaluateIsValid", true);
                this.RegisterExpandoAttribute(this.ClientID, "controlstovalidate", this.GenerateClientSideControlsToValidate(), true);
                
            }
        }
        #endregion
        #region Properties

        [Browsable(false),
        EditorBrowsable(EditorBrowsableState.Never)]
        public new bool SetFocusOnError
        {
            get
            {
                return false;
            }
            set
            {
                throw new NotSupportedException("SetFocusOnError is not supported because you have multiple controls to validate");
            }
        }

        [Browsable(false),
        EditorBrowsable(EditorBrowsableState.Never)]
        public new string ControlToValidate
        {
            get
            {
                return string.Empty;
            }
            set
            {
                throw new NotSupportedException("ControlToValidate is not supported because you have multiple controls to validate");
            }
        }

        /// <summary>
        /// Comma separated list of checkbox or radio button control IDs that you want to check
        /// </summary>Browsable(true)]
        [Category("Behavior"),
         Themeable(false),
         DefaultValue(""),
         Description("Comma separated list of checkbox or radio button control IDs that you want to check")]
        public string ControlsToValidate
        {
            get
            {
                return (string)(ViewState["ControlsToValidate"] ?? string.Empty);
            }
            set
            {
                ViewState["ControlsToValidate"] = value;
            }
        }
        #endregion
    }
}
