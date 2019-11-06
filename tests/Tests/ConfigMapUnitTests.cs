using System.Collections.Generic;
using NUnit.Framework;

namespace BuilderScenario.Tests
{
    public class ConfigMapUnitTests
    {
        [Test]
        public void Interpolate_ShouldIgnoreNotExistsVariables()
        {
            // Act
            var service = new ConfigMap(null);

            // Arrange
            var result = service.Interpolate("${variable}");

            // Assert
            Assert.AreEqual("${variable}", result);
        }
        
        [Test]
        public void Interpolate_ShouldIntrepolateFlatVariable()
        {
            // Act
            var service = new ConfigMap(null);
            service.Set("variable", "placed_data");
            
            // Arrange
            var result = service.Interpolate("${variable}");

            // Assert
            Assert.AreEqual("placed_data", result);
        }
        
        [Test]
        public void Interpolate_ShouldIntrepolate2LevelVariable()
        {
            // Act
            var service = new ConfigMap(null);
            service.Set("variable", "placed_data");
            service.Set("2level", "level_2+${variable}");
            
            // Arrange
            var result = service.Interpolate("${2level}");

            // Assert
            Assert.AreEqual("level_2+placed_data", result);
        }
        
        [Test]
        public void Interpolate_ShouldIntrepolate2LevelVariableWithFlat()
        {
            // Act
            var service = new ConfigMap(null);
            service.Set("variable", "placed_data");
            service.Set("2level", "level_2+${variable}");
            
            // Arrange
            var result = service.Interpolate("${2level}-${variable}");

            // Assert
            Assert.AreEqual("level_2+placed_data-placed_data", result);
        }
        
        [Test]
        public void GetSection_ShouldGetSection()
        {
            // Act
            var service = new ConfigMap(new ServiceCollection());
            service.Set("section", new MapConfigData
            {
                Values = new Dictionary<string, object>
                {
                    ["key"] = "value"
                }
            });

            // Arrange
            var result = service.GetSection("section");

            // Assert
            Assert.NotNull(result);
            Assert.NotNull("value", result["key"]);
        }
        
        [Test]
        public void GetSection_ShouldNestedSectionMustGetParentEntry()
        {
            // Act
            var service = new ConfigMap(new ServiceCollection());
            service.Set("parent", "value");
            service.Set("section", new MapConfigData
            {
                Values = new Dictionary<string, object>
                {
                    ["key"] = "${parent}"
                }
            });

            // Arrange
            var result = service.GetSection("section");

            // Assert
            Assert.NotNull("value", result["key"]);
        }
        
        [Test]
        public void GetSection_ShouldSectionCanInterpolateFlat()
        {
            // Act
            var service = new ConfigMap(new ServiceCollection());
            service.Set("section", new MapConfigData
            {
                Values = new Dictionary<string, object>
                {
                    ["data"] = "data",
                    ["key"] = "${data}"
                }
            });

            // Arrange
            var result = service.GetSection("section");

            // Assert
            Assert.NotNull("data", result["key"]);
        }
        
        [Test]
        public void GetSection_ShoulReturnNullIfNotExists()
        {
            // Act
            var service = new ConfigMap(new ServiceCollection());

            // Arrange
            var result = service.GetSection("section");

            // Assert
            Assert.Null(result);
        }
        
        [Test]
        public void GetSection_ShoulReturnNullIfKeyIsNotSection()
        {
            // Act
            var service = new ConfigMap(new ServiceCollection());
            service.Set("not_section", "data");

            // Arrange
            var result = service.GetSection("not_section");

            // Assert
            Assert.Null(result);
        }
    }
}
