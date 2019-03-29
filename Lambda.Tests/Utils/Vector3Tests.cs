using System;
using System.Collections.Generic;
using System.Text;
using Lambda;
using Lambda.Utils;
using Xunit;

namespace Lambda.Tests.Utils
{
    public class Vector3Tests
    {
        [Theory]
        [InlineData(0, 0, 0, 0, 4, 10, 6)]
        [InlineData(float.MaxValue, 0, 0, 0, 0, 0, float.MaxValue)]
        [InlineData(7, 17, 4, 6, 3, 2, 10.246951f)]
        public void Distance_SimpleDistancesShouldCalculate(float x1, float x2, float y1, float y2, float z1, float z2, double excepted)
        {
            //Arange
            Vector3 v1 = new Vector3(x1, y1, z1);
            Vector3 v2 = new Vector3(x2, y2, z2);

            //Act
            float actual = Vector3.Distance(v1, v2);

            //Assert
            Assert.Equal(excepted, actual);
        }
        [Theory]
        [InlineData(10, 0, 0, 10)]
        [InlineData(3, 0, 4, 5)]
        public void Magnitude_SimpleMagnitudeShouldCalculate(float x, float y, float z, float excepted)
        {
            //Arange
            Vector3 v = new Vector3(x, y, z);

            //Act
            float actual = v.Magnitude;

            //Assert
            Assert.Equal(excepted, actual);
        }
    }
}
