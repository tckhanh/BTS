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
        IDbFactory dbFactory;
        IBTSCertificateRepository objRepository;
        IUnitOfWork unitOfwork;

        [TestInitialize]
        public void Initialize()
        {
            dbFactory = new DbFactory();
            objRepository = new BTSCertificateRepository(dbFactory);
            unitOfwork = new UnitOfWork(dbFactory);
        }

        [TestMethod]
        public void BTSCertificate_Repository_Create()
        {
            BTSCertificate btsCertificate = new BTSCertificate();
            btsCertificate.ProfileID = 1;
            btsCertificate.CityID = "HCM";
            btsCertificate.DistrictID = 4;
            btsCertificate.OperatorID = "MOBIFONE";
            btsCertificate.Address = "60 Tân Canh, Phường 1, Quận Tân Bình, Thành phố Hồ Chí Minh";

            var result = objRepository.Add(btsCertificate);
            unitOfwork.Commit();

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.ID);

        }
    }
}
