using System.Globalization;
using Devantler.Keys.Age;

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
    await AgeKeygen.ToFile(path);

    // Assert
    Assert.True(File.Exists(path));
    string keyString = await File.ReadAllTextAsync(path);
    string[] lines = keyString.Split("\n");
    string publicKey = lines[1].Split(" ")[3];
    string privateKey = lines[2];
    var createdAt = DateTime.Parse(lines[0].Split(" ")[2], CultureInfo.InvariantCulture);
    var key = new AgeKey(
      publicKey,
      privateKey,
      createdAt
    );
    Assert.NotNull(key);
    Assert.NotEmpty(key.PublicKey);
    Assert.NotEmpty(key.PrivateKey);
    Assert.Contains($"""
    # created: {key.CreatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz", CultureInfo.InvariantCulture)}
    # public key: {key.PublicKey}
    {key.PrivateKey}
    """, keyString, StringComparison.Ordinal);
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
