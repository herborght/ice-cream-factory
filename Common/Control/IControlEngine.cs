using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ABB.InSecTT.Common.Control
{
    /// <summary>
    /// Interface for allowing different control engines to be used for module control. 
    /// </summary>
    public interface IControlEngine
    {
        /// <summary>
        /// Method running the controller.
        /// </summary>
        /// <param name="token">Token for cancellation of task.</param>
        /// <returns></returns>
        Task Control(CancellationToken token, int speedModifier);

        /// <summary>
        /// Commands for module orchestration - to be exposed upward, i.e., to OPC UA clients.
        /// </summary>
        IEnumerable<ICmd> Commands { get; }
    }
}
