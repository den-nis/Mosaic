using ImageMagick;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Mosaic.Tests
{
	[TestClass]
	public class MagickColorExtensionsTests
	{
		[TestMethod]
		public void GetHue_RedMaxImage_HasHue302()
		{
			MagickColor color = new(255, 0, 244);
			Assert.AreEqual(302, color.GetHue());
		}
	}
}
