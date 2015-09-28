using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Security.Permissions;

namespace DaleWare.Web.UI.Controls.Validators
{
    /// <summary>
    /// This control validates a control based upon the value set in another control. The other control can be a 
    /// checkbox or radio button.
    /// </summary>
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal),
     AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal),
     ToolboxData(@"<{0}:RequiredIfValidator runat=server></{0}:RequiredIfValidator>")]
    public class RequiredIfValidator : BaseValidator
    {
        /// <summary>
        /// In the BaseValidator's Render method, this method gets
        /// called to make sure all of the required properties have been set.
        /// The BaseValidator's implementation raises an exception if any
        /// required properties aren't set, so that's why I call the base class
        /// implementation but don't keep the return value.
        /// </summary>
        /// <returns>
        /// True if the required properties are all set. The method will raise
        /// an exception if any properties are missing.
        /// </returns>
        protected override bool ControlPropertiesValid()
        {
            base.ControlPropertiesValid();

            if (ControlToCompare == string.Empty)
            {
                throw new HttpException(string.Format("The ControlToCompare property of '{0}' cannot be blank.", this.ClientID));
            }

            //It's acceptable for TriggerValue to be blank, so that's why 
            //I compare against null.
            if (TriggerValue == null)
            {
                throw new HttpException(string.Format("The TriggerValue property of {0} cannot be null.", this.ClientID));
            }

            return true;
        }

        /// <summary>
        /// Right before the control is going to render, I add my client-side validation
        /// script if the browser is capable of handling it.
        /// </summary>		
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (RenderUplevel)
            {
                string script = "\r\n<script language=\"javascript\">\r\n" +
                    "	function RequiredIfValidatorEvaluateIsValid(val){\r\n" +
                    "		if (val.controltocompare != \"\") {\r\n" +
                    //"           debugger;\r\n" +
                    "           var control = document.getElementById(val.controltocompare);\r\n" +
                    "           if((control.type == 'radio' || control.type == 'checkbox') \r\n" +
                    "               && ((val.triggervalue == 'true' && control.checked) ||(val.triggervalue == 'false' && !control.checked ))) {\r\n" +
                    "				return RequiredFieldValidatorEvaluateIsValid(val);\r\n" +
                    "			}\r\n" +
                    "			else if (val.triggervalueoperator == 'EqualTo' && control.value == val.triggervalue) {\r\n" +
                    "				return RequiredFieldValidatorEvaluateIsValid(val);\r\n" +
                    "			} else if (val.triggervalueoperator == 'NotEqualTo' && control.value != val.triggervalue) {\r\n" +
                    "				return RequiredFieldValidatorEvaluateIsValid(val);\r\n" +
                    "			} else {\r\n" +
                    "				return true;\r\n" +
                    "			}\r\n" +
                    "		} else {\r\n" +
                    "			return true;\r\n" +
                    "		}\r\n" +
                    "	}\r\n" +
                    "</script>\r\n";

                this.RegisterClientScriptBlock(this.GetType(), "__RequiredIfValidatorMethod", script, false);
                
          
                this.RegisterExpandoAttribute(this.ClientID,"initialvalue", InitialValue, false);
                this.RegisterExpandoAttribute(this.ClientID, "evaluationfunction", "RequiredIfValidatorEvaluateIsValid", false);
                this.RegisterExpandoAttribute(this.ClientID, "controltocompare", this.GetControlRenderID(ControlToCompare), false);
                this.RegisterExpandoAttribute(this.ClientID, "triggervalue", TriggerValue, false);
                this.RegisterExpandoAttribute(this.ClientID, "triggervalueoperator", PropertyConverter.EnumToString(typeof(Operator), this.TriggerValueOperator), false);
            }
        }

        /// <summary>
        /// This method is the server-side validation method.
        /// </summary>
        /// <returns>
        /// True if the validation succeeded, false otherwise.
        /// </returns>
        protected override bool EvaluateIsValid()
        {
            bool isValid = false;
            //first see if our trigger condition is true

            string controlValue;
            if (CheckCompareCondition())
            {
                //if the condition was true, do exactly what the 
                //RequiredFieldValidator normally does.

                controlValue = this.GetControlValidationValue(this.ControlToValidate);

                if (controlValue == null)
                {
                    isValid = true;
                }
                else
                {
                    isValid = (controlValue.Trim() != this.InitialValue.Trim());
                }

            }
            else
            {
                //if the trigger condition was false, then the field isn't required,
                //so the validation is successful.
                isValid = true;
            }
            return isValid;
        }

        /// <summary>
        /// This checks to see if the value of the ControlToCompare is equal
        /// to our TriggerValue. If the two are equal, it means that the 
        /// ControlToValidate is now required.
        /// </summary>
        /// <returns>
        /// True if the value of ControlToCompare is equal to TriggerValue, false
        /// otherwise.
        /// </returns>
        private bool CheckCompareCondition()
        {
            bool isRequired = false;
            Control control = null;
            control = this.Parent.FindControl(this.ControlToCompare);

            if (control is CheckBox || control is RadioButton)
            {
                isRequired = (((CheckBox)control).Checked == Convert.ToBoolean(this.TriggerValue));
            }
            else
            {
                string compareValue = this.GetControlValidationValue(ControlToCompare);

                if (this.TriggerValueOperator == Operator.EqualTo && compareValue == TriggerValue)
                {
                    isRequired = true;
                }
                else if (this.TriggerValueOperator == Operator.NotEqualTo && compareValue != TriggerValue)
                {
                    isRequired = true;
                }
            }
            return isRequired;
        }

        /// <summary>
        /// This is the control whose value should be compared against to determine
        /// if the ControlToValidate is required.
        /// </summary>
        [Browsable(true),
        Category("Behavior"),
        Themeable(false),
        DefaultValue(""),
        Description("This is the control whose value should be compared against to determine if the ControlToValidate is required.")]
        public string ControlToCompare
        {
            get
            {
                string temp = (string)ViewState["ControlToCompare"];

                if (temp != null)
                {
                    return temp;
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                ViewState["ControlToCompare"] = value;
            }
        }

        /// <summary>
        /// This is the equivalent of the RequiredFieldValidator's InitialValue
        /// property.
        /// </summary>
        [Browsable(true),
        Category("Behavior"),
        Themeable(false),
        DefaultValue(""),
        Localizable(true),
        Description("This is the equivalent of the RequiredFieldValidator's InitialValue property.")]
        public string InitialValue
        {
            get
            {
                string temp = (string)ViewState["InitialValue"];

                if (temp != null)
                {
                    return temp;
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                ViewState["InitialValue"] = value;
            }
        }

        /// <summary>
        /// This is the value that the ControlToCompare's value must be equal to in
        /// order for the ControlToValidate to be required.
        /// </summary>
        /// <summary>
        [Browsable(true),
        Category("Behavior"),
        Themeable(false),
        DefaultValue(""),
        Description("This is the value that the ControlToCompare's value must be equal to in order for the ControlToValidate to be required.")]
        public string TriggerValue
        {
            get
            {
                return (string)ViewState["TriggerValue"];
            }
            set
            {
                ViewState["TriggerValue"] = value;
            }
        }
        /// <summary>
        /// The operator used to test the value of the trigger value.
        /// </summary>
        [Browsable(true),
        Themeable(false),
        Category("Behavior"),
        DefaultValue(Operator.EqualTo),
        Description("The operator used to test the value of the trigger value.")]
        public Operator TriggerValueOperator
        {
            get
            {
                return (Operator)(ViewState["Operator"] ?? Operator.EqualTo);
            }
            set
            {
                ViewState["Operator"] = value;
            }
        }
        #region Enum

        public enum Operator
        {
            /// <summary>
            /// Equals condition
            /// </summary>
            EqualTo,
            /// <summary>
            /// XOR Condition
            /// </summary>
            NotEqualTo,
        }
        #endregion
    }
}
