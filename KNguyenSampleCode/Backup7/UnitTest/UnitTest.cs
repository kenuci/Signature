using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KNguyenSampleCode;

namespace UnitTest
{
	/// <summary>
	/// Summary description for UnitTest
	/// </summary>
	[TestClass]
	public class UnitTest
	{
		public UnitTest()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		private TestContext testContextInstance;

		/// <summary>
		///Gets or sets the test context which provides
		///information about and functionality for the current test run.
		///</summary>
		public TestContext TestContext
		{
			get
			{
				return testContextInstance;
			}
			set
			{
				testContextInstance = value;
			}
		}

		#region Additional test attributes
		//
		// You can use the following additional attributes as you write your tests:
		//
		// Use ClassInitialize to run code before running the first test in the class
		// [ClassInitialize()]
		// public static void MyClassInitialize(TestContext testContext) { }
		//
		// Use ClassCleanup to run code after all tests in a class have run
		// [ClassCleanup()]
		// public static void MyClassCleanup() { }
		//
		// Use TestInitialize to run code before running each test 
		// [TestInitialize()]
		// public void MyTestInitialize() { }
		//
		// Use TestCleanup to run code after each test has run
		// [TestCleanup()]
		// public void MyTestCleanup() { }
		//
		#endregion

		[TestMethod]
		public void TestAlphaNumeric(){
			Assert.AreEqual( StringProcessing.Filter.AlphaNumeric( null), string.Empty);
			Assert.AreEqual( StringProcessing.Filter.AlphaNumeric( "ab@#%235"), "ab235");
			Assert.AreEqual( StringProcessing.Filter.AlphaNumeric( "714-253-2195 ext.200"), "7142532195ext200");
			Assert.AreEqual( StringProcessing.Filter.AlphaNumeric( "!@#$%"), string.Empty);
		}

		[TestMethod]
		public void TestAlphaNumericSpaces(){
			Assert.AreEqual( StringProcessing.Filter.AlphaNumericSpaces( null), string.Empty);
			Assert.AreEqual( StringProcessing.Filter.AlphaNumericSpaces( "ab@# %235"), "ab 235");
			Assert.AreEqual( StringProcessing.Filter.AlphaNumericSpaces( "714-253-2195 ext.200"), "7142532195 ext200");
			Assert.AreEqual( StringProcessing.Filter.AlphaNumericSpaces( "!@#$%"), string.Empty);
		}

		[TestMethod]
		public void TestTrimText(){
			string strLoremIpsum = @"Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";

			Assert.AreEqual( StringProcessing.TrimText( string.Empty, 10, 10, true), string.Empty);
			Assert.AreEqual( StringProcessing.TrimText( strLoremIpsum, 10, 10, false), string.Empty); //sentence-boundary
			Assert.AreEqual( StringProcessing.TrimText( strLoremIpsum, 10, 10, true), "Lorem"); //word-boundary
			Assert.AreEqual( StringProcessing.TrimText( strLoremIpsum, 100, 5, true), @"Lorem ipsum dolor sit amet,");
			Assert.AreEqual( StringProcessing.TrimText( strLoremIpsum, 5000, 500, true), strLoremIpsum);
			Assert.AreEqual( StringProcessing.TrimText( "<p>hello world</p>", 20, 20, true), "\nhello world\n");
		}
	}
}
