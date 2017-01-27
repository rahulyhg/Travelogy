using DomingoDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomingoBL
{
    /// <summary>
    /// 
    /// </summary>
    public class TripStepManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="newStep"></param>
        /// <returns></returns>
        public static DomingoBlError CreateTriptemplateStep(TripTemplateStep newStep)
        {
            try
            {
                using (TravelogyDevEntities1 context = new TravelogyDevEntities1())
                {
                    context.TripTemplateSteps.Add(newStep);
                    context.SaveChanges();

                    newStep.TripTemplateStepIdentifier = string.Format("{0}-{1}", newStep.TripTemplateId, newStep.Id);
                    context.SaveChanges();
                }

                return new DomingoBlError() { ErrorCode = 0, ErrorMessage = "" };
            }
            catch (Exception ex)
            {
                return new DomingoBlError() { ErrorCode = 100, ErrorMessage = ex.Message };
            }
        }
    }
}
