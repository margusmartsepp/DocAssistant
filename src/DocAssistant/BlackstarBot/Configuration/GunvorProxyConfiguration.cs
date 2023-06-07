namespace DocAssistant.Configuration;

public class GunvorProxyConfiguration
{
	public string Uri { get; set; }
	public bool BypassProxyOnLocal { get; set; }
	public bool UseDefaultCredentials { get; set; }
	public bool IsEnabled { get; set; }
}