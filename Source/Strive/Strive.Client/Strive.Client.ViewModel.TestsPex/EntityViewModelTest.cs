// <copyright file="EntityViewModelTest.cs">Copyright ©  2010</copyright>

using System;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using NUnit.Framework;
using Strive.Client.ViewModel;

namespace Strive.Client.ViewModel
{
    [TestFixture]
    [PexClass(typeof(EntityViewModel))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    public partial class EntityViewModelTest
    {
        [PexMethod]
        public string ToString01([PexAssumeUnderTest]EntityViewModel target)
        {
            string result = target.ToString();
            return result;
            // TODO: add assertions to method EntityViewModelTest.ToString01(EntityViewModel)
        }
    }
}
