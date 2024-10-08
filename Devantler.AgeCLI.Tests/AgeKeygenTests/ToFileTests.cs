namespace Devantler.AgeCLI.Tests.AgeKeygenTests;

/// <summary>
/// Tests for the <see cref="AgeKeygen.ToFile"/> method.
/// </summary>
public class ToFileTests
{
  /// <summary>
  /// Tests that an age key is written to a file.
  /// </summary>
  [Fact]
  public async Task ToFile_ShouldWriteAgeKeyToFile()
  {
    // Arrange
    string path = "test.key";
    if (File.Exists(path))
    {
      File.Delete(path);
    }

    // Act
    var ageKey = await AgeKeygen.ToFile(path);
    string keyString = await File.ReadAllTextAsync(path);
    keyString = keyString[..^1];
    keyString = keyString.Replace("\n", Environment.NewLine, StringComparison.InvariantCulture);

    // Assert
    Assert.True(File.Exists(path));
    Assert.Equal(keyString, ageKey.ToString());
  }

  /// <summary>
  /// Tests that an <see cref="InvalidOperationException"/> is thrown when the age-keygen CLI command fails.
  /// </summary>
  [Fact]
  public async Task ToFile_GivenInvalidOutputPath_ShouldThrowInvalidOperationException()
  {
    // Arrange
    string path = "/dev/null";

    // Act
    async Task Act() => await AgeKeygen.ToFile(path).ConfigureAwait(false);

    // Assert
    _ = await Assert.ThrowsAsync<InvalidOperationException>(Act);
  }
}
