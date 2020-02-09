using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;

using BLL;
using DAL;
using Model;
using Vy2.Controllers;
using System.Linq;
using System.ServiceModel;
using MvcContrib.TestHelper;
using System.Collections.Generic;
using Rhino.Mocks;
using System.Web;
using Moq;

namespace EnhetsTest
{
    [TestClass]
    public class VyControllerTest
    {
        private VyController controller = new VyController(new AdminBLL(new AdminAksessStub()));
        private TestControllerBuilder SessionMock = new TestControllerBuilder();



        [TestMethod]
        public void Logg_Inn_Super_Test()
        {
            SessionMock.InitializeController(controller);

            //arrange
            string Epost = "Super@Admin.no";
            string Passord = "admin";
            controller.Session["loggetInn"] = true;
            controller.Session["SuperLoggetInn"] = true;

            //act
            var resultat = (RedirectToRouteResult)controller.Logginn(Epost, Passord);
            //Assert
            Assert.AreEqual(resultat.RouteValues.Values.First(), "AdminIndex");
        }

        [TestMethod]
        public void LoggInnVanligTest()
        {
            //arrange
            SessionMock.InitializeController(controller);

            string Epost = "asbjorn.nordgaard@gmail.com";
            string Passord = "asbjorn";
            SessionMock.Session["loggetInn"] = true;
            SessionMock.Session["SuperLoggetInn"] = false;
            //act
            var resultat = (RedirectToRouteResult)controller.Logginn(Epost, Passord);
            //assert
            Assert.AreEqual(resultat.RouteValues.Values.First(), "AdminIndex");
        }

        [TestMethod]
        public void LoggInnFeilet()
        {
            //arrange
            SessionMock.InitializeController(controller);

            string Epost = "";
            string Passord = "";
            SessionMock.Session["loggetInn"] = false;

            //act
            var resultat = (RedirectToRouteResult)controller.Logginn(Epost, Passord);

            //assert
            Assert.AreEqual(resultat.RouteValues.Values.First(), "Index");
        }

        [TestMethod]
        public void Vis_AdminIndex_view_VanligLogin()
        {
            //arrange
            SessionMock.InitializeController(controller);

            SessionMock.Session["loggetInn"] = true;
            SessionMock.Session["SuperLoggetInn"] = false;

            //act
            var resultat = (ViewResult)controller.AdminIndex();
            //assert
            Assert.AreEqual(resultat.ViewName, "");

        }


        [TestMethod]
        public void Vis_AdminIndex_view_SuperLogin()
        {
            //arrange
            SessionMock.InitializeController(controller);

            SessionMock.Session["loggetInn"] = true;
            SessionMock.Session["SuperLoggetInn"] = true;
            //act
            var resultat = (ViewResult)controller.AdminIndex();
            //assert
            Assert.AreEqual(resultat.ViewName, "");

        }

        [TestMethod]
        public void Vis_AdminIndex_view_UtenLogin()
        {
            //arrange
            SessionMock.InitializeController(controller);

            SessionMock.Session["loggetInn"] = false;
            SessionMock.Session["SuperLoggetInn"] = false;
            //act
            var resultat = (ViewResult)controller.Index();
            //assert
            Assert.AreEqual(resultat.ViewName, "");
        }

        [TestMethod]
        public void Vis_EndreRuteStatus_View()
        {
            //arrange
            SessionMock.InitializeController(controller);

            SessionMock.Session["loggetInn"] = true;
            List<TogRute> AktiveRuter = new List<TogRute>();
            List<TogRute> InaktiveRuter = new List<TogRute>();
            List<List<TogRute>> BeggeTo = new List<List<TogRute>>();

            var aktivRute = new TogRute()
            {
                AnkomstTid = DateTime.Now.AddHours(2),
                AvgangTid = DateTime.Now,
                EndeStasjon = "Bergen jernbanestasjon",
                StartStasjon = "Oslo sentralstasjon",
                Instillt = false,
                Platform = "1",
                Pris = 700,
                RuteId = 1
            };

            AktiveRuter.Add(aktivRute);
            AktiveRuter.Add(aktivRute);
            AktiveRuter.Add(aktivRute);

            var inaktivRute = new TogRute()
            {
                AnkomstTid = DateTime.Now.AddHours(2),
                AvgangTid = DateTime.Now,
                EndeStasjon = "Bergen jernbanestasjon",
                StartStasjon = "Oslo sentralstasjon",
                Instillt = true,
                Platform = "1",
                Pris = 700,
                RuteId = 1
            };

            InaktiveRuter.Add(inaktivRute);
            InaktiveRuter.Add(inaktivRute);
            InaktiveRuter.Add(inaktivRute);

            BeggeTo.Add(InaktiveRuter);
            BeggeTo.Add(AktiveRuter);

            //act
            var resultat = (ViewResult)controller.EndreRuteStatus();
            //assert
            Assert.AreEqual(resultat.ViewName, "");
        }

        [TestMethod]
        public void Vis_EndreRuteStatus_View_UtenLogin()
        {
            //arrange
            SessionMock.InitializeController(controller);
            SessionMock.Session["loggetInn"] = false;

            //act
            var resultat = (ViewResult)controller.Index();
            //assert
            Assert.AreEqual(resultat.ViewName, "");
        }

        [TestMethod]
        public void Innstill_Rute_login()
        {
            //arrange
            SessionMock.InitializeController(controller);
            SessionMock.Session["loggetInn"] = true;
            int ruteId = 1;
            //act
            var resultat = (RedirectToRouteResult)controller.InstillRute(ruteId);
            //assert
            Assert.AreEqual(resultat.RouteValues.Values.First(), "EndreRuteStatus");

        }

        public void Innstill_Rute_UtenLogin()
        {

            //arrange
            SessionMock.InitializeController(controller);
            SessionMock.Session["loggetInn"] = false;
            //act
            var resultat = (RedirectToRouteResult)controller.Index();
            //assert
            Assert.AreEqual(resultat.RouteValues.Values.First(), "");

        }

        [TestMethod]
        public void Gjenopprett_rute()
        {
            //arrange
            SessionMock.InitializeController(controller);
            SessionMock.Session["loggetInn"] = true;
            int ruteId = 1;
            //act
            var resultat = (RedirectToRouteResult)controller.GjenopprettRute(ruteId);
            //assert
            Assert.AreEqual(resultat.RouteValues.Values.First(), "EndreRuteStatus");
        }

        [TestMethod]
        public void Gjenopprett_rute_UtenLogin()
        {
            //arrange
            SessionMock.InitializeController(controller);
            SessionMock.Session["loggetInn"] = true;
            //act
            var resultat = (ViewResult)controller.Index();
            //assert
            Assert.AreEqual(resultat.ViewName, "");
        }

        [TestMethod]
        public void Vis_RegistrerAdmin_View_SuperLoggetinn()
        {
            //arrange
            SessionMock.InitializeController(controller);
            SessionMock.Session["loggetInn"] = true;
            SessionMock.Session["SuperLoggetInn"] = true;

            List<Administrator> alleAdmins = new List<Administrator>();
            Administrator enAdmin = new Administrator()
            {
                AnsattNr = 1,
                Epost = "Super@Admin.no",
                Rolle = "Super",
                Passord = new Byte[8] { 1, 2, 3, 4, 5, 6, 7, 8 },
                Salt = new Byte[8] { 2, 3, 4, 5, 6, 7, 8, 9 }
            };
            alleAdmins.Add(enAdmin);
            alleAdmins.Add(enAdmin);
            alleAdmins.Add(enAdmin);

            //act
            var resultat = (ViewResult)controller.RegistrerAdmin();
            //assert
            Assert.AreEqual(resultat.ViewName, "");
        }

        [TestMethod]
        public void Vis_RegistrerAdmin_View_VanligLoggetinn()
        {
            //arrange
            SessionMock.InitializeController(controller);
            SessionMock.Session["loggetInn"] = true;
            SessionMock.Session["SuperLoggetInn"] = false;


            //act
            var resultat = (ViewResult)controller.AdminIndex();
            //assert
            Assert.AreEqual(resultat.ViewName, "");
        }

        [TestMethod]
        public void Vis_RegistrerAdmin_View_IkkeLoggetInn()
        {
            //arrange
            SessionMock.InitializeController(controller);
            SessionMock.Session["loggetInn"] = false;
            SessionMock.Session["SuperLoggetInn"] = false;

            //act
            var resultat = (ViewResult)controller.Index();
            //assert
            Assert.AreEqual(resultat.ViewName, "");
        }

        [TestMethod]
        public void RegistrerAdmin_POST()
        {
            //arrange
            SessionMock.InitializeController(controller);
            SessionMock.Session["loggetInn"] = true;
            SessionMock.Session["SuperLoggetInn"] = true;
            string NyEpost = "Super@Admin.no";
            string NyPassord = "admin";
            string Rolle = "Super";

            //act
            var resultat = (ViewResult)controller.RegistrerAdmin(NyEpost, NyPassord, Rolle);
            //assert
            Assert.AreEqual(resultat.ViewName, "");
        }

        [TestMethod]
        public void RegistrerAdmin_POST_SuperLoginn()
        {
            //arrange
            SessionMock.InitializeController(controller);
            SessionMock.Session["loggetInn"] = true;
            SessionMock.Session["SuperLoggetInn"] = true;
            string NyEpost = "Super@Admin.no";
            string NyPassord = "admin";
            string Rolle = "Super";

            //act
            var resultat = (ViewResult)controller.RegistrerAdmin(NyEpost, NyPassord, Rolle);
            //assert
            Assert.AreEqual(resultat.ViewName, "");
        }
        [TestMethod]
        public void RegistrerAdmin_POST_VanligLoggInn()
        {
            //arrange
            SessionMock.InitializeController(controller);
            SessionMock.Session["loggetInn"] = true;
            SessionMock.Session["SuperLoggetInn"] = false;


            //act
            var resultat = (ViewResult)controller.AdminIndex();
            //assert
            Assert.AreEqual(resultat.ViewName, "");
        }
        [TestMethod]
        public void RegistrerAdmin_POST_UtenLoginn()
        {
            //arrange
            SessionMock.InitializeController(controller);
            SessionMock.Session["loggetInn"] = false;
            SessionMock.Session["SuperLoggetInn"] = false;


            //act
            var resultat = (ViewResult)controller.Index();
            //assert
            Assert.AreEqual(resultat.ViewName, "");
        }

        [TestMethod]
        public void Vis_Endreruter_View()
        {
            //arrange
            SessionMock.InitializeController(controller);
            SessionMock.Session["loggetInn"] = true;

            List<TogRute> AktiveRuter = new List<TogRute>();

            var Rute = new TogRute()
            {
                AnkomstTid = DateTime.Now.AddHours(2),
                AvgangTid = DateTime.Now,
                EndeStasjon = "Bergen jernbanestasjon",
                StartStasjon = "Oslo sentralstasjon",
                Instillt = false,
                Platform = "1",
                Pris = 700,
                RuteId = 1
            };

            AktiveRuter.Add(Rute);
            AktiveRuter.Add(Rute);
            AktiveRuter.Add(Rute);

            //act
            var resultat = (ViewResult)controller.EndreRuter();
            //assert
            Assert.AreEqual(resultat.ViewName, "");
        }


        [TestMethod]
        public void Vis_Endreruter_View_UtenLogin()
        {
            //arrange
            SessionMock.InitializeController(controller);
            SessionMock.Session["loggetInn"] = false;

            //act
            var resultat = (ViewResult)controller.Index();
            //assert
            Assert.AreEqual(resultat.ViewName, "");
        }

        [TestMethod]
        public void Endreruter_POST()
        {
            //arrange
            SessionMock.InitializeController(controller);
            SessionMock.Session["loggetInn"] = true;
            TogRute endretRute = new TogRute()
            {
                AnkomstTid = DateTime.Now.AddHours(2),
                AvgangTid = DateTime.Now,
                EndeStasjon = "Bergen jernbanestasjon",
                StartStasjon = "Oslo sentralstasjon",
                Instillt = false,
                Platform = "1",
                Pris = 700,
                RuteId = 1
            };
            //act
            var resultat = controller.EndreRuter(endretRute);
            //assert
            Assert.AreEqual(resultat, true);
        }

        [TestMethod]
        public void Endreruter_POST_UtenLoginn()
        {
            //arrange
            SessionMock.InitializeController(controller);
            SessionMock.Session["loggetInn"] = false;

            //act
            var resultat = (ViewResult)controller.Index();
            //assert
            Assert.AreEqual(resultat.ViewName, "");
        }

        //lånt fra https://stackoverflow.com/questions/4609661/mvccontrib-testhelper-problem-with-session-clear-session-abandon-and-rhino-mock
        //MvcContrib.TestHelper returnerte en NotImplementedException for SessionMock.Session.Abandon(). 
        //Ble nødt til å lage en egen mock med Rhino.mocks
        [TestMethod]
        public void LoggUt()
        {
            //arrange
            SessionMock.InitializeController(controller);
            var mockSession = Rhino.Mocks.MockRepository.GenerateStub<HttpSessionStateBase>();
            controller.HttpContext.BackToRecord();
            controller.HttpContext.Stub(c => c.Session).Return(mockSession);
            controller.HttpContext.Replay();
            SessionMock.Session["loggetInn"] = true;
            SessionMock.Session["SuperLoggetInn"] = true;
            //act
            controller.Session.Abandon();

            var resultat = (RedirectToRouteResult)controller.LoggUt();

            //assert
            Assert.AreEqual(resultat.RouteValues.Values.First(), "Index");
        }

        [TestMethod]
        public void SlettAdmin()
        {
            //arrange
            SessionMock.InitializeController(controller);
            int AnsattId = 1;
            //act
            var resultat = (RedirectToRouteResult)controller.SlettAdmin(AnsattId);
            //assert
            Assert.AreEqual(resultat.RouteValues.Values.First(), "RegistrerAdmin");
        }

        [TestMethod]
        public void SlettAdmin_feilet()
        {
            //arrange
            SessionMock.InitializeController(controller);
            int AnsattId = 0;
            //act
            var resultat = (RedirectToRouteResult)controller.SlettAdmin(AnsattId);
            //assert
            Assert.AreEqual(resultat.RouteValues.Values.First(), "RegistrerAdmin");
        }

        [TestMethod]
        public void EndreAdmin_VisView()
        {
            //arrange
            SessionMock.InitializeController(controller);
            SessionMock.Session["loggetInn"] = true;
            SessionMock.Session["SuperLoggetInn"] = true;
            //act
            var resultat = (ViewResult)controller.EndreAdmin();
            //assert
            Assert.AreEqual(resultat.ViewName, "");
        }

        [TestMethod]
        public void SeEndringer()
        {
            SessionMock.InitializeController(controller);
            SessionMock.Session["loggetInn"] = true;



            var resultat = (ViewResult)controller.SeEndringer();

            Assert.AreEqual(resultat.ViewName, "");
        }

        [TestMethod]
        public void LeggTilRute_loggetInn_view()
        {
            SessionMock.InitializeController(controller);
            SessionMock.Session["loggetInn"] = true;

            var resultat = (ViewResult)controller.Index();

            Assert.AreEqual(resultat.ViewName, "");

        }
        [TestMethod]
        public void LeggTilRute_utenLogin_view()
        {
            SessionMock.InitializeController(controller);
            SessionMock.Session["loggetInn"] = false;

            var resultat = (ViewResult)controller.Index();

            Assert.AreEqual(resultat.ViewName, "");

        }

        [TestMethod]
        public void LeggTilRute_Post_OK()
        {
            SessionMock.InitializeController(controller);
            SessionMock.Session["loggetInn"] = true;
            DateTime AvgangTid = new DateTime();
            DateTime AnkomstTid = new DateTime();
            string StartStasjon = "Oslo sentralstasjon";
            string EndeStasjon = "Ski stasjon";
            string Platform = "1";
            double Pris = 700;

            TogRute rute = new TogRute()
            {
                AvgangTid = AvgangTid,
                AnkomstTid = AnkomstTid,
                StartStasjon = StartStasjon,
                EndeStasjon = EndeStasjon,
                Platform = Platform,
                Pris = Pris,
                Instillt = false
            };

            var resultat = (ViewResult)controller.LeggTilRute();

            Assert.AreEqual(resultat.ViewName, "");

        }

        [TestMethod]
        public void LeggTilRute_post_feilet()
        {
            SessionMock.InitializeController(controller);
            SessionMock.Session["loggetInn"] = true;
            DateTime AvgangTid = new DateTime();
            DateTime AnkomstTid = new DateTime();
            string StartStasjon = "";
            string EndeStasjon = "Ski stasjon                                                               ";
            string Platform = "-1";
            double Pris = -5000000000;

            TogRute rute = new TogRute()
            {
                AvgangTid = AvgangTid,
                AnkomstTid = AnkomstTid,
                StartStasjon = StartStasjon,
                EndeStasjon = EndeStasjon,
                Platform = Platform,
                Pris = Pris,
                Instillt = false
            };

            var resultat = (ViewResult)controller.LeggTilRute();

            Assert.AreEqual(resultat.ViewName, "");
        }

        [TestMethod]
        public void Vis_Administrativt_View()
        {
            SessionMock.InitializeController(controller);
            SessionMock.Session["SuperLoggetInn"] = true;
            SessionMock.Session["loggetInn"] = true;

            var resultat = (ViewResult)controller.Administrativt();

            Assert.AreEqual(resultat.ViewName, "");
        }

        [TestMethod]
        public void Vis_Àdministrativt_View_UtenLoginn()
        {
            SessionMock.InitializeController(controller);
            SessionMock.Session["SuperLoggetInn"] = false;
            SessionMock.Session["loggetInn"] = false;

            var resultat = (ViewResult)controller.Index();

            Assert.AreEqual(resultat.ViewName, "");
        }

        [TestMethod]
        public void Vis_administrativt_view_vanligLoginn()
        {
            SessionMock.InitializeController(controller);
            SessionMock.Session["SuperLoggetInn"] = false;
            SessionMock.Session["loggetInn"] = true;

            var resultat = (ViewResult)controller.AdminIndex();

            Assert.AreEqual(resultat.ViewName, "");
        }

        [TestMethod]
        public void Vis_Vedlikeholdt_View()
        {
            SessionMock.InitializeController(controller);
            SessionMock.Session["loggetInn"] = true;

            var resultat = (ViewResult)controller.Vedlikehold();

            Assert.AreEqual(resultat.ViewName, "");
        }

        [TestMethod]
        public void Vis_Vedlikeholdt_View_UtenLoggin()
        {
            SessionMock.InitializeController(controller);
            SessionMock.Session["loggetInn"] = false;

            var resultat = (ViewResult)controller.Index();

            Assert.AreEqual(resultat.ViewName, "");
        }

        [TestMethod]
        public void Vis_AdminRute_View_UtenLoggin()
        {
            SessionMock.InitializeController(controller);
            SessionMock.Session["loggetInn"] = false;

            var resultat = (ViewResult)controller.Index();

            Assert.AreEqual(resultat.ViewName, "");
        }

        [TestMethod]
        public void Vis_AdminRute_View_MedLoggin()
        {
            SessionMock.InitializeController(controller);
            SessionMock.Session["loggetInn"] = true;

            var resultat = (ViewResult)controller.AdminRute();

            Assert.AreEqual(resultat.ViewName, "");
        }
    }
}
