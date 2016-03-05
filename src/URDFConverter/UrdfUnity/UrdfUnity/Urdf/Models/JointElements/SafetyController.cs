﻿namespace UrdfUnity.Urdf.Models.JointElements
{
    /// <summary>
    /// Represents the joint property values wherein the safety controller limits the joint position.
    /// </summary>
    /// <remarks>
    /// The upper bound on effort is <c>-KVelocity * (velocity - velocity limit)</c>.
    /// The upper bound on velocity is <c>-KPosition * (position - SoftUpperLimit)</c>.
    /// </remarks>
    /// <seealso cref="Limit"/>
    /// <seealso cref="http://wiki.ros.org/urdf/XML/joint"/>
    /// <seealso cref="http://wiki.ros.org/pr2_controller_manager/safety_limits"/>
    public class SafetyController
    {
        private static readonly double DEFAULT_LOWER_LIMIT = 0d;
        private static readonly double DEFAULT_UPPER_LIMIT = 0d;
        private static readonly double DEFAULT_POSITION = 0d;

        /// <summary>
        /// The lower joint boundary where the safety controller starts limiting the position of the joint.
        /// </summary>
        /// <value>Optional. Default value is 0. This limit needs to be larger than the joint's <c>Limit.Lower</c></value>
        public double SoftLowerLimit { get; }

        /// <summary>
        /// The upper joint boundary where the safety controller starts limiting the position of the joint.
        /// </summary>
        /// <value>Optional. Default value is 0. This limit needs to be larger than the joint's <c>Limit.Upper</c></value>
        public double SoftUpperLimit { get; }

        /// <summary>
        /// The scale of the velocity boundary specifying the relation between position and velocity limits.
        /// </summary>
        /// <value>Optional. Default value is 0</value>
        public double KPostition { get; }

        /// <summary>
        /// The scale of the effort boundary specifying the relationship between effort and velocity limits.
        /// </summary>
        /// <value>Required.</value>
        public double KVelocity { get; }


        /// <summary>
        /// Creates a new instance of SafetyController with the specified k_velocity, and default limits and k_position.
        /// </summary>
        /// <param name="kVelocity">The scale of the bound on effort</param>
        public SafetyController(double kVelocity) : this(DEFAULT_LOWER_LIMIT, DEFAULT_UPPER_LIMIT, DEFAULT_POSITION, kVelocity)
        {
            // Invoke overloaded constructor.
        }

        /// <summary>
        /// Creates a new instance of SafetyController with the specified limits, k_position and k_velocity.
        /// </summary>
        /// <param name="lowerLimit">The soft lower limit of the joint position</param>
        /// <param name="upperLimit">The soft upper limit of the joint position</param>
        /// <param name="kPosition">The scale of the bound on velocity</param>
        /// <param name="kVelocity">The scale of the bound on effort</param>
        public SafetyController(double lowerLimit, double upperLimit, double kPosition, double kVelocity)
        {
            this.SoftLowerLimit = lowerLimit;
            this.SoftUpperLimit = upperLimit;
            this.KPostition = kPosition;
            this.KVelocity = kVelocity;
        }
    }
}