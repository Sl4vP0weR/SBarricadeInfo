namespace SBarricadeInfo;

/// <summary>
/// This class is XML serializing to config file. <br/>
/// Docs: <see href="https://docs.microsoft.com/en-us/dotnet/api/system.xml.serialization.xmlserializer?view=netframework-4.7.2"/>
/// </summary>
public partial class Config : IRocketPluginConfiguration
{
    public string Permission = "barricade_info";
    public void LoadDefaults()
    {
    }
}