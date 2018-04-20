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
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Applicant, ApplicantViewModel>();
                cfg.CreateMap<Operator, OperatorViewModel>();
                //lots more maps...?
            });

            IMapper mapper = config.CreateMapper();
            return mapper;
        }

        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<InCaseOf, InCaseOfViewModel>();
                cfg.CreateMap<Applicant, ApplicantViewModel>();
                cfg.CreateMap<Certificate, CertificateViewModel>();
                cfg.CreateMap<City, CityViewModel>();
                cfg.CreateMap<Operator, OperatorViewModel>();
                cfg.CreateMap<Model.Models.Profile, ProfileViewModel>();
                cfg.CreateMap<SubBtsInCert, SubBtsInCertViewModel>();

                cfg.CreateMap<Footer, FooterViewModel>();
                cfg.CreateMap<Slide, SlideViewModel>();
                cfg.CreateMap<WebPage, PageViewModel>();
                cfg.CreateMap<ContactDetail, ContactDetailViewModel>();

                cfg.CreateMap<ApplicationGroup, ApplicationGroupViewModel>();
                cfg.CreateMap<ApplicationRole, ApplicationRoleViewModel>();
                cfg.CreateMap<ApplicationUser, ApplicationUserViewModel>();
                /* etc */
            });
        }
    }
}