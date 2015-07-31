using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TaskManager;
using TaskManager.Controllers;
using TaskManager.Realizations;
using System.Data.Entity.Migrations;

namespace UnitTests
{

    [TestClass]
    public class ManagerTests
    {
        //private readonly TaskManagerEntities m_db = new TaskManagerEntities();
        private RealizeManager m_RealizeManager = new RealizeManager();
        private string m_ForIsTrue;
        
        [TestMethod]
        public void TesTaskFinihed()
        {

            m_ForIsTrue = m_RealizeManager.TaskStatusFin(38);



            Assert.IsTrue(m_ForIsTrue == "Задача завершина");


        }
    }
}
