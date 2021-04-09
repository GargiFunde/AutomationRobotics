// <copyright file=SimpleTimer company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Prabodhini Funde</author>
// <date> 03-10-2018 16:02:54</date>
// <summary></summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace CommonLibrary
{
    public class SimpleTimer
    {
        private Timer clock;

        private int _timeout;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleTimer"/> class.
        /// </summary>
        /// <param name="timeout">The timeout.</param>
        public SimpleTimer(int timeout)
        {
            if (timeout < 0)
            {
                throw new ArgumentOutOfRangeException("timeout", timeout, "Should be equal are greater then zero.");
            }

            _timeout = timeout;

            if (timeout > 0)
            {
                clock = new Timer(timeout * 1000);
                clock.AutoReset = false;
                clock.Elapsed += new ElapsedEventHandler(ElapsedEvent);
                clock.Start();
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="SimpleTimer"/> is elapsed.
        /// </summary>
        /// <value><c>true</c> if elapsed; otherwise, <c>false</c>.</value>
        public bool Elapsed
        {
            get { return (clock == null); }
        }

        /// <summary>
        /// The number of seconds after which this timer times out. The time out can only be
        /// set through the constructor.
        /// </summary>
        public int Timeout
        {
            get { return _timeout; }
        }

        private void ElapsedEvent(object source, ElapsedEventArgs e)
        {
            clock.Stop();
            clock.Close();
            clock = null;
        }
    }
}
