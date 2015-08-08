using System;
using System.Data.Entity.Migrations;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TaskManager;
using TaskManager.Controllers;
using TaskManager.Realizations;

namespace UnitTests
{
    [TestClass]
    public class TestManager
    {
        private readonly TaskManagerEF m_db = new TaskManagerEF();
        private RealizeManager m_RealizeManager = new RealizeManager();
        private string m_ForIsTrue;

        #region TestTaskStatusFin ­
        /// <summary>
        /// Testing for finishing task
        /// </summary>
        [TestMethod]
        public void TestTaskStatusFin()
        {
            // BUG
            var value = m_db.Tasks.Where(x => x.TaskId.Equals(38)).FirstOrDefault();
            Assert.IsTrue(value != null);
        }
        #endregion

        #region TestTaskStatu ­
        [TestMethod]
        public void TestTaskStatu()
        {

            var specialityRepositoryMock = new Mock<IAccount>();
            //specialityRepositoryMock.Setup(a => a.CommonUpdateStatus());
            var templateController = new AccountController(Mock.Of<IAccount>());
            var view = templateController.Registration();
            Assert.IsNotNull(view);


        }
        #endregion
    }
}
