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
    public static class AdminUtility
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        public static async Task<DomingoBlError> SaveTripTemplate(TripTemplate template)
        {
            try
            {
                if (template != null)
                {
                    using (var context = new TravelogyDevEntities1())
                    {
                        if (template.Id > 0)
                        {
                            var _tripTemmplate = context.TripTemplates.Find(template.Id);
                            _tripTemmplate.Description = template.Description;
                            _tripTemmplate.ThumbnailPath = template.ThumbnailPath;
                            await context.SaveChangesAsync();
                        }

                        else
                        {
                            context.TripTemplates.Add(template);
                            await context.SaveChangesAsync();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new DomingoBlError() { ErrorCode = 100, ErrorMessage = ex.Message };
            }

            return new DomingoBlError() { ErrorCode = 0, ErrorMessage = "" };
        }


        public static async Task<DomingoBlError> SaveTripTemplateStep(TripTemplateStep templateStep)
        {
            try
            {
                if (templateStep != null)
                {
                    using (var context = new TravelogyDevEntities1())
                    {
                        var _tripTemmplateStep = context.TripTemplateSteps.Where(p => p.TripTemplateId == templateStep.TripTemplateId
                                && p.TripTemplateStepIdentifier == templateStep.TripTemplateStepIdentifier).FirstOrDefault();

                        if (_tripTemmplateStep != null)
                        {
                            _tripTemmplateStep.ShortDescription = templateStep.ShortDescription;
                            _tripTemmplateStep.LongDescription = templateStep.LongDescription;
                            _tripTemmplateStep.NightStay = templateStep.NightStay;
                            await context.SaveChangesAsync();
                        }

                        if (string.IsNullOrEmpty(templateStep.TripTemplateStepIdentifier))
                        {
                            _tripTemmplateStep = new TripTemplateStep()
                            {
                                TripTemplateId = templateStep.TripTemplateId,
                                ShortDescription = templateStep.ShortDescription,
                                LongDescription = templateStep.LongDescription,
                                NightStay = templateStep.NightStay
                            };

                            context.TripTemplateSteps.Add(_tripTemmplateStep);
                            await context.SaveChangesAsync();

                            _tripTemmplateStep.TripTemplateStepIdentifier = string.Format("{0}-{1}", _tripTemmplateStep.TripTemplateId, _tripTemmplateStep.Id);
                            await context.SaveChangesAsync();                            
                        }
                    }
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
        /// <param name="model"></param>
        /// <returns></returns>
        public static async Task<DomingoBlError> SaveDestination(Destination model)
        {
            try
            {
                if (model != null)
                {
                    using (var context = new TravelogyDevEntities1())
                    {
                        if (model.Id == 0)
                        {
                            context.Destinations.Add(model);
                            context.SaveChanges();
                        }

                        else
                        {
                            var _dbDestinationObj = context.Destinations.Find(model.Id);
                            if (_dbDestinationObj != null)
                            {
                                _dbDestinationObj.BestTimeToVisit = model.BestTimeToVisit;
                                _dbDestinationObj.CircuitUrl = model.CircuitUrl;
                                _dbDestinationObj.Description = model.Description;
                                _dbDestinationObj.Name = model.Name;
                                _dbDestinationObj.Tagline = model.Tagline;
                                _dbDestinationObj.TemplateSearchAlias = model.TemplateSearchAlias;
                                _dbDestinationObj.ThumbnailPath = model.ThumbnailPath;
                                _dbDestinationObj.TourContinent = model.TourContinent;
                                _dbDestinationObj.TravelStyles = model.TravelStyles;
                                _dbDestinationObj.Weightage = model.Weightage;

                                await context.SaveChangesAsync();
                            }
                        }
                    }
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
        /// <param name="model"></param>
        /// <returns></returns>
        public static async Task<DomingoBlError> SaveTripProvider(TripProvider model)
        {
            try
            {
                if (model != null)
                {
                    using (var context = new TravelogyDevEntities1())
                    {
                        if (model.Id == 0)
                        {
                            context.TripProviders.Add(model);
                            context.SaveChanges();
                        }

                        else
                        {
                            var _tripProvider = context.TripProviders.Find(model.Id);
                            _tripProvider.Address = model.Address;
                            _tripProvider.Description = model.Description;
                            _tripProvider.EmailAddressCustSupport = model.EmailAddressCustSupport;
                            _tripProvider.EmailAddressMarketingSales = model.EmailAddressMarketingSales;
                            _tripProvider.EmailAddressPrimary = model.EmailAddressPrimary;
                            _tripProvider.Name = model.Name;
                            _tripProvider.Telephone01 = model.Telephone01;
                            _tripProvider.Telephone02 = model.Telephone02;
                            _tripProvider.Telephone03 = model.Telephone03;
                            _tripProvider.Type = model.Type;
                            _tripProvider.Website = model.Website;

                            await context.SaveChangesAsync();
                        }
                    }
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
