﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Microsoft.VisualStudio.ProjectSystem.VS.Properties
{
    [ProjectSystemTrait]
    public class OutputTypeExValueProviderTests
    {
        [Theory]
        [InlineData("WinExe", "0")]
        [InlineData("Exe", "1")]
        [InlineData("Library", "2")]
        [InlineData("WinMDObj", "3")]
        [InlineData("AppContainerExe", "4")]
        [InlineData("", "0")]
        public async void GetEvaluatedValue(object outputTypePropertyValue, string expectedMappedValue)
        {
            var properties = ProjectPropertiesFactory.Create(
                UnconfiguredProjectFactory.Create(),
                new PropertyPageData()
                {
                    Category = ConfigurationGeneral.SchemaName,
                    PropertyName = ConfigurationGeneral.OutputTypeProperty,
                    Value = outputTypePropertyValue
                });
            var provider = new OutputTypeExValueProvider(properties);

            var actualPropertyValue = await provider.OnGetEvaluatedPropertyValueAsync(string.Empty, null);
            Assert.Equal(expectedMappedValue, actualPropertyValue);
        }

        [Theory]
        [InlineData("0", "WinExe")]
        [InlineData("1", "Exe")]
        [InlineData("2", "Library")]
        [InlineData("3", "WinMDObj")]
        [InlineData("4", "AppContainerExe")]
        public async void SetValue(string incomingValue, string expectedOutputTypeValue)
        {
            var setValues = new List<object>();
            var properties = ProjectPropertiesFactory.Create(
                UnconfiguredProjectFactory.Create(),
                new PropertyPageData()
                {
                    Category = ConfigurationGeneral.SchemaName,
                    PropertyName = ConfigurationGeneral.OutputTypeProperty,
                    Value = "InitialValue",
                    SetValues = setValues
                });
            var provider = new OutputTypeExValueProvider(properties);

            var actualPropertyValue = await provider.OnSetPropertyValueAsync(incomingValue, null);
            Assert.Equal(setValues.Single(), expectedOutputTypeValue);
        }

        public async void SetValue_ThrowsKeyNotFoundException()
        {
            var setValues = new List<object>();
            var properties = ProjectPropertiesFactory.Create(
                UnconfiguredProjectFactory.Create(),
                new PropertyPageData()
                {
                    Category = ConfigurationGeneral.SchemaName,
                    PropertyName = ConfigurationGeneral.OutputTypeProperty,
                    Value = "InitialValue",
                    SetValues = setValues
                });
            var provider = new OutputTypeExValueProvider(properties);

            await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            {
                await provider.OnSetPropertyValueAsync("InvalidValue", null);
            });
        }
    }
}
