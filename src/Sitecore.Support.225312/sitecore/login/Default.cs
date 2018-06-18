namespace Sitecore.Support.sitecore.login
{
  using Sitecore.DependencyInjection;
  using Sitecore.Web;
  using System;

  /// <summary>
  /// </summary>
  [AllowDependencyInjection]
  public partial class Default : Sitecore.sitecore.login.Default
  {
    protected override bool LoggingIn()
    {
      var result = base.LoggingIn();

      if (result)
      {
        var startUrlField = typeof(Sitecore.sitecore.login.Default).GetField("startUrl", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (startUrlField != null)
        {
          try
          {
            var currentStartUrl = (string)startUrlField.GetValue(this);

            if (string.IsNullOrEmpty(currentStartUrl))
              return result;

            var currentHostName = WebUtil.GetHostName();
            var hostNameStart = currentStartUrl.IndexOf(currentHostName);

            if (hostNameStart != -1 && currentHostName.Length > 0)
            {
              var relativeUrlStart = currentStartUrl.IndexOf("/", hostNameStart + currentHostName.Length);

              if (relativeUrlStart == -1)
                relativeUrlStart = hostNameStart + currentHostName.Length;

              startUrlField.SetValue(this, currentStartUrl.Substring(relativeUrlStart)); 
            }           
          }       
          catch(Exception e)
          {
            Log.Warn("Sitecore.Support.225312: ", e, this);
          }
        }         
      }

      return result;
    }   
  }
}