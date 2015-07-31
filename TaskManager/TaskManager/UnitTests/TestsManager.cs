using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TaskManager.Realizations;

namespace TaskManager.UnitTests
{
    [TestClass]
    public class TestsManager
    {
        private readonly TaskManagerEntities m_db = new TaskManagerEntities();
        private RealizeManager m_RealizeManager = new RealizeManager();
        private string m_ForIsTrue;

        /// <summary>
        /// Testing for finishing task
        /// </summary>
        [TestMethod]
        public void TestTaskStatusFin()
        {
            m_ForIsTrue = m_RealizeManager.TaskStatusFin(38);
            Assert.IsTrue(m_ForIsTrue == "Задача завершина");
        }
    }

}
