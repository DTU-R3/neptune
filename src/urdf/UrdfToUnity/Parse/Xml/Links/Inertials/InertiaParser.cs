﻿using System;
using System.Collections.Generic;
using System.Xml;
//using NLog;
using UrdfToUnity.Urdf;
using UrdfToUnity.Urdf.Models.Links.Inertials;
using UrdfToUnity.Util;

namespace UrdfToUnity.Parse.Xml.Links.Inertials
{
    /// <summary>
    /// Parses a URDF &lt;inertia&gt; element from XML into a Inertia object.
    /// </summary>
    /// <seealso cref="http://wiki.ros.org/urdf/XML/inertial"/>
    /// <seealso cref="Urdf.Models.Links.Inertials.Inertia"/>
    public sealed class InertiaParser : AbstractUrdfXmlParser<Inertia>
    {
        /// <summary>
        /// The default value used if the inertia element is missing a required attribute.
        /// </summary>
        public static readonly double DEFAULT_VALUE = Double.NaN;


        //protected override Logger Logger { get; } = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// The name of the URDF XML element that this class parses.
        /// </summary>
        protected override string ElementName { get; } = UrdfSchema.INERTIA_ELEMENT_NAME;


        /// <summary>
        /// Parses a URDF &lt;inertia&gt; element from XML.
        /// </summary>
        /// <param name="node">The XML node of a &lt;inertia&gt; element. MUST NOT BE NULL</param>
        /// <returns>A Inertia object with values parsed from the XML, or the default value of <c>Double.NaN</c> for missing attributes</returns>
        public override Inertia Parse(XmlNode node)
        {
            ValidateXmlNode(node);

            Dictionary<string, XmlAttribute> attributes = new Dictionary<string, XmlAttribute>();
            Dictionary<string, double> values = new Dictionary<string, double>();

            attributes.Add(UrdfSchema.IXX_ATTRIBUTE_NAME, GetAttributeFromNode(node, UrdfSchema.IXX_ATTRIBUTE_NAME));
            attributes.Add(UrdfSchema.IXY_ATTRIBUTE_NAME, GetAttributeFromNode(node, UrdfSchema.IXY_ATTRIBUTE_NAME));
            attributes.Add(UrdfSchema.IXZ_ATTRIBUTE_NAME, GetAttributeFromNode(node, UrdfSchema.IXZ_ATTRIBUTE_NAME));
            attributes.Add(UrdfSchema.IYY_ATTRIBUTE_NAME, GetAttributeFromNode(node, UrdfSchema.IYY_ATTRIBUTE_NAME));
            attributes.Add(UrdfSchema.IYZ_ATTRIBUTE_NAME, GetAttributeFromNode(node, UrdfSchema.IYZ_ATTRIBUTE_NAME));
            attributes.Add(UrdfSchema.IZZ_ATTRIBUTE_NAME, GetAttributeFromNode(node, UrdfSchema.IZZ_ATTRIBUTE_NAME));

            foreach (string key in attributes.Keys)
            {
                if (attributes[key] == null)
                {
                    LogMissingRequiredAttribute(key);
                    values.Add(key, DEFAULT_VALUE);
                }
                else
                {
                    values.Add(key, RegexUtils.MatchDouble(attributes[key].Value, DEFAULT_VALUE));
                }
            }

            return new Inertia(values[UrdfSchema.IXX_ATTRIBUTE_NAME], values[UrdfSchema.IXY_ATTRIBUTE_NAME], 
                values[UrdfSchema.IXZ_ATTRIBUTE_NAME], values[UrdfSchema.IYY_ATTRIBUTE_NAME], 
                values[UrdfSchema.IYZ_ATTRIBUTE_NAME], values[UrdfSchema.IZZ_ATTRIBUTE_NAME]);
        }
    }
}
