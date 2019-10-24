using NUnit.Framework;

namespace BuilderScenario.Tests
{
    public class ConfigMapUnitTests
    {
        [Test]
        public void Interpolate_ShouldIgnoreNotExistsVariables()
        {
            // Act
            var service = new ConfigMap();

            // Arrange
            var result = service.Interpolate("${variable}");

            // Assert
            Assert.AreEqual("${variable}", result);
        }
        
        [Test]
        public void Interpolate_ShouldIntrepolateFlatVariable()
        {
            // Act
            var service = new ConfigMap();
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
            var service = new ConfigMap();
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
            var service = new ConfigMap();
            service.Set("variable", "placed_data");
            service.Set("2level", "level_2+${variable}");
            
            // Arrange
            var result = service.Interpolate("${2level}-${variable}");

            // Assert
            Assert.AreEqual("level_2+placed_data-placed_data", result);
        }
    }
}
