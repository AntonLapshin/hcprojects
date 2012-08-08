using SharedComponents.Connection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace SharedComponents.Tests
{


    /// <summary>
    ///Это класс теста для RMSConnectionTest, в котором должны
    ///находиться все модульные тесты RMSConnectionTest
    ///</summary>
    [TestClass()]
    public class RMSConnectionTest
    {
        private TestContext testContextInstance;

        /// <summary>
        ///Получает или устанавливает контекст теста, в котором предоставляются
        ///сведения о текущем тестовом запуске и обеспечивается его функциональность.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Дополнительные атрибуты теста
        // 
        //При написании тестов можно использовать следующие дополнительные атрибуты:
        //
        //ClassInitialize используется для выполнения кода до запуска первого теста в классе
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //ClassCleanup используется для выполнения кода после завершения работы всех тестов в классе
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //TestInitialize используется для выполнения кода перед запуском каждого теста
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //TestCleanup используется для выполнения кода после завершения каждого теста
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        /// <summary>
        ///Тест для GetConnectionString
        ///</summary>
        [TestMethod()]
        public void GetConnectionStringTest()
        {
            var target = RMSConnection.RMSP;
            string login = string.Empty;
            string password = string.Empty;
            string expected =
                "User Id=;Password=;Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=10.0.1.52)(PORT=1621))(ADDRESS=(PROTOCOL=TCP)(HOST=10.0.1.54)(PORT=1621))(LOAD_BALANCE = yes)(CONNECT_DATA=(SERVER = DEDICATED)(SERVICE_NAME=rmsp)));";
            string actual = target.GetConnectionString(login, password);
            Assert.AreEqual(expected, actual);
        }
    }
}
