using System;
using System.Web;
using System.Web.UI;

namespace DaleWare.Web.UI.Controls
{
    public static class ControlExtensions
    {
        private const string CANNOT_ACCESS_SCRIPT_MANAGER = "Cannot access the ScriptManager component on this page.";

        /// <summary>
        /// this method indicates whether the ASP.NET Control class is hosted in a page with an ASP.NET ScriptManager control.
        /// </summary>
        /// <param name="control">The control instance</param>
        /// <returns>true if the control instance is in a page with an ASP.NET Ajaxt ScriptManager control; false otherwise</returns>
        public static bool HasPageScriptManager(this Control control)
        {
            ScriptManager currentScriptManager = ScriptManager.GetCurrent(control.Page);
            if (currentScriptManager == null)
            {
                throw new Exception(ControlExtensions.CANNOT_ACCESS_SCRIPT_MANAGER);
            }
            return true;
        }

        /// <summary>
        /// This method indicates whether the ASP.NET control derived from Control is inside an ASP.NET update panel.
        /// </summary>
        /// <param name="control">The control instance</param>
        /// <returns>true if the control instance is inside an update panel; false otherwise</returns>
        public static bool IsInsideUpdatePanel(this Control control)
        {
            bool rv = false;
            Control parent = control.Parent;
            while (parent != null)
            {
                if (parent.GetType().FullName == typeof(System.Web.UI.UpdatePanel).FullName)
                {
                    rv = true;
                    break;
                }
                parent = parent.Parent;
            }
            return rv;
        }

        /// <summary>
        /// This method provides the ability for an ASP.NET Control class to register an expando attribute either with the 
        /// Page ClientScriptManager or the ASP.NET Ajax ScriptManager, to enable the control to function properly in a partial page 
        /// rendering scenario.
        /// </summary>
        /// <param name="control">The Control class instance</param>
        /// <param name="controlId">the id of the Control control</param>
        /// <param name="attributeName">The name of the expando attribute, such as evaluationfunction</param>
        /// <param name="attributeValue">The value to set on the expando attribute</param>
        /// <param name="encode">Whether to encode/escape the attribute value in the page output. Normally true is preferred.</param>
        public static void RegisterExpandoAttribute(this Control control, string controlId, string attributeName, string attributeValue, bool encode)
        {
            if (control.IsInsideUpdatePanel() && control.HasPageScriptManager())
            {
                ScriptManager.RegisterExpandoAttribute(control, controlId, attributeName, attributeValue, encode);
            }
            else
            {
                control.Page.ClientScript.RegisterExpandoAttribute(controlId, attributeName, attributeValue, encode);
            }
        }

        /// <summary>
        /// This method provides the ability for an ASP.NET Control class to register a startup script either with the 
        /// Page ClientScriptManager or the ASP.NET Ajax ScriptManager, to enable the control to function properly in a partial page 
        /// rendering scenario.
        /// </summary>
        /// <param name="control">The Control class instance</param>
        /// <param name="type">Type of the control</param>
        /// <param name="key">The key with which to register the script</param>
        /// <param name="script">The string with the script</param>
        /// <param name="addScriptTags">Whether to enclose the script in script tags.</param>
        public static void RegisterStartupScript(this Control control, Type type, string key, string script, bool addScriptTags)
        {
            if (control.IsInsideUpdatePanel() && control.HasPageScriptManager())
            {
                ScriptManager.RegisterStartupScript(control, type, key, script, addScriptTags);
            }
            else
            {
                if (!control.Page.ClientScript.IsStartupScriptRegistered(type, key))
                {
                    control.Page.ClientScript.RegisterStartupScript(type, key, script, addScriptTags);
                }
            }
        }

        /// <summary>
        /// This method provides the ability for an ASP.NET Control class to register a script include either with the 
        /// Page ClientScriptManager or the ASP.NET Ajax ScriptManager, to enable the control to function properly in a partial page 
        /// rendering scenario.
        /// </summary>
        /// <param name="control">The Control class instance</param>
        /// <param name="type">Type of the control,normally the control class</param>
        /// <param name="key">The key with which to register the script</param>
        /// <param name="url">The client side URL of the script include file.</param>
        public static void RegisterClientScriptInclude(this Control control, Type type, string key, string url)
        {
            if (control.IsInsideUpdatePanel() && control.HasPageScriptManager())
            {
                ScriptManager.RegisterClientScriptInclude(control, type, key, url);
            }
            else
            {
                if (!control.Page.ClientScript.IsClientScriptIncludeRegistered(type, key))
                {
                    control.Page.ClientScript.RegisterClientScriptInclude(type, key, url);
                }
            }
        }

        /// <summary>
        /// This method provides the ability for an ASP.NET Control class to register a client script block either with the 
        /// Page ClientScriptManager or the ASP.NET Ajax ScriptManager, to enable the control to function properly in a partial page 
        /// rendering scenario.
        /// </summary>
        /// <param name="control">The Control class instance</param>
        /// <param name="type">Type of the control</param>
        /// <param name="key">The key with which to register the script</param>
        /// <param name="url">The client side URL of the script include file.</param>
        public static void RegisterClientScriptBlock(this Control control, Type type, string key, string script, bool addScriptTags)
        {
            if (control.IsInsideUpdatePanel() && control.HasPageScriptManager())
            {
                ScriptManager.RegisterClientScriptBlock(control, type, key, script, addScriptTags);
            }
            else
            {
                if (!control.Page.ClientScript.IsClientScriptBlockRegistered(type, key))
                {
                    control.Page.ClientScript.RegisterClientScriptBlock(type, key, script, addScriptTags);
                }
            }
        }
        /// <summary>
        /// This method provides the ability for an ASP.NET Control class to register a client script resource either with the 
        /// Page ClientScriptManager or the ASP.NET Ajax ScriptManager, to enable the control to function properly in a partial page 
        /// rendering scenario.
        /// </summary>
        /// <param name="control">The control class instance.</param>
        /// <param name="type">Type of the control</param>
        /// <param name="resourceName">The resource name</param>
        public static void RegisterClientScriptResource(this Control control, Type type, string resourceName)
        {
            if (control.IsInsideUpdatePanel() && control.HasPageScriptManager())
            {
                ScriptManager.RegisterClientScriptResource(control, type, resourceName);
            }
            else
            {
                control.Page.ClientScript.RegisterClientScriptResource(type, resourceName);
            }

        }
        /// <summary>
        /// This method provides the ability for an ASP.NET control class to register for a submit statement either with the Page
        /// ClientScriptManager or the ASP.NET Ajax ScriptManager, to enable the control to function property in a partial page rendering
        /// scenario.
        /// </summary>
        /// <param name="control">The control instance</param>
        /// <param name="type">Type of the control</param>
        /// <param name="key">String to use as the registration key</param>
        /// <param name="script">String that contains the script code block</param>
        public static void RegisterOnSubmitStatement(this Control control, Type type, string key, string script)
        {
            if (control.IsInsideUpdatePanel() && control.HasPageScriptManager())
            {
                ScriptManager.RegisterOnSubmitStatement(control, typeof(Control), key, script);
            }
            else
            {
                control.Page.ClientScript.RegisterOnSubmitStatement(type, key, script);
            }
        
        }

        /// <summary>
        /// This method provides the capability to register a hidden field either with the Page ClientScriptManager or the 
        /// ASP.NET Ajax ScriptManager so that it can function correctly in a partial page rendering scenario.
        /// </summary>
        /// <param name="control">The control class instance.</param>
        /// <param name="hiddenFieldName">The name of the hidden field.</param>
        /// <param name="hiddenFieldInitialValue">The intended initial value of the hidden field.</param>
        public static void RegisterHiddenField(this Control control, string hiddenFieldName, string hiddenFieldInitialValue)
        {
            if (control.IsInsideUpdatePanel() && control.HasPageScriptManager())
         
            {
                ScriptManager.RegisterHiddenField(control, hiddenFieldName, hiddenFieldInitialValue);
            }
            else
            {
                control.Page.ClientScript.RegisterHiddenField(hiddenFieldName, hiddenFieldInitialValue);
            }

        }
        /// <summary>
        /// This method provides the capability to register an array declaration either with the Page ClientScriptManager
        /// or the ASP.NET ScriptManager so that it can function correctly in a partial page rendering scenario.
        /// </summary>
        /// <param name="control">The control class instance.</param>
        /// <param name="arrayName">Name of the array. See the MSDN documentation for usage.</param>
        /// <param name="arrayValue">Value(s) of the array. See the MSDN documentation for usage.</param>
        public static void RegisterArrayDeclaration(this Control control, string arrayName, string arrayValue)
        {
            if (control.IsInsideUpdatePanel() && control.HasPageScriptManager())
            {
                ScriptManager.RegisterArrayDeclaration(control, arrayName, arrayValue);
            }
            else
            {
                control.Page.ClientScript.RegisterArrayDeclaration(arrayName, arrayValue);
            }
        }       
    }
}
