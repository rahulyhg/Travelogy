﻿using DomingoDAL;
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
    public class TagManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tags"></param>
        /// <returns></returns>
        public static DomingoBlError GetAllTags(out List<Tag> tags)
        {
            tags = null;

            try
            {
                using (var context = new TravelogyDevEntities1())
                {
                    tags = context.Tags.Select(p => p).ToList();
                }
            }
            catch (Exception ex)
            {
                return new DomingoBlError() { ErrorCode = 100, ErrorMessage = ex.Message };
            }

            return new DomingoBlError() { ErrorCode = 0, ErrorMessage = "" };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="destinationId"></param>
        /// <param name="tags"></param>
        /// <returns></returns>
        public static DomingoBlError GetTagsForDestination(int destinationId, out List<View_TagDestination> tags)
        {
            tags = null;

            try
            {
                using (var context = new TravelogyDevEntities1())
                {
                    tags = context.View_TagDestination.Where(p => p.DestinationId == destinationId).ToList();
                }
            }
            catch (Exception ex)
            {
                return new DomingoBlError() { ErrorCode = 100, ErrorMessage = ex.Message };
            }

            return new DomingoBlError() { ErrorCode = 0, ErrorMessage = "" };
        }
    }
}
