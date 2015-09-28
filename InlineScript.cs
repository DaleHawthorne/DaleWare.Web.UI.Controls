using System;
using System.Web;
using System.Web.UI;
using System.ComponentModel;
using System.Security.Permissions;
using System.Text;
using System.IO;

namespace DaleWare.Web.UI.Controls
{
    /// <summary>
    /// This class permits inclusion of inline script inside an ASPX page with the use of control tags (<%%>)
    /// and VS intellisense.
    /// </summary>
    [
       AspNetHostingPermission(SecurityAction.Demand,
           Level = AspNetHostingPermissionLevel.Minimal),
       AspNetHostingPermission(SecurityAction.InheritanceDemand,
           Level = AspNetHostingPermissionLevel.Minimal),
       ToolboxData("<{0}:InlineScript runat=\"server\"> </{0}:InlineScript>"),
        Description("This class permits inclusion of inline script inside an ASPX page with the use of control tags (<%%>) and Visual Studio intellisense.")]
    public class InlineScript : Control
    {
        /// <summary>
        /// This property indicates how the script is to be rendered, as an inline block, or as an onsubmit statement script, or a startup script.
        /// </summary>
        [Browsable(true),
        Category("Behavior"),
        Themeable(false),
        DefaultValue(ScriptType.Block),
        Description("This property indicates how the script is to be rendered, as an inline block, or as an onsubmit statement script, or a startup script.")]
        public ScriptType Type { get; set; }
        /// <summary>
        /// This property sets the name of a hidden field which this control can use.
        /// </summary>
        [Browsable(true),
        Category("Behavior"),
        Themeable(false),
        DefaultValue(""),
        Description("This property sets the name of a hidden field which this control can use.")]
        public string HiddenFieldName { get; set; }
        /// <summary>
        /// This sets the initial value of the hidden field which this control uses.
        /// </summary>
        [Browsable(true),
        Category("Behavior"),
        Themeable(false),
        DefaultValue(""),
        Description("This property sets the initial value of the hidden field which this control uses.")]
        public string HiddenFieldInitialValue { get; set; }
        /// <summary>
        /// This property sets the name of an expando attribute which this control can use.
        /// </summary>
        [Browsable(true),
        Category("Behavior"),
        Themeable(false),
        DefaultValue(""),
        Description("This property sets the name of an expando attribute which this control can use.")]
        public string ExpandoAttributeName { get; set; }
        /// <summary>
        /// This property sets the name of the expando attribute which this control uses.
        /// </summary>
        [Browsable(true),
        Category("Behavior"),
        Themeable(false),
        DefaultValue(""),
        Description("This property sets the name of the expando attribute which this control uses.")]
        public string ExpandoAttributeValue { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Browsable(true),
        Category("Behavior"),
        Themeable(false),
        DefaultValue(""),
        Description("This property sets the name for an array which this control can use.")]
        public string ArrayName { get; set; }
        /// <summary>
        /// This property sets the values for the array which this control uses.
        /// </summary>
        [Browsable(true),
        Category("Behavior"),
        Themeable(false),
        DefaultValue(""),
        Description("This property sets the values for the array which this control uses.")]
        public string ArrayValue { get; set; }
        /// <summary>
        /// This property indicates whether to include script tags in the rendered output.
        /// </summary>
        [Browsable(true),
        Category("Behavior"),
        Themeable(false),
        DefaultValue(false),
        Description("This property indicates whether to include script tags in the rendered output.")]
        public bool IncludeScriptTags { get; set; }

        #region Protected Methods

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (!string.IsNullOrEmpty(this.HiddenFieldName))
            {
                this.RegisterHiddenField(this.HiddenFieldName, this.HiddenFieldInitialValue);
            }

            if (!string.IsNullOrEmpty(this.ArrayName))
            {
                this.RegisterArrayDeclaration(this.ArrayName, this.ArrayValue);
            }

            if (!string.IsNullOrEmpty(this.ExpandoAttributeName))
            {
                this.RegisterExpandoAttribute(this.UniqueID, this.ExpandoAttributeName, this.ExpandoAttributeValue, true);
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            StringBuilder sb = new StringBuilder();
            base.Render(new HtmlTextWriter(new StringWriter(sb)));
            switch (Type)
            {
                case ScriptType.Startup:
                    this.RegisterStartupScript(typeof(InlineScript), this.UniqueID, sb.ToString(), this.IncludeScriptTags);
                    break;
                case ScriptType.OnSubmit:
                    this.RegisterOnSubmitStatement(typeof(InlineScript), this.UniqueID, sb.ToString());
                    break;
                case ScriptType.Block:
                    this.RegisterClientScriptBlock(typeof(InlineScript), this.UniqueID, sb.ToString(), this.IncludeScriptTags);
                    break;
                default:
                    this.RegisterClientScriptBlock(typeof(InlineScript), this.UniqueID, sb.ToString(),  this.IncludeScriptTags);
                    break;

            }
        }
        #endregion

        public enum ScriptType
        {
            Startup,
            OnSubmit,
            Block
        }
    }
}
