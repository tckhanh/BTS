using BTS.Data.Infrastructure;
using BTS.Data.Repository;
using BTS.Model.Models;
using BTS.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTS.UnitTest.ServiceTest
{
    [TestClass]
    public class BTSCertificateServiceTest
    {
        private Mock<IBTSCertificateRepository> _mockBTSCertificateRepository;
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private IBTSCertificateService _BTSCertificateService;
        private List<BTSCertificate> _listBTSCertificate;

        [TestInitialize]
        public void Initialize()
        {
            _mockBTSCertificateRepository = new Mock<IBTSCertificateRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _BTSCertificateService = new BTSCertificateService(_mockBTSCertificateRepository.Object, _mockUnitOfWork.Object);
            _listBTSCertificate = new List<BTSCertificate>()
            {
                new BTSCertificate() { ID=1, ProfileID=1, CityID="HCM", OperatorID="MOBIFONE", Address="Một"},
                new BTSCertificate() { ID=2, ProfileID=2, CityID="NTN", OperatorID="MOBIFONE", Address="Hai"},
                new BTSCertificate() { ID=3, ProfileID=3, CityID="LAN", OperatorID="MOBIFONE", Address="Ba"},
            };

        }

        [TestMethod]
        public void BTSCertificate_Service_GetAll()
        {
            int totalRow = 1;

            // set up method
            _mockBTSCertificateRepository.Setup(m => m.GetMultiPaging(x => true, out totalRow, 1, 10, null)).Returns(_listBTSCertificate);

            // call action
            var result = _BTSCertificateService.getAll(totalRow) as List<BTSCertificate>;

            // check Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count);

        }
    }
}
