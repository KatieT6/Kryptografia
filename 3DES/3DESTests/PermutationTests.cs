using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _3DES.Tests
{
    [TestClass()]
    public class PermutationTests
    {
        [TestMethod()]
        public void Permute_ShouldReturnCorrectOutput_WhenGivenValidInput()
        {
            // Arrange
            int[] input = new int[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            int[] expectedOutput = new int[] { 57, 49, 41, 33, 25, 17, 9, 1, 58, 50, 42, 34, 26, 18, 10, 3 };

            // Act
            int[] result = DesAlgorithm.Permute(input, PC_Arrays.PC1);

            // Assert
            Assert.AreEqual(expectedOutput, result);
        }

    }
}