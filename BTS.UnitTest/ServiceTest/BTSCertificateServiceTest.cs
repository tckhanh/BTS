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
        private Mock<ICertificateRepository> _mockCertificateRepository;
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private ICertificateService _CertificateService;
        private List<Certificate> _listCertificate;

        [TestInitialize]
        public void Initialize()
        {
            _mockCertificateRepository = new Mock<ICertificateRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _CertificateService = new CertificateService(_mockCertificateRepository.Object, _mockUnitOfWork.Object);
            _listCertificate = new List<Certificate>()
            {
                new Certificate() { ID="1", ProfileID=1, CityID="HCM", OperatorID="MOBIFONE", Address="Một"},
                new Certificate() { ID="2", ProfileID=2, CityID="NTN", OperatorID="MOBIFONE", Address="Hai"},
                new Certificate() { ID="3", ProfileID=3, CityID="LAN", OperatorID="MOBIFONE", Address="Ba"},
            };

        }

        [TestMethod]
        public void BTSCertificate_Service_GetAll()
        {
            int totalRow = 1;

            // set up method
            _mockCertificateRepository.Setup(m => m.GetMultiPaging(x => true, out totalRow, 1, 10, null)).Returns(_listCertificate);

            // call action
            var result = _CertificateService.getAll(out totalRow) as List<Certificate>;

            // check Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count);

        }
    }
}
