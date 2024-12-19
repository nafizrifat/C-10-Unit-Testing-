using MyClasses;

namespace MyClassesTest;

[TestClass]
public class FileProcessTest : TestBase
{
    #region Class Initialize and Cleanup Methods
    [ClassInitialize()]
    public static void ClassInitialize(TestContext tc)
    {
        // This code runs once before all tests run in this class
        tc.WriteLine("In FileProcessTest.ClassInitialize() method");
    }

    [ClassCleanup()]
    public static void ClassCleanup()
    {
        // This code runs once after all tests in this class have run
        // NOTE: TestContext is not available in here
    }
    #endregion

    #region Test Initialize and Cleanup Methods
    [TestInitialize()]
    public void TestInitialize()
    {
        TestContext?.WriteLine("In FileProcessTest.TestInitialize() method");
        WriteDescription(this.GetType());
        WriteOwner(this.GetType());

        // Check to see which test we are running
        string testName = GetTestName();
        if (testName == "FileNameDoesExist")
        {
            // Get Good File Name
            string fileName = GetFileName("GoodFileName", TestConstants.GOOD_FILE_NAME);

            // Create the Good File
            File.AppendAllText(fileName, "Some Text");
        }
    }

    [TestCleanup()]
    public void TestCleanup()
    {
        // Check to see which test we are running
        string testName = GetTestName();
        if (testName == "FileNameDoesExist")
        {
            // Get Good File Name
            string fileName = GetFileName("GoodFileName", TestConstants.GOOD_FILE_NAME);

            // Delete the Good File if it Exists
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
        }
    }
    #endregion

    [TestMethod]
    [Ignore]
    [Timeout(3000)]
    public void SimulateTimeout()
    {
        System.Threading.Thread.Sleep(4000);
    }

    [TestMethod]
    [TestCategory("NoException")]
    [Owner("NafizR2")]
    [Priority(3)]
    [Description("Check to see if a file exists.")]
    public void FileNameDoesExist()
    {
        // Arrange
        FileProcess fp = new();
        bool fromCall;

        // Add Messages to Test Output
        string fileName = GetFileName("GoodFileName", TestConstants.GOOD_FILE_NAME);
        TestContext?.WriteLine($"Checking for file: '{fileName}'");

        
        // Act
        fromCall = fp.FileExists(fileName);

        // Assert
        Assert.IsTrue(fromCall,"File {0} does not exist", fileName);
    }

    [TestMethod]
    [TestCategory("NoException")]
    [Owner("NafizR3")]
    [Priority(3)]
    [Description("Check to see if file does not exist.")]
    public void FileNameDoesNotExist()
    {
        // Arrange
        FileProcess fp = new();
        bool fromCall;

        // Add Messages to Test Output
        string fileName = GetTestSetting<string>("BadFileName", TestConstants.BAD_FILE_NAME);
        TestContext?.WriteLine($"Checking file '{fileName}' does NOT exist.");

        // Act
        fromCall = fp.FileExists(fileName);

        // Assert
        Assert.IsFalse(fromCall);
    }

    [TestMethod]
    [TestCategory("Exception")]
    [Owner("NafizR")]
    [Priority(2)]
    [Description("Check for a thrown ArgumentNullException using try...catch.")]
    public void FileNameNullOrEmpty_UsingTryCatch_ShouldThrowArgumentNullException()
    {
        // Arrange
        FileProcess fp;
        string fileName = string.Empty;
        bool fromCall = false;

        // Add Messages to Test Output
        OutputMessage = GetTestSetting<string>("EmptyFileMsg", TestConstants.EMPTY_FILE_MSG);
        TestContext?.WriteLine(OutputMessage);

        try
        {
            // Act
            fp = new();

            fromCall = fp.FileExists(fileName);

            // Assert: Fail because we should not get here
            OutputMessage = GetTestSetting<string>("EmptyFileFailMsg", TestConstants.EMPTY_FILE_FAIL_MSG);
            Assert.Fail(OutputMessage);
        }
        catch (ArgumentNullException)
        {
            // Assert: Test was a success
            Assert.IsFalse(fromCall);
        }
    }

    [TestMethod]
    [TestCategory("Exception")]
    [Owner("NafizR")]
    [Priority(1)]
    [ExpectedException(typeof(ArgumentNullException))]
    [Description("Check for a thrown ArgumentNullException using ExpectedException.")]
    public void FileNameNullOrEmpty_UsingExpectedExceptionAttribute()
    {
        // Arrange
        FileProcess fp = new();
        string fileName = string.Empty;
        //string fileName = "Test";  // Uncomment to test failure
        bool fromCall;

        // Add Messages to Test Output
        OutputMessage = GetTestSetting<string>("EmptyFileMsg", TestConstants.EMPTY_FILE_MSG);
        TestContext?.WriteLine(OutputMessage);

        // Act
        fromCall = fp.FileExists(fileName);

        // Assert: Fail because we should not get here
        OutputMessage = GetTestSetting<string>("EmptyFileFailMsg", TestConstants.EMPTY_FILE_FAIL_MSG);
        Assert.Fail(OutputMessage);
    }
}