using BTS.Data.Infrastructure;
using BTS.Data.Repository;
using BTS.Model.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTS.UnitTest.RepositoryTest
{
    [TestClass]
    public class BTSCertificateRepositoryTest
    {
        private IDbFactory dbFactory;
        private ICertificateRepository objRepository;
        private IUnitOfWork unitOfwork;

        [TestInitialize]
        public void Initialize()
        {
            //dbFactory = new DbFactory();
            //objRepository = new CertificateRepository(dbFactory);
            //unitOfwork = new UnitOfWork(dbFactory);
        }

        [TestMethod]
        public void BTSCertificate_Repository_Create()
        {
            Certificate certificate = new Certificate();
            certificate.ProfileID = 1;
            certificate.CityID = "HCM";
            certificate.OperatorID = "MOBIFONE";
            certificate.Address = "60 Tân Canh, Phường 1, Quận Tân Bình, Thành phố Hồ Chí Minh";

            var result = objRepository.Add(certificate);
            unitOfwork.Commit();

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
        }

        [TestMethod]
        public void BTSCertificate_Repository_Stat()
        {
            var result1 = objRepository.GetIssuedCertStatByOperatorYear();

            Assert.IsNotNull(result1);
        }
    }
}