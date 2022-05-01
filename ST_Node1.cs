using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Threading;

namespace TVN_APP
{
    public class ST_Node1
    {
        public Thread WorkerThread;
        private bool Closing;
        private double Interval;

        // Work Event
        public EventHandler Work;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="IntervalMilliSeconds"></param>
        /// <param name="Prio"></param>
        public ST_Node1(int IntervalMilliSeconds = 1000, ThreadPriority Prio = ThreadPriority.Normal)
        {
            Interval = (double)IntervalMilliSeconds;

            WorkerThread = new Thread(new ThreadStart(WorkerLoop));
            WorkerThread.SetApartmentState(ApartmentState.STA);
            WorkerThread.Priority = Prio;
            WorkerThread.IsBackground = true;
            WorkerThread.Start();
        }

        /// <summary>
        /// Sets the interval at runtime
        /// </summary>
        public void SetInterval(int IntervalMilliSeconds)
        {
            Interval = (double)IntervalMilliSeconds;
        }

        /// <summary>
        /// Éndless Thread loop
        /// </summary>
        public void WorkerLoop()
        {
            DateTime LastStart;
            double NextProc = 0;

            while (!Closing)
            {
                LastStart = DateTime.Now;

                // Here we call the Work to do
                this.DoWork();

                NextProc = Interval - (DateTime.Now - LastStart).TotalMilliseconds;
                if (NextProc > 0.0d)
                {
                    Thread.Sleep((int)(NextProc));
                }
            }
        }

        /// <summary>
        /// Method that gets called in a cycle
        /// </summary>
        public void DoWork()
        {
            //Work here

            // or in Event....
            if (this.Work != null)
                this.Work(this, new EventArgs());
        }

        /// <summary>
        /// Shutodwn thread. Needs to be called when exiting the program
        /// </summary>
        public void Shutdown()
        {
            if (WorkerThread == null)
                return;

            this.Closing = true;

            WorkerThread.Join();
        }

    }
}
