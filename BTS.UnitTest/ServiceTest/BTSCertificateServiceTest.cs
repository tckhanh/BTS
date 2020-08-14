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
            //_CertificateService = new CertificateService(_mockCertificateRepository.Object, _mockUnitOfWork.Object);
            _listCertificate = new List<Certificate>()
            {
                new Certificate() { Id="1", ProfileID = Guid.NewGuid().ToString(), CityID ="HCM", OperatorID="MOBIFONE", Address="Một"},
                new Certificate() { Id="2", ProfileID =Guid.NewGuid().ToString(), CityID ="NTN", OperatorID="MOBIFONE", Address="Hai"},
                new Certificate() { Id="3", ProfileID =Guid.NewGuid().ToString(), CityID="LAN", OperatorID="MOBIFONE", Address="Ba"},
            };
        }

        [TestMethod]
        public void BTSCertificate_Service_GetAll()
        {
            int totalRow = 1;

            // set up method
            _mockCertificateRepository.Setup(m => m.GetMultiPaging(x => true, out totalRow,null, 1, 10)).Returns(_listCertificate);

            // call action
            var result = _CertificateService.getAll(out totalRow, true) as List<Certificate>;

            // check Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count);
        }
    }
}