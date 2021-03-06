﻿using System.Xml;
//using NLog;
using UrdfToUnity.Urdf;
using UrdfToUnity.Urdf.Models.Attributes;
using UrdfToUnity.Urdf.Models.Links.Geometries;
using UrdfToUnity.Util;

namespace UrdfToUnity.Parse.Xml.Links.Geometries
{
    /// <summary>
    /// Parses a URDF &lt;box&gt; element from XML into a Box object.
    /// </summary>
    /// <seealso cref="http://wiki.ros.org/urdf/XML/link"/>
    /// <seealso cref="http://wiki.ros.org/urdf/XML/visual"/>
    /// <seealso cref="Urdf.Models.Links.Geometries.Box"/>
    public sealed class BoxParser : AbstractUrdfXmlParser<Box>
    {
        private static readonly double DEFAULT_VALUE = 1d;


        //protected override Logger Logger { get; } = LogManager.GetCurrentClassLogger();


        /// <summary>
        /// The name of the URDF XML element that this class parses.
        /// </summary>
        protected override string ElementName { get; } = UrdfSchema.BOX_ELEMENT_NAME;


        /// <summary>
        /// Parses a URDF &lt;box&gt; element from XML.
        /// </summary>
        /// <param name="node">The XML node of a &lt;box&gt; element. MUST NOT BE NULL</param>
        /// <returns>A Box object parsed from the XML</returns>
        public override Box Parse(XmlNode node)
        {
            ValidateXmlNode(node);

            XmlAttribute sizeAttribute = GetAttributeFromNode(node, UrdfSchema.SIZE_ATTRIBUTE_NAME);
            SizeAttribute size = new SizeAttribute(DEFAULT_VALUE, DEFAULT_VALUE, DEFAULT_VALUE);

            if (sizeAttribute == null)
            {
                LogMissingRequiredAttribute(UrdfSchema.SIZE_ATTRIBUTE_NAME);
            }
            else
            {
                if (!RegexUtils.IsMatchNDoubles(sizeAttribute.Value, 3))
                {
                    LogMalformedAttribute(UrdfSchema.SIZE_ATTRIBUTE_NAME);
                }
                else
                {
                    double[] values = RegexUtils.MatchDoubles(sizeAttribute.Value);
                    size = new SizeAttribute(values[0], values[1], values[2]);
                }
            }

            return new Box(size);
        }
    }
}
