﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UrdfToUnity.Urdf.Models.Attributes;
using UrdfToUnity.Urdf.Models.Links;
using UrdfToUnity.Urdf.Models.Links.Geometries;

namespace UrdfToUnityTest.Urdf.Models.Links
{
    [TestClass]
    public class GeometryTest
    {
        [TestMethod]
        public void ConstructGeometryBox()
        {
            Box box = new Box(new SizeAttribute(1, 1, 1));
            Geometry geometry = new Geometry(box);

            Assert.AreEqual(Geometry.Shapes.Box, geometry.Shape);
            Assert.AreEqual(box, geometry.Box);
            Assert.IsNull(geometry.Cylinder);
            Assert.IsNull(geometry.Sphere);
            Assert.IsNull(geometry.Mesh);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void ConstructGeometryBoxNull()
        {
            new Geometry((Box)null);
        }

        [TestMethod]
        public void ConstructGeometryCylinder()
        {
            Cylinder cylinder = new Cylinder(1, 1);
            Geometry geometry = new Geometry(cylinder);

            Assert.AreEqual(Geometry.Shapes.Cylinder, geometry.Shape);
            Assert.AreEqual(cylinder, geometry.Cylinder);
            Assert.IsNull(geometry.Box);
            Assert.IsNull(geometry.Sphere);
            Assert.IsNull(geometry.Mesh);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructGeometryCylinderNull()
        {
            new Geometry((Cylinder)null);
        }

        [TestMethod]
        public void ConstructGeometrySphere()
        {
            Sphere sphere = new Sphere(1);
            Geometry geometry = new Geometry(sphere);

            Assert.AreEqual(Geometry.Shapes.Sphere, geometry.Shape);
            Assert.AreEqual(sphere, geometry.Sphere);
            Assert.IsNull(geometry.Box);
            Assert.IsNull(geometry.Cylinder);
            Assert.IsNull(geometry.Mesh);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructGeometrySphereNull()
        {
            new Geometry((Sphere)null);
        }

        [TestMethod]
        public void ConstructGeometryMesh()
        {
            Mesh mesh = new Mesh.Builder("name").Build();
            Geometry geometry = new Geometry(mesh);

            Assert.AreEqual(Geometry.Shapes.Mesh, geometry.Shape);
            Assert.AreEqual(mesh, geometry.Mesh);
            Assert.IsNull(geometry.Box);
            Assert.IsNull(geometry.Cylinder);
            Assert.IsNull(geometry.Sphere);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructGeometryMeshNull()
        {
            new Geometry((Mesh)null);
        }

        [TestMethod]
        public void ToStringGeometry()
        {
            Assert.AreEqual("<geometry>\r\n<box size=\"1 1 1\"/>\r\n</geometry>", new Geometry(new Box(new SizeAttribute(1, 1, 1))).ToString());
            Assert.AreEqual("<geometry>\r\n<cylinder radius=\"1\" length=\"1\"/>\r\n</geometry>", new Geometry(new Cylinder(1, 1)).ToString());
            Assert.AreEqual("<geometry>\r\n<sphere radius=\"1\"/>\r\n</geometry>", new Geometry(new Sphere(1)).ToString());
            Assert.AreEqual("<geometry>\r\n<mesh filename=\"file\"/>\r\n</geometry>", new Geometry(new Mesh.Builder("file").Build()).ToString());
        }

        [TestMethod]
        public void EqualsAndHash()
        {
            Geometry geometry = new Geometry(new Sphere(1));
            Geometry same = new Geometry(new Sphere(1));
            Geometry diff = new Geometry(new Cylinder(1, 2));

            Assert.IsTrue(geometry.Equals(geometry));
            Assert.IsFalse(geometry.Equals(null));
            Assert.IsTrue(geometry.Equals(same));
            Assert.IsTrue(same.Equals(geometry));
            Assert.IsFalse(geometry.Equals(diff));
            Assert.AreEqual(geometry.GetHashCode(), same.GetHashCode());
            Assert.AreNotEqual(geometry.GetHashCode(), diff.GetHashCode());
        }
    }
}
