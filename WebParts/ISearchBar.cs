using System;
  using System.Web;
  using System.Web.Security;
  using System.Security.Permissions;
  using System.Web.UI;
  using System.Web.UI.WebControls;
  using System.Web.UI.WebControls.WebParts;

namespace DaleWare.Web.UI.Controls.WebParts
{
    [AspNetHostingPermission(SecurityAction.Demand,
        Level = AspNetHostingPermissionLevel.Minimal),
    AspNetHostingPermission(SecurityAction.InheritanceDemand,
      Level = AspNetHostingPermissionLevel.Minimal)]
    public interface ISearchBar
    {
        string SearchString { get; set; }
    }
}
