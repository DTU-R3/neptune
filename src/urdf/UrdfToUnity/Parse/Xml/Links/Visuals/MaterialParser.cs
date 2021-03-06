﻿using System.Collections.Generic;
using System.Xml;
//using NLog;
using UrdfToUnity.Urdf;
using UrdfToUnity.Urdf.Models.Attributes;
using UrdfToUnity.Urdf.Models.Links.Visuals;

namespace UrdfToUnity.Parse.Xml.Links.Visuals
{
    /// <summary>
    /// Parses a URDF &lt;material&gt; element from XML into a Material object.
    /// </summary>
    /// <seealso cref="http://wiki.ros.org/urdf/XML/link"/>
    /// <seealso cref="http://wiki.ros.org/urdf/XML/visual"/>
    /// <seealso cref="Urdf.Models.Links.Visuals.Material"/>
    public sealed class MaterialParser : AbstractUrdfXmlParser<Material>
    {
        private static readonly int DEFAULT_COLOR_VALUE = 0; // Black


        //protected override Logger Logger { get; } = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// The name of the URDF XML element that this class parses.
        /// </summary>
        protected override string ElementName { get; } = UrdfSchema.MATERIAL_ELEMENT_NAME;


        private readonly ColorParser colorParser = new ColorParser();
        private readonly TextureParser textureParser = new TextureParser();
        private readonly Dictionary<string, Material> materialDictionary;


        /// <summary>
        /// Creates a new instance of MaterialParser.
        /// </summary>
        /// <param name="materialDictionary">A dictionary of defined materials with material names as keys</param>
        public MaterialParser(Dictionary<string, Material> materialDictionary)
        {
            this.materialDictionary = materialDictionary;
        }

        /// <summary>
        /// Parses a URDF &lt;material&gt; element from XML.
        /// </summary>
        /// <param name="node">The XML node of a &lt;material&gt; element</param>
        /// <returns>A Material object parsed from the XML</returns>
        public override Material Parse(XmlNode node)
        {
            ValidateXmlNode(node);

            XmlAttribute nameAttribute = GetAttributeFromNode(node, UrdfSchema.NAME_ATTRIBUTE_NAME);
            XmlElement colorElement = GetElementFromNode(node, UrdfSchema.COLOR_ELEMENT_NAME);
            XmlElement textureElement = GetElementFromNode(node, UrdfSchema.TEXTURE_ELEMENT_NAME);

            string name = ParseName(nameAttribute);
            Color color = ParseColor(colorElement);
            Texture texture = ParseTexture(textureElement);

            if (!name.Equals(Material.DEFAULT_NAME) && color == null && texture == null)
            {
                if (!this.materialDictionary.ContainsKey(name))
                {
                    //Logger.Warn("Unknown pre-defined material name referenced by material name attribute");
                    return new Material(name);
                }
                else
                {
                    return this.materialDictionary[name];
                }
            }

            if (colorElement != null && textureElement != null)
            {
                return new Material(name, color, texture);
            }
            if (textureElement != null)
            {
                return new Material(name, texture);
            }
            if (colorElement != null)
            {
                return new Material(name, color);
            }
            
            //Logger.Warn("Unable to parse malformed material element");
            return new Material(name, new Color(new RgbAttribute(DEFAULT_COLOR_VALUE, DEFAULT_COLOR_VALUE, DEFAULT_COLOR_VALUE)));
        }

        private string ParseName(XmlAttribute nameAttribute)
        {
            if (nameAttribute == null || nameAttribute.Value == null)
            {
                LogMissingRequiredAttribute(UrdfSchema.NAME_ATTRIBUTE_NAME);
                return Material.DEFAULT_NAME;
            }

            return nameAttribute.Value;
        }

        private Color ParseColor(XmlElement colorElement)
        {
            if (colorElement == null)
            {
                LogMissingOptionalAttribute(UrdfSchema.COLOR_ELEMENT_NAME);
                return null;
            }

            return this.colorParser.Parse(colorElement);
        }

        private Texture ParseTexture(XmlElement textureElement)
        {
            if (textureElement == null)
            {
                LogMissingOptionalAttribute(UrdfSchema.TEXTURE_ELEMENT_NAME);
                return null;
            }

            return this.textureParser.Parse(textureElement);
        }
    }
}
