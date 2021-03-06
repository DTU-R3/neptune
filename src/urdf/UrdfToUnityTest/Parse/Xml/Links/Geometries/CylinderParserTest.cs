﻿using System;
using System.IO;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UrdfToUnity.Parse.Xml.Links.Geometries;
using UrdfToUnity.Urdf.Models.Links.Geometries;

namespace UrdfToUnityTest.Parse.Xml.Links.Geometries
{
    [TestClass]
    public class CylinderParserTest
    {
        private static readonly string FORMAT_STRING = "<cylinder radius='{0}' length='{1}'/>";

        private readonly CylinderParser parser = new CylinderParser();
        private readonly XmlDocument xmlDoc = new XmlDocument();


        [TestMethod]
        public void ParseCylinder()
        {
            double radius = 1;
            double length = 2;
            string xml = String.Format(FORMAT_STRING, radius, length);

            this.xmlDoc.Load(XmlReader.Create(new StringReader(xml)));
            Cylinder cylinder = this.parser.Parse(this.xmlDoc.DocumentElement);

            Assert.AreEqual(radius, cylinder.Radius);
            Assert.AreEqual(length, cylinder.Length);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ParseCylinderNullNode()
        {
            this.parser.Parse(null);
        }

        [TestMethod]
        public void ParseCylinderNoRadius()
        {
            string xml = "<cylinder length='2'/>";

            this.xmlDoc.Load(XmlReader.Create(new StringReader(xml)));
            Cylinder cylinder = this.parser.Parse(this.xmlDoc.DocumentElement);

            Assert.AreEqual(1, cylinder.Radius);
            Assert.AreEqual(2, cylinder.Length);
        }

        [TestMethod]
        public void ParseCylinderNoLength()
        {
            string xml = "<cylinder radius='2'/>";

            this.xmlDoc.Load(new StringReader(xml));
            Cylinder cylinder = this.parser.Parse(this.xmlDoc.DocumentElement);

            Assert.AreEqual(2, cylinder.Radius);
            Assert.AreEqual(1, cylinder.Length);
        }
    }
}
