using Xunit;

namespace DocAssistant.Test
{
	public class UnitTest1
	{
		[Fact]
		public void Test1()
		{
			var a = 1 + 1;

			Assert.Equal(2, a);
		}
	}
}