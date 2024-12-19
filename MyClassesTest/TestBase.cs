using System.Reflection;

namespace MyClassesTest;

public class TestBase
{
    public TestContext? TestContext { get; set; }
    public string OutputMessage { get; set; } = string.Empty;

    #region GetTestSetting Method
    protected T GetTestSetting<T>(string name, T defaultValue)
    {
        T ret = defaultValue;

        try
        {
            var tmp = TestContext?.Properties[name];
            if (tmp != null)
            {
                ret = (T)Convert.ChangeType(tmp, typeof(T));
            }
        }
        catch
        {
            // Ignore exception, return the defaultValue
        }

        return ret;
    }
    #endregion

    #region GetTestName Method
    protected string GetTestName()
    {
        var ret = TestContext?.TestName;
        if (ret == null)
        {
            return string.Empty;
        }
        else
        {
            return ret.ToString();
        }
    }
    #endregion

    #region GetFileName Method
    protected string GetFileName(string name, string defaultValue)
    {
        string fileName = GetTestSetting<string>(name, defaultValue);
        fileName = fileName.Replace("[AppDataPath]", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));

        return fileName;
    }
    #endregion

    #region WriteDescription Method
    protected void WriteDescription(Type typ)
    {
        // Retrieve the [Description] attribute if it exists
        DescriptionAttribute? attr = GetAttribute<DescriptionAttribute>(typ);
        if (attr != null)
        {
            // Output the test description
            TestContext?.WriteLine("Test Purpose: " + attr.Description);
        }
    }
    #endregion

    #region WriteOwner Method
    protected void WriteOwner(Type typ)
    {
        // Retrieve the [Owner] attribute if it exists
        OwnerAttribute? attr = GetAttribute<OwnerAttribute>(typ);
        if (attr != null)
        {
            // Output the test owner
            TestContext?.WriteLine("Test Owner: " + attr.Owner);
        }
    }
    #endregion

    #region GetAttribute Method
    protected TAttr? GetAttribute<TAttr>(Type typ)
    {
        // Get the currently executing test name
        string testName = GetTestName();

        // Retrieve the <TAttr> attribute if it exists
        Attribute? attr = typ.GetMethod(testName)?
          .GetCustomAttribute(typeof(TAttr));
        if (attr != null)
        {
            // Cast the attribute to a <TAttr> type
            return (TAttr)Convert.ChangeType(attr, typeof(TAttr));
        }
        else
        {
            return default;
        }
    }
    #endregion
}
