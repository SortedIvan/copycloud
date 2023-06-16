using Azure.Storage.Blobs;
using projectservice;
using projectservice.Dto;
using projectservice.Models;
using projectservice.Services;
using projectservice.Utility;
using Xunit;

namespace projectservice.unittests
{
    public class UnitTests
    {
      

        [Theory]
        [InlineData("invitee@example.com;sender123;project123;secret123")]
        public void ParseInviteToken_ReturnsValidResult(string token)
        {
            // Act
            var result = InvitationTokenUtil.ParseInviteToken(token);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("invitee@example.com", result.Item1);
            Assert.Equal("sender123", result.Item2);
            Assert.Equal("project123", result.Item3);
            Assert.Equal("secret123", result.Item4);
        }

        [Theory]
        [InlineData("sender123;project123;secret123")]
        public void ParseInviteLink_ReturnsValidResult(string token)
        {
            // Act
            var result = InvitationTokenUtil.ParseInviteLink(token);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("sender123", result.Item1);
            Assert.Equal("project123", result.Item2);
            Assert.Equal("secret123", result.Item3);
        }

        [Theory]
        [InlineData("plainText")]
        public void Base64Encode_DecodesSuccessfully(string plainText)
        {
            // Act
            var encodedText = InvitationTokenUtil.Base64Encode(plainText);
            var decodedText = InvitationTokenUtil.Base64Decode(encodedText);

            // Assert
            Assert.NotNull(encodedText);
            Assert.NotNull(decodedText);
            Assert.NotEqual(encodedText, plainText);
            Assert.Equal(decodedText, plainText);
        }

        [Theory]
        [InlineData("value123")]
        public void sha256_hash_GeneratesHash(string value)
        {
            // Act
            var hash = InvitationTokenUtil.sha256_hash(value);

            // Assert
            Assert.NotNull(hash);
            Assert.NotEmpty(hash);
        }
 
    }
}