using ImageMagick;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mosaic.Tests
{
	[TestClass]
	public class MagickImageExtensionsTests
	{
		[TestMethod]
		public void ConvertToSquare_HorizontalImageWithCenter_IsSquare()
		{
			using var expected = new MagickImage(@"Images\ConvertSquare\center_horizontal_expected.png");
			using var input = new MagickImage(@"Images\ConvertSquare\center_horizontal_input.png");

			input.ConvertToSquare(TileImageMode.Center);

			ImageAssert.AreEqual(expected, input);
		}

		[TestMethod]
		public void ConvertToSquare_VerticalImageWithCenter_IsSquare()
		{
			using var expected = new MagickImage(@"Images\ConvertSquare\center_vertical_expected.png");
			using var input = new MagickImage(@"Images\ConvertSquare\center_vertical_input.png");

			input.ConvertToSquare(TileImageMode.Center);

			ImageAssert.AreEqual(expected, input);
		}

		[TestMethod]
		public void ConvertToSquare_AlreadySquare_IsUntouched()
		{
			using var expected = new MagickImage(@"Images\ConvertSquare\center_already_square_expected_input.png");
			using var input = new MagickImage(expected);

			input.ConvertToSquare(TileImageMode.Center);

			ImageAssert.AreEqual(expected, input);
		}
	}
}
