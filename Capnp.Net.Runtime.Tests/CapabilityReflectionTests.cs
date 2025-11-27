using System;
using Capnp.Rpc;
using Capnproto_test.Capnp.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Capnp.Net.Runtime.Tests;

[TestClass]
[TestCategory("Coverage")]
public class CapabilityReflectionTests
{
    [TestMethod]
    public void ValidateCapabilityInterface()
    {
        Assert.Throws<ArgumentNullException>(() => CapabilityReflection.ValidateCapabilityInterface(null));
        CapabilityReflection.ValidateCapabilityInterface(typeof(ITestInterface));
        Assert.Throws<InvalidCapabilityInterfaceException>(() =>
            CapabilityReflection.ValidateCapabilityInterface(typeof(CapabilityReflectionTests)));
    }

    [TestMethod]
    public void IsValidCapabilityInterface()
    {
        Assert.Throws<ArgumentNullException>(() => CapabilityReflection.IsValidCapabilityInterface(null));
        Assert.IsTrue(CapabilityReflection.IsValidCapabilityInterface(typeof(ITestInterface)));
        Assert.IsFalse(CapabilityReflection.IsValidCapabilityInterface(typeof(CapabilityReflectionTests)));
    }
}