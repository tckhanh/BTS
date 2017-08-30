using AutoMapper;
using BTS.Model.Models;
using BTS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BTS.Web.Mappings
{
    public class AutoMapperConfiguration
    {
        public static IMapper GetMapper()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Applicant, ApplicantViewModel>();
                //lots more maps...?
            });

            IMapper mapper = config.CreateMapper();
            return mapper;
        }

        public static void Configure()
        {
            Mapper.Initialize(cfg => {
                cfg.CreateMap<Applicant, ApplicantViewModel>();
                cfg.CreateMap<BTSCertificate, BTSCertificateViewModel>();
                cfg.CreateMap<City, CityViewModel>();
                cfg.CreateMap<District, DistrictViewModel>();
                cfg.CreateMap<Operator, OperatorViewModel>();
                cfg.CreateMap<Model.Models.Profile, ProfileViewModel>();
                cfg.CreateMap<SubBTS, SubBTSViewModel>();
                /* etc */
            });            
        }
    }
}