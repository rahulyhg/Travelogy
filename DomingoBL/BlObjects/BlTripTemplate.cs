using DomingoDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomingoBL.BlObjects
{
    /// <summary>
    /// 
    /// </summary>
    public class BlTripTemplate
    {
        /// <summary>
        /// 
        /// </summary>
        public TripTemplate DlTemplate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<TripTemplateStep> DlTemplateSteps { get; set; }
    }
}
