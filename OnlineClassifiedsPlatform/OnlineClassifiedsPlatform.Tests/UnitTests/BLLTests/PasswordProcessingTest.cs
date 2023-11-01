using Bogus;
using System;
using NUnit.Framework;
using System.Threading.Tasks;
using OnlineClassifiedsPlatform.BLL.Interfaces;
using OnlineClassifiedsPlatform.BLL.Cryptography;

namespace OnlineClassifiedsPlatform.Tests.UnitTests.BLLTests
{
    [TestFixture]
    public class PasswordProcessingTest
    {
        private IPasswordProcessing _passProcessing;
        private string _testPassword;
        private string _salt;

        [SetUp]
        public void SetUp()
        {
            _passProcessing = new PasswordProcessing();
            _testPassword = new Faker().Internet.Password(length: 6);
            _salt = _passProcessing.GenerateSalt();
        }

        [Test, Order(0)]
        public async Task GetHashCode_GettingHashCode_ReturnsTrue()
        {
            var generatedHashCode = _passProcessing.GetHashCode(_testPassword, _salt);
            var testGeneratedHashCode = _passProcessing.GetHashCode(_testPassword, _salt);

            Assert.AreEqual(generatedHashCode, testGeneratedHashCode);
        }

        [Test, Order(1)]
        public async Task GetHashCode_GettingHashCodeWithDifferentSalt_ReturnsFalse()
        {
            var generatedHashCode = _passProcessing.GetHashCode(_testPassword, _salt);

            var testSalt = _passProcessing.GenerateSalt();
            var testGeneratedHashCode = _passProcessing.GetHashCode(_testPassword, testSalt);

            Assert.AreNotEqual(generatedHashCode, testGeneratedHashCode);
        }

        [Test, Order(2)]
        public async Task GetHashCode_ComparingHashCodesWithDifferentPasswords_ReturnsTrue()
        {
            var secondPassword = new Faker().Internet.Password(length: 6);
            var generatedHashCode = _passProcessing.GetHashCode(_testPassword, _salt);
            var testGeneratedHashCode = _passProcessing.GetHashCode(secondPassword, _salt);

            Assert.AreNotEqual(generatedHashCode, testGeneratedHashCode);
        }

        [Test, Order(3)]
        public void GetHashCode_NullArguments_ThrowsNullReferenceException()
        {
            Assert.Throws<ArgumentException>(
                () => _passProcessing.GetHashCode(null, null));
        }


        [Test, Order(4)]
        public void GetHashCode_EmptyArguments_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(
                () => _passProcessing.GetHashCode("", ""));
        }

        [Test, Order(5)]
        public void GetHashCode_WhiteSpaceArguments_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(
                () => _passProcessing.GetHashCode(" ", " "));
        }
    }
}
