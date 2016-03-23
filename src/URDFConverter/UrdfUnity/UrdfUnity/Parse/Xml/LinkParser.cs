﻿using System.Collections.Generic;
using System.Xml;
using NLog;
using UrdfUnity.Parse.Xml.LinkElements;
using UrdfUnity.Urdf.Models;
using UrdfUnity.Urdf.Models.LinkElements;
using UrdfUnity.Urdf.Models.LinkElements.VisualElements;
using UrdfUnity.Util;

namespace UrdfUnity.Parse.Xml
{
    /// <summary>
    /// Parses a URDF &lt;link&gt; element from XML into a Link object.
    /// </summary>
    /// <seealso cref="http://wiki.ros.org/urdf/XML/link"/>
    /// <seealso cref="Urdf.Models.Link"/>
    public sealed class LinkParser : AbstractUrdfXmlParser<Link>
    {
        private static readonly string NAME_ATTRIBUTE_NAME = "name";
        private static readonly string INERTIAL_ELEMENT_NAME = "inertial";
        private static readonly string VISUAL_ELEMENT_NAME = "visual";
        private static readonly string COLLISION_ELEMENT_NAME = "collision";


        protected override Logger Logger { get; } = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// The name of the URDF XML element that this class parses.
        /// </summary>
        protected override string ElementName { get; } = "link";


        private readonly InertialParser inertialParser = new InertialParser();
        private readonly VisualParser visualParser;
        private readonly CollisionParser collisionParser = new CollisionParser();


        /// <summary>
        /// Creates a new instance of LinkParser.
        /// </summary>
        /// <param name="materialDictionary"></param>
        public LinkParser(Dictionary<string, Material> materialDictionary)
        {
            this.visualParser = new VisualParser(materialDictionary);
        }

        /// <summary>
        /// Parses a URDF &lt;link&gt; element from XML.
        /// </summary>
        /// <param name="node">The XML node of a &lt;link&gt; element</param>
        /// <returns>A Link object parsed from the XML</returns>
        public override Link Parse(XmlNode node)
        {
            ValidateXmlNode(node);

            XmlAttribute nameAttribute = GetAttributeFromNode(node, NAME_ATTRIBUTE_NAME);
            XmlElement inertialElement = GetElementFromNode(node, INERTIAL_ELEMENT_NAME);
            XmlNodeList visualElements = node.SelectNodes(VISUAL_ELEMENT_NAME);
            XmlNodeList collisionElements = node.SelectNodes(COLLISION_ELEMENT_NAME);

            Link.Builder builder;

            if (nameAttribute == null)
            {
                LogMissingRequiredAttribute(NAME_ATTRIBUTE_NAME);
                builder = new Link.Builder(Link.DEFAULT_NAME);
            }
            else
            {
                builder = new Link.Builder(nameAttribute.Value);
            }

            if (inertialElement != null)
            {
                builder.SetInertial(this.inertialParser.Parse(inertialElement));
            }

            if (visualElements != null)
            {
                builder.SetVisual(ParseVisuals(visualElements));
            }

            if (collisionElements != null)
            {
                builder.SetCollision(ParseCollisions(collisionElements));
            }

            return builder.Build();
        }

        private List<Visual> ParseVisuals(XmlNodeList nodeList)
        {
            List<Visual> visuals = new List<Visual>();

            if (nodeList == null || nodeList.Count == 0)
            {
                LogMissingRequiredElement(VISUAL_ELEMENT_NAME);
            }
            else
            {
                foreach (XmlNode node in nodeList)
                {
                    visuals.Add(this.visualParser.Parse(node));
                }
            }

            return visuals;
        }

        private List<Collision> ParseCollisions(XmlNodeList nodeList)
        {
            List<Collision> collisions = new List<Collision>();

            if (nodeList == null || nodeList.Count == 0)
            {
                LogMissingOptionalElement(COLLISION_ELEMENT_NAME);
            }
            else
            {
                foreach (XmlNode node in nodeList)
                {
                    collisions.Add(this.collisionParser.Parse(node));
                }
            }

            return collisions;
        }
    }
}
