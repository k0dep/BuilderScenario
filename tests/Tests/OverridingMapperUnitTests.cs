using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEditor.VersionControl;

namespace BuilderScenario.Tests
{
    public class OverridingMapperUnitTests
    {
        [Test]
        public void ToString_ShouldReturnCurrentInsteadOrigin()
        {
            // Act
            var service = new OverridingMapper("current", "origin");

            // Arrange
            var result = service.ToString();

            // Assert
            Assert.AreEqual("current", result);
        }
        
        [Test]
        public void ToString_ShouldReturnOriginIfCurrentNull()
        {
            // Act
            var service = new OverridingMapper(null, "origin");

            // Arrange
            var result = service.ToString();

            // Assert
            Assert.AreEqual("origin", result);
        }
        
        [Test]
        public void ToString_ShouldReturnCurrentIfOriginNull()
        {
            // Act
            var service = new OverridingMapper("current", null);

            // Arrange
            var result = service.ToString();

            // Assert
            Assert.AreEqual("current", result);
        }
        
        [Test]
        public void Interpolate_ShouldReturnCurrentIfOriginExists()
        {
            // Act
            var service = new OverridingMapper("current", "origin");

            // Arrange
            var result = service.Interpolate(new ConfigMapMock());

            // Assert
            Assert.AreEqual("current", result);
        }
        
        [Test]
        public void Interpolate_ShouldReturnOriginIfCurrentNull()
        {
            // Act
            var service = new OverridingMapper(null, "origin");

            // Arrange
            var result = service.Interpolate(new ConfigMapMock());

            // Assert
            Assert.AreEqual("origin", result);
        }
        
        [Test]
        public void Interpolate_ShouldReturnCurrentIfOriginIsNull()
        {
            // Act
            var service = new OverridingMapper("current", null);

            // Arrange
            var result = service.Interpolate(new ConfigMapMock());

            // Assert
            Assert.AreEqual("current", result);
        }
    }
    
    public class ConfigMapSequenceUnitTests
    {
        [Test]
        public void Enumerator_ShouldMergeNotCollidedSets()
        {
            // Act
            var service = new ConfigMapSequence();
            service.Sequence = new IConfigMapData[]
            {
                new MapConfigData()
                {
                    Values = new Dictionary<string, object>()
                    {
                        ["first"] = "value1"
                    }
                },
                new MapConfigData()
                {
                    Values = new Dictionary<string, object>()
                    {
                        ["second"] = "value2"
                    }
                }
            };

            // Arrange
            var result = service.ToArray();

            // Assert
            Assert.True(Enumerable.SequenceEqual(new KeyValuePair<string, object>[]
            {
                new KeyValuePair<string, object>("first", "value1"),
                new KeyValuePair<string, object>("second", "value2"),
            }, result));
        }
        
        [Test]
        public void Enumerator_ShouldMergeCollidedKeys()
        {
            // Act
            var service = new ConfigMapSequence();
            service.Sequence = new IConfigMapData[]
            {
                new MapConfigData()
                {
                    Values = new Dictionary<string, object>()
                    {
                        ["first"] = "value1"
                    }
                },
                new MapConfigData()
                {
                    Values = new Dictionary<string, object>()
                    {
                        ["first"] = "value2"
                    }
                }
            };

            // Arrange
            var result = service.ToArray();

            // Assert
            Assert.AreEqual("first", result.First().Key);
            Assert.AreEqual(new OverridingMapper("value2", "value1"), result.First().Value);
        }
    }
}