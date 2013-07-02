﻿// ------------------------------------------------------------------------
// <copyright file="CountdownTime.cs" company="HSA.InfoSys">
//     Copyright statement. All right reserved
// </copyright>
// ------------------------------------------------------------------------
namespace HSA.InfoSys.Common.Services.LocalServices
{
    using System;

    /// <summary>
    /// This class represents the time object.
    /// </summary>
    public class CountdownTime
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CountdownTime" /> class.
        /// </summary>
        /// <param name="time">The time.</param>
        /// <param name="days">The days.</param>
        /// <param name="id">The id.</param>
        /// <param name="repeat">if set to <c>true</c> [repeat].</param>
        public CountdownTime(
            int time,
            int days,
            Guid id,
            bool repeat)
        {
            var now = DateTime.Now;
            this.RepeatIn = new DateTime(now.Year, now.Month, now.Day, time, 0, 0).AddDays(days);
            this.Repeat = repeat;
            this.Id = id;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CountdownTime"/> class.
        /// </summary>
        /// <param name="time">The time.</param>
        /// <param name="days">The days.</param>
        /// <param name="repeat">if set to <c>true</c> [repeat].</param>
        public CountdownTime(
            int time,
            int days,
            bool repeat)
        {
            var now = DateTime.Now;
            this.RepeatIn = new DateTime(now.Year, now.Month, now.Day, time, 0, 0).AddDays(days);
            this.Repeat = repeat;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CountdownTime"/> class.
        /// </summary>
        /// <param name="repeatIn">The repeat in.</param>
        /// <param name="id">The id.</param>
        /// <param name="repeat">if set to <c>true</c> [repeat].</param>
        public CountdownTime(
            DateTime repeatIn,
            Guid id,
            bool repeat)
        {
            this.RepeatIn = repeatIn;
            this.Repeat = repeat;
            this.Id = id;
        }

        /// <summary>
        /// Gets the duration.
        /// </summary>
        /// <value>
        /// The duration.
        /// </value>
        public DateTime RepeatIn { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="CountdownTime"/> is repeat.
        /// </summary>
        /// <value>
        ///   <c>true</c> if repeat; otherwise, <c>false</c>.
        /// </value>
        public bool Repeat { get; set; }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The ID.
        /// </value>
        public Guid Id { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format(
                Properties.Resources.TIME_TO_STRING,
                this.Id,
                this.Repeat,
                this.RepeatIn.Subtract(DateTime.Now).TotalHours);
        }
    }
}
