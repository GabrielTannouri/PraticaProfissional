using Pratica_Profissional.DAO;
using Pratica_Profissional.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pratica_Profissional.Controllers
{
    public class PaisesController : Controller
    {
        DAOPaises daoPaises = new DAOPaises();
        // GET: Paises
        public ActionResult Index()
        {
            var daoPaises = new DAOPaises();
            List<Paises> lista = daoPaises.GetPaises();
            return View(lista);

        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Paises paises)
        {
            var dtAtual = DateTime.Today;
            paises.dtCadastro = dtAtual;
            try
            {
                var daoPaises = new DAOPaises();

                if (daoPaises.Create(paises))
                {
                    ViewBag.Message = "País inserido com sucesso!";
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var daoPaises = new DAOPaises();

            return View(daoPaises.GetPaises().Find(u => u.idPais == id));
        }

        [HttpPost]
        public ActionResult Edit(Paises paises)
        {
            var dtAtualizacao = DateTime.Today;
            paises.dtAtualizacao = dtAtualizacao;
            try
            {
                var daoPaises = new DAOPaises();
                daoPaises.Edit(paises);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var daoPaises = new DAOPaises();

            return View(daoPaises.GetPaises().Find(u => u.idPais == id));
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                var daoPaises = new DAOPaises();

                if (daoPaises.Delete(id))
                {
                    ViewBag.AlertMsg = "País excluído com sucesso.";
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var daoPaises = new DAOPaises();

            return View(daoPaises.GetPaises().Find(u => u.idPais == id));
        }
    }
}